using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Node
{
    public short headId;
    public List<object> list;
}


public class TCPClick : MonoBehaviour
{
    private static TCPClick instance;
    public static TCPClick Instance => instance;
    public int user_ID;
    public Socket socket;
    public ConcurrentQueue<Node> sendQueue = new ConcurrentQueue<Node>();
    public ConcurrentQueue<(int, byte[])> receiveQueue = new ConcurrentQueue<(int, byte[])>();
    public byte[] bytes, by;
    public int nowPos;
    private int rpos, rlen;
    public Node Id;
    public byte[] sendBy = new byte[1024 * 1024];
    private bool isConnect;
    private CancellationTokenSource cts;
    private float time;
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        instance = this;
        bytes = new byte[1024 * 1024];
        by = new byte[1024 * 1024];
    }

    private void Start()
    {
        BackPackManager.Instance.Init();
        cts = new CancellationTokenSource();
        //Connect("192.168.202.133", 8000);
        Connect("47.97.126.163", 8000);
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= 5f)
        {
            time -= 5;
            SendMessage(0, null);
        }
        if (receiveQueue.Count > 0)
            {
                (int a1, byte[] a2) a;
                receiveQueue.TryDequeue(out a);
                switch (a.a1)
                {
                    case 1:
                        PlayerMessage1 aa = PlayerMessage1.Get(a.a2);
                        EventSystemManager.Instance.Publish(a.a1, aa);
                        Debug.Log("收到玩家消息");
                    // print("sex: " + aa.sex);
                    // print("name: " + aa.name);
                    // print("hp: " + aa.hp);
                    // print("atk: " + aa.atk);
                    // print("pier: " + aa.piercing);
                    // print("x: " + aa.x);
                    // print("y: " + aa.y);
                    // print("z: " + aa.z);
                    // print("scene: " + aa.scene_id);
                    break;
                    case 2:
                        BackPackMessage2 backPackMessage2 = BackPackMessage2.Get(a.a2);
                        EventSystemManager.Instance.Publish(a.a1, backPackMessage2);
                        Debug.Log("收到背包消息");
                        //print("锟斤拷锟斤拷:" + backPackMessage2.len);
                        //print("1: " + backPackMessage2.item_id[0]);
                        //print("2: " + backPackMessage2.item_type[0]);
                        //print("3: " + backPackMessage2.item_num[0]);

                        break;
                    case 3:
                        TaskMessage3 taskMessage3 = TaskMessage3.Get(a.a2);
                        EventSystemManager.Instance.Publish(a.a1, taskMessage3);
                        //print("锟斤拷锟斤拷:" + taskMessage3.len);
                        //print("1: " + taskMessage3.task_id[0]);
                        //print("2: " + taskMessage3.task_type[0]);
                        //print("3: " + taskMessage3.require_num[0]);
                        break;
                    case 4:
                        RegisterLoginMessage56 registerLoginMessage4 = RegisterLoginMessage56.Get(a.a2);
                        EventSystemManager.Instance.Publish(a.a1, registerLoginMessage4);
                        break;
                    case 5:
                        RegisterLoginMessage56 registerLoginMessage5 = RegisterLoginMessage56.Get(a.a2);
                        EventSystemManager.Instance.Publish(a.a1, registerLoginMessage5);
                        break;
                    case 11:
                        GameObjectMsg11 gameObjectMsg11 = GameObjectMsg11.Get(a.a2);
                        EventSystemManager.Instance.Publish(a.a1, gameObjectMsg11);
                        break;
                }
            }
    }

    public void SendMessage(short headId,List<object> list)
    {
        Node a = new Node();
        a.headId = headId;
        a.list = list;
        sendQueue.Enqueue(a);
    }

    private void Connect(string ip,int port)
    {
        if (socket == null)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        try
        {
            socket.Connect(ip, port);
            isConnect = true;
            Task.Run(() => Send(cts.Token));
            Task.Run(() => Receive(cts.Token));
        }
        catch (SocketException e)
        {
            print("连接失败\n" + e.Message);
        }
    }
    private byte[] Change2(byte[] b, int start, int len, int id)
    {
        int nowPos = 0;
        byte[] tmp = new byte[len];
        Array.Copy(b, start, tmp, 0, len);
        if (id == 1)
        {
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            nowPos += 56;
            for (int i = 0; i < 7; i++)
            {
                Array.Reverse(tmp, nowPos + i * 4, 4);
            }
        }
        else if (id == 2)
        {
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Array.Reverse(tmp, nowPos, 4);
                    nowPos += 4;
                }
            }
        }
        else if (id == 3)
        {
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    Array.Reverse(tmp, nowPos, 4);
                    nowPos += 4;
                }
            }
        }
        else if (id == 4 || id == 5)
        {
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
        }
        else if (id == 11)
        {
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            nowPos += 56;
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
            Array.Reverse(tmp, nowPos, 4);
            nowPos += 4;
        }
        return tmp;
    }
    private byte[] Change(byte[] b, int start, int len)
    {
        byte[] tmp = new byte[len];
        Array.Copy(b, start, tmp, 0, len);
        Array.Reverse(tmp);
        return tmp;
    }
    public void Receive(CancellationToken token)
    {
        
        while (!token.IsCancellationRequested && isConnect)
        {
            try
            {
                while (nowPos <= bytes.Length && !token.IsCancellationRequested && isConnect)
                {
                    int sum = socket.Receive(bytes, nowPos, bytes.Length - nowPos, SocketFlags.None);
                    nowPos += sum;
                    if (nowPos >= 2)
                    {
                        if (rlen == 0)
                        {
                            rlen = BitConverter.ToInt16(Change(bytes, rpos, 2));
                            rpos += 2;
                            
                        }
                        while (nowPos < rlen)
                        {
                            sum = socket.Receive(bytes, nowPos, bytes.Length - nowPos, SocketFlags.None);
                            nowPos += sum;
                        }
                        (int a1, byte[] a2) a;
                        a.a1 = BitConverter.ToInt16(Change(bytes, rpos, 2));
                        rpos += 2;
                        int crc = BitConverter.ToInt32(Change(bytes, rpos, 4));
                        rpos += 4;
                        byte[] tmp = new byte[rlen - 8];
                        Array.Copy(bytes, rpos, tmp, 0, rlen - 8);
                        if (CRC32.Instance.GetCrc(tmp, rlen - 8) != crc)
                        {
                            Debug.Log("CRC校验失败");
                            if (nowPos > rlen)
                            {
                                Array.Copy(bytes, rpos, bytes, 0, nowPos - rpos);
                                nowPos -= rlen;
                            }
                            else nowPos = 0;
                            rpos = 0;
                            rlen = 0;
                            continue;
                        }
                        a.a2 = new byte[rlen - 8];
                        tmp = Change2(tmp, 0, rlen - 8, a.a1);
                        Array.Copy(tmp, 0, a.a2, 0, rlen - 8);
                        receiveQueue.Enqueue(a);
                        rpos += rlen - 8;
                        if (nowPos > rlen)
                        {
                            Array.Copy(bytes, rpos, bytes, 0, nowPos - rpos);
                            nowPos -= rlen;
                        }
                        else nowPos = 0;
                        rpos = 0;
                        rlen = 0;
                    }
                }
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10060)
                    continue;
            }
            catch (Exception s)
            {
                Debug.LogException(s);
                isConnect = false;
                break;
            }

        }
    }
    public void Send(CancellationToken token)
    {
        while (!token.IsCancellationRequested && isConnect)
        {
            if (sendQueue.Count > 0)
            {
                print("取出消息");
                sendQueue.TryDequeue(out Id);
                int len = 0;
                if (Id.list != null)
                {
                    foreach (var i in Id.list)
                    {
                        if (i is int)
                        {
                            byte[] bz = BitConverter.GetBytes((int)i);
                            Array.Reverse(bz);
                            bz.CopyTo(sendBy, len);
                            len += 4;
                        }
                        else if (i is char[])
                        {
                            char[] a = i as char[];
                            string a1 = new string(a);
                            byte[] bzz = Encoding.UTF8.GetBytes(a1);
                            bzz.CopyTo(sendBy, len);
                            len += bzz.Length;
                        }
                        else if (i is float)
                        {
                            byte[] bzzz = BitConverter.GetBytes((float)i);
                            Array.Reverse(bzzz);
                            bzzz.CopyTo(sendBy, len);
                            len += 4;
                        }
                        else if (i is byte[])
                        {
                            byte[] zz = i as byte[];
                            zz.CopyTo(sendBy, len);
                            len += zz.Length;
                        }
                        else if (i is int[])
                        {
                            int[] ints = i as int[];
                            byte[] bb = new byte[ints.Length * 4];
                            for (int j = 0; j < ints.Length; j++)
                            {
                                byte[] tp = BitConverter.GetBytes(ints[j]);
                                Array.Reverse(tp);
                                tp.CopyTo(bb, j * 4);
                            }
                            bb.CopyTo(sendBy, len);
                            len += bb.Length;
                        }

                    }
                }
                try
                    {
                        int crc32 = CRC32.Instance.GetCrc(sendBy, len);
                        byte[] b = BitConverter.GetBytes((short)(len + 8));
                        Array.Reverse(b);
                        b.CopyTo(by, 0);
                        byte[] bb = BitConverter.GetBytes(Id.headId);
                        Array.Reverse(bb);
                        bb.CopyTo(by, 2);
                        byte[] bbb = BitConverter.GetBytes(crc32);
                        Array.Reverse(bbb);
                        bbb.CopyTo(by, 4);
                        Array.Copy(sendBy, 0, by, 8, len);
                        socket.Send(by, 0, 8 + len, SocketFlags.None);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }


            }
        }
    }

    private void ForceQuit()
    {
        isConnect = false;
        if (cts != null)
        {
            cts.Cancel(); 
            cts.Dispose();
            cts = null;
        }
        if (socket != null)
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both); 
                socket.Close(1); 
                socket.Dispose();
            }
            catch (Exception ex)
            {
                Debug.Log($"锟截憋拷Socket锟斤拷锟斤拷{ex.Message}");
            }
            socket = null;
        }
        Debug.Log("锟斤拷强锟斤拷锟剿筹拷锟斤拷锟斤拷");
    }
