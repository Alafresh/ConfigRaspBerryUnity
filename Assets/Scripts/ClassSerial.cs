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
    [SerializeField] GameObject ganaste;
    [SerializeField] GameObject perdiste;
    [SerializeField] InputField inputField;
    [SerializeField] Image LedWrong;
    [SerializeField] Image LedRight;
    [SerializeField] ExtractNumbers actualizarSliders;
    [SerializeField] Text tiempo;
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        switch (taskState)
        {
            case TaskState.INIT:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    string password = inputField.text;
                    _serialPort.Write(password + "\n");
                    if (password == "Iniciar")
                    {
                        taskState = TaskState.WAIT_COMMANDS;
                        Debug.Log("WAIT COMMANDS");
                    }
                    if(password == "SubirTiempo" || password == "BajarTiempo")
                    {
                        string response = _serialPort.ReadLine();
                        counter = int.Parse(tiempo.text);
                        tiempo.text = response;
                    }   
                    ganaste.SetActive(false);
                    perdiste.SetActive(false);
                }
                if (_serialPort.BytesToRead > 0)
                {
                    string response = _serialPort.ReadLine();
                    Debug.Log(response);
                    respuesta.text = response;                    
                    Debug.Log("Counter11: " + counter);
                }
                break;
            case TaskState.WAIT_COMMANDS:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    string password = inputField.text;
                    if(password == "C1234")
                    {
                        LedRight.color = Color.green;
                        LedWrong.color = new Color(115, 160, 166);
                        _serialPort.Write("LEDOn\n");
                        ganaste.SetActive(true);
                        yield return new WaitForSeconds(1);
                        _serialPort.Write(password + "\n");
                        Debug.Log("Send password 1" + password);
                        taskState = TaskState.FINAL;
                    }
                    else
                    {
                        LedWrong.color = Color.red;
                        LedRight.color = new Color(115, 160, 166);
                        _serialPort.Write("LEDOff\n");
                    }
                }
                if (_serialPort.BytesToRead > 0)
                {
                    string response = _serialPort.ReadLine();
                    Debug.Log(response);
                    respuesta.text = response;
                    counter--;
                    if (counter > 0)
                    {
                        tiempo.text = counter.ToString();
                        Debug.Log("Counter: " + counter);
                    }                    
                    actualizarSliders.Cambiar();
                }
                if (counter <= 0)
                {
                    perdiste.SetActive(true);
                    _serialPort.Write("C1234\n");
                    taskState = TaskState.FINAL;
                }
                break;
            case TaskState.FINAL:
                taskState = TaskState.INIT;
                break;
            default:
                Debug.Log("State Error");
                break;

        }
        
    }
    public void LedOn()
    {
        _serialPort.Write("LEDOn\n");
    }
    public void LedOff()
    {
        _serialPort.Write("LEDOff\n");
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
    void Update()
    {
       StartCoroutine(Wait());
    }
}
