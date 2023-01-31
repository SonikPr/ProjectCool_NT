using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics.PerformanceData;
using System.Windows.Controls;

namespace ProjectCool_NT.Class
{
    class Device : Fan
    {
        SerialPort MainPort = new SerialPort();
        const string ProgramSettingsFile = "programsettings.config";
        const string DeviceSettings = "DeviceSettings.config";
        const string DeviceSensorData = "SensorData.sensors";
        const int BaudRate = 9600;

        private string device_model = "";
        private string device_firmware = "";
        private string device_port;
        private int update_rate;
        private string[] ports;
        private int PortError_count = 0;
        private int[] SensorsData = new int[13];
        private int[] SettingsData = new int[13];
        private string[] IncomingSensorData = new string[13];
        private string[] IncomingSettingsData = new string[13];
        private bool DeviceConnected = false;
        private string project_folder;
        private string IO_files_folder;
        private string settings_folder;



        private bool DirectoryAndFilesCheckup()//DocumentsFolderSetup
        {
            bool directoryabscent = false;
            project_folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\ProjectCool NT";
            IO_files_folder = project_folder + "\\IO_files";
            settings_folder = project_folder + "\\settings";

            if (!Directory.Exists(project_folder))
            {
                Directory.CreateDirectory(project_folder);
                directoryabscent = true;
            }
            if (!Directory.Exists(IO_files_folder))
            {
                Directory.CreateDirectory(IO_files_folder);
            }
            if (!Directory.Exists(settings_folder))
            {
                Directory.CreateDirectory(settings_folder);
                directoryabscent = true;
            }
            if(!File.Exists(settings_folder + "/programsettings.config"))
            {
                directoryabscent = true;
            }

            if (directoryabscent)
            {
                return false;
            }
            return true;
            

        }

        private struct DeviceTemp
        {
            private int _t;
            private int _h;

            public DeviceTemp(int t, int h)
            {
                this._t = t;
                this._h = h;
            }

            public int ChassisTemp
            {
                get { return this._t; }
                set { this._t = value; }
            }

            public int ChassisHumidity
            {
                get { return this._h; }
                set { this._h = value; }
            }

        }

        DeviceTemp systemps = new DeviceTemp();

        public void CreateDevice() //Automatic device search
        {
            bool errflag = false;
            if (DirectoryAndFilesCheckup())
            {
                RestoreSoftwareSettings();
                    if (CreateSerial(9600, device_port) != "OK")
                    {
                        errflag= true;
                        goto OnPortError;
                    }
            }
            else
            {
                errflag= true;
                goto OnPortError; 
            }

        OnPortError:
            if (errflag)
            {
                Thread CheckDeviceAvailability = new Thread(this.AutoDeviceSearch);
                CheckDeviceAvailability.Start();
            }
             
            
        }

        public int UpdateRate
        {
            get { return update_rate; }
            set { update_rate = value; }
        }

        public int chassis_temp
        {
            get { return systemps.ChassisTemp; }
        }
        public int chassis_humidity
        {
            get { return systemps.ChassisHumidity; }
        }

        private void AutoDeviceSearch()
        {
            ports = SerialPort.GetPortNames();
            for(int i = 0; i<ports.Length; i++)
            {
                try
                {
                    MainPort.PortName = ports[i];
                    MainPort.BaudRate = BaudRate;
                    MainPort.Open();
                    this.SendData("F");
                    while (true)
                    {
                        
                        if (this.ReceiveData().Contains("PC"))
                        {
                            device_port = ports[i];
                            DeviceConnected = true;
                            break;
                        }
                        if (PortError_count++ > 5)
                        {
                            this.StopSerial();
                            PortError_count= 0;
                            break;
                        }
                        Thread.Sleep(500);
                    }


                }
                catch (Exception ex)
                {
                    this.StopSerial();
                }
            }
            SaveSoftwareSettings();
        }

        public string[] AvailablePorts
        {
            get
            {
                ports = SerialPort.GetPortNames();
                return ports;
            }
        }

