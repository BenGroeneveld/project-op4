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

        private static void runBackground()
        {
            Thread thBackground = new Thread(bg);
            thBackground.Start();
        }

        private static void bg()
        {
            Background bg = new Background();
            Application.Run(bg);
        }

        private static void runProgram()
        {
            SystemGood = false; //USE THIS IN NORMAL SITUATIONS
            //SystemGood = true; //USE THIS FOR DEBUGGING ONLY

            runBackground();
            while(true)
            {
                if(MainBackend.checkAllConnections())
                {
                    SystemGood = true;
                }
                if(SystemGood)
                {
                    Application.Run(new Welkom());
                }
                else
                {
                    Application.Run(new BuitenGebruik());
                }
            }
        }
    }
}
