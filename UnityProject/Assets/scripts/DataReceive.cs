using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class UDPReceive : MonoBehaviour
{

    Thread receiveThread;
    UdpClient client;
    public int port = 5052;
    public bool startRecieving = true;
    public bool printToConsole = false;
    public static string datareceived="-1";
    public static string prevData="-1";
    public static bool used1 = false;
    public static bool used2 = false;
    public void Start()
    {

        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }


    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(port);
        while (startRecieving)
        {

            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                prevData = datareceived;
                datareceived = Encoding.UTF8.GetString(dataByte);
                used1 = false;
                used2 = false;
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

}
