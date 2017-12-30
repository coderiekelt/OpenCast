using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace OpenCast.Sdk.Extension
{
    public class PluginManager
    {
        public PluginManager()
        {
            if (!Directory.Exists("plugins"))
            {
                Directory.CreateDirectory("plugins");
            }
        }

        public void LoadPlugins()
        {
            string[] files = Directory.GetFiles("plugins");

            foreach (string file in files)
            {
                Assembly dllAssembly = Assembly.LoadFile(Path.GetFullPath(file));

                foreach (Type type in dllAssembly.GetExportedTypes())
                {
                    if (type.BaseType.ToString() == "OpenCast.Sdk.Extension.Plugin")
                    {
                        Plugin plugin = (Plugin)Activator.CreateInstance(type);
                        
                        Thread pluginThread = new Thread(plugin.Initialize);
                        pluginThread.Start();
                    }
                }
            }
        }
    }
}