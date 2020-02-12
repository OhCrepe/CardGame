using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetworkConnection : MonoBehaviour
{

    private StreamReader reader;
    private StreamWriter writer;
    private TcpClient tcp;

    private const int port = 8888;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeRemoteIO();
    }

    private void InitializeRemoteIO(){

        tcp = new TcpClient("localhost", port);
        NetworkStream stream = tcp.GetStream();
        reader = new StreamReader(stream);
        writer = new StreamWriter(stream);
        writer.AutoFlush = true;

    }

    public void SendMessage(string message){
        Debug.Log(message);
        writer.WriteLine(message);
    }

}
