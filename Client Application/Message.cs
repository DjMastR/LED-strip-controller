using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_program
{
    public enum Commands{
        NoCommand, ReadTime, WriteTime, ReadProfile, WriteProfile, Test, TestError
    }
    public class Message
    {
        //Data fields
        private Commands command;
        private List<int> data;

        public Commands Command
        {
            get { return command; }
            set { command = value; }
        }
        public List<int> Data
        {
            get { return data; }
        }

        //Constructors
        public Message() 
        {
            this.data = new List<int>();
            command = Commands.NoCommand;
        }

        public Message(Commands command)
        {
            this.command = command;
            this.data = new List<int>();
        }

        //Funcitons
        public void Add(int value)
        {
            switch (command)
            {
                case Commands.NoCommand:
                    throw new ArgumentException("Set command first, then try to set data field", "command");
                case Commands.ReadTime:
                    throw new ArgumentException("You can not set data in 'ReadTime' command mode", "command");
                case Commands.ReadProfile: 
                    throw new ArgumentException("You can not set data in 'ReadProfile' command mode", "command");
                case Commands.Test:
                    throw new ArgumentException("You can not set data in 'Test' command mode", "command");
                case Commands.TestError:
                    throw new ArgumentException("You can not set data in 'Test' command mode", "command");
                case Commands.WriteTime:
                    if(data.Count < ProfileData.MinutesPerDay)
                        if (value > 100 || value < 0)
                            throw new ArgumentOutOfRangeException("value", "Time param should be in 0<=x<=99 range");
                    this.data.Add(value);
                    break;
                case Commands.WriteProfile:
                    if (this.data.Count < 1440)
                        if (value > 100 || value < 0)
                            throw new ArgumentOutOfRangeException("value", "Time param should be in 0<=x<=99 range");
                    this.data.Add(value);
                    break;
            }
        }

        public bool CheckValidity()
        {
            bool valid = false;

            switch (command)
            {
                case Commands.NoCommand:
                    return false;
                case Commands.ReadTime:
                    if(this.data.Count == 0)
                        valid = true;
                    break;
                case Commands.ReadProfile:
                    if(this.data.Count == 0)
                        valid = true;
                    break;
                case Commands.Test:
                    if (this.data.Count == 0)
                        valid = true;
                    break;
                case Commands.TestError:
                    if (this.data.Count == 0)
                        valid = true;
                    break;
                case Commands.WriteTime:
                    if(this.data.Count == 8)
                    {
                        int max = 0xFF;
                        for(int i = 0; i < 8; i++)
                        {
                            switch (i) 
                            {
                                //Year
                                case 0:
                                case 1:
                                    max = 99;
                                    break;
                                //Month
                                case 2:
                                    max = 12;
                                    break;
                                //Day
                                case 3:
                                    max = 31;
                                    break;
                                //Day of the week
                                case 4:
                                    max = 6;
                                    break;
                                //Hour
                                case 5:
                                    max = 24;
                                    break;
                                //Min, sec
                                case 6:
                                case 7:
                                    max = 60;
                                    break;
                            }
                            //Checking each element
                            if (this.data[i] < 0 || this.data[i] > max)
                                return false;
                        }
                        valid = true;
                    }
                    break;
                case Commands.WriteProfile:
                    if(this.data.Count == 1444)
                    {
                        for(int i = 0; i < ProfileData.MinutesPerDay; i++)
                        {
                            if (data[i] < 0 || data[i] > 100)
                                return false;
                        }
                        valid = true;
                    }
                    break;
            }
            return valid;
        }

        public override string ToString()
        {
            switch (command)
            {
                case Commands.NoCommand:
                    return "No command is set";
                case Commands.ReadTime:
                    return "Command: Read the time";
                case Commands.ReadProfile:
                    return "Command: Read the profile";
                case Commands.Test:
                    return "Command: Test communication";
                case Commands.TestError:
                    return "Command: Test error communication";
                case Commands.WriteTime:
                    if (this.CheckValidity())
                        return String.Format("Command: Write {0:D2}{1:D2}.{2:D2}.{3:D2}. ({4}) {5:D2}:{6:D2}:{7:D2} time",
                            data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7]);
                    else
                        return "The command is incomplete or invalid";
                case Commands.WriteProfile:
                    if (this.CheckValidity())
                        return String.Format("Command: Write {0} datapoint into profile memory", data.Count);
                    else
                        return "The command is incomplete or invalid";
                default:
                    return "Unkown command is in the message";
            }
        }

        public void SetData(DateTime time)
        {
            this.Add(time.Year / 100);
            this.Add(time.Year % 100);
            this.Add(time.Month);
            this.Add(time.Day);

            int DOW;
            if (time.DayOfWeek == DayOfWeek.Sunday)
                DOW = 6;
            else
                DOW = (int)time.DayOfWeek - 1;
            this.Add(DOW);

            this.Add(time.Hour);
            this.Add(time.Minute);
            this.Add(time.Second);
        }

        public void SetData(int[] profile)
        {
            byte[] data = new byte[ProfileData.MinutesPerDay];
            for (int i = 0; i < ProfileData.MinutesPerDay; i++)
            {
                this.Add(profile[i]);
                data[i] = (byte)profile[i];
            }
            byte[] crc = CRC.xcrc32_bytes(data);
            for (int i = 0; i < crc.Length; i++)
            {
                this.Add((int)crc[i]);
            }
        }

        public byte[] ToByteFlow()
        {
            byte[] byteFlow;
            int numberOfData = 0;
            switch(this.Command)
            {
                case Commands.NoCommand:
                    byteFlow = new byte[2];
                    byteFlow[0] = (byte)0xFF;
                    break;
                case Commands.ReadTime:
                    byteFlow = new byte[2];
                    byteFlow[0] = (byte)0x80;
                    break;
                case Commands.ReadProfile:
                    byteFlow = new byte[2];
                    byteFlow[0] = (byte)0x81;
                    break;
                case Commands.Test:
                    byteFlow = new byte[2];
                    byteFlow[0] = (byte)0x84;
                    break;
                case Commands.TestError:
                    byteFlow = new byte[2];
                    byteFlow[0] = (byte)0x8f;
                    break;
                case Commands.WriteTime:
                    byteFlow = new byte[10];
                    byteFlow[0] = (byte)0x82;
                    numberOfData = 8;
                    break;
                case Commands.WriteProfile:
                    byteFlow = new byte[1440+4+2];
                    byteFlow[0] = (byte)0x83;
                    numberOfData = 1444;
                    break;
                default:
                    byteFlow = new byte[2];
                    byteFlow[0] = 0xFF;
                    break;

            }
            for(int i = 0; i < numberOfData; i++)
            {
                byteFlow[i+1] = (byte)this.data[i];
            }
            byteFlow[byteFlow.Length-1] = (byte)0xF0;
            return byteFlow;
        }
    }
}
