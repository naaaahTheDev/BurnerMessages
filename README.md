# BurnerMessages
 A simple command-line messaging app that has maximum privacy as messages don't save. Written in C#.

# How to Use

## 1. First Download the Code
- Download the code as a zip file and unzip it.

## 2. Set Server IP Address
- Go into the `ServerSide` Folder.
- Once inside, find the variable `ipAd` and change it to **YOUR SERVER IP**:
```csharp
IPAddress ipAd = IPAddress.Parse("INPUT_IP_HERE");
```
- Example:
```csharp
IPAddress ipAd = IPAddress.Parse("0.0.0.0");
```

## 3. Set Client Connection IP
- In order for your client to connect to the server, you must pass in the IP of the server you wish to connect to.
- Go into the `ClientSide` Folder.
- Once inside, go to the following line of code and change the IP to the server IP:
```csharp
tcpClient.Connect("INPUT_IP_HERE", 5000);
```

## 4. Launch Server & Client:
- Once done setting up all IP configuration, you must launch the server as well as clients that will connect (note that clients can connect at any time, does not have to be in the start)
  ### - Server
  - Navigate to `ServerSide/Bin/Release` and run `ServerSide.exe`
  ### - Client
  - Navigate to `ClientSide/Bin/Release` and run `ClientSide.exe`


## 5. Extra Info:
- The default port is port 5000, if you wish to change the port you may. It is the same process as changing the IP.
