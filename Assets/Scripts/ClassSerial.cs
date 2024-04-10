using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
using UnityEngine;

enum TaskState
{
    INIT,
    WAIT_COMMANDS
}

public class ClassSerial : MonoBehaviour
{
    private static TaskState taskState = TaskState.INIT;
    private SerialPort _serialPort;
    private byte[] buffer;
    private int counter = 0;
    [SerializeField] TextMeshProUGUI respuesta;
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        counter++;

        switch (taskState)
        {
            case TaskState.INIT:
                taskState = TaskState.WAIT_COMMANDS;
                Debug.Log("WAIT COMMANDS");
                break;
            case TaskState.WAIT_COMMANDS:
                if (Input.GetKeyDown(KeyCode.A))
                {
                    _serialPort.Write("ledON\n");
                    Debug.Log("Send ledON");
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    _serialPort.Write("ledOFF\n");
                    Debug.Log("Send ledOFF");
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    _serialPort.Write("readBUTTONS\n");
                    Debug.Log("Send readBUTTONS");
                }
                if (_serialPort.BytesToRead > 0)
                {
                    string response = _serialPort.ReadLine();
                    Debug.Log(response);
                    respuesta.text = response;
                }
                break;
            default:
                Debug.Log("State Error");
                break;

        }
        
    }
    void Start()
    {
        _serialPort = new SerialPort();
        _serialPort.PortName = "COM16";
        _serialPort.BaudRate = 115200;
        _serialPort.DtrEnable = true;
        _serialPort.NewLine = "\n";
        _serialPort.Open();
        Debug.Log("Open Serial Port");
        buffer = new byte[128];
    }
    void Update()
    {
       StartCoroutine(Wait());
    }
}
