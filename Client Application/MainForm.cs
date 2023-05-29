using System.Diagnostics;

namespace Client_program
{
    public partial class MainForm : Form
    {
        private delegate void SetTimeTextDelegate(DateTime time);
        private delegate void UpdateProfileDelegate(int[] profile);
        private delegate void AppendMessageLogDelegate(string message);
        private delegate void SetLabelTextDelegate(string text);

        public TextBox ReadFilePath_tb { get { return ReadProfileFile_tb; } }
        public TextBox WriteFilePath_tb { get { return WriteProfileFile_tb; } }

        public MainForm()
        {
            InitializeComponent();
            App.Initialize(this);
            openToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem1.Enabled = false;
            ReadProfileFile_btn.Enabled = false;
            WriteProfileFile_btn.Enabled = false;
        }

        private string ReadFilePath = null;
        private string WriteFilePath = null;

        #region Time controlls
        private void Sync_button_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.Instance.CheckConnection())
                {
                    App.Port.NewMessage();
                    App.Port.Message.Command = Commands.WriteTime;
                    App.Port.Message.SetData(DateTime.Now);
                    App.Port.SendMessage();
                    Trace.WriteLine("Sent message");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void ReadTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.Instance.CheckConnection())
                {
                    App.Port.NewMessage();
                    App.Port.Message.Command = Commands.ReadTime;
                    App.Port.SendMessage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public void Update_current_time_label_text(DateTime time)
        {
            if (current_time_label.InvokeRequired)
            {
                var method = new SetTimeTextDelegate(Update_current_time_label_text);
                current_time_label.Invoke(method, new object[] { time });
            }
            else
            {
                current_time_label.Text = $"{time.Year}.{time.Month:d2}.{time.Day:d2}. {time.Hour:d2}:{time.Minute:d2}";
            }
        }

        #endregion

        #region Menu Strip
        #region Connection Menu Strip
        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                App.Instance.ConnectCommunication();
                App.Port.OpenSerialPort();
                Trace.WriteLine("Connected and opened serial port");
                SetConnected();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        public void SetConnected()
        {
            connectToolStripMenuItem.Enabled = false;
            openToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem1.Enabled = true;
        }

        public void SetDisconnected()
        {
            openToolStripMenuItem.Enabled = true;
            closeToolStripMenuItem1.Enabled = false;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!App.Instance.PortInitialized)
                {
                    string message = "No connection was found to the microcontroller. Would you like to try to initialize adn open the serial port?";
                    string caption = "No connection found";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    if (MessageBox.Show(message, caption, buttons) == DialogResult.Yes)
                    {
                        App.Instance.ConnectCommunication();
                        App.Port.OpenSerialPort();
                    }
                }
                else
                    App.Port.OpenSerialPort();
                Trace.WriteLine("Opened serial port");
                SetConnected();
                SetNoConnectionLabel(false);
                App.Port.AddReceivedDataEvent(App.ReceiveData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!App.Instance.PortInitialized)
                {
                    string message = "No connection was found to the microcontroller. Would you like to try to initialize the serial port?";
                    string caption = "No connection found";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    if (MessageBox.Show(message, caption, buttons) == DialogResult.Yes)
                    {
                        App.Instance.ConnectCommunication();
                    }
                }
                App.Port.CloseSerialPort();
                Trace.WriteLine("Closed serial port");
                SetDisconnected();
                SetNoConnectionLabel(true);
                App.Port.RemoveReceivedDataEvent(App.ReceiveData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        #endregion

        #region File Menu Strip
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to exit?";
            string caption = "Exit";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            if (MessageBox.Show(message, caption, buttons) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void saveProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            profileManager1.Save();
        }

        private void saveProfileAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            profileManager1.SaveAs();
        }

        private void loadProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "All non-saved data will be lost from current profile. Do you want to procede?";
            string caption = "Data loss warning";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            if (MessageBox.Show(message, caption, buttons) == DialogResult.Yes)
            {
                profileManager1.LoadAs();
            }

        }