        public string CreateSerial(int BaudRates, string port_name)
        {
            try
            {
                MainPort.PortName = port_name;
                MainPort.BaudRate = BaudRate;
                MainPort.Open();
                DeviceConnected = true;   
                return "OK";

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public int StopSerial()
        {
            if (MainPort.IsOpen)
            {
                DeviceConnected = false;
                this.ResetBuffer();
                MainPort.Close();
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public void SendData(string data)
        {
            if (MainPort.IsOpen)
            {
                MainPort.DiscardInBuffer();
                MainPort.DiscardOutBuffer();
                MainPort.Write(data);
            }
        }

        public void ResetBuffer()
        {

            if (MainPort.IsOpen)
            {
                MainPort.ReadExisting();
                MainPort.DiscardInBuffer();
                MainPort.DiscardOutBuffer();
            }
        }
        public string ReceiveData()
        {
            try
            {
                string reply = MainPort.ReadExisting().Replace(";\r\n", "");
                MainPort.DiscardInBuffer();
                MainPort.DiscardOutBuffer();
                return reply;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void PC1GetSensorData()
        {
            string data;
            this.SendData("S");
            while (true)
            {
                if (MainPort.BytesToRead > 1)
                {
                    data = this.ReceiveData();
                    break;
                }
            }
                    IncomingSensorData = data.Split(';');
                    try
                    {
                        for (int i = 0; i < 4; i++) //Converting each character into 
                        {
                            SensorsData[i] = Convert.ToInt32(IncomingSensorData[i]);
                        }

                        systemps.ChassisTemp = SensorsData[0];
                        systemps.ChassisHumidity = SensorsData[1];
                        this.ProgramFanSpeed = SensorsData[2];
                        this.TachoFanSpeed = SensorsData[3];
                    }
                    catch (Exception EX)
                    {
                        this.ResetBuffer();
                    }
                    PC1SaveSensorData();
        }

        void PC1GetSettings()
        {
           
                string data;
                this.SendData("P");
                while (true)
                {
                    if (MainPort.BytesToRead > 1)
                    {
                        data = this.ReceiveData();
                        break;
                    }
                }
                IncomingSettingsData = data.Split(';');
                    try
                    {
                        for (int i = 0; i < 11; i++) //Converting each character into 
                        {
                            SettingsData[i] = Convert.ToInt32(IncomingSettingsData[i]);
                        }

                        this.CurrentFanMode = SettingsData[0];
                        this.Hysteresis = SettingsData[1];
                        this.ManualFanSpeed = SettingsData[2];
                        this.Mode = (byte)SettingsData[3];
                        this.setBrightnessFromDevice(SettingsData[4]);
                        this.Hue = SettingsData[5];
                        this.Sat = SettingsData[6];
                        this.ColorChangeSpeed = SettingsData[7];
                        this.BreatheSpeed = SettingsData[8];
                        this.variable_brightness_mode = (byte)SettingsData[9];
                        this.variable_brightness_value = (byte)SettingsData[10];
                    }
                    catch (Exception EX)
                    {

                        this.ResetBuffer();
                    }
                    PC1SaveSettingsData();  
        }

        private void GetDeviceInfo()
        {
            device_model = "";
            string data;
            string[] DeviceInfo = new string[2];
            this.SendData("F");
            while (true)
            {
                if (MainPort.BytesToRead > 1)
                {
                    data = this.ReceiveData();
                    break;
                }
            }
            DeviceInfo = data.Split(';');
            this.device_firmware = DeviceInfo[0];
            this.device_model = DeviceInfo[1];
        }

        private void PC1Update()
        {
            string queue = "";
            queue = CurrentFanMode + ";" + Hysteresis + ";" + ManualFanSpeed + ";" + Mode + ";" + brightness255 + ";" + Hue + ";" + Sat + ";" + ColorChangeSpeed + ";" + BreatheSpeed + ";" + variable_brightness_mode + ";" + variable_brightness_value + ";" + "E";
            this.SendData(queue);
            
        }

        private void PC1SaveSensorData()
        {
            int[] SensorValues = new int[4];
            SensorValues[0] = chassis_temp;
            SensorValues[1] = chassis_humidity;
            SensorValues[2] = ProgramFanSpeed;
            SensorValues[3] = TachoFanSpeed;
            using (StreamWriter SensorData = new StreamWriter(IO_files_folder + "\\" + DeviceSensorData, false))
            {
                for (int i = 0; i < SensorValues.Length; i++)
                {
                    SensorData.WriteLine(SensorValues[i]);
                }

            }
        }


        private Stack<string> AllSensors = new Stack<string>();
        private void PC1RestoreSensorData()
        {
            string file = IO_files_folder + "\\" + DeviceSensorData;
            if (File.Exists(file))
            {
                string NewData;
                using (StreamReader DatabaseFetcher = new StreamReader(file))
                {
                    while ((NewData = DatabaseFetcher.ReadLine()) != null)
                    {
                        AllSensors.Push(NewData);
                    }
                }
            }
            TachoFanSpeed = Convert.ToInt32(AllSensors.Pop()); ;
            ProgramFanSpeed = Convert.ToInt32(AllSensors.Pop());
            systemps.ChassisHumidity = Convert.ToInt32(AllSensors.Pop());
            systemps.ChassisTemp = Convert.ToInt32(AllSensors.Pop());
            AllSensors.Clear();



        }

        
        private void PC1SaveSettingsData()
        {
            int[] SettingsValues = new int[11];
            SettingsValues[0] = CurrentFanMode;
            SettingsValues[1] = Hysteresis;
            SettingsValues[2] = ProgramFanSpeed;
            SettingsValues[3] = Mode;
            SettingsValues[4] = brightness255;
            SettingsValues[5] = Hue;
            SettingsValues[6] = Sat;
            SettingsValues[7] = ColorChangeSpeed;
            SettingsValues[8] = BreatheSpeed;
            SettingsValues[9] = variable_brightness_mode;
            SettingsValues[10] = variable_brightness_value;
            using (StreamWriter SensorData = new StreamWriter(IO_files_folder + "\\" + DeviceSettings, false))
            {
                for (int i = 0; i < SettingsValues.Length; i++)
                {
                    SensorData.WriteLine(SettingsValues[i]);
                }

            }
        }

        private Stack<string> AllSettings = new Stack<string>();
        private void PC1RestoreSettingsData() 
        {
            string file = IO_files_folder + "\\" + DeviceSettings;
            if (File.Exists(file))
            {
                string NewData;
                using (StreamReader DatabaseFetcher = new StreamReader(file))
                {
                    while ((NewData = DatabaseFetcher.ReadLine()) != null)
                    {
                        AllSettings.Push(NewData);
                    }
                }
            }      
            this.variable_brightness_value = (byte)Convert.ToInt32(AllSettings.Pop());
            this.variable_brightness_mode = (byte)Convert.ToInt32(AllSettings.Pop());
            this.BreatheSpeed = Convert.ToInt32(AllSettings.Pop());
            this.ColorChangeSpeed = Convert.ToInt32(AllSettings.Pop());
            this.Sat = Convert.ToInt32(AllSettings.Pop());
            this.Hue = Convert.ToInt32(AllSettings.Pop());
            this.setBrightnessFromDevice(Convert.ToInt32(AllSettings.Pop()));
            this.Mode = (byte)Convert.ToInt32(AllSettings.Pop());
            this.ManualFanSpeed = Convert.ToInt32(AllSettings.Pop());
            this.Hysteresis = Convert.ToInt32(AllSettings.Pop());
            this.CurrentFanMode = Convert.ToInt32(AllSettings.Pop());
            AllSettings.Clear();
        }

        private void SaveSoftwareSettings()
        {
            string[] SettingsValues = new string[2];
            SettingsValues[0] = device_port;
            SettingsValues[1] = update_rate.ToString();
            using (StreamWriter SensorData = new StreamWriter(settings_folder + "\\" + ProgramSettingsFile, false))
            {
                for (int i = 0; i < SettingsValues.Length; i++)
                {
                    SensorData.WriteLine(SettingsValues[i]);
                }

            }
        }

        private Stack<string> ProgramSettings = new Stack<string>();
        private void RestoreSoftwareSettings()
        {
            string file = settings_folder + "\\" + ProgramSettingsFile;
            if (File.Exists(file))
            {
                string NewData;
                using (StreamReader DatabaseFetcher = new StreamReader(file))
                {
                    while ((NewData = DatabaseFetcher.ReadLine()) != null)
                    {
                        ProgramSettings.Push(NewData);
                    }
                }
                update_rate = Convert.ToInt32(ProgramSettings.Pop());
                device_port = ProgramSettings.Pop();
            }   
        }
        private void InitialSetup()
        {
            GetDeviceInfo();
            GetSensors();
            GetSettings();
        }

        public void GetSensors()
        {
            
            if (!device_model.Contains("PC") && DeviceConnected)
            {
                InitialSetup();
            }

            switch (device_model)
            {
                case "PC1.0":
                    PC1GetSensorData();
                    PC1SaveSensorData();
                    break;
            }
        }

        public void SaveSettings()
        {
            if (!device_model.Contains("PC") && DeviceConnected)
            {
                InitialSetup();
            }

            switch (device_model)
            {
                case "PC1.0":
                    PC1SaveSettingsData();
                    break;
            }
        }

        public void RestoreSensor()
        {
            if (!device_model.Contains("PC") && DeviceConnected)
            {
                InitialSetup();
            }

            switch (device_model)
            {
                case "PC1.0":
                    PC1RestoreSensorData();
                    break;
            }
        }

        public void RestoreSettings()
        {
            if (!device_model.Contains("PC") && DeviceConnected)
            {
                InitialSetup();
            }

            switch (device_model)
            {
                case "PC1.0":
                    PC1RestoreSettingsData();
                    break;
            }
        }

        public void GetSettings()
        {
            if (!device_model.Contains("PC") && DeviceConnected)
            {
                InitialSetup();
            }

            switch (device_model)
            {
                case "PC1.0":
                    PC1GetSettings();
                    PC1SaveSettingsData();
                    break;
            }
        }

        public void FlashToDevice()
        {
            if (!device_model.Contains("PC") && DeviceConnected)
            {
                InitialSetup();
            }

            switch (device_model)
            {
                case "PC1.0":
                    PC1Update();
                    break;
            }
        }



    }
}
