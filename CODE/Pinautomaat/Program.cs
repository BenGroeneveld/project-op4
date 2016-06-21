using System;
using System.Threading;
using System.Windows.Forms;

namespace Pinautomaat
{
    public class Program
    {
        private static MainBackend mainBackend;

        private static bool SystemGood { get; set; }

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
            string text = mainBackend.makeHash("SOFA1995", "0123");
            System.IO.File.WriteAllText(@"D:\Downloads\SOFA1995.txt", text);

            string text2 = mainBackend.makeHash("SOFA1996", "0124");
            System.IO.File.WriteAllText(@"D:\Downloads\SOFA1996.txt", text2);

            string text3 = mainBackend.makeHash("SOFA2016", "0125");
            System.IO.File.WriteAllText(@"D:\Downloads\SOFA2016.txt", text3);
            */
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
            //runBackground();

            mainBackend = new MainBackend();
            while(true)
            {
                if(mainBackend.checkAllConnections())
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
