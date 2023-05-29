using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Client_program
{
    public partial class ProfileManager : UserControl, IView
    {
        #region Variables
        private ProfileData pd;

        public ProfileData ProfileData { get { return pd; } }
        private Series s1;
        private ChartArea ca1;

        DataPoint curPoint = null;
        private bool safeMode = false;
        private bool synched = false;

        private object beforeChange = null;
        private bool changeValid = false;
        private string savePath = null;

        private bool initialized = false;
        #endregion

        #region Initializer
        public ProfileManager()
        {
            InitializeComponent();
            pd = new ProfileData("default");
            pd.AttachView(this);

            dataGridView1.DataError += dataGridView1_DataError;
            s1 = chart1.Series[0];
            ca1 = chart1.ChartAreas[0];

            ca1.AxisX.LabelStyle.Format = "HH:mm";
            ca1.AxisX.IntervalType = DateTimeIntervalType.Hours;
            ca1.AxisX.Minimum = 0;
            ca1.AxisX.Maximum = 1;
            ca1.AxisY.Minimum = 0;
            ca1.AxisY.Maximum = 100;

            Font axisFont = new Font("Roboto", 15);
            ca1.AxisX.TitleFont = axisFont;
            ca1.AxisX.Title = "Hours of a day";
            ca1.AxisY.TitleFont = axisFont;
            ca1.AxisY.Title = "PWM Duty cycle [%]";


            foreach (ProfileListElem p in pd.Points)
            {
                s1.Points.AddXY(p.Time, p.Value);
            }

            s1.MarkerSize = 15;
            s1.MarkerStyle = MarkerStyle.Circle;
            s1.BorderWidth = 5;

            initialized = true;
        }

        public void SetPoints(int[] profile)
        {
            pd = new ProfileData(profile);
            Update();
        }
        #endregion

        #region File manager section
        public void NewProfile()
        {
            pd = new ProfileData(0);
            pd.AttachView(this);
            Update();
        }
        public void LoadProfile(string filePath)
        {
            pd = new ProfileData(filePath);
            pd.AttachView(this);
            Update();
        }
        public void SaveProfile(string filePath)
        {
            pd.SaveDocument(filePath);
        }

        public void Save()
        {
            if (savePath == null)
                SaveAs();
            else
            {
                SaveProfile(savePath);
            }
        }
        public void SaveAs()
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
                    savePath = openFileDialog.FileName;
                    SaveProfile(savePath);
                }
            }
        }
        public void LoadAs()
        {
            loading_label.Visible = true;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files(*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadProfile(openFileDialog.FileName);
                    Update();
                }
            }
            loading_label.Visible = false;
        }
        #endregion

        #region IView section
        public Document GetDocument()
        {
            return pd;
        }

        public void UpdateView()
        {
            Update();
        }

        public void Update()
        {
            if (!initialized) return;
            changeValid = false;
            SyncChart();
            SyncGridView();
            if (pd.ValidatePoints())
            {
                valid_label.Text = "Valid";
                valid_label.BackColor = Color.LightGreen;
            }
            else
            {
                valid_label.Text = "Not valid";
                valid_label.BackColor = Color.PaleVioletRed;
            }
            Invalidate();
            changeValid = true;
        }
        #endregion

        #region Adding points
        void AddPoint(Point pos)
        {
            Axis ax = ca1.AxisX;
            Axis ay = ca1.AxisY;

            double x = ax.PixelPositionToValue(pos.X);
            double y = ay.PixelPositionToValue(pos.Y);

            DateTime t = ProfileData.DoubleToTime(x);

            ProfileListElem newData = new ProfileListElem(0, t, Convert.ToInt32(y));

            ProfileListElem item = null;
            foreach (ProfileListElem pgv in pd.Points)
            {
                if (pgv.Time.TimeOfDay < t.TimeOfDay)
                    item = pgv;
                else
                    break;
            }
            if (item == null)
                pd.Points.Insert(0, newData);
            else if (pd.Points.Last().Index == item.Index)
                pd.Points.Add(newData);
            else
                pd.Points.Insert(item.Index + 1, newData);

            pd.UpdateViews();
        }

        void AddPoint(DateTime t, int value)
        {
            ProfileListElem newData = new ProfileListElem(0, t, value);
            ProfileListElem item = null;
            foreach (ProfileListElem pgv in pd.Points)
            {
                if (pgv.Time.TimeOfDay < t.TimeOfDay)
                    item = pgv;
                else
                    break;
            }
            if (item == null)
                pd.Points.Insert(0, newData);
            else if (pd.Points.Last().Index == item.Index)
                pd.Points.Add(newData);
            else
                pd.Points.Insert(item.Index + 1, newData);
            pd.UpdateViews();
        }
        #endregion

        #region Syncronizations

        void SyncAllPoints(ChartArea ca, Series s)
        {
            foreach (DataPoint dp in s.Points) SyncAPoint(ca, s, dp);
            synched = true;
        }

        void SyncAPoint(ChartArea ca, Series s, DataPoint dp)
        {
            float mh = dp.MarkerSize / 2f;
            float px = (float)ca.AxisX.ValueToPixelPosition(dp.XValue);
            float py = (float)ca.AxisY.ValueToPixelPosition(dp.YValues[0]);
            dp.Tag = (new RectangleF(px - mh, py - mh, dp.MarkerSize, dp.MarkerSize));
        }

        private void SyncChart()
        {
            s1.Points.Clear();
            foreach (ProfileListElem p in pd.Points)
            {
                s1.Points.AddXY(p.Time, p.Value);
            }
        }

        private void SyncGridView()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            dataGridView1.DataSource = pd.Points;
            dataGridView1.Refresh();
        }
        #endregion

        //Events

        //
        // Profile manager
        //
        private void ProfileManager_Load(object sender, EventArgs e)
        {
            SyncGridView();
            changeValid = true;
        }

        #region Chart

        private void chart1_Resize(object sender, EventArgs e)
        {
            synched = false;
        }

        private void chart1_PrePaint(object sender, ChartPaintEventArgs e)
        {
            if (!synched) SyncAllPoints(ca1, s1);
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            DataPoint dp = null;
            HitTestResult hit = chart1.HitTest(e.X, e.Y);
            if (hit.PointIndex > 0)
                dp = hit.Series.Points[hit.PointIndex];

            if (dp != null)
                e.Text = $"x: {ProfileData.DoubleToTime(dp.XValue).TimeOfDay}\n\ry: {dp.YValues[0]}";
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                Axis ax = ca1.AxisX;
                Axis ay = ca1.AxisY;

                RectangleF ippRect = InnerPlotPositionClientRectangle(chart1, ca1);
                if (!ippRect.Contains(e.Location)
                    || ay.PixelPositionToValue(e.Y) < 0
                    || ax.PixelPositionToValue(e.X) > 1
                    )
                    return;



                HitTestResult hit = chart1.HitTest(e.X, e.Y);
                if (hit.PointIndex >= 0)
                    curPoint = hit.Series.Points[hit.PointIndex];


                if (curPoint != null)
                {
                    Series s = hit.Series;
                    double dx = ax.PixelPositionToValue(e.X);
                    double dy = ay.PixelPositionToValue(e.Y);

                    int index = s1.Points.IndexOf(curPoint);
                    if (index != 0 && safeMode)
                        if (dx <= s1.Points[index - 1].XValue && curPoint.XValue > s1.Points[index - 1].XValue)
                            return;

                    if (s1.Points.Last() != s1.Points[index] && safeMode)
                        if (dx >= s1.Points[index + 1].XValue && curPoint.XValue < s1.Points[index + 1].XValue)
                            return;

                    curPoint.XValue = dx;
                    curPoint.YValues[0] = dy;
                }
            }
        }

        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (curPoint != null)
            {
                int index = s1.Points.IndexOf(curPoint);

                DateTime t = ProfileData.DoubleToTime(curPoint.XValue);
                pd.Points[index].Time = t;
                pd.Points[index].Value = Convert.ToInt32(curPoint.YValues[0]);
                curPoint = null;
                pd.UpdateViews();
                Update();
            }
        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            double x = ca1.AxisX.PixelPositionToValue(e.X);
            double y = ca1.AxisY.PixelPositionToValue(e.Y);
            if (x < 0 || x > 1 || y < 0 || y > 100) return;
            if (e.Button == MouseButtons.Right)
            {
                Point pos = new Point(e.X, e.Y);
                contextMenuStrip1.Tag = pos;

                editPointCoordinatesToolStripMenuItem.Enabled = false;
                deletePointToolStripMenuItem.Enabled = false;

                HitTestResult hit = chart1.HitTest(e.X, e.Y);
                if (hit.PointIndex >= 0)
                {
                    editPointCoordinatesToolStripMenuItem.Tag = pd.Points[hit.PointIndex];
                    deletePointToolStripMenuItem.Tag = pd.Points[hit.PointIndex].Index;
                    ((ProfileListElem)editPointCoordinatesToolStripMenuItem.Tag).Tag = pd;
                    editPointCoordinatesToolStripMenuItem.Enabled = true;
                    deletePointToolStripMenuItem.Enabled = true;
                }
                contextMenuStrip1.Show(chart1, pos);
                pd.UpdateViews();
                Refresh();
            }
        }
        #endregion

        #region Strip Menu

        private void addPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (contextMenuStrip1.Tag == null)
                throw new Exception("An error has occured while creating new point");
            Point pos = (Point)contextMenuStrip1.Tag;
            AddPoint(pos);
            pd.OrderPoints();
            pd.UpdateViews();
            Update();
            Refresh();
            dataGridView1.Refresh();
        }
        private void editPointCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CoordinateSetter cs = new CoordinateSetter((ProfileListElem)editPointCoordinatesToolStripMenuItem.Tag))
            {
                cs.ShowDialog();
            }
            pd.UpdateViews();
            Update();
            Refresh();
            dataGridView1.Refresh();
        }
        private void deletePointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to delete this data point?";
            string caption = "Deleting";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            if (MessageBox.Show(message, caption, buttons) == DialogResult.Yes)
            {
                
                pd.Points.RemoveAt((int)(deletePointToolStripMenuItem.Tag));
                MessageBox.Show("Data is sucessfuly deleted");
                pd.OrderPoints();
                pd.UpdateViews();
                Update();
                Refresh();
                dataGridView1.Refresh();
            }
        }

        #endregion

        //
        // Check Box 1
        //
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            safeMode = checkBox1.Checked;
        }

        #region Data Grid View
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "timeDataGridViewTextBoxColumn")
                {
                    if (e.Value != null)
                        try
                        {
                            StringBuilder sb = new StringBuilder();
                            DateTime time = DateTime.Parse(e.Value.ToString());

                            sb.Append($"{time.Hour:d2}");
                            sb.Append(":");
                            sb.Append($"{time.Minute:d2}");
                            e.Value = sb.ToString();
                            e.FormattingApplied = true;
                        }
                        catch (FormatException)
                        {
                            e.FormattingApplied = false;
                        }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (!changeValid) return;
                if (e.ColumnIndex == 0)
                {
                    return;
                }
                if (e.ColumnIndex == 2)
                {
                    int d = (int)dataGridView1.Rows[e.RowIndex].Cells[2].Value;
                    if (d < 0 || d > 100)
                    {
                        changeValid = false;
                        if (beforeChange == null)
                        {
                            changeValid = true;
                            throw new Exception("The previous data wasn't saved;\n\r");
                        }
                        dataGridView1.Rows[e.RowIndex].Cells[2].Value = beforeChange;
                        changeValid = true;

                        throw new ArgumentException("The value is out of range");
                    }

                }
                ProfileListElem row = new ProfileListElem(
                    (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value,
                    (DateTime)dataGridView1.Rows[e.RowIndex].Cells[1].Value,
                    (int)dataGridView1.Rows[e.RowIndex].Cells[2].Value
                    );
                pd.Points[row.Index].Time = row.Time;
                pd.Points[row.Index].Value = row.Value;
                pd.OrderPoints();
                pd.UpdateViews();
            }
            catch (ArgumentException ex) { MessageBox.Show(ex.Message); }
            catch (System.FormatException ex)
            {

                MessageBox.Show("The time has an unsupported format");
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            finally { beforeChange = null; }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            DataGridViewCell cell = row.Cells[e.ColumnIndex];
            beforeChange = cell.Value;
            DeleteIndex_tb.Text = row.Cells[0].Value.ToString();
            DeleteTime_tb.Text = $"{((DateTime)row.Cells[1].Value).Hour:d2}:{((DateTime)row.Cells[1].Value).Minute:d2}";
            DeleteValue_tb.Text = row.Cells[2].Value.ToString();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Cells[1].Value = (DateTime)beforeChange;

            MessageBox.Show("The time has an unsupported format\n\r" + e.Exception.Message);
            dataGridView1.Refresh();
            //beforeChange = null;
        }
        #endregion

        #region Element Adding/Deleting section

        private void Add_btn_Click(object sender, EventArgs e)
        {
            AddElement_groupBox.Visible = true;
        }

        private void CancelNewElement_btn_Click(object sender, EventArgs e)
        {
            AddElement_groupBox.Visible = false;
            Value_textBox.Text = null;
            Hour_textBox.Text = null;
        }
        private void AddNewElement_btn_Click(object sender, EventArgs e)
        {
            int hour = 0;
            int min = 0;
            try
            {
                string[] ts = Hour_textBox.Text.Trim().Split(':');
                hour = int.Parse(ts[0]);
                min = int.Parse(ts[1]);
                DateTime t = ProfileData.IntsToTime(hour, min);
                int y = int.Parse(Value_textBox.Text);
                if (y < 0 || y > 100)
                    throw new ArgumentException("The PWM value is out of range");
                AddPoint(t, y);
                Value_textBox.Text = null;
                Hour_textBox.Text = null;
                AddElement_groupBox.Visible = false;
                pd.UpdateViews();
                dataGridView1.Refresh();
                Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }
        
        private void DeleteRow_btn_Click(object sender, EventArgs e)
        {

            string message = "Are you sure you want to delete this data point?";
            string caption = "Deleting";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            if (MessageBox.Show(message, caption, buttons) == DialogResult.Yes)
            {
                pd.Points.RemoveAt(int.Parse(DeleteIndex_tb.Text));
                MessageBox.Show("Data is sucessfuly deleted");
                pd.OrderPoints();
                pd.UpdateViews();
                dataGridView1.Refresh();
                Refresh();
            }
        }
        #endregion

        #region Chart helper functions
        //Helper functions
        RectangleF ChartAreaClientRectangle(Chart chart, ChartArea CA)
        {
            RectangleF CAR = CA.Position.ToRectangleF();
            float pw = chart.ClientSize.Width / 100f;
            float ph = chart.ClientSize.Height / 100f;
            return new RectangleF(pw * CAR.X, ph * CAR.Y, pw * CAR.Width, ph * CAR.Height);
        }
        RectangleF InnerPlotPositionClientRectangle(Chart chart, ChartArea CA)
        {
            RectangleF IPP = CA.InnerPlotPosition.ToRectangleF();
            RectangleF CArp = ChartAreaClientRectangle(chart, CA);

            float pw = CArp.Width / 100f;
            float ph = CArp.Height / 100f;

            return new RectangleF(CArp.X + pw * IPP.X, CArp.Y + ph * IPP.Y,
                                    pw * IPP.Width, ph * IPP.Height);
        }
        #endregion
    }
}
