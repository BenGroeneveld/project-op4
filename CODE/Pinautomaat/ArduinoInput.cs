using System;
using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;

namespace Pinautomaat
{
    public static class ArduinoInput
    {
        public static string port = "";
        public static SerialPort currentPort;
        public static string strCardID = "";
        public static int connectionCorrect = 128;

        public static void disconnect()
        {
            connectionCorrect = 128;
            MainBackend.LoggedInValue = 0;
            currentPort.Dispose();
            //currentPort.Close();
        }

        public static void connect(int baud, string recognizeText, int loggedInValue)
        {
            while(!isConnected(baud, recognizeText, loggedInValue))
            {
                tryConnect(baud, recognizeText, loggedInValue);
            }
        }

        public static bool isConnected(int baud, string recognizeText, int loggedInValue)
        {
            if(currentPort == null)
            {
                return false;
            }
            else
            {
                if(currentPort.IsOpen)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static void tryConnect(int baud, string recognizeText, int loggedInValue)
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
                try
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
                        break;
                    }
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public static string strRFID()
        {
            while(!strCardID.Contains("ID"))
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

        public static void checkKeypad()
        {
            privateCheckKeypad();
        }

        private static void privateCheckKeypad()
        {
            string str = "";
            while(str.Equals("") || str.Contains("ID"))
            {
                str = strInputText();
            }
            SendKeys.SendWait(str.ToLower());
        }
    }
}
