//using UnityEngine;
//using System.IO.Ports;
//using TMPro;

//enum TaskState
//{
//    INIT,
//    WAIT_START,
//    WAIT_EVENTS
//}

//public class Serial : MonoBehaviour
//{
//    private static TaskState taskState = TaskState.INIT;
//    private SerialPort _serialPort;
//    private byte[] buffer;
//    public TextMeshProUGUI myText;
//    private int counter = 0;

//    void Start()
//    {
//        _serialPort = new SerialPort();
//        _serialPort.PortName = "COM6";
//        _serialPort.BaudRate = 115200;
//        _serialPort.DtrEnable = true;
//        _serialPort.Open();
//        Debug.Log("Open Serial Port");
//        buffer = new byte[128];
//    }

//    void Update()
//    {
//        myText.text = counter.ToString();
//        counter++;

//        switch (taskState)
//        {
//            case TaskState.INIT:
//                taskState = TaskState.WAIT_START;
//                Debug.Log("WAIT START");
//                break;
//            case TaskState.WAIT_START:
//                if (Input.GetKeyDown(KeyCode.A))
//                {
//                    byte[] data = { 0x31 };// start
//                    _serialPort.Write(data, 0, 1);
//                    Debug.Log("WAIT EVENTS");
//                    taskState = TaskState.WAIT_EVENTS;
//                }
//                break;
//            case TaskState.WAIT_EVENTS:
//                if (Input.GetKeyDown(KeyCode.B))
//                {
//                    byte[] data = { 0x32 };// stop
//                    _serialPort.Write(data, 0, 1);
//                    Debug.Log("WAIT START");
//                    taskState = TaskState.WAIT_START;
//                }
//                if (_serialPort.BytesToRead > 0)
//                {
//                    int numData = _serialPort.Read(buffer, 0, 128);
//                    Debug.Log(System.Text.Encoding.ASCII.GetString(buffer));
//                }
//                break;
//            default:
//                Debug.Log("State Error");
//                break;
//        }
//    }
//}