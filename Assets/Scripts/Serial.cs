using UnityEngine;
using System.IO.Ports;
public class Serial : MonoBehaviour
{
    private SerialPort _serialPort = new SerialPort();
    private byte[] buffer = new byte[32];

    void Start()
    {
        _serialPort.PortName = "COM6";
        _serialPort.BaudRate = 115200;
        _serialPort.DtrEnable = true;
        _serialPort.Open();
        Debug.Log("Open Serial Port");
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            byte[] data = { 0x31 };// or byte[] data = {'1'};            
            _serialPort.Write(data, 0, 1);
            Debug.Log("Send Data");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (_serialPort.BytesToRead >= 16)
            {
                _serialPort.Read(buffer, 0, 20);
                Debug.Log("Receive Data");
                Debug.Log(System.Text.Encoding.ASCII.GetString(buffer));
            }
        }

    }
}