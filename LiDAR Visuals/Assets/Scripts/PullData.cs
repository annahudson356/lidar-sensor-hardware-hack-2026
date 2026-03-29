using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class PullData : MonoBehaviour
{
    public string portName = "COM3";
    public int baudRate = 9600;

    public static float CurrentPitch;
    public static float CurrentYaw;
    public static float CurrentDistance;
    private SerialPort port;
    private Thread readThread;
    private volatile bool running;

    void Start(){
        try{
            port = new SerialPort(portName, baudRate);
            port.ReadTimeout = 500;
            port.DtrEnable = true;
            port.RtsEnable = true;
            port.Open();
            Debug.Log("Port opened successfully");
            running = true;
            readThread = new Thread(ReadData);
            readThread.IsBackground = true;
            readThread.Start();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open port: " + e.Message);
        }
    }

    void ReadData(){
        while (running){
            try
            {
                string line = port.ReadLine();
                if (line.Contains("Pitch")) continue; 
                string[] data = line.Trim().Split(',');
                if (data.Length == 3)
                {
                    CurrentPitch = float.Parse(data[0].Trim());
                    CurrentYaw = float.Parse(data[1].Trim());
                    CurrentDistance = float.Parse(data[2].Trim());
                }
            }
            catch { }
        }
    }

    void OnDestroy(){
        running = false;
        Thread.Sleep(100);
        if (port != null && port.IsOpen) port.Close();
    }

    void OnApplicationQuit(){
        running = false;
        Thread.Sleep(100);
        if (port != null && port.IsOpen) port.Close();
    }
}