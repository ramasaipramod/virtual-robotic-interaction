using System.Net.Sockets;
using UnityEngine;
using System.Text;

public class FlaskUnityConnector : MonoBehaviour
{
    private TcpListener server;
    private const int PORT = 12345;
    private const string IP_ADDRESS = "127.0.0.1";
    public static int TransformedNumber { get; private set; }
    public static bool used = false;

    void Start()
    {
        // Start the TCP listener
        server = new TcpListener(System.Net.IPAddress.Parse(IP_ADDRESS), PORT);
        server.Start();
        Debug.Log("Waiting for connection...");
        StartListening();
    }

    void StartListening()
    {
        // Asynchronously wait for a client connection
        server.BeginAcceptTcpClient(new System.AsyncCallback(OnClientConnect), server);
    }

    void OnClientConnect(System.IAsyncResult ar)
    {
        TcpClient client = server.EndAcceptTcpClient(ar);
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        // Keep listening for data from Flask
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            int transformedNumber;
            if (int.TryParse(dataReceived, out transformedNumber))
            {
                Debug.Log("Received Transformed Number: " + transformedNumber);
                TransformedNumber = transformedNumber;
                used = false;
            }
        }

        // After finishing with the current client, start listening for new clients again
        StartListening();
    }

    void OnApplicationQuit()
    {
        server.Stop();
    }
}