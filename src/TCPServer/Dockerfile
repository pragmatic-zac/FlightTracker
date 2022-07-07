FROM golang:1.18

WORKDIR /app

# pre-copy/cache go.mod for pre-downloading dependencies and only redownloading them in subsequent builds if they change
COPY TCPServer/go.mod TCPServer/go.sum ./
RUN go mod download && go mod verify

# having some caching issues so don't cache the go file
ARG CACHEBUST=1
RUN echo "${CACHEBUST}"

# this pathing is necessary because I used Visual Studio to manage docker-compose, don't do that again
COPY TCPServer/*.go ./
RUN go build -o /flight-server

CMD [ "/flight-server" ]