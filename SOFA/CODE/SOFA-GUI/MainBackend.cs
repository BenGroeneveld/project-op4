using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Security.Cryptography;

namespace Pinautomaat
{
    public static class MainBackend
    {
        public static string makeHash(string RekeningID, string pincode)
        {
            string input = string.Concat(RekeningID, pincode);
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            SHA512Managed hashstring = new SHA512Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach(byte x in hash)
            {
                hashString += string.Format("{0:x2}", x);
            }
            return hashString;
        }
        public static int baud = 9600;
        public static string recognizeText = "Arduino";
        public static int loggedInValue = 0;
        
        public static bool AdminKaart { get; set; }
        public static int AantalBiljetten10 { get; set; }
        public static int AantalBiljetten20 { get; set; }
        public static int AantalBiljetten50 { get; set; }

        public static bool checkAllConnections()
        {
            return privateCheckAllConnections();
        }

        private static bool privateCheckAllConnections()
        {
            try
            {
                if(checkArduino() && DatabaseConnection.isConnected().Result && Dispenser.testDispense())// && checkPrinter())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void doWelkom()
        {
            checkCard();
        }

        private static bool checkArduino()
        {
            try
            {
                if(!ArduinoInput.isConnected())
                {
                    ArduinoInput.connect(baud, recognizeText, loggedInValue);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool isPrinterConnected()
        {
            return checkPrinter();
        }

        private static bool checkPrinter()
        {
            try
            {
                string print = @"atm.label";
                var label = DYMO.Label.Framework.Label.Open(print);
                var printer = DYMO.Label.Framework.Framework.GetPrinters().GetPrinterByName("DYMO LabelWriter 400");
                if(printer.IsConnected)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private static void checkCard()
        {
            loggedInValue = 0;

            try
            {
                Program.PasID = ArduinoInput.strRFID();
                Program.RekeningID = DatabaseConnection.getPas().Result.RekeningID;
                Program.Actief = DatabaseConnection.getPas().Result.Actief;
                Program.Balans = DatabaseConnection.getRekening().Result.Balans;
                Program.Hash = DatabaseConnection.getRekening().Result.Hash;
                Program.KlantID = DatabaseConnection.getPas().Result.KlantID;
                Program.Poging = DatabaseConnection.getPas().Result.Poging;
                loggedInValue = 255;
            }
            catch
            {
                Program.SystemGood = false;
            }
        }

        public static void restart()
        {
            privateRestart();
        }

        private static void privateRestart()
        {
            List<Form> openForms = new List<Form>();

            foreach(Form f in Application.OpenForms)
            {
                openForms.Add(f);
            }

            foreach(Form f in openForms)
            {
                try
                {
                    if(f.Name != "Background")
                    {
                        f.Close();
                    }
                }
                catch { }
            }

            Program.Actief = 0;
            Program.Balans = 0;
            Program.Hash = null;
            Program.KlantID = 0;
            Program.PasID = null;
            Program.Poging = 0;
            Program.RekeningID = null;
        }

        public static void closePrevForms()
        {
            List<Form> openForms = new List<Form>();

            foreach(Form f in Application.OpenForms)
            {
                openForms.Add(f);
            }

            foreach(Form f in openForms)
            {
                try
                {
                    if(f.Name != Form.ActiveForm.Name && f.Name != "Welkom" && f.Name != "Background")
                    {
                        f.Close();
                    }
                }
                catch { }
            }
        }

        public static void blokkeerPas(string rfidCard)
        {
            Program.Actief = 0;
            Pas pas = new Pas();
            pas.Actief = Program.Actief;
            pas.RekeningID = Program.RekeningID;
            pas.KlantID = Program.KlantID;
            pas.PasID = Program.PasID;
            pas.Poging = Program.Poging;
            DatabaseConnection.setPas(pas);
            pas = null;
        }

        public static void doTransactie(int bedrag)
        {
            Program.Balans = bedrag;
            Rekening rekening = new Rekening();
            rekening.Balans = Program.Balans;
            rekening.RekeningID = Program.RekeningID;
            rekening.Hash = Program.Hash;
            DatabaseConnection.setRekening(rekening);
            rekening = null;
        }

        public static void printBon(string geldOpname, string rekeningID, string labelPasID)
        {
            string printer = @"atm.label";
            var label = DYMO.Label.Framework.Label.Open(printer);
            label.SetObjectText("labelGeldOpname", "Geldopname: €" + geldOpname);
            label.SetObjectText("labelPasID", "Pas-ID: " + labelPasID);
            label.SetObjectText("labelRekeningID", "Rekeningnummer: " + rekeningID);
            label.Print("DYMO LabelWriter 400");
        }
    }
}
