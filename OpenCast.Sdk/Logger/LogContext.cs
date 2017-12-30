using System;
using System.Collections.Generic;

namespace OpenCast.Sdk.Logger
{
    public static class LogContext
    {
        public static Dictionary<string, LogScope> Scopes;

        public static void LogToScope(string scope, string message)
        {
            if (Scopes == null)
            {
                Scopes = new Dictionary<string, LogScope>();
            }

            string compiled = String.Format("[{0}][{1}] {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                scope.ToUpper(), message);

            Console.WriteLine(compiled);

            if (!Scopes.ContainsKey(scope))
            {
                Scopes.Add(scope, new LogScope {Filename = scope});
            }

            Scopes[scope].LogToFile(compiled);
        }
    }
}