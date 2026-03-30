using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

[System.Serializable]
public struct PointData {
    public float x, y, z, dist;
}

public class UDPReceiver : MonoBehaviour {
    public int port = 5005;
    public PointCloudRenderer renderer;

    UdpClient client;
    Thread receiveThread;
    readonly Queue<PointData> queue = new Queue<PointData>();
    readonly object lockObj = new object();

    void Start() {
        client = new UdpClient(port);
        receiveThread = new Thread(Receive) { IsBackground = true };
        receiveThread.Start();
    }

    void Receive() {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);
        while (true) {
            try {
                byte[] data = client.Receive(ref ep);
                string json = Encoding.UTF8.GetString(data);
                PointData pt = JsonUtility.FromJson<PointData>(json);
                lock (lockObj) { queue.Enqueue(pt); }
            } catch { }
        }
    }

    void Update() {
        lock (lockObj) {
            while (queue.Count > 0)
                renderer.AddPoint(queue.Dequeue());
        }
    }

    void OnDestroy() {
        receiveThread?.Abort();
        client?.Close();
    }
}