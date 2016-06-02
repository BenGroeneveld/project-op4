using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public static class Program
    {
        public static string Rfid { get; set; }
        public static string StrBedrag { get; set; }
        public static string StrRekeningID { get; set; }
        public static int processId = 0;

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

        private static void runProgram()
        {
            while(true)
            {
                Welkom welkom = new Welkom();
                Application.Run(welkom);
            }
        }
    }
}
