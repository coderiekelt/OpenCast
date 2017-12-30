using System.CodeDom;
using System.Collections.Generic;

namespace OpenCast.Shoutcast
{
    public static class ShoutcastContext
    {
        public static uint LastAssignedListenerId;

        public static Dictionary<uint, ShoutcastListener> Listeners;

        public static void Initialize()
        {
            LastAssignedListenerId = 0;
            Listeners = new Dictionary<uint, ShoutcastListener>();
        }

        public static void Broadcast(byte[] bytes)
        {
            try
            {
                foreach (KeyValuePair<uint, ShoutcastListener> keyValuePair in Listeners)
                {
                    keyValuePair.Value.Write(bytes);
                }
            }
            catch
            {
                Broadcast(bytes);
            }
        }
    }
}