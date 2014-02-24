using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;   // required for Marshal
using System.IO;
using Microsoft.Win32.SafeHandles;


namespace KFly.Communication
{
    /// <summary>
    /// Detects all changes on USB
    /// Based on code from DriveDetector http://www.codeproject.com/Articles/18062/Detecting-USB-Drive-Removal-in-a-C-Program
    /// </summary>
    public class USBHandler
    {
        // Win32 constants
        private const int DBT_DEVTYP_DEVICEINTERFACE = 5;
        private const int DBT_DEVTYP_HANDLE = 6;
        private const int BROADCAST_QUERY_DENY = 0x424D5144;
        public const int WM_DEVICECHANGE = 0x0219;

        public const int DBT_CONFIGCHANGED = 0x0018;
        public const int DBT_CONFIGCHANGEDCANCELED = 0x0017;
        public const int DBT_DEVICEARRIVAL = 0x8000; // system detected a new device
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;   // Preparing to remove (any program can disable the removal)
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const int DBT_DEVNODES_CHANGED = 0x0007;

        public const int DBT_DEVTYP_VOLUME = 0x00000002; // drive type is logical volume
    }
}

