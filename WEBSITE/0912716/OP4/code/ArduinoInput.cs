using System;
using System.Threading;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;

namespace Pinautomaat
{
    public static class ArduinoInput
    {
        private static string port = "";
        private static SerialPort currentPort;
        private static int connectionCorrect = 128;

        public static void connect(int baud, string recognizeText, int loggedInValue)
        {
            tryConnect(baud, recognizeText, loggedInValue);
        }

        public static bool isConnected()
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
                    currentPort.WriteTimeout = 1000;
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
                    else
                    {
                        currentPort.Close();
                    }
                }
                catch
                {

                }
            }
        }

        public static string strRFID()
        {
            string str = "";
            
            while(!str.StartsWith("PasID: ") && isConnected())
            {
                str = currentPort.ReadLine().Trim();
            }
            str = str.Remove(0, 7);
            if(str.EndsWith("126"))
            {
                str = "1000";
                MainBackend.AdminKaart = true;
            }
            else
            {
                MainBackend.AdminKaart = false;
            }
            return str;
        }

        public static string strInputText()
        {
            string str = "";
            while(str.Equals("") || str.Contains("*") || str.Contains("#") || str.Contains("PasID"))
            {
                str = currentPort.ReadLine().ToString().Trim();
            }
            return str;
        }

        public static void checkKeypad(Form f)
        {
            privateCheckKeypad(f);
        }

        private static void privateCheckKeypad(Form f)
        {
            if(f.Focused)
            {
                string str = "";
                while(str.Equals("") || str.Contains("PasID"))
                {
                    str = strInputText();
                }
                SendKeys.SendWait(str.ToLower());
            }
        }
    }
}
