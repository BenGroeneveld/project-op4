using System;
using System.Threading;
using System.Windows.Forms;

namespace Pinautomaat
{
    public static class Program
    {
        private static Welkom welkom;
        private static BuitenGebruik buitenGebruik;
        private static Thread thBuitenGebruik;
        private static Thread thBackground;

        public static bool SystemGood
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

        private static void runProgram()
        {
            SystemGood = false; //USE THIS IN NORMAL SITUATIONS
            //SystemGood = true; //USE THIS FOR DEBUGGING ONLY

            thBuitenGebruik = new Thread(runBuitenGebruik);
            thBackground = new Thread(bg);

            try
            {
                background(thBackground);
                SystemGood = MainBackend.checkAllConnections();
                while(SystemGood)
                {
                    MainBackend.closePrevForms();
                    welkom = new Welkom();
                    welkom.Activate();
                    Application.Run(welkom);
                    welkom = null;
                    SystemGood = MainBackend.checkAllConnections();
                }
                startBuitenGebruik();
            }
            catch
            {
                startBuitenGebruik();
            }
        }

        private static void bg()
        {
            Application.Run(new Background());
        }

        private static void background(Thread th)
        {
            th.Start();
        }

        private static void runBuitenGebruik()
        {
            MainBackend.closePrevForms();
            welkom = null;
            buitenGebruik = new BuitenGebruik();
            buitenGebruik.Activate();
            Application.Run(buitenGebruik);
            buitenGebruik = null;
        }

        private static void startBuitenGebruik()
        {
            thBuitenGebruik.Start();
        }
    }
}
