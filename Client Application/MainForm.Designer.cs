namespace Client_program
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            current_time_label = new Label();
            Sync_button = new Button();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newProfileToolStripMenuItem = new ToolStripMenuItem();
            saveProfileToolStripMenuItem = new ToolStripMenuItem();
            saveProfileAsToolStripMenuItem = new ToolStripMenuItem();
            loadProfileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            connectionToolStripMenuItem = new ToolStripMenuItem();
            connectToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            closeToolStripMenuItem1 = new ToolStripMenuItem();
            messagesToolStripMenuItem = new ToolStripMenuItem();
            clearLogToolStripMenuItem = new ToolStripMenuItem();
            testToolStripMenuItem = new ToolStripMenuItem();
            testErrorToolStripMenuItem = new ToolStripMenuItem();
            ReadTime_btn = new Button();
            profileManager1 = new ProfileManager();
            groupBox1 = new GroupBox();
            groupBox5 = new GroupBox();
            NoCon_label = new Label();
            messageLog_rtb = new RichTextBox();
            groupBox4 = new GroupBox();
            groupBox3 = new GroupBox();
            ReadProfile_label = new Label();
            panel1 = new Panel();
            ReadFile_btn = new Button();
            ReadProfileFile_tb = new TextBox();
            ReadProfileFile_btn = new Button();
            ReadProfile_btn = new Button();
            groupBox2 = new GroupBox();
            WriteProfile_label = new Label();
            panel2 = new Panel();
            WriteFile_btn = new Button();
            WriteProfileFile_btn = new Button();
            WriteProfileFile_tb = new TextBox();
            WriteProfile_btn = new Button();
            splitContainer1 = new SplitContainer();
            menuStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            panel1.SuspendLayout();
            groupBox2.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // current_time_label
            // 
            current_time_label.Dock = DockStyle.Fill;
            current_time_label.Location = new Point(3, 19);
            current_time_label.Name = "current_time_label";
            current_time_label.Size = new Size(232, 78);
            current_time_label.TabIndex = 0;
            current_time_label.Text = "Time";
            current_time_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // Sync_button
            // 
            Sync_button.Dock = DockStyle.Top;
            Sync_button.Location = new Point(3, 19);
            Sync_button.Name = "Sync_button";
            Sync_button.Size = new Size(232, 23);
            Sync_button.TabIndex = 1;
            Sync_button.Text = "Sync";
            Sync_button.UseVisualStyleBackColor = true;
            Sync_button.Click += Sync_button_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, connectionToolStripMenuItem, messagesToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1206, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newProfileToolStripMenuItem, saveProfileToolStripMenuItem, saveProfileAsToolStripMenuItem, loadProfileToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newProfileToolStripMenuItem
            // 
            newProfileToolStripMenuItem.Name = "newProfileToolStripMenuItem";
            newProfileToolStripMenuItem.Size = new Size(151, 22);
            newProfileToolStripMenuItem.Text = "New Profile";
            newProfileToolStripMenuItem.Click += newProfileToolStripMenuItem_Click;
            // 
            // saveProfileToolStripMenuItem
            // 
            saveProfileToolStripMenuItem.Name = "saveProfileToolStripMenuItem";
            saveProfileToolStripMenuItem.Size = new Size(151, 22);
            saveProfileToolStripMenuItem.Text = "Save Profile";
            saveProfileToolStripMenuItem.Click += saveProfileToolStripMenuItem_Click;
            // 
            // saveProfileAsToolStripMenuItem
            // 
            saveProfileAsToolStripMenuItem.Name = "saveProfileAsToolStripMenuItem";
            saveProfileAsToolStripMenuItem.Size = new Size(151, 22);
            saveProfileAsToolStripMenuItem.Text = "Save Profile As";
            saveProfileAsToolStripMenuItem.Click += saveProfileAsToolStripMenuItem_Click;
            // 
            // loadProfileToolStripMenuItem
            // 
            loadProfileToolStripMenuItem.Name = "loadProfileToolStripMenuItem";
            loadProfileToolStripMenuItem.Size = new Size(151, 22);
            loadProfileToolStripMenuItem.Text = "Load Profile";
            loadProfileToolStripMenuItem.Click += loadProfileToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(151, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // connectionToolStripMenuItem
            // 
            connectionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { connectToolStripMenuItem, openToolStripMenuItem, closeToolStripMenuItem1 });
            connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            connectionToolStripMenuItem.Size = new Size(81, 20);
            connectionToolStripMenuItem.Text = "Connection";
            // 
            // connectToolStripMenuItem
            // 
            connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            connectToolStripMenuItem.Size = new Size(119, 22);
            connectToolStripMenuItem.Text = "Connect";
            connectToolStripMenuItem.Click += connectToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(119, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // closeToolStripMenuItem1
            // 
            closeToolStripMenuItem1.Name = "closeToolStripMenuItem1";
            closeToolStripMenuItem1.Size = new Size(119, 22);
            closeToolStripMenuItem1.Text = "Close";
            closeToolStripMenuItem1.Click += closeToolStripMenuItem1_Click;
            // 
            // messagesToolStripMenuItem
            // 
            messagesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { clearLogToolStripMenuItem, testToolStripMenuItem, testErrorToolStripMenuItem });
            messagesToolStripMenuItem.Name = "messagesToolStripMenuItem";
            messagesToolStripMenuItem.Size = new Size(70, 20);
            messagesToolStripMenuItem.Text = "Messages";
            // 
            // clearLogToolStripMenuItem
            // 
            clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            clearLogToolStripMenuItem.Size = new Size(124, 22);
            clearLogToolStripMenuItem.Text = "Clear Log";
            clearLogToolStripMenuItem.Click += clearLogToolStripMenuItem_Click;
            // 
            // testToolStripMenuItem
            // 
            testToolStripMenuItem.Name = "testToolStripMenuItem";
            testToolStripMenuItem.Size = new Size(124, 22);
            testToolStripMenuItem.Text = "Test";
            testToolStripMenuItem.Click += testToolStripMenuItem_Click;
            // 
            // testErrorToolStripMenuItem
            // 
            testErrorToolStripMenuItem.Name = "testErrorToolStripMenuItem";
            testErrorToolStripMenuItem.Size = new Size(124, 22);
            testErrorToolStripMenuItem.Text = "Test Error";
            testErrorToolStripMenuItem.Click += testErrorToolStripMenuItem_Click;
            // 
            // ReadTime_btn
            // 
            ReadTime_btn.Dock = DockStyle.Bottom;
            ReadTime_btn.Location = new Point(3, 74);
            ReadTime_btn.Name = "ReadTime_btn";
            ReadTime_btn.Size = new Size(232, 23);
            ReadTime_btn.TabIndex = 3;
            ReadTime_btn.Text = "Read Time";
            ReadTime_btn.UseVisualStyleBackColor = true;
            ReadTime_btn.Click += ReadTime_Click;
            // 
            // profileManager1
            // 
            profileManager1.Dock = DockStyle.Fill;
            profileManager1.Location = new Point(0, 0);
            profileManager1.Name = "profileManager1";
            profileManager1.Size = new Size(961, 585);
            profileManager1.TabIndex = 4;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(groupBox5);
            groupBox1.Controls.Add(groupBox4);
            groupBox1.Controls.Add(groupBox3);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(241, 585);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Microcontroller controllls";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(NoCon_label);
            groupBox5.Controls.Add(messageLog_rtb);
            groupBox5.Location = new Point(3, 419);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(235, 134);
            groupBox5.TabIndex = 13;
            groupBox5.TabStop = false;
            groupBox5.Text = "Message log";
            // 
            // NoCon_label
            // 
            NoCon_label.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            NoCon_label.ForeColor = Color.Red;
            NoCon_label.Location = new Point(6, 22);
            NoCon_label.Name = "NoCon_label";
            NoCon_label.Size = new Size(223, 106);
            NoCon_label.TabIndex = 1;
            NoCon_label.Text = "There is no connection to the microcontroller";
            NoCon_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // messageLog_rtb
            // 
            messageLog_rtb.BackColor = Color.White;
            messageLog_rtb.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
            messageLog_rtb.Location = new Point(6, 22);
            messageLog_rtb.Name = "messageLog_rtb";
            messageLog_rtb.ReadOnly = true;
            messageLog_rtb.ScrollBars = RichTextBoxScrollBars.Vertical;
            messageLog_rtb.Size = new Size(223, 106);
            messageLog_rtb.TabIndex = 0;
            messageLog_rtb.Text = "";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(Sync_button);
            groupBox4.Controls.Add(ReadTime_btn);
            groupBox4.Controls.Add(current_time_label);
            groupBox4.Location = new Point(3, 33);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(238, 100);
            groupBox4.TabIndex = 12;
            groupBox4.TabStop = false;
            groupBox4.Text = "Time controlls";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(ReadProfile_label);
            groupBox3.Controls.Add(panel1);
            groupBox3.Controls.Add(ReadProfile_btn);
            groupBox3.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            groupBox3.Location = new Point(3, 139);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(235, 134);
            groupBox3.TabIndex = 11;
            groupBox3.TabStop = false;
            groupBox3.Text = "Read profile from microcontroller";
            // 
            // ReadProfile_label
            // 
            ReadProfile_label.Location = new Point(3, 50);
            ReadProfile_label.Name = "ReadProfile_label";
            ReadProfile_label.Size = new Size(229, 23);
            ReadProfile_label.TabIndex = 14;
            ReadProfile_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(ReadFile_btn);
            panel1.Controls.Add(ReadProfileFile_tb);
            panel1.Controls.Add(ReadProfileFile_btn);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(3, 81);
            panel1.Name = "panel1";
            panel1.Size = new Size(229, 50);
            panel1.TabIndex = 13;
            // 
            // ReadFile_btn
            // 
            ReadFile_btn.Location = new Point(196, 27);
            ReadFile_btn.Name = "ReadFile_btn";
            ReadFile_btn.Size = new Size(27, 23);
            ReadFile_btn.TabIndex = 11;
            ReadFile_btn.Text = "...";
            ReadFile_btn.UseVisualStyleBackColor = true;
            ReadFile_btn.Click += ReadFile_btn_Click;
            // 
            // ReadProfileFile_tb
            // 
            ReadProfileFile_tb.Location = new Point(0, 27);
            ReadProfileFile_tb.Name = "ReadProfileFile_tb";
            ReadProfileFile_tb.Size = new Size(192, 23);
            ReadProfileFile_tb.TabIndex = 6;
            ReadProfileFile_tb.TextChanged += ReadProfileFile_tb_TextChanged;
            ReadProfileFile_tb.Leave += ReadProfileFile_tb_Leave;
            // 
            // ReadProfileFile_btn
            // 
            ReadProfileFile_btn.Dock = DockStyle.Top;
            ReadProfileFile_btn.Location = new Point(0, 0);
            ReadProfileFile_btn.Name = "ReadProfileFile_btn";
            ReadProfileFile_btn.Size = new Size(229, 23);
            ReadProfileFile_btn.TabIndex = 5;
            ReadProfileFile_btn.Text = "Read profile into file";
            ReadProfileFile_btn.UseVisualStyleBackColor = true;
            ReadProfileFile_btn.Click += ReadProfileFile_btn_Click;
            // 
            // ReadProfile_btn
            // 
            ReadProfile_btn.Dock = DockStyle.Top;
            ReadProfile_btn.Location = new Point(3, 19);
            ReadProfile_btn.Name = "ReadProfile_btn";
            ReadProfile_btn.Size = new Size(229, 23);
            ReadProfile_btn.TabIndex = 4;
            ReadProfile_btn.Text = "Read profile into editor";
            ReadProfile_btn.UseVisualStyleBackColor = true;
            ReadProfile_btn.Click += ReadProfile_btn_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(WriteProfile_label);
            groupBox2.Controls.Add(panel2);
            groupBox2.Controls.Add(WriteProfile_btn);
            groupBox2.Location = new Point(3, 279);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(235, 134);
            groupBox2.TabIndex = 10;
            groupBox2.TabStop = false;
            groupBox2.Text = "Write profile to microcontroller";
            // 
            // WriteProfile_label
            // 
            WriteProfile_label.Location = new Point(3, 50);
            WriteProfile_label.Name = "WriteProfile_label";
            WriteProfile_label.Size = new Size(229, 23);
            WriteProfile_label.TabIndex = 14;
            WriteProfile_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Controls.Add(WriteFile_btn);
            panel2.Controls.Add(WriteProfileFile_btn);
            panel2.Controls.Add(WriteProfileFile_tb);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(3, 81);
            panel2.Name = "panel2";
            panel2.Size = new Size(229, 50);
            panel2.TabIndex = 13;
            // 
            // WriteFile_btn
            // 
            WriteFile_btn.Location = new Point(196, 26);
            WriteFile_btn.Name = "WriteFile_btn";
            WriteFile_btn.Size = new Size(27, 23);
            WriteFile_btn.TabIndex = 10;
            WriteFile_btn.Text = "...";
            WriteFile_btn.UseVisualStyleBackColor = true;
            WriteFile_btn.Click += WriteFile_btn_Click;
            // 
            // WriteProfileFile_btn
            // 
            WriteProfileFile_btn.Dock = DockStyle.Top;
            WriteProfileFile_btn.Location = new Point(0, 0);
            WriteProfileFile_btn.Name = "WriteProfileFile_btn";
            WriteProfileFile_btn.Size = new Size(229, 23);
            WriteProfileFile_btn.TabIndex = 8;
            WriteProfileFile_btn.Text = "Write profile from file";
            WriteProfileFile_btn.UseVisualStyleBackColor = true;
            WriteProfileFile_btn.Click += WriteProfileFile_btn_Click;
            // 
            // WriteProfileFile_tb
            // 
            WriteProfileFile_tb.Location = new Point(0, 27);
            WriteProfileFile_tb.Name = "WriteProfileFile_tb";
            WriteProfileFile_tb.Size = new Size(192, 23);
            WriteProfileFile_tb.TabIndex = 9;
            WriteProfileFile_tb.TextChanged += WriteProfileFile_tb_TextChanged;
            WriteProfileFile_tb.Leave += WriteProfileFile_tb_Leave;
            // 
            // WriteProfile_btn
            // 
            WriteProfile_btn.Dock = DockStyle.Top;
            WriteProfile_btn.Location = new Point(3, 19);
            WriteProfile_btn.Name = "WriteProfile_btn";
            WriteProfile_btn.Size = new Size(229, 23);
            WriteProfile_btn.TabIndex = 7;
            WriteProfile_btn.Text = "Write currnet editor profile";
            WriteProfile_btn.UseVisualStyleBackColor = true;
            WriteProfile_btn.Click += WriteProfile_btn_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(profileManager1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(groupBox1);
            splitContainer1.Size = new Size(1206, 585);
            splitContainer1.SplitterDistance = 961;
            splitContainer1.TabIndex = 6;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1206, 609);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "LED strip controller";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox2.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label current_time_label;
        private Button Sync_button;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem connectionToolStripMenuItem;
        private ToolStripMenuItem connectToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem closeToolStripMenuItem1;
        private Button ReadTime_btn;
        private ProfileManager profileManager1;
        private GroupBox groupBox1;
        private SplitContainer splitContainer1;
        private ToolStripMenuItem saveProfileToolStripMenuItem;
        private ToolStripMenuItem saveProfileAsToolStripMenuItem;
        private ToolStripMenuItem loadProfileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem newProfileToolStripMenuItem;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private Panel panel1;
        private TextBox ReadProfileFile_tb;
        private Button ReadProfileFile_btn;
        private Button ReadProfile_btn;
        private GroupBox groupBox2;
        private Panel panel2;
        private Button WriteProfileFile_btn;
        private Button WriteProfile_btn;
        private TextBox WriteProfileFile_tb;
        private Button ReadFile_btn;
        private Button WriteFile_btn;
        private ToolStripMenuItem messagesToolStripMenuItem;
        private ToolStripMenuItem clearLogToolStripMenuItem;
        private ToolStripMenuItem testToolStripMenuItem;
        private GroupBox groupBox5;
        private RichTextBox messageLog_rtb;
        private ToolStripMenuItem testErrorToolStripMenuItem;
        private Label NoCon_label;
        private Label ReadProfile_label;
        private Label WriteProfile_label;
    }
}