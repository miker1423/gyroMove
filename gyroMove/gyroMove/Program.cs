using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;

namespace gyroMove
{
#if WINDOWS
    static class Program
    {
        public  static bool _continue, Check;
        public static SerialPort _serialPort;
        public static Mensaje mes = new Mensaje();
        public static Datos LOL = new Datos();
        public static Thread readThread;
        public static int xEst, yEst;

        static void Main(string[] args)
        {
            StringComparer stringCompare = StringComparer.OrdinalIgnoreCase;
            readThread = new Thread(Read);

             _serialPort = new SerialPort();

            _serialPort.PortName = "COM3";
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = Parity.Even;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            _serialPort.ReadTimeout = 1;
            _serialPort.WriteTimeout = 1;

            _serialPort.Open();
            _continue = true;
            readThread.Start();

            _serialPort.WriteLine(String.Format("<{0}:{1}>", null, null));

            using (Game1 game = new Game1())
            {
                game.Run();
            }

            readThread.Join();
            _serialPort.Close();
            

        }

        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    mes.mensaje = message.Split(new char[] { ' ', '\r' });
                    if ((-10 < Convert.ToInt16(mes.mensaje[0]) || Convert.ToInt16(mes.mensaje[0]) < 10) || (-10 < Convert.ToInt16(mes.mensaje[1]) || Convert.ToInt16(mes.mensaje[1]) < 10))
                    {
                        if (Check)
                        {
                            LOL.Xi = 0;
                            LOL.Yi = 0;
                        }
                        else
                        {
                            LOL.Xf = 0;
                            LOL.Yf = 0;
                        }
                    }
                    if (Check)
                    {
                        LOL.Xi = Convert.ToInt32(mes.mensaje[0]);
                        LOL.Yi = Convert.ToInt32(mes.mensaje[1]);
                        Check = false;
                    }
                    else
                    {
                        LOL.Xf = Convert.ToInt32(mes.mensaje[0]);
                        LOL.Yf = Convert.ToInt32(mes.mensaje[1]);
                        Check = true;
                        check();
                    }
                    Console.WriteLine(" ");
                }
                catch (TimeoutException) { }
            }
        }

        public static int[] check()
        {
            
            int[] datos = new int[2];
            int promX = (LOL.Xi + LOL.Xf) / 2, promY=(LOL.Yi+LOL.Yf)/2;



            if ((Math.Abs(LOL.Xi) > Math.Abs(LOL.Yi)))
            {
                
                    if (promX< 0)
                    {
                        //se mueve a la derecha
                        datos[0] = 1;
                        xEst = datos[0];
                    }
                    else if (promX > 0)
                    {
                        //se mueve a la izquierda
                        datos[0] = 2;
                        xEst = datos[0];
                    }
                    //else if (LOL.Xi.CompareTo(LOL.Xf) == 1)
                    //{
                    //    //se mueve a la derecha
                    //    datos[0] = 1;
                    //    xEst = datos[0];
                    //}
                    //else if (LOL.Xi.CompareTo(LOL.Xf) == -1)
                    //{
                    //    //se mueve a la izquierda
                    //    datos[0] = 2;
                    //    xEst = datos[0];
                    //}
                    else if (LOL.Xi.CompareTo(LOL.Xf) == 0)
                    {
                        datos[0] = xEst;
                    }
                
                datos[1] = 0;
            }
            else
            {
                if (promY< 0)
                {
                    //se mueve hacia abajo
                    datos[1] = 1;
                    yEst = datos[1];
                }
                else if (promY> 0)
                {
                    //se mueve hacia arriba
                    datos[1] = 2;
                    yEst = datos[1];
                }
                //else if ((LOL.Yi > 0 && LOL.Yf < 0) && (LOL.Yi.CompareTo(LOL.Yf) == 1))
                //{
                //    datos[1] = 1;
                //    yEst = datos[1];
                //    //se mueve hacia abajo
                //}
                //else if ((LOL.Yi < 0 && LOL.Yf > 0) && (LOL.Yi.CompareTo(LOL.Yf) == -1))
                //{
                //    //se mueve hacia arriba
                //    datos[1] = 2;
                //    yEst = datos[1];
                //}
                else if (LOL.Yi.CompareTo(LOL.Yf)==0) 
                {
                    datos[1] = yEst;
                }

                datos[0] = 0;
            }
            return datos;
        }
    }
}
#endif

