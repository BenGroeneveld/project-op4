using MonoBrick;
using MonoBrick.EV3;
using System.Threading;
using System;

namespace Pinautomaat
{
    public static class Dispenser
    {
        private static Brick<Sensor, Sensor, Sensor, Sensor> brickEV3;
        private static Motor motor10;
        private static Motor motor20;
        private static Motor motor50;
        private static int biljetten10 = 0;
        private static int biljetten20 = 0;
        private static int biljetten50 = 0;

        public static void dispense()
        {
            privateDispense(biljetten10, biljetten20, biljetten50);
        }

        public static bool testDispense()
        {
            if(connect())
            {
                try
                {
                    geefBiljetten(0, 0, 0);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private static void privateDispense(int aantal10, int aantal20, int aantal50)
        {
            try
            {
                geefBiljetten(aantal10, aantal20, aantal50);
            }
            catch { }
        }

        public static bool isGeldBeschikbaar(int bedrag)
        {
            return privateIsGeldBeschikbaar(bedrag);
        }

        private static bool privateIsGeldBeschikbaar(int bedrag)
        {
            int aantalBiljetten10 = MainBackend.AantalBiljetten10;
            int aantalBiljetten20 = MainBackend.AantalBiljetten20;
            int aantalBiljetten50 = MainBackend.AantalBiljetten50;

            int aantalBiljetten10Eind = aantalBiljetten10;
            int aantalBiljetten20Eind = aantalBiljetten20;
            int aantalBiljetten50Eind = aantalBiljetten50;
            
            int aantal50;
            int aantal20;
            int aantal10;
            int restGetal = 1;

            int i;
            int j;
            int n;

            bool checking = true;

            bedrag = bedrag / 100;

            aantal50 = bedrag / 50;
            if(aantalBiljetten50 > aantal50)
            {
                i = aantal50;
            }
            else
            {
                i = aantalBiljetten50;
            }
            while(i >= 0 && checking == true)
            {
                aantal50 = bedrag - (i * 50);
                aantalBiljetten50Eind = i;

                aantal20 = aantal50 / 20;
                if(aantalBiljetten20 > aantal20)
                {
                    j = aantal20;
                }
                else
                {
                    j = aantalBiljetten20;
                }
                while(j >= 0 && checking == true)
                {
                    aantal20 = aantal50 - (j * 20);
                    aantalBiljetten20Eind = j;

                    aantal10 = aantal20 / 10;
                    if(aantalBiljetten10 > aantal10)
                    {
                        n = aantal10;
                    }
                    else
                    {
                        n = aantalBiljetten10;
                    }
                    while(n >= 0 && checking == true)
                    {
                        aantal10 = aantal20 - (n * 10);
                        aantalBiljetten10Eind = n;

                        restGetal = aantal10;

                        if(restGetal == 0)
                        {
                            aantalBiljetten50 -= i;
                            aantalBiljetten20 -= j;
                            aantalBiljetten10 -= n;
                            checking = false;
                        }
                        n--;
                    }
                    j--;
                }
                i--;
            }

            if(restGetal > 0)
            {
                return false;
            }
            else
            {
                MainBackend.AantalBiljetten50 = aantalBiljetten50;
                MainBackend.AantalBiljetten20 = aantalBiljetten20;
                MainBackend.AantalBiljetten10 = aantalBiljetten10;

                biljetten10 = aantalBiljetten10Eind;
                biljetten20 = aantalBiljetten20Eind;
                biljetten50 = aantalBiljetten50Eind;

                return true;
            }
        }

        private static void geefBiljetten(int aantalBiljettenA, int aantalBiljettenB, int aantalBiljettenC)
        {
            uint degrees = 700;
            sbyte speed = -35;
            sbyte speedReverse = 35;
            int time = 2000;
            bool brake = true;

            for(int i = 0; i < aantalBiljettenA; i++)
            {
                motor10.On(speed, degrees, brake);
                Thread.Sleep(time);

                motor10.On(speedReverse, degrees, brake);
                Thread.Sleep(time);
            }
            motor10.Off();

            for(int i = 0; i < aantalBiljettenB; i++)
            {
                motor20.On(speed, degrees, brake);
                Thread.Sleep(time);
                
                motor20.On(speedReverse, degrees, brake);
                Thread.Sleep(time);
            }
            motor20.Off();

            for(int i = 0; i < aantalBiljettenC; i++)
            {
                motor50.On(speed, degrees, brake);
                Thread.Sleep(time);
                
                motor50.On(speedReverse, degrees, brake);
                Thread.Sleep(time);
            }
            motor50.Off();
        }

        private static bool connect()
        {
            try
            {
                if(brickEV3 != null)
                {
                    string str = "";
                    str = brickEV3.Connection.ToString();
                    if(str.Contains("USB"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    brickEV3 = new Brick<Sensor, Sensor, Sensor, Sensor>("usb");
                    brickEV3.Connection.Open();
                    brickEV3.Beep(8, 100);
                    motor10 = brickEV3.MotorA;
                    motor20 = brickEV3.MotorB;
                    motor50 = brickEV3.MotorC;
                    return true;
                }
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
