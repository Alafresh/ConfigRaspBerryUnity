using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum TaskState
{
    INIT,
    WAIT_COMMANDS,
    FINAL
}

public class ClassSerial : MonoBehaviour
{
    private static TaskState taskState = TaskState.INIT;
    private SerialPort _serialPort;
    private byte[] buffer;
    private int counter = 0;
    [SerializeField] TextMeshProUGUI respuesta;
    [SerializeField] InputField inputField;
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
                    string password = inputField.text;
                    _serialPort.Write(password+"\n");
                    Debug.Log("Send password" + password);
                    if(password == "C1234")
                    {
                        taskState = TaskState.FINAL;
                    }

                }
                if (_serialPort.BytesToRead > 0)
                {
                    string response = _serialPort.ReadLine();
                    Debug.Log(response);
                    respuesta.text = response;
                }
                break;
            case TaskState.FINAL:
                respuesta.text = "SALVASTE EL DIA";
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
