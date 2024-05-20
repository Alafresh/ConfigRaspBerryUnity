using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*Estado de maquina*/
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
    /*Hilo secundario*/
    private Thread serialThread;
    /*se utiliza para controlar el ciclo de vida del hilo que maneja la comunicación serial. Indicador de cuando debe ejecutarse*/
    private bool isRunning = false;
    /*Variable intermediaria entre leer y procesar los datos de la cola*/
    private string receivedData = "";
    /*Cola para almacenar datos del puerto serial en orden FIFO*/
    private Queue<string> dataQueue = new Queue<string>();

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

    void Start()
    {
        _serialPort = new SerialPort
        {
            PortName = "COM6",
            BaudRate = 115200,
            DtrEnable = true,
            NewLine = "\n"
        };
        _serialPort.Open();
        Debug.Log("Open Serial Port");

        isRunning = true;
        serialThread = new Thread(SerialThreadFunction);
        serialThread.Start();
    }
    /* se cierre correctamente cuando el objeto se destruye  */
    void OnDestroy()
    {
        isRunning = false;
        if (serialThread != null && serialThread.IsAlive)
        {
            serialThread.Join();
        }
        if (_serialPort != null && _serialPort.IsOpen)
        {
            _serialPort.Close();
        }
    }
    /*se encarga de la lectura de datos del puerto serial y los encola para su posterior procesamiento en el hilo principal*/
    private void SerialThreadFunction()
    {
        while (isRunning)
        {
            try
            {
                if (_serialPort.BytesToRead > 0)
                {
                    string data = _serialPort.ReadLine();
                    /*Encolando los datos leido del puerto serial*/
                    dataQueue.Enqueue(data);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Serial thread exception: " + ex.Message);
            }
            Thread.Sleep(50);
        }
    }
    /*Hilo principal que se encarga de escribir en el puerto seria a travez de la funcion HandleInput que se comportara segun el estado de TaskState
     Al mismo tiempo que La cola este llena procesara cada dato a travez de la funcion ProccessReceivedData que se encarga de desplegar la info en la UI*/
    void Update()
    {
        if (taskState == TaskState.INIT && Input.GetKeyDown(KeyCode.Return))
        {
            HandleInput();
        }
        else if (taskState == TaskState.WAIT_COMMANDS && Input.GetKeyDown(KeyCode.Return))
        {
            HandleInput();
        }

            /*Se procesan los datos del puerto serial para posteriormente mostrarlos en la UIs*/
            while (dataQueue.Count > 0)
            {
                receivedData = dataQueue.Dequeue();
                ProcessReceivedData(receivedData);
            }
    }
    /*Funcion que escribe en el puerto serial segun el estado de maquina*/
    private void HandleInput()
    {
        string password = inputField.text;
        _serialPort.Write(password + "\n");

        if (password == "Iniciar")
        {
            frame.material = frameMaterial;
            taskState = TaskState.WAIT_COMMANDS;
            Debug.Log("WAIT COMMANDS");
        }
        else if (password == "SubirTiempo" || password == "BajarTiempo")
        {
            // Esto es solo para enviar el comando, la respuesta se procesará en Update
            Debug.Log("Comando de tiempo enviado: " + password);
        }
        else if (taskState == TaskState.WAIT_COMMANDS)
        {
            if (password == "C1234")
            {
                LedRight.color = Color.green;
                LedWrong.color = new Color(115, 160, 166);
                _serialPort.Write("LEDOn\n");
                ganaste.SetActive(true);
                taskState = TaskState.FINAL;
            }
            else
            {
                LedWrong.color = Color.red;
                LedRight.color = new Color(115, 160, 166);
                _serialPort.Write("s\n");
            }
        }
    }
    /*Despliqega datos en la UI*/
    private void ProcessReceivedData(string data)
    {
        Debug.Log("Received data: " + data);
        respuesta.text = data;

        if (taskState == TaskState.INIT)
        {
            if (data == "SubirTiempo" || data == "BajarTiempo")
            {
                // Suponiendo que el formato de los datos recibidos es correcto para análisis
                try
                {
                    float responseValue = float.Parse(data);
                    counter = responseValue; // Actualiza el contador
                    tiempo.text = responseValue.ToString(); // Actualiza la UI
                }
                catch (FormatException ex)
                {
                    Debug.LogError("Error al parsear el tiempo recibido: " + ex.Message);
                }
            }
        }
        else if (taskState == TaskState.WAIT_COMMANDS)
        {
            try
            {
                float value = float.Parse(data);
                counter -= 0.3f;
                if (counter > 0)
                {
                    tiempo.text = counter.ToString();
                    Debug.Log("Counter: " + counter);
                }
                actualizarSliders.Cambiar();

                if (_serialPort.BytesToRead >= 30)
                {
                    byte[] buffer = new byte[20];
                    _serialPort.Read(buffer, 0, 20);
                    float temperatura = BitConverter.ToSingle(buffer, 0);
                    float intervalo = BitConverter.ToSingle(buffer, 4);
                    float presion = BitConverter.ToSingle(buffer, 8);
                    float nivel = BitConverter.ToSingle(buffer, 12);
                    float checksum = BitConverter.ToSingle(buffer, 16);

                    float checksum2 = temperatura + intervalo + presion + nivel;

                    Debug.Log(checksum);
                    Debug.Log(checksum2);
                }

                if (counter <= 0)
                {
                    perdiste.SetActive(true);
                    _serialPort.Write("C1234\n");
                    taskState = TaskState.FINAL;
                }
            }
            catch (FormatException ex)
            {
                Debug.LogError("Error al parsear el dato recibido: " + ex.Message);
            }
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
}
