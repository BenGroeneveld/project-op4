using System;
using System.Windows.Forms;

namespace Pinautomaat
{
    public static class Program
    {
        public static string Rfid { get; set; }
        public static string StrBedrag { get; set; }
        public static string StrRekeningID { get; set; }
        public static bool SystemGood { get; set; }

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
            SystemGood = false; //USE THIS IN NORMAL SITUATIONS
            //SystemGood = true; //USE THIS FOR DEBUGGING ONLY
            if(MainBackend.checkAllConnections())
            {
                SystemGood = true;
            }
            if(SystemGood)
            {
                while(SystemGood)
                {
                    Application.Run(new Welkom());
                }
            }
            else
            {
                while(!SystemGood)
                {
                    Application.Run(new BuitenGebruik());
                }
            }
        }
    }
}
