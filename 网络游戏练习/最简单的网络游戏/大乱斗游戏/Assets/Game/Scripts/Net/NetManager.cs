using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public static class NetManager
{

    /// <summary>
    /// 定义套接字
    /// </summary>
    private static Socket socket;

    /// <summary>
    /// 接受缓冲区
    /// </summary>
    private static byte[] readBuff = new byte[1024];

    /// <summary>
    /// 委托类型
    /// </summary>
    /// <param name="str"></param>
    public delegate void MsgListener(string str);

    /// <summary>
    /// 监听列表
    /// </summary>
    private static Dictionary<string, MsgListener> listeners = new Dictionary<string, MsgListener>();

    /// <summary>
    /// 消息列表
    /// </summary>
    private static List<string> msgList = new List<string>();

    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="msgName"></param>
    /// <param name="listener"></param>
    public static void AddListener(string msgName, MsgListener listener)
    {
        listeners[msgName] = listener;
    }

    /// <summary>
    /// 获取描述
    /// </summary>
    /// <returns></returns>
    public static string GetDesc()
    {
        if (socket == null)
        {
            return "";
        }

        if (!socket.Connected)
        {
            return "";
        }

        return socket.LocalEndPoint.ToString();
    }

    /// <summary>
    /// 链接
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public static void Connect(string ip, int port)
    {
        //Socket
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //Connect(用同步方式简化代码)
        socket.Connect(ip, port);

        //BeginReceive
        socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallBack, socket);
    }

    /// <summary>
    /// Receive 回调
    /// </summary>
    /// <param name="ar"></param>
    private static void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndReceive(ar);
            string recvStr = System.Text.Encoding.Default.GetString(readBuff, 0, count);
            msgList.Add(recvStr);
            socket.BeginReceive(readBuff, 0, 1024, 0, ReceiveCallBack, socket);
        }
        catch (SocketException ex)
        {
            Debug.LogError("Socket Receive fail" + ex.ToString());            
        }
    }

    /// <summary>
    /// 发送
    /// </summary>
    /// <param name="sendStr"></param>
    public static void Send(string sendStr)
    {
        Debug.Log(sendStr);
        if (socket == null)
        {
            return;
        }

        if (!socket.Connected)
        {
            return;
        }

        byte[] sendBytes = System.Text.Encoding.Default.GetBytes(sendStr);
        socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socket);
    }

    //Send回调
    private static void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            //int count = socket.EndSend(ar);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Send fail" + ex.ToString());
        }
    }

    /// <summary>
    /// Update
    /// </summary>
    public static void Update()
    {
        if (msgList.Count <= 0)
        {
            return;
        }

        String msgStr = msgList[0];
        msgList.RemoveAt(0);
        string[] split = msgStr.Split('|');
        string msgName = split[0];
        string msgArgs = split[1];

        //监听回调
        if (listeners.ContainsKey(msgName))
        {
            listeners[msgName](msgArgs);
        }
    }
}
