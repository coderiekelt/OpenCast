using System;
using System.CodeDom;
using System.Collections.Generic;

namespace OpenCast.Sdk.Shoutcast
{
    public static class ShoutcastContext
    {
        public static ShoutcastServer ShoutcastServer;

        public static uint LastAssignedListenerId;

        public static Dictionary<uint, ShoutcastListener> Listeners;

        public static void Initialize()
        {
            LastAssignedListenerId = 0;
            Listeners = new Dictionary<uint, ShoutcastListener>();
        }

        public static void Broadcast(byte[] bytes)
        {
            for (uint i = 1; i <= LastAssignedListenerId; i++)
            {
                if (!Listeners.ContainsKey(i))
                {
                    continue;
                }

                Listeners[i].Write(bytes);
            }
        }
    }
}