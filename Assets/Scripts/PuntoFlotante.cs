using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum Estado
{
    INIT,
    WAIT_COMMANDS,
    FINAL
}

public class PuntoFlotante : MonoBehaviour
{
    private static Estado taskState = Estado.INIT;
    private SerialPort _serialPort;
    private byte[] buffer;
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

    }
}