#if UNITY_EDITOR
    private void OnDestroy()
    {
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            ForceQuit();
        }
    }
#endif

    //public void Receive()
    //{
    //    while (socket.Connected)
    //    {
    //        int sum = socket.Receive(bytes, nowPos, bytes.Length - nowPos, SocketFlags.None);
    //        nowPos += sum;
    //        if (nowPos >= 4)
    //        {
    //            if (rlen == 0)
    //            {
    //                rlen = BitConverter.ToInt16(bytes, 0);
    //                rpos += 2;
    //            }
    //            if (nowPos < rlen) continue;
    //            rid = BitConverter.ToInt16(bytes, rpos);
    //            rpos += 2;
    //            rcrc = BitConverter.ToInt32(bytes, rpos);
    //            rpos += 4;
    //            (int a1, byte[] a2) a;
    //            a.a1 = rid;
    //            a.a2 = new byte[rlen - 12];
    //            Array.Copy(bytes, rpos, a.a2, 0, rlen - 12);
    //            receiveQueue.Enqueue(a);
    //            rpos += rlen - 12;
    //            if (nowPos > rlen)//锟斤拷锟斤拷
    //            {
    //                Array.Copy(bytes, rpos, bytes, 0, nowPos - rpos);
    //                nowPos -= rlen;
    //            }
    //            rpos = 0;
    //            rlen = 0;
    //        }

    //    }

    //}



    //public void Send()
    //{
    //    while(socket.Connected)
    //    {
    //        if (sendQueue.Count > 0)
    //        {
    //            sendQueue.TryDequeue(out imsg);
    //            byte[] b = imsg.ToByteArray();
    //            int len = b.Length;
    //            int crc32 = CRC32.Instance.CalculateCRC(b);
    //            BitConverter.GetBytes(len + 12).CopyTo(by, 0);
    //            BitConverter.GetBytes(crc32).CopyTo(by, 4);
    //            string name = imsg.Descriptor.Name;
    //            int id = 0;
    //            switch (name)
    //            {
    //                case "TestMsg":
    //                    id = 1;
    //                    break;
    //            }
    //            BitConverter.GetBytes(id).CopyTo(by, 8);
    //            b.CopyTo(by, 12);
    //            socket.Send(by, 0, 12 + len, SocketFlags.None);
    //        }
    //    }
    //}
}
