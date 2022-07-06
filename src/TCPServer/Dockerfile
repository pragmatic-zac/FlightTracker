FROM golang:1.18

WORKDIR /app

# pre-copy/cache go.mod for pre-downloading dependencies and only redownloading them in subsequent builds if they change
COPY TCPServer/go.mod ./
RUN go mod download && go mod verify

# this pathing is necessary because I used Visual Studio to manage docker-compose, don't do that again
COPY TCPServer/*.go ./
RUN go build -o /flight-server

CMD [ "/flight-server" ]