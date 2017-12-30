using System;
using System.IO;

namespace OpenCast.Sdk.Logger
{
    public class LogScope
    {
        public string Filename;

        public void LogToFile(string message)
        {
            if (!Directory.Exists("logs"))
            {
                Directory.CreateDirectory("logs");
            }

            File.AppendAllText(String.Format("logs/{0}.log", this.Filename), message + Environment.NewLine);
        }
    }
}