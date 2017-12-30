using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenCast.Sdk.Extension;
using OpenCast.Sdk.Logger;
using OpenCast.Sdk.Shoutcast;

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

                LogContext.LogToScope("info", "No configuration found, created default configuration in server.json.");

                return;
            }

            LogContext.LogToScope("info", "Loading server configuration...");

            shoutcastServer = JsonConvert.DeserializeObject<ShoutcastServer>(File.ReadAllText("server.json"));

            ShoutcastContext.ShoutcastServer = shoutcastServer;

            PluginManager pluginManager = new PluginManager();
            pluginManager.LoadPlugins();

            shoutcastServer.Start();
        }
    }
}
