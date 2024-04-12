# Comunicación Serial con Unity

![alt_tag](https://cdn.discordapp.com/attachments/977607304264425522/1228339052881641603/image.png?ex=662baee3&is=661939e3&hm=44c50ac7c9e912124ee03471a75eec9a0134443ab3f108da4b8e18c806702760&)

[![Video](https://media.discordapp.net/attachments/977607304264425522/1228348014981283892/image.png?ex=662bb73c&is=6619423c&hm=6cd826bdc351b543a3a5e3a12a5acde43da5d77100ddce8f9dbc6da20b0aba72&=&format=webp&quality=lossless&width=687&height=343)](https://www.youtube.com/watch?v=y2WqV1QcpEs)


## Configuración del Puerto Serial

Para establecer la comunicación serial, es importante considerar dos propiedades claves:
1. **PortName**: Esta propiedad especifica el nombre del puerto serial al que se conectará el dispositivo. En Windows, los puertos seriales se identifican como `COM1`, `COM2`, etc. En Unix/Linux, se identifican como `/dev/ttyS0`, `/dev/ttyS1`, etc. Es esencial especificar el puerto correcto para establecer la comunicación con el dispositivo deseado.

2. **BaudRate**: Esta propiedad especifica la velocidad de transmisión de datos en bits por segundo (baudios). Es crucial que esta velocidad coincida con la configuración del dispositivo conectado. Si el dispositivo está configurado para comunicarse a una velocidad de 115200 baudios, como en este ejemplo, entonces la propiedad `BaudRate` debe establecerse en 115200 para que la comunicación sea exitosa.

## Relación con el Microcontrolador

Estas propiedades son fundamentales para establecer una comunicación correcta entre Unity y el microcontrolador. Si no se configuran correctamente, la comunicación puede no establecerse o puede ser inestable, lo que dificultaría la transmisión de datos entre ambos dispositivos Configurar correctamente es un paso importante para su correcto funcionamiento.

# Conexión entre Raspberry Pi Pico y Unity para Intercambio de Datos 
Estamos desarrollando una aplicación que establece una conexión entre un microcontrolador Raspberry Pi Pico y el motor gráfico Unity. Esta conexión permite intercambiar datos entre ambos sistemas, lo que nos permite controlar y monitorear dispositivos físicos desde una interfaz personalizada creada en Unity.

## Inclusiones de bibliotecas y declaración de enumeración
```c#
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
using UnityEngine;

// Enum para representar los estados de la tarea
enum TaskState
{
    INIT,               // Estado inicial
    WAIT_COMMANDS       // Esperando comandos
}
```
- Se incluyen las bibliotecas necesarias para el funcionamiento del código.
- Se declara una enumeración TaskState que define los posibles estados de la tarea.

  ##  Declaración de la clase y variables miembro
```c#
  public class ClassSerial : MonoBehaviour
{
    // Estado actual de la tarea
    private static TaskState taskState = TaskState.INIT;
    // Puerto serial
    private SerialPort _serialPort;
    // Buffer para datos del puerto serial
    private byte[] buffer;
    // Texto para mostrar información en la interfaz
    public TextMeshProUGUI myText;
    // Contador de frames
    private int counter = 0;
    // Texto para mostrar la respuesta del puerto serial en la interfaz
    [SerializeField] TextMeshProUGUI respuesta;
```

- Se declara la clase ClassSerial que hereda de MonoBehaviour, lo que permite que sea un componente de Unity.
- Se definen las variables miembro que se utilizarán en la clase, incluyendo el estado de la tarea, el puerto serial, un buffer para datos, objetos de texto para la interfaz, y un contador de frames.

## Corrutina para esperar un tiempo
```c#
// Corrutina para esperar un tiempo antes de realizar acciones
IEnumerator Wait()
{
    yield return new WaitForSeconds(3); // Esperar 3 segundos
    myText.text = "Frames: " + counter.ToString(); // Mostrar el número de frames
    counter++; // Incrementar el contador de frames

    // Switch para manejar el estado actual de la tarea
    switch (taskState)
    {
        case TaskState.INIT:
            // Cambiar al estado de espera de comandos
            taskState = TaskState.WAIT_COMMANDS;
            Debug.Log("WAIT COMMANDS"); // Mostrar estado en la consola
            break;
        case TaskState.WAIT_COMMANDS:
            // Manejar entrada de teclado para enviar comandos al puerto serial
            // Leer datos del puerto serial si están disponibles
            break;
        default:
            Debug.Log("State Error");
            break;
    }
}
```
- Se define una corrutina llamada Wait que espera 3 segundos antes de realizar acciones.
- Se actualiza el texto en la interfaz para mostrar el número de frames.
- Se utiliza un switch para manejar el estado actual de la tarea y realizar acciones según dicho estado.

## Método Start y configuración del puerto serial
```c#
  // Método llamado al iniciar el script
void Start()
{
    // Configuración del puerto serial
    _serialPort = new SerialPort();
    _serialPort.PortName = "COM14"; // Puerto serial a utilizar
    _serialPort.BaudRate = 115200; // Velocidad de comunicación en baudios
    _serialPort.DtrEnable = true; // Habilitar control de flujo DTR
    _serialPort.NewLine = "\n"; // Carácter de nueva línea
    _serialPort.Open(); // Abrir el puerto serial
    Debug.Log("Open Serial Port"); // Mostrar en la consola
    buffer = new byte[128]; // Inicializar el buffer de datos
}
```
- Se define el método Start, que se llama al iniciar el script.
- Se configura el puerto serial especificando el nombre del puerto, la velocidad de comunicación, y otras opciones.
- Se abre el puerto serial para establecer la conexión.

## Métodos para controlar botones de la interfaz
```c#
// Métodos para controlar los botones de la interfaz
public void ButtonOneON() {
    _serialPort.Write("1ON\n");
    Debug.Log("Send Btn1: ON");
}
public void ButtonOneOFF()
{
    _serialPort.Write("1OFF\n");
    Debug.Log("Send Btn1: OFF");
}
// Métodos para controlar otros botones de la interfaz (ButtonTwoON, ButtonTwoOFF, ...)
```
- Se definen métodos públicos para controlar los botones de la interfaz.
- Estos métodos envían comandos específicos al puerto serial según la acción realizada en la interfaz.

## Método Update
```c#
// Método que se ejecuta en cada frame
void Update()
{
   StartCoroutine(Wait()); // Llamar a la corrutina Wait
}
```
- Se define el método Update, que se llama en cada frame.
- En cada frame, se inicia la corrutina Wait para realizar acciones según el estado actual de la tarea.

# Documentos adicionales
[Narrativa](https://docs.google.com/document/d/1h1A-v5upudZEDbUCTuFwZtA7TT6g6vtgDPxW0Iy4LtM/edit?usp=drivesdk)


# Integrantes 
- juan Guillermo ID 000464690
- Cristian Usuga ID 000
- Jhonier Mosquera Id 000

