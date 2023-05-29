namespace Client_program
{
    partial class ProfileManager
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            contextMenuStrip1 = new ContextMenuStrip(components);
            addPointToolStripMenuItem = new ToolStripMenuItem();
            editPointCoordinatesToolStripMenuItem = new ToolStripMenuItem();
            checkBox1 = new CheckBox();
            dataGridView1 = new DataGridView();
            indexDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            timeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            valueDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            profileGridViewBindingSource = new BindingSource(components);
            Add_btn = new Button();
            Hour_textBox = new TextBox();
            Value_textBox = new TextBox();
            AddNewElement_btn = new Button();
            CancelNewElement_btn = new Button();
            DeleteRow_btn = new Button();
            splitContainer1 = new SplitContainer();
            AddElement_groupBox = new GroupBox();
            DeleteValue_tb = new TextBox();
            DeleteTime_tb = new TextBox();
            DeleteIndex_tb = new TextBox();
            valid_label = new Label();
            loading_label = new Label();
            deletePointToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)profileGridViewBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            AddElement_groupBox.SuspendLayout();
            SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            chart1.Location = new Point(3, 28);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "PWM";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            chart1.Series.Add(series1);
            chart1.Size = new Size(939, 367);
            chart1.TabIndex = 0;
            chart1.Text = "chart1";
            chart1.GetToolTipText += chart1_GetToolTipText;
            chart1.PrePaint += chart1_PrePaint;
            chart1.SizeChanged += chart1_Resize;
            chart1.MouseDown += chart1_MouseDown;
            chart1.MouseMove += chart1_MouseMove;
            chart1.MouseUp += chart1_MouseUp;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { addPointToolStripMenuItem, editPointCoordinatesToolStripMenuItem, deletePointToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(193, 92);
            // 
            // addPointToolStripMenuItem
            // 
            addPointToolStripMenuItem.Name = "addPointToolStripMenuItem";
            addPointToolStripMenuItem.Size = new Size(192, 22);
            addPointToolStripMenuItem.Text = "Add Point";
            addPointToolStripMenuItem.Click += addPointToolStripMenuItem_Click;
            // 
            // editPointCoordinatesToolStripMenuItem
            // 
            editPointCoordinatesToolStripMenuItem.Name = "editPointCoordinatesToolStripMenuItem";
            editPointCoordinatesToolStripMenuItem.Size = new Size(192, 22);
            editPointCoordinatesToolStripMenuItem.Text = "Edit Point Coordinates";
            editPointCoordinatesToolStripMenuItem.Click += editPointCoordinatesToolStripMenuItem_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(860, 4);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(82, 19);
            checkBox1.TabIndex = 1;
            checkBox1.Text = "Safe Mode";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { indexDataGridViewTextBoxColumn, timeDataGridViewTextBoxColumn, valueDataGridViewTextBoxColumn });
            dataGridView1.DataSource = profileGridViewBindingSource;
            dataGridView1.Location = new Point(3, 401);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(374, 171);
            dataGridView1.TabIndex = 2;
            dataGridView1.CellEnter += dataGridView1_CellEnter;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            // 
            // indexDataGridViewTextBoxColumn
            // 
            indexDataGridViewTextBoxColumn.DataPropertyName = "Index";
            indexDataGridViewTextBoxColumn.DividerWidth = 2;
            indexDataGridViewTextBoxColumn.HeaderText = "Index";
            indexDataGridViewTextBoxColumn.Name = "indexDataGridViewTextBoxColumn";
            indexDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // timeDataGridViewTextBoxColumn
            // 
            timeDataGridViewTextBoxColumn.DataPropertyName = "Time";
            timeDataGridViewTextBoxColumn.DividerWidth = 2;
            timeDataGridViewTextBoxColumn.HeaderText = "Time";
            timeDataGridViewTextBoxColumn.Name = "timeDataGridViewTextBoxColumn";
            // 
            // valueDataGridViewTextBoxColumn
            // 
            valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            valueDataGridViewTextBoxColumn.DividerWidth = 2;
            valueDataGridViewTextBoxColumn.HeaderText = "Value";
            valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            // 
            // profileGridViewBindingSource
            // 
            profileGridViewBindingSource.DataSource = typeof(ProfileListElem);
            // 
            // Add_btn
            // 
            Add_btn.Location = new Point(12, 20);
            Add_btn.Name = "Add_btn";
            Add_btn.Size = new Size(130, 34);
            Add_btn.TabIndex = 3;
            Add_btn.Text = "Add new point";
            Add_btn.UseVisualStyleBackColor = true;
            Add_btn.Click += Add_btn_Click;
            // 
            // Hour_textBox
            // 
            Hour_textBox.Location = new Point(6, 17);
            Hour_textBox.Name = "Hour_textBox";
            Hour_textBox.PlaceholderText = "Time (HH:mm)";
            Hour_textBox.Size = new Size(127, 23);
            Hour_textBox.TabIndex = 4;
            // 
            // Value_textBox
            // 
            Value_textBox.Location = new Point(6, 41);
            Value_textBox.Name = "Value_textBox";
            Value_textBox.PlaceholderText = "PWM value (0-100%)";
            Value_textBox.Size = new Size(127, 23);
            Value_textBox.TabIndex = 5;
            // 
            // AddNewElement_btn
            // 
            AddNewElement_btn.Location = new Point(139, 22);
            AddNewElement_btn.Name = "AddNewElement_btn";
            AddNewElement_btn.Size = new Size(128, 35);
            AddNewElement_btn.TabIndex = 6;
            AddNewElement_btn.Text = "Add";
            AddNewElement_btn.UseVisualStyleBackColor = true;
            AddNewElement_btn.Click += AddNewElement_btn_Click;
            // 
            // CancelNewElement_btn
            // 
            CancelNewElement_btn.Location = new Point(273, 22);
            CancelNewElement_btn.Name = "CancelNewElement_btn";
            CancelNewElement_btn.Size = new Size(128, 35);
            CancelNewElement_btn.TabIndex = 7;
            CancelNewElement_btn.Text = "Cancel";
            CancelNewElement_btn.UseVisualStyleBackColor = true;
            CancelNewElement_btn.Click += CancelNewElement_btn_Click;
            // 
            // DeleteRow_btn
            // 
            DeleteRow_btn.Location = new Point(12, 26);
            DeleteRow_btn.Name = "DeleteRow_btn";
            DeleteRow_btn.Size = new Size(130, 34);
            DeleteRow_btn.TabIndex = 8;
            DeleteRow_btn.Text = "Delete selected row";
            DeleteRow_btn.UseVisualStyleBackColor = true;
            DeleteRow_btn.Click += DeleteRow_btn_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new Point(383, 401);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(AddElement_groupBox);
            splitContainer1.Panel1.Controls.Add(Add_btn);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(DeleteValue_tb);
            splitContainer1.Panel2.Controls.Add(DeleteTime_tb);
            splitContainer1.Panel2.Controls.Add(DeleteIndex_tb);
            splitContainer1.Panel2.Controls.Add(DeleteRow_btn);
            splitContainer1.Size = new Size(558, 171);
            splitContainer1.SplitterDistance = 75;
            splitContainer1.TabIndex = 9;
            // 
            // AddElement_groupBox
            // 
            AddElement_groupBox.Controls.Add(AddNewElement_btn);
            AddElement_groupBox.Controls.Add(Value_textBox);
            AddElement_groupBox.Controls.Add(Hour_textBox);
            AddElement_groupBox.Controls.Add(CancelNewElement_btn);
            AddElement_groupBox.Location = new Point(148, 3);
            AddElement_groupBox.Name = "AddElement_groupBox";
            AddElement_groupBox.Size = new Size(407, 69);
            AddElement_groupBox.TabIndex = 8;
            AddElement_groupBox.TabStop = false;
            AddElement_groupBox.Visible = false;
            // 
            // DeleteValue_tb
            // 
            DeleteValue_tb.Location = new Point(422, 33);
            DeleteValue_tb.Name = "DeleteValue_tb";
            DeleteValue_tb.ReadOnly = true;
            DeleteValue_tb.Size = new Size(127, 23);
            DeleteValue_tb.TabIndex = 11;
            // 
            // DeleteTime_tb
            // 
            DeleteTime_tb.Location = new Point(288, 33);
            DeleteTime_tb.Name = "DeleteTime_tb";
            DeleteTime_tb.ReadOnly = true;
            DeleteTime_tb.Size = new Size(127, 23);
            DeleteTime_tb.TabIndex = 10;
            // 
            // DeleteIndex_tb
            // 
            DeleteIndex_tb.Location = new Point(154, 33);
            DeleteIndex_tb.Name = "DeleteIndex_tb";
            DeleteIndex_tb.ReadOnly = true;
            DeleteIndex_tb.Size = new Size(127, 23);
            DeleteIndex_tb.TabIndex = 9;
            // 
            // valid_label
            // 
            valid_label.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            valid_label.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            valid_label.Location = new Point(742, 0);
            valid_label.Name = "valid_label";
            valid_label.Size = new Size(94, 28);
            valid_label.TabIndex = 10;
            valid_label.Text = "Validity";
            valid_label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // loading_label
            // 
            loading_label.AutoSize = true;
            loading_label.Location = new Point(3, 4);
            loading_label.Name = "loading_label";
            loading_label.Size = new Size(158, 15);
            loading_label.TabIndex = 11;
            loading_label.Text = "The file is currently loading...";
            loading_label.Visible = false;
            // 
            // deletePointToolStripMenuItem
            // 
            deletePointToolStripMenuItem.Name = "deletePointToolStripMenuItem";
            deletePointToolStripMenuItem.Size = new Size(192, 22);
            deletePointToolStripMenuItem.Text = "Delete Point";
            deletePointToolStripMenuItem.Click += deletePointToolStripMenuItem_Click;
            // 
            // ProfileManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(loading_label);
            Controls.Add(valid_label);
            Controls.Add(splitContainer1);
            Controls.Add(dataGridView1);
            Controls.Add(checkBox1);
            Controls.Add(chart1);
            Name = "ProfileManager";
            Size = new Size(944, 575);
            Load += ProfileManager_Load;
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)profileGridViewBindingSource).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            AddElement_groupBox.ResumeLayout(false);
            AddElement_groupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem addPointToolStripMenuItem;
        private CheckBox checkBox1;
        private DataGridView dataGridView1;
        private BindingSource profileGridViewBindingSource;
        private ToolStripMenuItem editPointCoordinatesToolStripMenuItem;
        private Button Add_btn;
        private TextBox Hour_textBox;
        private TextBox Value_textBox;
        private Button AddNewElement_btn;
        private Button CancelNewElement_btn;
        private Button DeleteRow_btn;
        private SplitContainer splitContainer1;
        private GroupBox AddElement_groupBox;
        private TextBox DeleteValue_tb;
        private TextBox DeleteTime_tb;
        private TextBox DeleteIndex_tb;
        private DataGridViewTextBoxColumn indexDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn timeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private Label valid_label;
        private Label loading_label;
        private ToolStripMenuItem deletePointToolStripMenuItem;
    }
}
