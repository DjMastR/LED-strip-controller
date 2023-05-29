using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Client_program
{
    public partial class CoordinateSetter : Form
    {
        private DateTime x;
        private int y;
        private int index;

        private ProfileData profileData;
        public CoordinateSetter(ProfileListElem point)
        {
            InitializeComponent();

            x = point.Time;
            y = point.Value;
            index = point.Index;

            if (point.Tag != null)
            {
                profileData = (ProfileData)point.Tag;
            }
            else
            {
                throw new ArgumentException("The coordinate editor didn't receive the 'Profile Data' reference");
            }

            XCoord.Text = $"{x.Hour:d2}:{x.Minute:d2}";
            YCoord.Text = y.ToString();
        }

        private void EditPoint()
        {
            int hour = 0;
            int min = 0;
            try
            {
                string[] ts = XCoord.Text.Trim().Split(':');
                hour = int.Parse(ts[0]);
                min = int.Parse(ts[1]);
                profileData.Points[index].Time = ProfileData.IntsToTime(hour, min);
            }
            catch
            {
                MessageBox.Show("An error occured in time's format");
            }
            try
            {
                if (int.Parse(YCoord.Text) < 0 || int.Parse(YCoord.Text) > 100)
                    throw new Exception();
                profileData.Points[index].Value = int.Parse(YCoord.Text);
            }
            catch
            {
                MessageBox.Show("An error occured in PWM value's format");
            }
        }

        private void Reorder_button_Click(object sender, EventArgs e)
        {
            EditPoint();
            profileData.OrderPoints();
            profileData.UpdateViews();
            this.Close();
        }

        private void Place_button_Click(object sender, EventArgs e)
        {
            EditPoint();
            profileData.UpdateViews();
        }
    }
}
