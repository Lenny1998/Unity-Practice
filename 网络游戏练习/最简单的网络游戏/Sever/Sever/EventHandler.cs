using System;
using System.Collections.Generic;
using System.Text;

namespace Sever
{
    class EventHandler
    {
        public static void OnDisconnect(ClientState c)
        {
            Console.WriteLine("OnDisconnect");
        }
    }
}
