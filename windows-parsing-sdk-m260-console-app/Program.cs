using Nautilus;
using System;
using System.IO;
using System.IO.Ports;

public class PortChat
{
    static bool _continue;
    static SerialPort _serialPort;
    static string buffer;
    static StreamWriter file;

    public static void Main()
    {
        string message;
        DriverLicense dl = new DriverLicense();

        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

        // Create a new SerialPort object with default settings.
        _serialPort = new SerialPort();

        // Allow the user to set the appropriate properties.
        _serialPort.PortName = SetPortName(_serialPort.PortName);
        _serialPort.BaudRate = 9600;
        _serialPort.Parity = Parity.None;
        _serialPort.DataBits = 8;
        _serialPort.StopBits = StopBits.One;
        _serialPort.Handshake = Handshake.None;
        // Set the read/write timeouts
        _serialPort.ReadTimeout = 500;
        _serialPort.WriteTimeout = 500;

        _serialPort.Open();
        _serialPort.DataReceived += serialPort_DataReceived;
        buffer = string.Empty;

        _continue = true;

        file = new StreamWriter("drivers-license.txt", append: true);

        Console.WriteLine("Type Parse to parse the ID");

        while (_continue)
        {
            message = Console.ReadLine();

            if (stringComparer.Equals("parse", message))
            {
                file.Close();
                string text = System.IO.File.ReadAllText("drivers-license.txt");
                dl.ExtractInfo(text);

                Console.Out.WriteLine(dl.FullName);
                Console.Out.WriteLine(dl.FirstName);
                Console.Out.WriteLine(dl.LastName);
                Console.Out.WriteLine(dl.IssueDate);
                Console.Out.WriteLine(dl.ExpirationDate);



            }
            else
            {
                _serialPort.WriteLine(message);
            }
        }
        _serialPort.Close();
    }


    public static string SetPortName(string defaultPortName)
    {
        string portName;

        Console.WriteLine("Available Ports:");
        foreach (string s in SerialPort.GetPortNames())
        {
            Console.WriteLine("{0}", s);
        }

        Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
        portName = Console.ReadLine();

        if (portName == "" || !(portName.ToLower()).StartsWith("com"))
        {
            portName = defaultPortName;
        }
        return portName;
    }

    public static void serialPort_DataReceived(object s, SerialDataReceivedEventArgs e)
    {

        var message = _serialPort.ReadExisting();
        foreach(char a in message)
        {
            file.Write(a);
           
        }
    }
}