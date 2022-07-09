package main

import (
	"bufio"
	"encoding/json"
	"fmt"
	"log"
	"net"
	"strconv"
	"strings"
	"time"

	amqp "github.com/rabbitmq/amqp091-go"
)

// create a struct for storing CSV lines and annotate it with JSON struct field tags
// the field number is 0 based
type Flight struct {
	Hex          string  `json:"hex"`          // 4
	Callsign     string  `json:"callsign"`     // 10
	Altitude     int     `json:"altitude"`     // 11
	GroundSpeed  float64 `json:"groundSpeed"`  // 12
	Track        int     `json:"track"`        // 13
	Latitude     float64 `json:"latitude"`     // 14
	Longitude    float64 `json:"longitude"`    // 15
	VerticalRate int     `json:"verticalRate"` // 16
	Squawk       int     `json:"squawk"`       // 17
	Emergency    int     `json:"emergency"`    // 19
	Timestamp    string  `json:"timestamp"`
}

type Envelope struct {
	Message     Flight   `json:"message"`
	MessageType []string `json:"messageType"`
}

// create stringified flight for sending over RabbitMQ
func createFlight(data string) Flight {
	// println(data)

	chunks := strings.Split(data, ",")

	altitude, err := strconv.Atoi(chunks[11])
	track, err := strconv.Atoi(chunks[13])
	groundSpeed, err := strconv.ParseFloat(chunks[12], 32)
	lat, err := strconv.ParseFloat(chunks[14], 32)
	long, err := strconv.ParseFloat(chunks[15], 32)
	vertical, err := strconv.Atoi(chunks[16])
	squawk, err := strconv.Atoi(chunks[17])
	emergency, err := strconv.Atoi(chunks[19])

	toReturn := Flight{
		Hex:          chunks[4],
		Timestamp:    time.Now().Format(time.Stamp),
		Callsign:     chunks[10],
		Altitude:     altitude,
		GroundSpeed:  groundSpeed,
		Track:        track,
		Latitude:     lat,
		Longitude:    long,
		VerticalRate: vertical,
		Squawk:       squawk,
		Emergency:    emergency,
	}

	// e, err := json.Marshal(toReturn)
	// if err != nil {
	// 	println("could not convert to json")
	// }

	// return string(e)

	if err != nil {
		println("error creating flight")
	}

	return toReturn
}

// this wraps the Flight in an envelope that is expected by MassTransit
func formMessage(flight Flight) string {
	envelope := Envelope{
		Message:     flight,
		MessageType: []string{"urn:message:FlightTracker.Common:ConduitPing"},
	}

	e, err := json.Marshal(envelope)
	if err != nil {
		println("could not convert to json")
	}

	return string(e)
}

func handleConnection(c net.Conn, ch *amqp.Channel, q amqp.Queue) {
	fmt.Print(".")
	for {
		netData, err := bufio.NewReader(c).ReadString('\n')
		if err != nil {
			fmt.Println(err)
			return
		}

		temp := strings.TrimSpace(string(netData))
		if temp == "STOP" {
			break
		}

		f := createFlight(temp)
		m := formMessage(f)
		publishMessage(ch, q, m)
	}
	c.Close()
}

func failOnError(err error, msg string) {
	if err != nil {
		log.Panicf("%s: %s", msg, err)
	}
}

func publishMessage(ch *amqp.Channel, q amqp.Queue, body string) {
	err := ch.Publish(
		"",     // exchange
		q.Name, // routing key
		false,  // mandatory
		false,  // immediate
		amqp.Publishing{
			ContentType: "text/plain",
			Body:        []byte(body),
		})
	failOnError(err, "Failed to publish a message")
}

func main() {
	// hardcode a delay to let RabbitMQ get running
	println("Starting FlightTrack.Conduit...")
	time.Sleep(10 * time.Second)

	// RabbitMQ Setup
	conn, err := amqp.Dial("amqp://guest:guest@track-rabbitmq:5672")
	failOnError(err, "Failed to connect to RabbitMQ")
	defer conn.Close()

	ch, err := conn.Channel()
	failOnError(err, "Failed to open a channel")
	defer ch.Close()

	q, err := ch.QueueDeclare(
		"adsb-pings", // name
		true,         // durable
		false,        // delete when unused
		false,        // exclusive
		false,        // no-wait
		nil,          // arguments
	)
	failOnError(err, "Failed to declare a queue")

	// TCP
	println("STARTING TCP")
	l, err := net.Listen("tcp4", ":2100")
	if err != nil {
		fmt.Println(err)
		return
	}
	defer l.Close()

	for {
		c, err := l.Accept()
		if err != nil {
			fmt.Println(err)
			return
		}
		go handleConnection(c, ch, q)
	}
}
