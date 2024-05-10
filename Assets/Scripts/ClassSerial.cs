using System;
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
    private float counter = 0;
    [SerializeField] TextMeshProUGUI respuesta;
    [SerializeField] GameObject ganaste;
    [SerializeField] GameObject perdiste;
    [SerializeField] InputField inputField;
    [SerializeField] Image LedWrong;
    [SerializeField] Image LedRight;
    [SerializeField] ExtractNumbers actualizarSliders;
    [SerializeField] Text tiempo;
    [SerializeField] Image frame;
    [SerializeField] Material frameMaterial;
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        switch (taskState)
        {
            case TaskState.INIT:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    string password = inputField.text;
                    _serialPort.Write(password + "\n");
                    if (password == "Iniciar")
                    {
                        frame.material = frameMaterial;
                        taskState = TaskState.WAIT_COMMANDS;
                        Debug.Log("WAIT COMMANDS");
                    }
                    if(password == "SubirTiempo" || password == "BajarTiempo")
                    {
                        string response = _serialPort.ReadLine();
                        counter = float.Parse(tiempo.text);
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
                        yield return new WaitForSeconds(0.5f);
                        _serialPort.Write(password + "\n");
                        Debug.Log("Send password 1" + password);
                        taskState = TaskState.FINAL;
                    }
                    if (password == "q")
                    { 
                        _serialPort.Write(password + "\n");
                    }
                    if (password == "w")
                    {
                        _serialPort.Write(password + "\n");
                    }
                    if (password == "e")
                    {
                        _serialPort.Write(password + "\n");
                    }
                    if (password == "r")
                    {
                        _serialPort.Write(password + "\n");
                    }
                    if (password == "t")
                    {
                        _serialPort.Write(password + "\n");
                    }
                    if (password == "y")
                    {
                        _serialPort.Write(password + "\n");
                    }
                    else
                    {
                        LedWrong.color = Color.red;
                        LedRight.color = new Color(115, 160, 166);
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
                if (_serialPort.BytesToRead >= 30)
                {
                    byte[] buffer = new byte[4];

                    _serialPort.Read(buffer, 0, 4);
                    float temperatura = System.BitConverter.ToSingle(buffer, 0);

                    _serialPort.Read(buffer, 4, 4);
                    float temperaturaEstado = System.BitConverter.ToSingle(buffer, 4);

                    _serialPort.Read(buffer, 8, 4);
                    float intervalo = System.BitConverter.ToSingle(buffer, 8);

                    _serialPort.Read(buffer, 12, 4);
                    float presion = System.BitConverter.ToSingle(buffer, 12);

                    _serialPort.Read(buffer, 16, 4);
                    float presionEstado = System.BitConverter.ToSingle(buffer, 16);

                    _serialPort.Read(buffer, 20, 4);
                    float nivel = System.BitConverter.ToSingle(buffer, 20);

                    _serialPort.Read(buffer, 24, 4);
                    float nivelEstado = System.BitConverter.ToSingle(buffer, 24);

                    _serialPort.Read(buffer, 28, 4);
                    float checksum = System.BitConverter.ToSingle(buffer, 28);

                    float checksum2 = temperatura + temperaturaEstado + intervalo + presion + presionEstado + nivel + nivelEstado;

                    Debug.Log(checksum);
                    Debug.Log(checksum2);

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
        _serialPort.PortName = "COM16";
        _serialPort.BaudRate = 115200;
        _serialPort.DtrEnable = true;
        _serialPort.NewLine = "\n";
        _serialPort.Open();
        Debug.Log("Open Serial Port");
    }
    void Update()
    {
       StartCoroutine(Wait());
    }
}
