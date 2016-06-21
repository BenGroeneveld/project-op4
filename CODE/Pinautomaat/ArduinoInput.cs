﻿using System;
using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;

namespace Pinautomaat
{
    public static class ArduinoInput
    {
        public static string port = "";
        public static SerialPort currentPort;
        public static int connectionCorrect = 128;

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
                }
                catch
                {

                }
            }
        }

        public static string strRFID()
        {
            string str = "";
            while(!str.StartsWith("PasID: "))
            {
                str = currentPort.ReadLine().ToString().Trim();
            }
            str = str.Remove(0, 7);

            if(str.EndsWith("125"))
            {
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
            //SendKeys.Send(str.ToLower());
        }
    }
}
