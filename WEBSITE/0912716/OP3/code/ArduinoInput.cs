using System;
using System.Threading;
using System.IO.Ports;

namespace Pinautomaat
{
    public static class ArduinoInput
    {
        public static string port = "";
        public static SerialPort currentPort;
        public static string strCardID = "";
        public static int connectionCorrect = 128;

        public static void connect(int baud, string recognizeText, int loggedInValue)
        {
            if(!isConnected(baud, recognizeText, loggedInValue))
            {
                tryConnect(baud, recognizeText, loggedInValue);
            }
            else
            {
                currentPort.Close();
            }
        }

        public static bool isConnected(int baud, string recognizeText, int loggedInValue)
        {
            if(currentPort == null)
            {
                return false;
            }
            else if(currentPort.IsOpen)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void tryConnect(int baud, string recognizeText, int loggedInValue)
        {
            try
            {
                byte[] buffer = new byte[5];
                buffer[0] = Convert.ToByte(16);
                buffer[1] = Convert.ToByte(connectionCorrect);
                buffer[2] = Convert.ToByte(0);
                buffer[3] = Convert.ToByte(loggedInValue);
                buffer[4] = Convert.ToByte(4);

                int intReturnASCII = 0;
                char charReturnValue = (Char)intReturnASCII;

                string[] ports = SerialPort.GetPortNames();
                foreach(string newport in ports)
                {
                    currentPort = new SerialPort(newport, baud);
                    currentPort.Open();
                    currentPort.Write(buffer, 0, 5);
                    Thread.Sleep(1000);
                    int count = currentPort.BytesToRead;
                    string returnMessage = "";

                    while(count > 0)
                    {
                        intReturnASCII = currentPort.ReadByte();
                        returnMessage = returnMessage + Convert.ToChar(intReturnASCII);
                        count--;
                    };
                    port = newport;
                    if(returnMessage.Contains(recognizeText))
                    {
                        connectionCorrect = 127;
                    }
                }
            }
            catch(UnauthorizedAccessException e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        public static string strRFID()
        {
            string strCard = "ID";
            while(!strCardID.Contains(strCard))
            {
                strCardID = currentPort.ReadLine().ToString().Trim();
            }

            return strCardID;
        }

        public static string strInputText()
        {
            string str = "";
            while(str.Equals("") || str.Contains("*") || str.Contains("#") || str.Contains("ID"))
            {
                str = currentPort.ReadLine().ToString().Trim();
            }

            return str;
        }

        public static string strInputNumber()
        {
            string str = "";
            while(str.Equals("") || str.Contains("A") || str.Contains("B") || str.Contains("C") || str.Contains("D") || str.Contains("*") || str.Contains("#"))
            {
                str = currentPort.ReadLine().ToString().Trim();
            }

            return str;
        }

        public static int intInputText()
        {
            string str = "";
            while(str.Equals("") || str.Contains("A") || str.Contains("B") || str.Contains("C") || str.Contains("D") || str.Contains("*") || str.Contains("#"))
            {
                str = currentPort.ReadLine().ToString().Trim();
            }

            int i = Convert.ToInt32(str);
            return i;
        }
    }
}
