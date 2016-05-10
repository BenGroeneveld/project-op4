using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace Gui
{
    public static class MainBackend
    {
        private static int baud = 9600;
        private static string recognizeText = "Arduino";
        private static int loggedInValue = 0;

        private static MySqlConnection connection;
        private static string rfid = "";

        public static void doWelkom()
        {
            makeDatabaseConnection();
            checkCard();
        }

        private static void checkCard()
        {
            loggedInValue = 0;
            ArduinoInput.strCardID = "";
            if(!ArduinoInput.isConnected(baud, recognizeText, loggedInValue))
            {
                ArduinoInput.connect(baud, recognizeText, loggedInValue);
            }
            else
            {
                while(!ArduinoInput.isConnected(baud, recognizeText, loggedInValue))
                {
                    ArduinoInput.connect(baud, recognizeText, loggedInValue);
                }
            }
            rfid = ArduinoInput.strRFID();
            ArduinoInput.strCardID = rfid;
            Program.Rfid = rfid;
            loggedInValue = 255;
        }

        public static void restart()
        {
            privateRestart();
        }

        private static void privateRestart()
        {
            Welkom next = new Welkom();

            List<Form> openForms = new List<Form>();

            foreach(Form f in Application.OpenForms)
            {
                openForms.Add(f);
            }
            
            foreach(Form f in openForms)
            {
                Type currentType = f.GetType();
                Type welkom = typeof(Welkom);
                Type pincode = typeof(Pincode);
                Type background = typeof(Background);

                if(!currentType.Equals(background))
                {
                    f.Close();
                }
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

    }
}
