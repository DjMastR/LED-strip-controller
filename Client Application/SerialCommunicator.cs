using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Diagnostics;

namespace Client_program
{
    public class SerialCommunicator
    {
        private static SerialCommunicator serCom;

        private Message message;

        private SerialPort port;

        public bool Connected { get; set; }

        public static SerialCommunicator Instance
        {
            get { return serCom; }
        }

        public Message Message { get { return message; } }

        private SerialCommunicator()
        {
            bool connected = false;
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                if (port.Contains("6"))
                {
                    connected = true;
                    break;
                }
            }
            if (!connected)
                throw new Exception("Please connect the controller to the pc before trying to communicate with it.");

            port = new SerialPort();

            port.PortName = "com6";
            port.BaudRate = 115200;
            port.Parity = Parity.None;
            port.StopBits = StopBits.One;
            port.DataBits = 8;
            port.Handshake = Handshake.None;

            port.DataReceived += ReceivedMessage;

            message = new Message();
        }

        

        public static void InitilizeSerialPort()
        {
            serCom = new SerialCommunicator();
        }

        public void OpenSerialPort()
        {
            if(Connected)  throw new Exception("You are already connected to the serial port"); 
            Instance.port.Open();
            Connected = true;
        }
        public void CloseSerialPort()
        {
            if (!Connected) throw new Exception("The serial port isn't open, you can't close it");
            Instance.port.Close();
            Connected = false;
        }

        public void NewMessage()
        {
            message = new Message();
        }

        public void ClearMessage()
        {
            message.Command = Commands.NoCommand;
            message.Data.Clear();
        }

        public void SendMessage()
        {
            byte[] data = message.ToByteFlow();
            port.Write(data, 0, data.Length);
            Trace.WriteLine("Message Sent");
        }

        public void SendMessage(bool threaded)
        {
            if (!message.CheckValidity())
                throw new Exception("The generated message is not valid");
            byte[] data = message.ToByteFlow();
            Thread t = new Thread(new ParameterizedThreadStart(SendMessageOnThread));
            t.IsBackground = true;
            t.Start(data);
        }

        private void SendMessageOnThread(object param)
        {
            if (param == null)
                return;
            byte[] data = param as byte[];
            port.Write(data, 0, data.Length);
            Trace.WriteLine("\nData sending is done");
        }

        public void AddReceivedDataEvent(SerialDataReceivedEventHandler h)
        {
            port.DataReceived += h;
        }

        public void RemoveReceivedDataEvent(SerialDataReceivedEventHandler h)
        {
            port.DataReceived -= h;
        }

        void ReceivedMessage(object sender, SerialDataReceivedEventArgs e)
        {
            Trace.WriteLine("UART received a message");
        }
    }
}
