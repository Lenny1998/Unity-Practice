using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Sever
{
    class ClientState
    {
        public Socket socket;
        public byte[] readBuff = new byte[1024];
        public int hp = -100;
        public float x = 0;
        public float y = 0;
        public float z = 0;
        public float eulY = 0;
    }

    class MainClass
    {
        /// <summary>
        /// 监听Socket
        /// </summary>
        private static Socket listenfd;

        /// <summary>
        /// 客户端Socket及状态信息
        /// </summary>
        public static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();

        static void Main(string[] args)
        {
            //Socket
            listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Bind
            IPAddress ipAdr = IPAddress.Parse("192.168.1.104");
            IPEndPoint ipEp = new IPEndPoint(ipAdr, 8888);
            listenfd.Bind(ipEp);

            //Listen
            listenfd.Listen(0);
            Console.WriteLine("[服务器]启动成功");

            //checkRead
            List<Socket> checkRead = new List<Socket>();

            while (true)
            {
                //填充checkRead列表
                checkRead.Clear();
                checkRead.Add(listenfd);
                foreach (ClientState s in clients.Values)
                {
                    checkRead.Add(s.socket);
                }

                //select
                Socket.Select(checkRead, null, null, 1000);

                //检查可读对象
                foreach (Socket s in checkRead)
                {
                    if (s == listenfd)
                    {
                        ReadListenfd(s);
                    }
                    else
                    {
                        ReadClientfd(s);
                    }
                }
            }
        }

        /// <summary>
        /// 读取Listenfd
        /// </summary>
        /// <param name="listenfd"></param>
        public static void ReadListenfd(Socket listenfd)
        {
            Console.WriteLine("Accept");
            Socket clientfd = listenfd.Accept();
            ClientState state = new ClientState();
            state.socket = clientfd;
            clients.Add(clientfd, state);
        }

        /// <summary>
        /// 读取Clientfd
        /// </summary>
        /// <param name="s"></param>
        public static bool ReadClientfd(Socket clientfd)
        {
            ClientState state = clients[clientfd];

            //接受
            int count = 0;
            try
            {
                count = clientfd.Receive(state.readBuff);
            }
            catch (SocketException ex)
            {
                MethodInfo mei = typeof(EventHandler).GetMethod("OnDisconnect");
                object[] ob = { state };
                mei.Invoke(null, ob);

                clientfd.Close();
                clients.Remove(clientfd);
                Console.WriteLine("Receive SocketException " + ex.ToString());
                return false;
            }

            if (count <= 0)
            {
                MethodInfo mei = typeof(EventHandler).GetMethod("OnDisconnect");
                object[] ob = { state };
                mei.Invoke(null, ob);

                clientfd.Close();
                clients.Remove(clientfd);
                Console.WriteLine("Socket Close");
                return false;
            }

            //消息处理
            string recvStr = System.Text.Encoding.Default.GetString(state.readBuff, 0, count);
            string[] split = recvStr.Split('|');
            Console.WriteLine("Receive: " + recvStr);
            string msgName = split[0];
            string msgArgs = split[1];
            string funName = "Msg" + msgName;
            MethodInfo mi = typeof(MsgHandler).GetMethod(funName);
            object[] o = { state, msgArgs };
            mi.Invoke(null, o);
            string sendStr = recvStr;

            return true;
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="cs"></param>
        /// <param name="sendStr"></param>
        public static void Send(ClientState cs, string sendStr)
        {
            byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
            cs.socket.Send(sendBytes);
        }
    }
}
