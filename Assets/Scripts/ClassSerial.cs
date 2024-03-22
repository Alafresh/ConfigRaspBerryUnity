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
    public TextMeshProUGUI myText;
    private int counter = 0;
    [SerializeField] TextMeshProUGUI respuesta;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        myText.text = "Frames: " + counter.ToString();
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
        _serialPort.PortName = "COM14";
        _serialPort.BaudRate = 115200;
        _serialPort.DtrEnable = true;
        _serialPort.NewLine = "\n";
        _serialPort.Open();
        Debug.Log("Open Serial Port");
        buffer = new byte[128];
    }
    public void ButtonOneON() {
        _serialPort.Write("1ON\n");
        Debug.Log("Send Btn1: ON");
    }
    public void ButtonOneOFF()
    {
        _serialPort.Write("1OFF\n");
        Debug.Log("Send Btn1: OFF");
    }
    public void ButtonTwoON()
    {
        _serialPort.Write("2ON\n");
        Debug.Log("Send Btn2: ON");
    }
    public void ButtonTwoOFF()
    {
        _serialPort.Write("2OFF\n");
        Debug.Log("Send Btn2: OFF");
    }
    public void ButtonThreeON()
    {
        _serialPort.Write("3ON\n");
        Debug.Log("Send Btn3: ON");
    }
    public void ButtonThreeOFF()
    {
        _serialPort.Write("3OFF\n");
        Debug.Log("Send Btn3: OFF");
    }
    public void ButtonFourON()
    {
        _serialPort.Write("4ON\n");
        Debug.Log("Send Btn4: ON");
    }
    public void ButtonFourOFF()
    {
        _serialPort.Write("4OFF\n");
        Debug.Log("Send Btn4: OFF");
    }
    void Update()
    {
       StartCoroutine(Wait());
    }
}
