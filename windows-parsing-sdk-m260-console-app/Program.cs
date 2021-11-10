using System;
using System.IO.Ports;
using System.Threading;

namespace windows_parsing_sdk_m260_console_app
{
    class Program
    {
        static void Main(string[] args)
        {
            SerialPort port = new SerialPort("COM5",9600,Parity.None, 8, StopBits.One );
            port.Handshake = Handshake.None;

            port.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);


        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(500);
            string data = _serialPort.ReadLine();
            // Invokes the delegate on the UI thread, and sends the data that was received to the invoked method.  
            // ---- The "si_DataReceived" method will be executed on the UI thread which allows populating of the textbox.  
            this.BeginInvoke(new SetTextDeleg(si_DataReceived), new object[] { data });
        }

        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }

    }
}
