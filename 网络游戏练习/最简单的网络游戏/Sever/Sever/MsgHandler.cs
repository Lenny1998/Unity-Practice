using System;
using System.Collections.Generic;
using System.Text;

namespace Sever
{
    class MsgHandler
    {
        public static void MsgEnter(ClientState c, string msgArgs)
        {
            //解析参数
            string[] split = msgArgs.Split(',');
            string desc = split[0];
            float x = float.Parse(split[1]);
            float y = float.Parse(split[2]);
            float z = float.Parse(split[3]);
            float eulY = float.Parse(split[4]);

            //赋值
            c.hp = 100;
            c.x = x;
            c.y = y;
            c.z = z;
            c.eulY = eulY;

            //广播
            string sendStr = "Enter|" + msgArgs;

            foreach (ClientState cs in MainClass.clients.Values)
            {
                MainClass.Send(cs, sendStr);
            }

            Console.WriteLine("MsgEnter" + msgArgs);
        }

        public static void MsgList(ClientState c, string msgArgs)
        {
            Console.WriteLine("MsgList" + msgArgs);

            string sendStr = "List|";
            foreach (ClientState cs in MainClass.clients.Values)
            {
                sendStr += cs.socket.RemoteEndPoint.ToString() + ",";
                sendStr += cs.x.ToString() + ",";
                sendStr += cs.y.ToString() + ",";
                sendStr += cs.z.ToString() + ",";
                sendStr += cs.eulY.ToString() + ",";
                sendStr += cs.hp.ToString() + ",";
            }
            Console.WriteLine(sendStr);
            MainClass.Send(c, sendStr);
        }
    }
}
