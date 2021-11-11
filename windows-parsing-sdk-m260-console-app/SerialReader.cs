using Nautilus;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace windows_parsing_sdk_m260_console_app
{
    public class SerialReader : IDisposable{

        private const int TIMEOUT = 500;
        private readonly M260 m260;
        private readonly DriverLicense driverlicense;
        private SerialPort serialPort;
        private readonly StringBuilder data;
        private readonly System.Threading.Timer timer;
        private bool connected;
        private bool disposed;


        public SerialPort SerialPort
        {
            get { return serialPort; }
            set { serialPort = value; }
        }
        public bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }



        public SerialReader(M260 M260)
        {
            m260 = M260;
            driverlicense = new DriverLicense();
            data = new StringBuilder();
            timer = new System.Threading.Timer(TimerExpired, this, Timeout.Infinite, Timeout.Infinite);
            InitializePort();
            disposed = false;
        }

        public void OpenPort()
        {
            try
            {
                serialPort.Open();
            }
            catch
            {
                return;
            }
            serialPort.DataReceived += serialPort_DataReceived;
        }
        public void ClosePort()
        {
            if (!connected) return;
            serialPort.Close();
            serialPort.DataReceived -= serialPort_DataReceived;
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            data.Append(serialPort.ReadExisting());
            Restart();
        }



        private void OnDataReceived(string licensedata)
        {
            driverlicense.ExtractInfo(licensedata);
            string[] firstname = System.Text.RegularExpressions.Regex.Split(driverlicense.FirstName, @"\s+");
            m260.dlfile.Record.Address1 = driverlicense.Address1;
            m260.dlfile.Record.Address2 = driverlicense.Address2;
            m260.dlfile.Record.Birthdate = driverlicense.Birthdate;
            m260.dlfile.Record.CardRevisionDate = driverlicense.CardRevisionDate;
            m260.dlfile.Record.City = driverlicense.City;
            m260.dlfile.Record.ClassificationCode = driverlicense.VehicleClassCode;
            m260.dlfile.Record.ComplianceType = driverlicense.ComplianceType;
            m260.dlfile.Record.Country = driverlicense.Country;
            m260.dlfile.Record.DocumentType = driverlicense.DocumentType;
            m260.dlfile.Record.EndorsementCodeDescription = driverlicense.EndorsementCodeDescription;
            m260.dlfile.Record.EndorsementsCode = driverlicense.EndorsementsCode;
            m260.dlfile.Record.ExpirationDate = driverlicense.ExpirationDate;
            m260.dlfile.Record.EyeColor = driverlicense.EyeColor;
            m260.dlfile.Record.FirstName = firstname.Length > 0 ? firstname[0] : driverlicense.FirstName;
            m260.dlfile.Record.FullName = driverlicense.FullName;
            m260.dlfile.Record.Gender = driverlicense.Gender;
            m260.dlfile.Record.HairColor = driverlicense.HairColor;
            m260.dlfile.Record.HAZMATExpDate = driverlicense.HAZMATExpDate;
            m260.dlfile.Record.Height = driverlicense.Height;
            m260.dlfile.Record.IIN = driverlicense.IIN;
            m260.dlfile.Record.IssueDate = driverlicense.IssueDate;
            m260.dlfile.Record.IssuedBy = driverlicense.IssuedBy;
            m260.dlfile.Record.JurisdictionCode = driverlicense.JurisdictionCode;
            m260.dlfile.Record.LastName = driverlicense.LastName;
            m260.dlfile.Record.LicenseNumber = driverlicense.LicenseNumber;
            m260.dlfile.Record.LimitedDurationDocument = driverlicense.LimitedDurationDocument;
            m260.dlfile.Record.MiddleName = firstname.Length > 1 ? firstname[1] : driverlicense.MiddleName;
            m260.dlfile.Record.NamePrefix = driverlicense.NamePrefix;
            m260.dlfile.Record.NameSuffix = driverlicense.NameSuffix;
            m260.dlfile.Record.OrganDonor = driverlicense.OrganDonor;
            m260.dlfile.Record.PostalCode = driverlicense.PostalCode;
            m260.dlfile.Record.Race = driverlicense.Race;
            m260.dlfile.Record.RestrictionCode = driverlicense.RestrictionCode;
            m260.dlfile.Record.RestrictionCodeDescription = driverlicense.RestrictionCodeDescription;
            m260.dlfile.Record.Specification = driverlicense.Specification;
            m260.dlfile.Record.VehicleClassCode = driverlicense.VehicleClassCode;
            m260.dlfile.Record.VehicleClassCodeDescription = driverlicense.VehicleClassCodeDescription;
            m260.dlfile.Record.Veteran = driverlicense.Veteran;
            m260.dlfile.Record.WeightKG = driverlicense.WeightKG;
            m260.dlfile.Record.WeightLBS = driverlicense.WeightLBS;
            m260.readerdata = licensedata;
            m260.onDataReceived();
        }



        private void TimerExpired(object instance)
        {
            var reader = (SerialReader)instance;
            if (reader.data.Length == 0) return;

            var str = reader.data.ToString();
            reader.data.Remove(0, reader.data.Length);
            reader.OnDataReceived(str);
        }
        private void Restart()
        {
            timer.Change(TIMEOUT, Timeout.Infinite);
        }
        private void InitializePort()
        {
            connected = false;
            if (m260.commport == string.Empty) { FindPort(); }
            if (m260.commport != string.Empty) { SetPort(); }
        }
        private void FindPort()
        {
            var ports = SerialPort.GetPortNames();
            switch (ports.Length)
            {
                case 1:
                    serialPort = new SerialPort(ports[0]);
                    m260.commport = ports[0];
                    connected = true;
                    break;
                case 2:
                    serialPort = new SerialPort(ports[1]);
                    m260.commport = ports[1];
                    connected = true;
                    break;
                case 3:
                    serialPort = new SerialPort(ports[2]);
                    m260.commport = ports[2];
                    connected = true;
                    break;
                case 4:
                    serialPort = new SerialPort(ports[3]);
                    m260.commport = ports[3];
                    connected = true;
                    break;
                case 5:
                    serialPort = new SerialPort(ports[4]);
                    m260.commport = ports[4];
                    connected = true;
                    break;
                default:
                    serialPort = null;
                    m260.commport = string.Empty;
                    connected = false;
                    break;
            }
        }
        private void SetPort()
        {
            try
            {
                serialPort = new SerialPort(m260.commport);
                connected = true;
            }
            catch
            {
                connected = false;
                FindPort();
            }
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (disposed) { return; }
            if (disposing)
            {
                timer?.Dispose();
                serialPort?.Dispose();
            }
            disposed = true;
        }

    }



}
