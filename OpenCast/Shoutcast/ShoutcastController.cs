using OpenCast.Network;

namespace OpenCast.Shoutcast
{
    public class ShoutcastController
    {
        private Connection _connection;

        public ShoutcastController(Connection connection)
        {
            this._connection = connection;
        }

        public void Broadcast()
        {
            this._connection.WriteString("OK2\r\nicy-caps:11\r\n\r\n");

            // Fetch bytes, and broadcast them
            byte[] buffer;
            int bytesReceived;
            while ((bytesReceived = (buffer = this._connection.Read(0, 4096)).Length) > 0)
            {
                ShoutcastContext.Broadcast(buffer);
            }
        }
    }
}