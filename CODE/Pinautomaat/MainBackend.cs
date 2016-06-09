using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace Pinautomaat
{
    public static class MainBackend
    {
        private static int baud = 9600;
        private static string recognizeText = "Arduino";
        private static int loggedInValue = 0;

        private static MySqlConnection connection;
        private static string rfid = "";
        private static string rekening = "";
        
        public static bool AdminKaart { get; set; }
        public static int AantalBiljetten10 { get; set; }
        public static int AantalBiljetten20 { get; set; }
        public static int AantalBiljetten50 { get; set; }

        public static bool checkAllConnections()
        {
            if(privateCheckAllConnections())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void setGeldBeschikbaar()
        {

        }

        private static bool privateCheckAllConnections()
        {
            try
            {
                makeDatabaseConnection();
                if(checkArduino())// && Dispenser.testDispense() && checkPrinter())
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
                ArduinoInput.strCardID = "";
                if(!ArduinoInput.isConnected(baud, recognizeText, loggedInValue))
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
            Program.Rfid = null;
            Program.StrBedrag = null;
            Program.StrRekeningID = null;
            ArduinoInput.strCardID = "";
            loggedInValue = 0;

            try
            {
                rfid = ArduinoInput.strRFID();
                ArduinoInput.strCardID = rfid;
                Program.Rfid = rfid;
                rekening = strDbQuery("RekeningID", rfid);
                Program.StrRekeningID = rekening;
                loggedInValue = 255;
            }
            catch { }
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
                    else if(f.Name == "Welkom")
                    {
                        f.Hide();
                    }
                }
                catch { }
            }
        }

        public static void makeDatabaseConnection()
        {
            privateMakeDatabaseConnection();
        }

        private static void privateMakeDatabaseConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDbContextConnectionStringRemote"].ConnectionString;

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
            }
            catch(MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void blokkeerPas(string rfidCard)
        {
            privateBlokkeerPas(rfidCard);
        }

        public static void doTransactie(int bedrag, string rfidCard)
        {
            privateDoTransactie(bedrag, rfidCard);
        }

        public static string strDbQuery(string getAttribute, string rfidCard)
        {
            return strPrivateDbQuery(getAttribute, rfidCard);
        }

        private static string strPrivateDbQuery(string getAttribute, string rfidCard)
        {
            MySqlCommand cmd = connection.CreateCommand();
            if(getAttribute == "RekeningID")
            {
                cmd.CommandText = "SELECT RekeningID FROM Pas WHERE PasID ='" + rfidCard + "'";
            }
            else if(getAttribute == "KlantID")
            {
                cmd.CommandText = "SELECT KlantID FROM Pas WHERE PasID ='" + rfidCard + "'";
            }
            else if(getAttribute == "Actief")
            {
                cmd.CommandText = "SELECT Actief FROM Pas WHERE PasID ='" + rfidCard + "'";
            }
            else if(getAttribute == "Pincode")
            {
                cmd.CommandText = "SELECT Pincode FROM Pas WHERE PasID ='" + rfidCard + "'";
            }
            else if(getAttribute == "Balans")
            {
                cmd.CommandText = "SELECT Balans FROM Rekening INNER JOIN Pas ON Pas.RekeningID = Rekening.RekeningID WHERE PasID = '" + rfidCard + "'";
            }
            else if(getAttribute == "RekeningType")
            {
                cmd.CommandText = "SELECT RekeningType FROM Rekening INNER JOIN Pas ON Pas.RekeningID = Rekening.RekeningID WHERE PasID = '" + rfidCard + "'";
            }
            return cmd.ExecuteScalar().ToString();
        }

        private static void privateBlokkeerPas(string rfidCard)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Pas SET Actief = 0 WHERE PasID ='" + rfidCard + "'";
            cmd.ExecuteNonQuery();
        }

        private static void privateDoTransactie(int bedrag, string rfidCard)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Rekening INNER JOIN Pas ON Pas.RekeningID = Rekening.RekeningID SET Balans = " + bedrag + " WHERE PasID = '" + rfidCard + "'";
            cmd.ExecuteNonQuery();
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
