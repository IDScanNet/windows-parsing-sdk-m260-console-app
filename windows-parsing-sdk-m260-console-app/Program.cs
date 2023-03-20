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
        DriverLicense resDL = new DriverLicense();

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

        Console.WriteLine("Scan your license first then type Parse to parse the ID");

        while (_continue)
        {
            message = Console.ReadLine();

            if (stringComparer.Equals("parse", message))
            {
                file.Close();
                string text = System.IO.File.ReadAllText("drivers-license.txt");
                resDL.ExtractInfo(text);

                Console.Out.WriteLine($"To learn about a specific field visit\nhttps://docs.idscan.net/idparsing/dotnet.html\n");
                Console.WriteLine($"Address : {resDL.Address1} {resDL.Address2}");
                Console.WriteLine($"\nDocument Type : {resDL.DocumentType}");
                Console.WriteLine($"\nBirthday:{resDL.Birthdate}");
                Console.WriteLine($"\nCity : {resDL.City}");
                Console.WriteLine($"\nVehicle Classification Code : {resDL.VehicleClassCode}");
                Console.WriteLine($"\nCompliance Type: {resDL.ComplianceType}");
                Console.WriteLine($"\nCountry : {resDL.Country}");
                Console.WriteLine($"\nDocument Type : {resDL.DocumentType}");
                Console.WriteLine($"\nDocument Discriminator : {resDL.DocumentDiscriminator}");
                Console.WriteLine($"\nEndorsment Code Description : {resDL.EndorsementCodeDescription}");
                Console.WriteLine($"\nEndorsements Code : {resDL.EndorsementsCode}");
                Console.WriteLine($"\nExpiration Date : {resDL.ExpirationDate}");
                Console.WriteLine($"\nEye Color : {resDL.EyeColor}");
                Console.WriteLine($"\nFirst Name : {resDL.FirstName}");
                Console.WriteLine($"\nFull Name : {resDL.FullName}");
                Console.WriteLine($"\nGender : {resDL.Gender}");
                Console.WriteLine($"\nHair Color : {resDL.HairColor}");
                Console.WriteLine($"\nHazmat Exp Date : {resDL.HAZMATExpDate}");
                Console.WriteLine($"\nHeight : {resDL.Height}");
                Console.WriteLine($"\nIIN : {resDL.IIN}");
                Console.WriteLine($"\nIssueDate : {resDL.EndorsementsCode}");
                Console.WriteLine($"\nJurisdiction Code : {resDL.JurisdictionCode}");
                Console.WriteLine($"\nLast Name : {resDL.LastName}");
                Console.WriteLine($"\nLicense Number : {resDL.LicenseNumber}");
                Console.WriteLine($"\nLimited Duration Document : {resDL.LimitedDurationDocument}");
                Console.WriteLine($"\nMiddle Name : {resDL.MiddleName}");
                Console.WriteLine($"\nName Prefix : {resDL.NamePrefix}");
                Console.WriteLine($"\nName Suffix : {resDL.NameSuffix}");
                Console.WriteLine($"\nOrgan Donor : {resDL.OrganDonor}");
                Console.WriteLine($"\nPostal Box : {resDL.PostalBox}");
                Console.WriteLine($"\nPostal Code : {resDL.PostalCode}");

                Console.WriteLine($"\nRace : {resDL.Race}");
                Console.WriteLine($"\nRealId  : {resDL.RealId}");
                Console.WriteLine($"\nRestriction Code : {resDL.RestrictionCode}");
                Console.WriteLine($"\nRestriction Code Description : {resDL.RestrictionCodeDescription}");
                Console.WriteLine($"\nSpecification : {resDL.Specification}");
                Console.WriteLine($"\nVehicle Class Code Description : {resDL.VehicleClassCodeDescription}");
                Console.WriteLine($"\nVechile Registration Data : {resDL.vehicleRegistrationData}");
                Console.WriteLine($"\nVehicle Class Code  : {resDL.VehicleClassCode}");
                Console.WriteLine($"\nValidation Conf : {resDL.ValidationConfidence}");
                foreach (string s in resDL.ValidationCodes)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine($"\nWeight KG : {resDL.WeightKG}");
                Console.WriteLine($"\nWeight LBS : {resDL.WeightLBS}");

                Console.Out.Write("To use this sample again, please clear/delete driver-license.txt file");


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
