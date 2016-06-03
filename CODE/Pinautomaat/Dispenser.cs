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


        public static void dispense()
        {
            privateDispense();
        }

        public static void privateDispense()
        {
            connect();
            geefBiljetten(motor10);
        }

        private static void waitForMotorToStop(Motor motor)
        {
            Thread.Sleep(150);
            while(motor.IsRunning())
            {
                Thread.Sleep(10);
            }
        }

        private static void geefBiljetten(Motor motor)
        {
            motor.MoveTo(50, 200, false);
            waitForMotorToStop(motor);
            motor.MoveTo(50, -200, false);
            waitForMotorToStop(motor);
        }

        private static void connect()
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
                    }
                    catch
                    {
                        // Ga verder...
                    }
                }
            }
            motor10 = brickEV3.MotorA;
            motor20 = brickEV3.MotorB;
            motor50 = brickEV3.MotorC;
        }
    }
}
