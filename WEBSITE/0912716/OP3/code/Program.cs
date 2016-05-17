using System;
using System.Threading;
using System.Windows.Forms;

namespace Pinautomaat
{
    public static class Program
    {
        public static string Rfid { get; set; }
        public static string StrBedrag { get; set; }
        public static string StrRekeningID { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            runProgram();
        }

        public static void startProgram()
        {
            privateStartProgram();
        }

        private static void privateStartProgram()
        {
            while(true)
            {
                Application.Run(new Welkom());
                Application.Run(new Pincode());
            }
        }

        private static void runProgram()
        {
            Thread backgroundThread = new Thread(runBackground);
            backgroundThread.Start();
            startProgram();
        }

        private static void runBackground()
        {
            Application.Run(new Background());
        }
    }
}
