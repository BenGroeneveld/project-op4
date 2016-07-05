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

            /*
            Rekening sofa2016 = new Rekening();
            sofa2016.RekeningID = "SOFA2016";
            sofa2016.Balans = 100000;
            sofa2016.Hash = "881b45354a3b6b0ef7713ad3e5dd20e4bd639b2ed7b4c1a4a9752ad23a82e1127f3e384b61279f331148ab9d73992c81edc15d58c3b230babe0b83533cf363f1";

            Rekening sofa1996 = new Rekening();
            sofa1996.RekeningID = "SOFA1996";
            sofa1996.Balans = 100000;
            sofa1996.Hash = "68e579629e66054028b62eecdc0506b7a33f87bed8f569b84708ccabe1ce8726816764b76081a48b88dacdcb200879a155c68709d2a5009684d44a923ef5a703";

            DatabaseConnection.setRekening(sofa2016);
            DatabaseConnection.setRekening(sofa1996);
            */

            /*
            MainBackend.AantalBiljetten10 = 5;
            MainBackend.AantalBiljetten20 = 5;
            MainBackend.AantalBiljetten50 = 7;
            int bedrag = 230 * 100;

            Dispenser.testDispense();
            if(Dispenser.isGeldBeschikbaar(bedrag))
            {
                Dispenser.dispense();
            }
            */
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
