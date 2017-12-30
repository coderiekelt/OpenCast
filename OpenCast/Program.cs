using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenCast.Shoutcast;

namespace OpenCast
{
    class Program
    {
        static void Main(string[] args)
        {
            ShoutcastContext.Initialize();

            ShoutcastServer shoutcastServer;

            if (!File.Exists("server.json"))
            {
                shoutcastServer = new ShoutcastServer();
                File.WriteAllText("server.json", JsonConvert.SerializeObject(shoutcastServer));

                Console.WriteLine("[INFO] No configuration found, created default configuration in server.json.");

                return;
            }

            Console.WriteLine("[INFO] Loading server configuration...");

            shoutcastServer = JsonConvert.DeserializeObject<ShoutcastServer>(File.ReadAllText("server.json"));
            shoutcastServer.Start();
        }
    }
}
