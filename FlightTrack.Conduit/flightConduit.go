package main

import (
	"bufio"
	"encoding/json"
	"fmt"
	"net"
	"os"
	"strings"
)

// create a struct for storing CSV lines and annotate it with JSON struct field tags
type Flight struct {
	Hex          string  `json:"hex"`          // 5
	Date         string  `json:"date"`         // 6
	Time         string  `json:"time"`         // 7
	Callsign     string  `json:"callsign"`     // 10
	Altitude     string  `json:"altitude"`     // 11
	GroundSpeed  float32 `json:"groundSpeed"`  // 12
	Track        int     `json:"track"`        // 13
	Latitude     float32 `json:"latitude"`     // 14
	Longitude    float32 `json:"longitude"`    // 15
	VerticalRate int     `json:"verticalRate"` // 16
	Squawk       int     `json:"squawk"`       // 17
	Emergency    int     `json:"emergency"`    // 19
}

func createFlight(data string) Flight {
	chunks := strings.Split(data, ",")

	toReturn := Flight{
		Hex:  chunks[4],
		Date: chunks[5],
	}

	println("created", toReturn.Hex)

	e, err := json.Marshal(toReturn)
	if err != nil {
		println("could not convert to json")
	}

	println("json format", e)

	return toReturn
}

// this is just a small app that pipes data from a locally running dump1090 instance to the TCP server running on docker
// ideally this would be running on the Pi itself but this is a proof of concept so it lives here for now
// this app runs independently of the rest of the docker stuff
func main() {
	// this is where dump1090 is running
	servAddr := "192.168.1.190:30003"
	tcpAddr, err := net.ResolveTCPAddr("tcp", servAddr)
	if err != nil {
		println("ResolveTCPAddr failed:", err.Error())
		os.Exit(1)
	}

	conn, err := net.DialTCP("tcp", nil, tcpAddr)
	if err != nil {
		println("Dial failed:", err.Error())
		os.Exit(1)
	}

	defer conn.Close()

	// connect to (local) server to feed data to
	localServAddr := "localhost:2100"
	localTcpAddr, err := net.ResolveTCPAddr("tcp", localServAddr)
	if err != nil {
		println("ResolveTCPAddr failed:", err.Error())
		os.Exit(1)
	}

	serverConn, err := net.DialTCP("tcp", nil, localTcpAddr)
	if err != nil {
		println("Dial to server failed:", err.Error())
		os.Exit(1)
	}
	defer serverConn.Close()

	for {
		// does this reader need to be moved outside of the loop so it is reused?
		message, _ := bufio.NewReader(conn).ReadString('\n')

		// createFlight(message)
		// instead of calling createFlight I could just send to the other server here
		fmt.Fprintf(serverConn, message+"\n")

		// println("from server=", string(message))
	}
}
