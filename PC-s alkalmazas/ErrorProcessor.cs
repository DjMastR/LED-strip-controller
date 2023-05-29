using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_program
{
    public class ErrorProcessor
    {
        enum Commands{ NoCommand, WriteTime, ReadTime, WriteProfile, ReadProfile, Test, TestError };
        enum ErrorType
        {
            NoError, InterruptACK, ReadTimeError, WriteDigits, SetTime, TimeSending,
            ProfileSending, CRCError, TestingError, UnkownCommand, MissingEndChar
        };
        enum Status {Ok, Error, Busy, Timeout };

        public static string ProcessErrorInts(int[] data, bool test)
        {
            try
            {
                if (data.Last() != 0xf0 || data.Count() != 5) return "The error message wasn't received correctly";

                Commands command = (Commands)data[1];
                Status stat = (Status)data[2];
                ErrorType type = (ErrorType)data[3];

                string message = "";
                bool commandNeeded = false;
                switch (type)
                {
                    case ErrorType.NoError:
                        message = "No error type was set";
                        commandNeeded = true;
                        break;
                    case ErrorType.InterruptACK:
                        message = "Couldn't acknowledge the clock interrupt";
                        break;
                    case ErrorType.ReadTimeError:
                        message = "Couldn't read the current time from clock";
                        break;
                    case ErrorType.WriteDigits:
                        message = "Couldn't write out digits to the screen";
                        break;
                    case ErrorType.SetTime:
                        message = "Couldn't write time into clock";
                        break;
                    case ErrorType.TimeSending:
                        message = "Couldn't send time to client";
                        break;
                    case ErrorType.ProfileSending:
                        message = "Couldn't send profile to client";
                        break;
                    case ErrorType.CRCError:
                        message = "A CRC error has occured in the received profile";
                        break;
                    case ErrorType.TestingError:
                        message = "Couldn't test the communication with client";
                        break;
                    case ErrorType.UnkownCommand:
                        message = "Received an unkown command";
                        commandNeeded = true;
                        break;
                    case ErrorType.MissingEndChar:
                        message = "Didn't receive end character";
                        commandNeeded = true;
                        break;
                    default:
                        message = "Unknown error happened";
                        commandNeeded = true;
                        break;
                }

                if (commandNeeded)
                {
                    switch (command)
                    {
                        case Commands.WriteTime:
                            message += " in 'WriteTime' command";
                            break;
                        case Commands.ReadTime:
                            message += " in 'ReadTime' command";
                            break;
                        case Commands.WriteProfile:
                            message += " in 'WriteProfile' command";
                            break;
                        case Commands.ReadProfile:
                            message += " in 'ReadProfile' command";
                            break;
                        case Commands.Test:
                            message += " in 'Test' command";
                            break;
                        case Commands.TestError:
                            message += " in 'Test' command";
                            break;
                        case Commands.NoCommand:
                        default:
                            message += " in unknown command";
                            break;
                    }
                }

                switch (stat)
                {
                    case Status.Ok:
                        message += "; Status: OK";
                        break;
                    case Status.Busy:
                        message += "; Status: Busy";
                        break;
                    case Status.Error:
                        message += "; Status: Error";
                        break;
                    case Status.Timeout:
                        message += "; Status: Timeout";
                        break;
                }


                return message;
            }
            catch (Exception ex){ return ex.Message; }

        }
    }
}
