using MonoBrick.EV3;
using System.IO.Ports;
using System.Threading;

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
                    geefBiljetten(motor10, 0);
                    geefBiljetten(motor20, 0);
                    geefBiljetten(motor50, 0);
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
                geefBiljetten(motor10, aantal10);
                geefBiljetten(motor20, aantal20);
                geefBiljetten(motor50, aantal50);
            }
            catch { }
        }

        public static bool checkBeschikbaarGeld(int bedrag)
        {
            int aantalBiljetten10 = MainBackend.AantalBiljetten10;
            int aantalBiljetten20 = MainBackend.AantalBiljetten20;
            int aantalBiljetten50 = MainBackend.AantalBiljetten50;

            int aantalBiljetten10Eind = aantalBiljetten10;
            int aantalBiljetten20Eind = aantalBiljetten20;
            int aantalBiljetten50Eind = aantalBiljetten50;

            bedrag = bedrag / 100;

            int aantal50 = bedrag / 50;
            int aantalOver50 = bedrag % 50;
            if(aantal50 > aantalBiljetten50)
            {
                if(aantalBiljetten50 > 0)
                {
                    aantalOver50 = bedrag - (aantalBiljetten50 * 50);
                }
                else
                {
                    aantalOver50 = bedrag;
                }
                aantalBiljetten50Eind = aantalBiljetten50;
                aantalBiljetten50 = 0;
            }
            else
            {
                aantalBiljetten50 -= aantal50;
                aantalBiljetten50Eind -= aantal50;
            }

            int aantal20 = aantalOver50 / 20;
            int aantalOver20 = aantalOver50 % 20;
            if(aantal20 > aantalBiljetten20)
            {
                if(aantalBiljetten20 > 0)
                {
                    aantalOver20 = aantalOver50 - (aantalBiljetten20 * 20);
                }
                else
                {
                    aantalOver20 = aantalOver50;
                }
                aantalBiljetten20Eind = aantalBiljetten20;
                aantalBiljetten20 = 0;
            }
            else
            {
                aantalBiljetten20 -= aantal20;
                aantalBiljetten20Eind -= aantal20;
            }

            int aantal10 = aantalOver20 / 10;
            int aantalOver10 = aantalOver20 % 10;
            if(aantal10 > aantalBiljetten10)
            {
                if(aantalBiljetten10 > 0)
                {
                    aantalOver10 = aantalOver20 - (aantalBiljetten10 * 10);
                }
                else
                {
                    aantalOver10 = aantalOver20;
                }
                aantalBiljetten10Eind = aantalBiljetten10;
                aantalBiljetten10 = 0;
            }
            else
            {
                aantalBiljetten10 -= aantal10;
                aantalBiljetten10Eind -= aantal10;
            }

            if(aantalOver10 > 0)
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

        private static void geefBiljetten(Motor motor, int aantalBiljetten)
        {
            for(int i = 0; i < aantalBiljetten; i++)
            {
                motor.Reverse = true;
                motor.On(20);
                Thread.Sleep(3000);
                motor.Off();
                correctie(motor);
            }
        }

        private static void correctie(Motor motor)
        {
            motor.Reverse = false;
            motor.On(20);
            Thread.Sleep(1500);
            motor.Off();
        }

        private static bool connect()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach(string port in ports)
            {
                Brick<Sensor, Sensor, Sensor, Sensor> brick = new Brick<Sensor, Sensor, Sensor, Sensor>(port.ToLower());

                if(port.Contains("COM"))
                {
                    try
                    {
                        brick.Connection.Open();
                        brick.Beep(20, 100);
                        brickEV3 = brick;
                        motor10 = brickEV3.MotorA;
                        motor20 = brickEV3.MotorB;
                        motor50 = brickEV3.MotorC;
                        return true;
                    }
                    catch
                    {
                        // Ga verder...
                    }
                }
            }
            return false;
        }
    }
}
