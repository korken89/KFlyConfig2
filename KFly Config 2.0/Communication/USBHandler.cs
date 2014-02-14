using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;             // required for Message
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

   
        #region WindowProc
        /// <summary>
        /// Message handler which must be called from client form.
        /// Processes Windows messages and calls event handlers. 
        /// </summary>
        /// <param name="m"></param>
        public void WndProc(ref Message m)
        {
            int devType;
            //char c;

            if (m.Msg == WM_DEVICECHANGE)
            {
                // WM_DEVICECHANGE can have several meanings depending on the WParam value...
                  switch (m.WParam.ToInt32())
                  {
                      //
                      // New device has just arrived
                      //
                      case DBT_DEVICEARRIVAL:
                          devType = Marshal.ReadInt32(m.LParam, 4);
                          break;

                      //
                      // Device is about to be removed
                      // Any application can cancel the removal
                      //
                      case DBT_DEVICEQUERYREMOVE:

                          devType = Marshal.ReadInt32(m.LParam, 4);
                          break;


                      //
                      // Device has been removed
                      //
                      case DBT_DEVICEREMOVECOMPLETE:

                          devType = Marshal.ReadInt32(m.LParam, 4);
                          break;
                  }

              }
              
            }
        }
        #endregion

    }

