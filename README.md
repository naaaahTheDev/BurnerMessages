# Burner Messages
A simple command-line messaging app written in C#. The server allows multiple clients to connect and chat with each other.

## Getting Started

### Prerequisites

- Make sure you have the .NET SDK installed on your machine.
- Download and install either `ClientSide` or `ServerSide` depending on your use from the [releases](https://github.com/naaaahTheDev/BurnerMessages/releases) page.

### Running the Server

1. Open the terminal.
2. Navigate to the `ServerSide` directory.
3. Run the `ServerSide.exe` file.
4. Follow the prompts to set up the server by providing a server name, IP address, and port.

### Running the Client

1. Open the terminal.
2. Navigate to your installed `ClientSide` directory.
3. Run the `ClientSide.exe` file.
4. Follow the prompts to set up the client by providing the server's IP address and port.

## Usage

### Server

1. Start the server by running `ServerSide.exe`.
2. Follow the prompts to provide a server name, IP address, and port.
3. The server will display messages when clients connect or disconnect, as well as the messages they send.

### Client

1. Start the client by running `ClientSide.exe`.
2. Follow the prompts to provide the server's IP address and port.
3. Enter a username to identify yourself in the chat.
4. You can now send messages to the server, and receive messages from other connected clients.
5. Type `/exit` to quit the client application.

## Features

- Multiple clients can connect to the server simultaneously.
- Clients can send and receive messages in real-time.
- Simple setup process for both the server and client.

## Troubleshooting

- If you encounter any errors while setting up the server or client, ensure you have entered valid inputs for IP addresses and port numbers.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
