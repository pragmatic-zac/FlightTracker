package main

import (
	"bufio"
	"encoding/json"
	"fmt"
	"net"
	"strconv"
	"strings"
	"time"
)

var count = 0

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

func createFlight(data string) Flight {
	println(data)

	chunks := strings.Split(data, ",")

	altitude, err := strconv.Atoi(chunks[11])
	track, err := strconv.Atoi(chunks[13])
	groundSpeed, err := strconv.ParseFloat(chunks[12], 32)
	lat, err := strconv.ParseFloat(chunks[14], 32)
	long, err := strconv.ParseFloat(chunks[15], 32)
	vertical, err := strconv.Atoi(chunks[15])
	squawk, err := strconv.Atoi(chunks[16])
	emergency, err := strconv.Atoi(chunks[18])

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

	e, err := json.Marshal(toReturn)
	if err != nil {
		println("could not convert to json")
	}

	println(string(e))

	return toReturn
}

func handleConnection(c net.Conn) {
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

		createFlight(temp)

		counter := strconv.Itoa(count) + "\n"
		c.Write([]byte(string(counter)))
	}
	c.Close()
}

func main() {
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
		go handleConnection(c)
		count++
	}
}
