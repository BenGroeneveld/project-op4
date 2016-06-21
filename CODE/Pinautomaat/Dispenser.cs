using MonoBrick.EV3;
using System.IO.Ports;
using System.Threading;

namespace Pinautomaat
{
    public class Dispenser
    {
        private Brick<Sensor, Sensor, Sensor, Sensor> brickEV3;
        private Motor motor10;
        private Motor motor20;
        private Motor motor50;
        private int biljetten10 = 0;
        private int biljetten20 = 0;
        private int biljetten50 = 0;

        private DatabaseConnection dbConnect;
        private MainBackend mainBackend;
        private Dispenser dispenser;
        private Rekening rekening;
        private Pas pas;

        public void dispense()
        {
            privateDispense(biljetten10, biljetten20, biljetten50);
        }

        public bool testDispense()
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

        private void privateDispense(int aantal10, int aantal20, int aantal50)
        {
            try
            {
                geefBiljetten(motor10, aantal10);
                geefBiljetten(motor20, aantal20);
                geefBiljetten(motor50, aantal50);
            }
            catch { }
        }

        public bool isGeldBeschikbaar(int bedrag)
        {
            if(privateIsGeldBeschikbaar(bedrag))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool privateIsGeldBeschikbaar(int bedrag)
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

        private void geefBiljetten(Motor motor, int aantalBiljetten)
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

        private void correctie(Motor motor)
        {
            motor.Reverse = false;
            motor.On(20);
            Thread.Sleep(1500);
            motor.Off();
        }

        private bool connect()
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
