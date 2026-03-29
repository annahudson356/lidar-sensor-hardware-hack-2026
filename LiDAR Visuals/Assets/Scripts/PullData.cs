using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class PullData : MonoBehaviour {
    public static float CurrentAngle;
    public static float CurrentDistance;

    private SerialPort port;
    private Thread readThread;
    private bool running;

    void Start(){
        // Initialize the port
        port = new SerialPort("COM3", 9600);
        port.Open();
        running = true;
        readThread = new Thread(ReadData);
        readThread.Start();
    }

    void ReadData(){
        while (running){
            try{
                // String parsing
                string line = port.ReadLine();
                char delim = ',';
                string[] data  = line.Trim().Split(delim);
                if (data.Length == 2){
                    CurrentAngle = float.Parse(data[0]);
                    CurrentDistance = float.Parse(data[1]);
                }
            }
            catch { }
        }
    }
    void OnDestroy(){
        running = false;
        if (port != null && port.IsOpen) port.Close();
    }
    void OnApplicationQuit(){
        running = false;
        if (port != null && port.IsOpen)
            port.Close();
    }

}