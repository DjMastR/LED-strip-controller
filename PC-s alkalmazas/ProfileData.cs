using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Client_program
{
    public class ProfileData : Document
    {
        private int[] profile;
        private double[] slope;
        private List<ProfileListElem> points;

        private const int precision = 1;

        public List<ProfileListElem> Points { get { return points; } }

        public int[] Profile
        {
            get { return profile; }
            set
            {
                if (value.Length != minutesPerDay) throw new ArgumentNullException($"The array must be {minutesPerDay} long");
                profile = value;
            }
        }
        private const int minutesPerDay = 1440;

        public static int MinutesPerDay { get { return minutesPerDay; } }


        public ProfileData()
        {
            this.profile = new int[minutesPerDay];
            this.points = new List<ProfileListElem>();
            this.slope = new double[minutesPerDay];
        }

        public ProfileData(int v){
            this.points = new List<ProfileListElem>();
            this.profile = new int[minutesPerDay];
            this.slope = new double[minutesPerDay];
            for (int i = 0; i < minutesPerDay; i++)
            {
                profile[i] = v;
                slope[i] = 0;
            }
                
            this.CalcPoints();
        }

        public ProfileData(string filePath)
        {
            this.points = new List<ProfileListElem>();
            this.profile = new int[minutesPerDay];
            this.slope = new double[minutesPerDay];
            for (int i = 0; i < minutesPerDay; i++)
            {
                profile[i] = 0;
                slope[i] = 0;
            }
                
            if (filePath.Trim().ToLower().CompareTo("default") == 0)
            {
                for (int i = 6 * 60; i < 9 * 60; i++)
                    profile[i] = 100;
                for (int i = 16 * 60; i < 22 * 60; i++)
                    profile[i] = 100;

                slope[6 * 60 - 1] = 100;
                slope[9 * 60 - 1] = -100;
                slope[16 * 60 - 1] = 100;
                slope[22 * 60 - 1] = -100;
                
            }
            else
            {
                LoadDocument(filePath);
            }
            this.CalcPoints();
        }

        public ProfileData(int[] profile)
        {
            this.points = new List<ProfileListElem>();
            this.slope = new double[minutesPerDay];
            this.profile = profile;
            this.CalcSlopeAndPoints();
        }

        public bool ValidatePoints()
        {
            bool valid = true;
            foreach (ProfileListElem p in this.points)
            {
                if (p == points.Last()) break;

                DateTime next = points[p.Index + 1].Time;
                if(next.TimeOfDay <= p.Time.TimeOfDay)
                {
                    valid = false;
                    break;
                }
            }
            return valid;
        }

        public void CalcSlopeAndPoints()
        {
            List<Point> pps = new List<Point>
            {
                new Point(0, profile[0]),
                new Point(minutesPerDay - 1, profile[minutesPerDay - 1])
            };
            int maxError = 0;
            int index = 0;
            int i = 0;
            do
            {
                int cur_error = 0;
                i = 0;
                foreach (Point p in pps)
                {
                    if (p.X == minutesPerDay - 1)
                        break;
                    if (p.X + 1 == pps[i + 1].X)
                    {
                        i++;
                        continue;
                    }

                    double m = (double)(pps[i + 1].Y - p.Y) / (double)(pps[i + 1].X - p.X);
                    int err = 0;
                    int calculated = 0;
                    for (int j = p.X; j < pps[i + 1].X; j++)
                    {
                        calculated = p.Y + Convert.ToInt32(m * (j - p.X));
                        err = Math.Abs(profile[j] - calculated);
                        if (err > cur_error)
                        {
                            cur_error = err;
                            index = j;
                        }
                    }
                    i++;
                }
                if (index != 0)
                {
                    Point temp = new Point(index, profile[index]);
                    for (i = 1; i < pps.Count; i++)
                    {
                        if (index < pps[i].X)
                        {
                            pps.Insert(i, temp);
                            break;
                        }
                    }
                    if (i == pps.Count)
                        pps.Add(temp);
                    index = 0;
                    
                }
                maxError = cur_error;
            }
            while (maxError > precision);

            i = 0;
            foreach (Point p in pps)
            {
                points.Add(new ProfileListElem(i, IndexToTime(p.X), p.Y));
                i++;
            }

            i = 0;
            foreach (ProfileListElem p in this.points)
            {
                if (p == points.Last()) break;

                ProfileListElem next = points[p.Index + 1];
                double dx = (next.Time.TimeOfDay - p.Time.TimeOfDay).TotalMinutes;
                if (dx < 0)
                    throw new Exception("An error has occured while calculating dx");

                double dy = (double)(next.Value - p.Value);
                if (dy < -100 || dy > 100)
                    throw new Exception("An error has occured while calculating dy");
                double m = dy / dx;

                for (i = Convert.ToInt32(p.Time.TimeOfDay.TotalMinutes); i < Convert.ToInt32(next.Time.TimeOfDay.TotalMinutes); i++)
                {
                    slope[i] = m;
                }
            }
        }

        public void CalcProfile()
        {   
            if (points[0].Time.TimeOfDay.TotalMinutes != 0)
                points.Add(new ProfileListElem(0,IntsToTime(0,0), 0));
            if(points.Last().Time.TimeOfDay.TotalMinutes != (1440-1))
                points.Add(new ProfileListElem(0,IntsToTime(23,59), 0));

            if (!ValidatePoints())
                throw new Exception("Make sure to have a valid profile before generating array");

            int i = 0;
            int offset = points[0].Value;
            foreach (ProfileListElem p in this.points)
            {
                if (p == points.Last()) break;
                
                ProfileListElem next = points[p.Index + 1];
                double dx = (next.Time.TimeOfDay - p.Time.TimeOfDay).TotalMinutes;
                if (dx < 0)
                    throw new Exception("An error has occured while calculating dx");

                double dy = (double)(next.Value - p.Value);
                if (dy < -100 || dy > 100)
                    throw new Exception("An error has occured while calculating dy");
                double m = dy / dx;

                for (i = Convert.ToInt32(p.Time.TimeOfDay.TotalMinutes); i < Convert.ToInt32(next.Time.TimeOfDay.TotalMinutes); i++)
                {
                    profile[i] = offset + Convert.ToInt32(m * ((double)(i) - p.Time.TimeOfDay.TotalMinutes));
                    slope[i] = m;
                }
                offset = next.Value;
                
            }
            profile[minutesPerDay - 1] = points.Last().Value;
        }

        void CalcPoints()
        {
            points.Add(new ProfileListElem(0, IndexToTime(0), profile[0]));
            int index = 1;
            int start = 0;
            while(start < minutesPerDay)
            {
                int next;
                for (next = start+1; next < minutesPerDay; next++)
                    if (slope[next] != slope[start])
                        break;
                if (next == minutesPerDay)
                    break;
                if (slope[next] == 100 || slope[next] == -100)
                {
                    points.Add(new ProfileListElem(index++, IndexToTime(next), profile[next]));
                    points.Add(new ProfileListElem(index++, IndexToTime(next + 1), profile[next + 1]));
                    next++;
                }
                else
                {
                    points.Add(new ProfileListElem(index++, IndexToTime(next), profile[next]));
                }
                start = next;
            }
            if (points.Last().Time.TimeOfDay.TotalMinutes != 1440 - 1)
                points.Add(new ProfileListElem(index++, IndexToTime(minutesPerDay - 1), profile[minutesPerDay - 1]));
        }

        public void SyncPoints()
        {
            foreach(ProfileListElem p in points)
            {
                p.Index = this.points.IndexOf(p);
            }

        }

        public void OrderPoints()
        {
            points.Sort(ProfileListElem.SortProfileListElements);
        }

        static public int TimeToIndex(DateTime time)
        {
            return time.Hour * 60 + time.Minute;
        }

        static public DateTime IndexToTime(int index)
        {
            if(index == minutesPerDay)
                index = 0;
            return new DateTime(2023,5,20,index/60, index%60,0);
        }

        static public DateTime DoubleToTime(double v)
        {
            if (v < 0.001)
                v = 0;
            int hour = (int)(v * 1440d / 60d);
            int minute = (int)(v * 1440d) % 60;
            DateTime t = IntsToTime(hour, minute);
            return t;
        }

        static public DateTime IntsToTime(int hour, int minute)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute,0);
        }

        public void UpdateViews()
        {
            this.SyncPoints();
            this.UpdateAllViews();
        }
        public override void SaveDocument(string filePath)
        {
            try
            {
                CalcProfile();
                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    for (int i = 0; i < minutesPerDay; i++)
                    {
                        sw.WriteLine($"{i};{profile[i]:d3};{slope[i]:F10}");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public override void LoadDocument(string filePath) 
        {
            using(StreamReader sr = new StreamReader(filePath, false))
            {
                string s;
                while((s=sr.ReadLine()) != null)
                {
                    Trace.WriteLine(s);
                    s.Trim();
                    string[] columns = s.Split(";");
                    int index = int.Parse(columns[0]);
                    int value = int.Parse(columns[1]);
                    double m = double.Parse(columns[2]);
                    profile[index] = value;
                    slope[index] = m;
                }
            }
        }
    }
}
