## Listening
When connecting to a Shoutcast socket you should await the OK message:  
`ICY 200 OK\r\n`  
  
If you receive this message, it's safe to assume that it will be followed up with the stream meta data: *comments do not work obviously*
```
Icy-MetaData: 0\r\n // Stream does not support metadata
icy-br: 0\r\n // Bitrate
icy-genre: Anime\r\n // Genre
icy-name: AnimeFm\r\n // Stream name
icy-url: http://turboweeb.com\r\n // Stream website
icy-pub: 0\r\n // Public? (does nothing)
icy-notice1: Notice one!\r\n // Notice 1
icy-notice2: Notice two!\r\n // Notice 2
```  
  
This should then be followed by a final `\r\n` before the actual raw audio data is sent. (this is important, Chrome can handle without it, but VLC won't)

## Broadcasting
Broadcasting to Shoutcast is a bit different, to start, you must sent the password followed by `\r\n` and await a response:  
`Password\r\n`  
  
Shoutcast will then either respond with either:
```
OK2\r\n
icy-caps:11\r\n
\r\n
```
Or anything else in the event of a bad password.  
  
After this, you are free to send your audio data!