        private void newProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            profileManager1.NewProfile();
        }
        #endregion

        #region Messages

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.Instance.CheckConnection())
                {
                    App.Port.NewMessage();
                    App.Port.Message.Command = Commands.Test;
                    App.Port.SendMessage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            messageLog_rtb.Clear();
        }

        public void AppendMessageLog(string message)
        {

            if (messageLog_rtb.InvokeRequired)
            {
                var method = new AppendMessageLogDelegate(AppendMessageLog);
                profileManager1.Invoke(method, new object[] { message });
            }
            else
            {
                DateTime time = DateTime.Now;
                messageLog_rtb.AppendText($"{time.Month:d2}.{time.Day:d2} {time.Hour:d2}:{time.Minute:d2}: {message} \n");
            }
        }

        public void SetNoConnectionLabel(bool visible)
        {
            NoCon_label.Visible = visible;
        }

        private void testErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.Instance.CheckConnection())
                {
                    App.Port.NewMessage();
                    App.Port.Message.Command = Commands.TestError;
                    App.Port.SendMessage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #endregion

        #region Read Profile from Microcontroller
        private void ReadFile_btn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files(*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ReadFilePath = openFileDialog.FileName;
                }
            }
            ReadProfileFile_tb.Text = ReadFilePath;
        }

        private void ReadProfileFile_tb_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(Path.GetFullPath(ReadProfileFile_tb.Text)))
                    throw new Exception();
                if (Path.GetExtension(ReadProfileFile_tb.Text) != ".txt")
                    throw new Exception();
                ReadProfileFile_btn.Enabled = true;
            }
            catch
            {
                ReadProfileFile_btn.Enabled = false;
            }
        }

        private void ReadProfileFile_tb_Leave(object sender, EventArgs e)
        {
            if (ReadProfileFile_tb.Text == "")
                return;
            try
            {
                if (!File.Exists(Path.GetFullPath(ReadProfileFile_tb.Text)))
                    throw new ArgumentException("The file doesn't exist");
                if (Path.GetExtension(ReadProfileFile_tb.Text) != ".txt")
                    throw new ArgumentException("The file must be .txt\n\r");
            }
            catch (ArgumentException ex)
            {
                string caption = "Read profile into file path error";
                MessageBox.Show(ex.Message, caption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void UpdateProfile(int[] profile)
        {
            if (profileManager1.InvokeRequired)
            {
                var method = new UpdateProfileDelegate(UpdateProfile);
                profileManager1.Invoke(method, new object[] { profile });
            }
            else
            {
                profileManager1.SetPoints(profile);
                profileManager1.ProfileData.AttachView(profileManager1);
            }
        }

        private void ReadProfile_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.Instance.CheckConnection())
                {
                    SetReadProfileLabelText("Profile is loading...");
                    App.SetProfileReadingDestinaation(false);
                    App.Port.NewMessage();
                    App.Port.Message.Command = Commands.ReadProfile;
                    App.Port.SendMessage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ReadProfileFile_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.Instance.CheckConnection())
                {
                    SetReadProfileLabelText("Profile is loading...");
                    App.SetProfileReadingDestinaation(true);
                    App.Port.NewMessage();
                    App.Port.Message.Command = Commands.ReadProfile;
                    App.Port.SendMessage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        public void SetReadProfileLabelText(string text)
        {
            if (ReadProfile_label.InvokeRequired)
            {
                var method = new SetLabelTextDelegate(SetReadProfileLabelText);
                ReadProfile_label.Invoke(method, new object[] { text });

            }
            else
            {
                ReadProfile_label.Text = text;
            }
        }

        public void SetWriteProfileLabelText(string text)
        {
            if (WriteProfile_label.InvokeRequired)
            {
                var method = new SetLabelTextDelegate(SetReadProfileLabelText);
                WriteProfile_label.Invoke(method, new object[] { text });

            }
            else
            {
                WriteProfile_label.Text = text;
            }
        }


        #endregion

        #region Write Profile to Microcontroller
        private void WriteFile_btn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files(*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    WriteFilePath = openFileDialog.FileName;
                }
            }
            WriteProfileFile_tb.Text = WriteFilePath;
        }

        private void WriteProfileFile_tb_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(Path.GetFullPath(WriteProfileFile_tb.Text)))
                    throw new Exception();
                if (Path.GetExtension(WriteProfileFile_tb.Text) != ".txt")
                    throw new Exception();
                WriteProfileFile_btn.Enabled = true;
            }
            catch
            {
                WriteProfileFile_btn.Enabled = false;
            }
        }
        private void WriteProfileFile_tb_Leave(object sender, EventArgs e)
        {
            if (WriteProfileFile_tb.Text == "")
                return;
            try
            {
                if (!File.Exists(Path.GetFullPath(WriteProfileFile_tb.Text)))
                    throw new ArgumentException("The file doesn't exist");
                if (Path.GetExtension(WriteProfileFile_tb.Text) != ".txt")
                    throw new ArgumentException("The file must be .txt\n\r");

            }
            catch (ArgumentException ex)
            {
                string caption = "wRITE profile into file path error";
                MessageBox.Show(ex.Message, caption);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void WriteProfile_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.Instance.CheckConnection())
                {
                    SetWriteProfileLabelText("Profile is being sent...");
                    App.Port.NewMessage();
                    App.Port.Message.Command = Commands.WriteProfile;
                    if (!profileManager1.ProfileData.ValidatePoints())
                        throw new Exception("Make sure the profile is valid before sending");
                    profileManager1.ProfileData.CalcProfile();
                    App.Port.Message.SetData(profileManager1.ProfileData.Profile);
                    App.Port.SendMessage(true);
                    SetWriteProfileLabelText("Profile has been written...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void WriteProfileFile_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (App.Instance.CheckConnection())
                {
                    SetWriteProfileLabelText("Profile is being sent...");
                    Thread t = new Thread(new ParameterizedThreadStart(WriteThread));
                    t.IsBackground = true;
                    t.Start(WriteProfileFile_tb.Text);
                    SetWriteProfileLabelText("Profile has been written...");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void WriteThread(object param)
        {
            App.Port.NewMessage();
            App.Port.Message.Command = Commands.WriteProfile;
            string path = param as string;
            ProfileData profileData = new ProfileData(path);
            App.Port.Message.SetData(profileData.Profile);
            App.Port.SendMessage(true);
            Trace.WriteLine("Profile writer thread exists");
        }

        #endregion

    }
}