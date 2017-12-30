using System.Net;
using System.Net.Sockets;
using System.Threading;
using OpenCast.Sdk.EventArgs;

namespace OpenCast.Sdk.Network
{
    public delegate void ClientConnectedHandler(object sender, ClientConnectedEventArgs arguments);

    public class SimpleServer
    {
        public event ClientConnectedHandler OnClientConnected;

        private TcpListener _tcpListener;

        private string _bindAddress;

        private int _bindPort;

        private Thread _listenThread;

        public SimpleServer(string bindAddress, int bindPort)
        {
            this._bindAddress = bindAddress;
            this._bindPort = bindPort;

            this._tcpListener = new TcpListener(IPAddress.Parse(this._bindAddress), this._bindPort);

            this._listenThread = new Thread(this.Listen);
        }

        public void Start()
        {
            this._tcpListener.Start();
            this._listenThread.Start();
        }

        private void Listen()
        {
            while (true)
            {
                TcpClient tcpClient = this._tcpListener.AcceptTcpClient();

                Connection connection = new Connection(tcpClient);

                Thread clientThread = new Thread(() =>
                    {
                        this.OnClientConnected(this,
                            new ClientConnectedEventArgs {Server = this, Connection = connection});
                    });

                clientThread.Start();
            }
        }
    }
}