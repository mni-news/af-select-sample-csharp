# AlphaFlash Select C# Sample

[[_TOC_]]

This is a sample C# client for AlphaFlash Select. 


>
> ### Using This Code
>
> This code is meant to provide a demonstration for connecting to the AlphaFlash Select service. It is provided
> "as-is".
>

## Dependencies

This project uses NuGet to manage third-party dependencies. 

| Name				            | Description			| Links
| --				            | --					| --
| Stomp.Net			            | STOMP Library			| [NuGet](https://www.nuget.org/packages/Stomp.Net/)
| System.Text.Json              | JSON Library          | [NuGet](https://www.nuget.org/packages/System.Text.Json/)
| System.Net.Http.Json			| JSON Http Extensions	| [NuGet](https://www.nuget.org/packages/System.Net.Http.Json)

## Code

### Entry Point

The main entry point for this samples is in [Program.cs](src/AlphaFlash.Select/Program.cs). This will authenticate, make 
a few REST api requests, and start a STOMP listener. 

### [Services Namespace (AlphaFlash.Select.Service)](src/AlphaFlash.Select/Service)

This contains service implementations that correspond to parts of the Select API

#### [Authentication Service](src/AlphaFlash.Select/Service/AuthenticationService.cs)

This service handles interactions with the [Authentication API](https://git.alphaflash.com/select/af-select-documentation/-/blob/master/AUTH-API.md#using-the-authentication-api). It allows for the creation and refresh of access tokens.

#### [Select Data Service (REST API)](src/AlphaFlash.Select/Service/SelectDataService.cs)

This services handles calls to the [Select REST API](https://git.alphaflash.com/select/af-select-documentation/-/blob/master/SELECT-API.md#alpha-flash-select-rest-api).

#### [Select Real Time Service (STOMP API)](/home/will/git/af-select-sample-csharp/src/AlphaFlash.Select/Service/SelectRealTimeDataService.cs)

This handles connections to the [Select Real Time API](https://git.alphaflash.com/select/af-select-documentation/-/blob/master/SELECT-RT-API.md#alpha-flash-select-real-time-api)

Two implementations are provided: one using [Stomp.Net](https://github.com/DaveSenn/Stomp.Net) (a third party STOMP library), and 
a ground-up implementation using plain TCP sockets.

##### [Stomp.Net implementation](src/AlphaFlash.Select/Service/StompNetRealTimeDataService.cs)

This provides a STOMP service based on [Stomp.Net](https://www.nuget.org/packages/Stomp.Net/).

##### [Ground Up implementation](src/AlphaFlash.Select/Service/SimpleSelectRealTimeDataService.cs)

This provides a STOMP implementation with no external dependencies. Additional code to support this implementation can 
be found in the [AlphaFlash.Select.Stomp](src/AlphaFlash.Select/Stomp) namespace.

### [DTO Namespace (AlphaFlash.Select.Dto)](src/AlphaFlash.Select/Dto)

This contains DTO and model objects that are used to serialize/deserialze API content. 

### [STOMP Namespace (AlphaFlash.Select.Stomp)](src/AlphaFlash.Select/Stomp)

This provides a grounds-up stomp implementation, using the .NET TcpClient. 