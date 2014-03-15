using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace KFly
{
    public class KFlyData: KFlyConfigurationData
    {
        private String _sTM32F4DeviceId = "";

        public String STM32F4DeviceId
        {
            get { return _sTM32F4DeviceId; }
            set 
            { 
                _sTM32F4DeviceId = value;
                NotifyPropertyChanged("STM32F4DeviceId");
            }
        }
        private String _userId = "";

        public String UserId
        {
            get { return _userId; }
            set 
            { 
                _userId = value;
                NotifyPropertyChanged("UserId");
            }
        }
        private String _firmwareVersion = "";

        public String FirmwareVersion
        {
            get { return _firmwareVersion; }
            set 
            { 
                _firmwareVersion = value;
                NotifyPropertyChanged("FirmwareVersion");
            }
        }
        private String _bootloaderVersion = "";

        public String BootloaderVersion
        {
            get { return _bootloaderVersion; }
            set 
            { 
                _bootloaderVersion = value;
                NotifyPropertyChanged("BootloaderVersion");
            }
        }

        public override bool IsValid
        {
            get
            {
                return true; //Nothing here is important for flight
            }
        }

        public void SetBytes(List<byte> bytes)
        {
            if (bytes.Count >= 12)
            {
                byte[] data = bytes.ToArray();
                STM32F4DeviceId = BitConverter.ToString(data, 0, 12);
                String[] res  = System.Text.ASCIIEncoding.Default.GetString(bytes.GetRange(12, bytes.Count-12).ToArray()).Split('\0');
                BootloaderVersion = res[0];
                FirmwareVersion = res[1];
                UserId = res[2];
            }

        }

        public static KFlyData FromBytes(List<byte> bytes)
        {
            var pi = new KFlyData();
            pi.SetBytes(bytes);
            return pi;
        }
    }
}
