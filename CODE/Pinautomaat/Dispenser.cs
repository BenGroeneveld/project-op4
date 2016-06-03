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


        public static void dispense(int bedrag)
        {
            privateDispense(bedrag);
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

        private static void privateDispense(int bedrag)
        {
            int aantal50 = bedrag / 50;
            int aantalOver50 = bedrag % 50;
            int aantal20 = aantalOver50 / 20;
            int aantalOver20 = aantalOver50 % 20;
            int aantal10 = aantalOver20 / 10;

            try
            {
                geefBiljetten(motor10, aantal10);
                geefBiljetten(motor20, aantal20);
                geefBiljetten(motor50, aantal50);
            }
            catch { }
        }

        private static void geefBiljetten(Motor motor, int aantalBiljetten)
        {
            for(int i = 0; i < aantalBiljetten; i++)
            {
                motor.Reverse = true;
                motor.On(20);
                Thread.Sleep(2500);
                motor.Off();
                correctie(motor);
            }
        }

        private static void correctie(Motor motor)
        {
            motor.Reverse = false;
            motor.On(20);
            Thread.Sleep(1000);
            motor.Off();
        }

        private static bool connect()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach(string port in ports)
            {
                if(port.Contains("COM"))
                {
                    Brick<Sensor, Sensor, Sensor, Sensor> brick = new Brick<Sensor, Sensor, Sensor, Sensor>(port.ToLower());
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
