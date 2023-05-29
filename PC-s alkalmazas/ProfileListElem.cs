using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_program
{
    public class ProfileListElem
    {
        public int Index{get; set;}
        public DateTime Time { get; set; }
        public int Value { get; set; }

        public object Tag{ get; set; }

        public ProfileListElem(int i, DateTime time, int value) 
        {
            Index = i;
            Time = time;
            Value = value;

            Tag = null;
        }

        public static int SortProfileListElements(ProfileListElem x, ProfileListElem y)
        {

            return x.Time.TimeOfDay.CompareTo(y.Time.TimeOfDay);
        }
    }
}
