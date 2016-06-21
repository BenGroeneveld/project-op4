using System;
using System.Threading;
using System.Windows.Forms;

namespace Pinautomaat
{
    public static class Program
    {
        private static bool SystemGood
        {
            get; set;
        }

        public static string PasID
        {
            get; set;
        }
        public static int Poging
        {
            get; set;
        }
        public static int Actief
        {
            get; set;
        }
        public static string RekeningID
        {
            get; set;
        }
        public static int KlantID
        {
            get; set;
        }
        public static int Balans
        {
            get; set;
        }
        public static string Hash
        {
            get; set;
        }

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
            try
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
                        runBuitenGebruik();
                    }
                }
            }
            catch
            {
                runBuitenGebruik();
            }
        }

        private static void runBuitenGebruik()
        {
            Application.Run(new BuitenGebruik());
        }
    }
}
