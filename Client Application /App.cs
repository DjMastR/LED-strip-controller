using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using static System.Collections.Specialized.BitVector32;

namespace Client_program
{
    public class App
    {
        private Form mainForm;
        private static App theApp;
        private SerialCommunicator communicator;
        private string filePath;

        private static bool isFile = false;

        private bool portInitialized = false;
        public bool PortInitialized { get { return portInitialized; } }

        public string FilePath { get { return filePath; } }

        public DateTime Time { get; set; }

        public static App Instance
        {
            get { return theApp; }
        }

        public static SerialCommunicator Port
        {
            get { return Instance.communicator; }
        }

        public static void Initialize(Form form)
        {
            theApp = new App();
            theApp.mainForm = form;
        }

        #region Connection

        public void ConnectCommunication()
        {
            if (PortInitialized) throw new Exception("The serial port is already initialized");
            SerialCommunicator.InitilizeSerialPort();
            Instance.communicator = SerialCommunicator.Instance;
            portInitialized = true;
            Port.AddReceivedDataEvent(ReceiveData);
            ((MainForm)mainForm).SetNoConnectionLabel(false);
        }

        public bool CheckConnection()
        {

            if (!App.Instance.PortInitialized)
            {
                string message = "No connection was found to the microcontroller. Would you like to try to initialize and open the serial port?";
                string caption = "No connection found";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                if (MessageBox.Show(message, caption, buttons) == DialogResult.Yes)
                {
                    App.Instance.ConnectCommunication();
                    App.Port.OpenSerialPort();
                    ((MainForm)Instance.mainForm).SetConnected();
                    return true;
                }
                return false;
            }
            else if (!App.Port.Connected)
            {
                string message = "No connection was found to the microcontroller. Would you like to try to connect to it?";
                string caption = "No connection found";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                if (MessageBox.Show(message, caption, buttons) == DialogResult.Yes)
                {
                    App.Port.OpenSerialPort();
                    return true;
                }
                return false;
            }
            return true;
        }

        #endregion

        #region Serial data receive

        public static void SetProfileReadingDestinaation(bool file)
        {
            isFile = file;
        }

        public static void ReceiveData(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(400);
            System.IO.Ports.SerialPort sp = (System.IO.Ports.SerialPort)sender;
            string input = sp.ReadExisting();
            string[] data = input.Split(';');
            List<int> ints = new List<int>();
            sp.DiscardInBuffer();
            foreach (string s in data)
            {
                ints.Add(Convert.ToInt32(s));
            }
            switch (ints[0])
            {
                case 0x80:
                    ReceiveTime(ints);
                    break;
                case 0x81:
                    int[] profile = ReceiveProfile(ints);
                    if (isFile)
                    {
                        ReceiveProfileIntoFile(profile);
                        isFile = false;
                    }
                    else
                        ReceiveProfileIntoChart(profile);
                    break;
                case 0xff:
                case 0xfe:
                    ReceiveError(ints);
                    break;
                case 0x84:
                    ReceiveTest(ints);
                    break;
            }
        }

        public static void ReceiveTime(List<int> ints)
        {
            if (ints.Last() != 0xF0) 
                throw new Exception("An error has occured in command and/or closing byte(s)");
            if (ints.Count != 10) 
                throw new Exception("An error has occured in the amount of received bytes");
            Instance.Time = new DateTime(ints[1] * 100 + ints[2], ints[3], ints[4], ints[6], ints[7], ints[8]);
            ((MainForm)Instance.mainForm).Update_current_time_label_text(App.Instance.Time);
        }
        private static int[] ReceiveProfile(List<int> ints)
        {
            if (ints.Last() != 0xF0)
                throw new Exception("An error has occured in command and/or closing byte(s)");
            if (ints.Count() != 1440 + 2 + 4)
                throw new Exception("An error has occured in the amount of received bytes");
            int[] profile = new int[ProfileData.MinutesPerDay];
            byte[] profile_byte = new byte[ProfileData.MinutesPerDay];
            for (int i = 0; i < profile.Length; i++)
            {
                profile[i] = ints[i + 1];
                profile_byte[i] = (byte)profile[i];
            }
            byte[] crc = new byte[4];
            for (int i = 0; i < 4; i++)
                crc[i] = (byte)(ints[profile.Length + 1 + i]);
            if (!CRC.CRC_Match(profile_byte, crc))
                throw new Exception("CRC detected an error during the transmission");

            return profile;
        }

        public static void ReceiveProfileIntoChart(int[] profile)
        {
            try
            {
                if (profile == null)
                    return;
                ((MainForm)Instance.mainForm).UpdateProfile(profile);
                ((MainForm)Instance.mainForm).SetReadProfileLabelText("Profile has been loaded");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void ReceiveProfileIntoFile(int[] profile)
        {
            try
            {
                if (profile == null)
                    return;
                ProfileData data = new ProfileData(profile);
                data.SaveDocument(((MainForm)App.Instance.mainForm).ReadFilePath_tb.Text);
                ((MainForm)Instance.mainForm).SetReadProfileLabelText("Profile has been loaded into the file");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void ReceiveTest(List<int> ints)
        {   
            if (ints.Last() != 0xF0)
                throw new Exception("An error has occured in command and/or closing byte(s)");
            if (ints.Count() != 2)
                throw new Exception("An error has occured in the amount of received bytes");

            string message = "The connection is online";
            ((MainForm)Instance.mainForm).AppendMessageLog(message);
        }
    
        public static void ReceiveError(List<int> ints)
        {
            bool test;
            if (ints[0] == 0xff)
                test = false;
            else if (ints[0] == 0xfe)
                test = true;
            else
                throw new Exception("An error has occured in command and/or closing byte(s)");

            string message = ErrorProcessor.ProcessErrorInts(ints.ToArray(), test);
            ((MainForm)Instance.mainForm).AppendMessageLog(message);
            
        }

        #endregion
    }
}
