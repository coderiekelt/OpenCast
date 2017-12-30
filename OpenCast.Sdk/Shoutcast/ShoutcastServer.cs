using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCast.Sdk.EventArgs;
using OpenCast.Sdk.Logger;
using OpenCast.Sdk.Network;

namespace OpenCast.Sdk.Shoutcast
{
    public class ShoutcastServer
    {
        // General Meta
        public string Name { get; set; }

        public string Genre { get; set; }

        public string Url { get; set; }

        public bool Public { get; set; }

        public string NoticeOne { get; set; }

        public string NoticeTwo { get; set; }

        public string Password { get; set; }

        // Technical Info
        public string BindAddress { get; set; }

        public int BindPort { get; set; }

        public int BitRate { get; set; }

        // Code stuff below
        private SimpleServer _listenServer;

        private SimpleServer _controlServer;

        private Dictionary<string, string> _icyHeaders;

        public ShoutcastServer()
        {
            if (this.BindAddress == null || this.BindPort == null)
            {
                return;
            }

            this._listenServer = new SimpleServer(this.BindAddress, this.BindPort);
            this._controlServer = new SimpleServer(this.BindAddress, this.BindPort + 1);
        }

        public void Start()
        {
            if (this._listenServer == null && this.BindAddress != null)
            {
                this._listenServer = new SimpleServer(this.BindAddress, this.BindPort);
                this._controlServer = new SimpleServer(this.BindAddress, this.BindPort + 1);
            }

            this._listenServer.OnClientConnected += this.OnListenerConnected;
            this._controlServer.OnClientConnected += this.OnControllerConnected;

            this._listenServer.Start();
            this._controlServer.Start();

            LogContext.LogToScope("info", String.Format("Server started on {0}:{1} ({0}:{2} for control)", this.BindAddress, this.BindPort, this.BindPort + 1));
        }

        private void ValidateControllerPassword(Connection connection)
        {
            byte[] buffer = new byte[64];

            buffer = connection.Read(0, buffer.Length);

            buffer = buffer.TakeWhile((v, index) => buffer.Skip(index).Any(w => w != 0x00)).ToArray(); // Opbokken null bytes

            string password = (Encoding.ASCII.GetString(buffer)).Replace("\r\n", String.Empty);

            if (password != this.Password)
            {
                connection.WriteString("invalid password");
                connection.Drop();
            }
        }

        private void OnControllerConnected(object sender, ClientConnectedEventArgs args)
        {
            LogContext.LogToScope("info", "Client accepted (control)");

            ShoutcastController controller = new ShoutcastController(args.Connection);
            this.ValidateControllerPassword(args.Connection);

            controller.Broadcast();
        }

        private void WelcomeListener(ShoutcastListener listener)
        {
            listener.WriteString("ICY 200 OK\r\n");

            this._icyHeaders = new Dictionary<string, string>();

            this._icyHeaders.Add("Icy-MetaData", "0");
            this._icyHeaders.Add("icy-br", this.BitRate.ToString());
            this._icyHeaders.Add("icy-genre", this.Genre);
            this._icyHeaders.Add("icy-name", this.Name);
            this._icyHeaders.Add("icy-url", this.Url);
            this._icyHeaders.Add("icy-pub", this.Public ? "1" : "0");
            this._icyHeaders.Add("icy-notice1", this.NoticeOne);
            this._icyHeaders.Add("icy-notice2", this.NoticeTwo);

            foreach (KeyValuePair<string, string> keyValuePair in this._icyHeaders)
            {
                listener.WriteString(String.Format("{0}: {1}\r\n", keyValuePair.Key, keyValuePair.Value));
            }

            listener.WriteString("\r\n");
        }

        private void OnListenerConnected(object sender, ClientConnectedEventArgs args)
        {
            LogContext.LogToScope("info", "Client accepted (listener)");

            ShoutcastListener listener = new ShoutcastListener(args.Connection);
            this.WelcomeListener(listener);
        }
    }
}