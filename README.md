# OpenCast
OpenCast is a free and open source implementation of the Shoutcast **v1** protocol.  
  
To start using OpenCast, launch it once, then modify `server.json` to your likings. Then start the server, and you should be able to enjoy your Shoutcast server!

## License
OpenCast is licensed under the MIT license, do with is as you please, credit is always appreciated (it took some time to figure out this protocol.)

## Cross Platform
OpenCast is mono compatible, this means that it will run fine on Windows, Linux and Mac OS.

## Supported Clients
OpenCast was tested on the following clients:  
- Google Chrome (<audio>)  
- VLC media player

## Supported DSPs
Waive has currently only been tested using `PlayIt live`, however any Shoutcast **v1** source should work.  
**IceCast / Shoutcast v2 clients do not work.**  
  
Clients must send their data in chunks of maximum **4096 bytes** in order for the payload to be successfully redirect to all clients.