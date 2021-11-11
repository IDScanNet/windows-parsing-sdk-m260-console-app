using System;



namespace windows_parsing_sdk_m260_console_app
{
    public class M260 : IDisposable {
       
        
        private SerialReader reader;

        public string readerdata;
        public string commport;
        public readonly DLFILE dlfile;
 
        private bool disposed;
       

        public M260(string commPort = "")
        {
            readerdata = string.Empty;
            commport = commPort;
            dlfile = new DLFILE();
            disposed = false;
            InitializeReader();
        }
        public void Start()
        {
            Open();
        }
        public void Stop()
        {
            Close();
        }

        public void onDataReceived()
        {
            Console.Out.Write(this.DLFILE.Record.FullName);
        }


        private void Open(){
            readerdata = string.Empty;
            dlfile.Record.Initialize();
            reader?.OpenPort();
        }
        private void Close(){
            readerdata = string.Empty;
            dlfile.Record.Initialize();
            reader?.ClosePort();
        }

        private void InitializeReader(){
            readerdata = string.Empty;
            reader = new SerialReader(this);
        }
 
        public DLFILE DLFILE
        {
            get { return dlfile; }
        }
       
        public string CommPort
        {
            get { return commport; }
            set { commport = value; }
        }
        public string ReaderData
        {
            get { return readerdata; }
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
                reader?.Dispose();
            }
            disposed = true;
        }

    }
}
