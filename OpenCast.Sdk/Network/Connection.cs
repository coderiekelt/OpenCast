using System;
using System.Net.Sockets;
using System.Text;
using OpenCast.Sdk.EventArgs;

namespace OpenCast.Sdk.Network
{
    public delegate void ClientDisconnectedHandler(object sender, ClientDisconnectedEventArgs arguments);

    public class Connection
    {
        public event ClientDisconnectedHandler OnClientDisconnected;

        private TcpClient _tcpClient;

        private NetworkStream _networkStream;

        private bool _disconnected;

        public bool Dropped;

        public Connection(TcpClient tcpClient)
        {
            this._disconnected = false;
            this.Dropped = false;

            this._tcpClient = tcpClient;
            this._networkStream = this._tcpClient.GetStream();
        }

        public void WriteString(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);

            this.Write(bytes);
        }

        public void Write(byte[] bytes)
        {
            if (this._disconnected)
            {
                return;
            }

            try
            {
                this._networkStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception exception)
            {
                this._disconnected = true;

                if (OnClientDisconnected != null)
                { 
                    OnClientDisconnected(this, new ClientDisconnectedEventArgs());
                }
            }
        }

        public byte[] Read(int offset, int length)
        {
            if (this._disconnected)
            {
                return new byte[1];
            }

            try
            {
                byte[] buffer = new byte[length];

                this._networkStream.Read(buffer, offset, buffer.Length);

                return buffer;
            }
            catch (Exception exception)
            {
                this._disconnected = true;

                if (OnClientDisconnected != null)
                {
                    OnClientDisconnected(this, new ClientDisconnectedEventArgs());
                }

                return new byte[1];
            }
        }

        public void Drop()
        {
            this.Dropped = true;

            try
            {
                this._networkStream.Close();
                this._tcpClient.Close();
            }
            finally
            {
                this._disconnected = true;

                if (OnClientDisconnected != null)
                {
                    OnClientDisconnected(this, new ClientDisconnectedEventArgs());
                }
            }
        }
    }
}