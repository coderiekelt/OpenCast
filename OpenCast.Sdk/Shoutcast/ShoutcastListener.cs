using System;
using OpenCast.Sdk.EventArgs;
using OpenCast.Sdk.Logger;
using OpenCast.Sdk.Network;

namespace OpenCast.Sdk.Shoutcast
{
    public class ShoutcastListener
    {
        public uint ListenerId;

        private Connection _connection;

        public ShoutcastListener(Connection connection)
        {
            ShoutcastContext.LastAssignedListenerId++;
            this.ListenerId = ShoutcastContext.LastAssignedListenerId;

            ShoutcastContext.Listeners.Add(this.ListenerId, this);

            this._connection = connection;

            this._connection.OnClientDisconnected += this.OnDisconnected;
        }

        public void WriteString(string message)
        {
            this._connection.WriteString(message);
        }

        public void Write(byte[] bytes)
        {
            this._connection.Write(bytes);
        }

        public byte[] Read(int offset, int length)
        {
            return this._connection.Read(offset, length);
        }

        private void OnDisconnected(object sender, ClientDisconnectedEventArgs args)
        {
            LogContext.LogToScope("info", String.Format("Listener {0} dropped", this.ListenerId));

            ShoutcastContext.Listeners.Remove(this.ListenerId); // GC will (probably) do the rest
        }
    }
}