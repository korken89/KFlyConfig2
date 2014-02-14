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
        private const int WM_DEVICECHANGE = 0x0219;
        private const int DBT_DEVICEARRIVAL = 0x8000; // system detected a new device
        private const int DBT_DEVICEQUERYREMOVE = 0x8001;   // Preparing to remove (any program can disable the removal)
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004; // removed 
        private const int DBT_DEVTYP_VOLUME = 0x00000002; // drive type is logical volume

   

        #region WindowProc
        /// <summary>
        /// Message handler which must be called from client form.
        /// Processes Windows messages and calls event handlers. 
        /// </summary>
        /// <param name="m"></param>
        public void WndProc(ref Message m)
        {
           // int devType;
           // char c;

            if (m.Msg == WM_DEVICECHANGE)
            {
                // WM_DEVICECHANGE can have several meanings depending on the WParam value...
                /*  switch (m.WParam.ToInt32())
                  {
                      //
                      // New device has just arrived
                      //
                      case DBT_DEVICEARRIVAL:

                          devType = Marshal.ReadInt32(m.LParam, 4);
                          if (devType == DBT_DEVTYP_VOLUME)
                          {
                              DEV_BROADCAST_VOLUME vol;
                              vol = (DEV_BROADCAST_VOLUME)
                                  Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));

                              // Get the drive letter 
                              c = DriveMaskToLetter(vol.dbcv_unitmask);


                              //
                              // Call the client event handler
                              //
                              // We should create copy of the event before testing it and
                              // calling the delegate - if any
                              DriveDetectorEventHandler tempDeviceArrived = DeviceArrived;
                              if (tempDeviceArrived != null)
                              {
                                  DriveDetectorEventArgs e = new DriveDetectorEventArgs();
                                  e.Drive = c + ":\\";
                                  tempDeviceArrived(this, e);

                                  // Register for query remove if requested
                                  if (e.HookQueryRemove)
                                  {
                                      // If something is already hooked, unhook it now
                                      if (mDeviceNotifyHandle != IntPtr.Zero)
                                      {
                                          RegisterForDeviceChange(false, null);
                                      }

                                      RegisterQuery(c + ":\\");
                                  }
                              }     // if  has event handler


                          }
                          break;



                      //
                      // Device is about to be removed
                      // Any application can cancel the removal
                      //
                      case DBT_DEVICEQUERYREMOVE:

                          devType = Marshal.ReadInt32(m.LParam, 4);
                          if (devType == DBT_DEVTYP_HANDLE)
                          {
                              // TODO: we could get the handle for which this message is sent 
                              // from vol.dbch_handle and compare it against a list of handles for 
                              // which we have registered the query remove message (?)                                                 
                              //DEV_BROADCAST_HANDLE vol;
                              //vol = (DEV_BROADCAST_HANDLE)
                              //   Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HANDLE));
                              // if ( vol.dbch_handle ....


                              //
                              // Call the event handler in client
                              //
                              DriveDetectorEventHandler tempQuery = QueryRemove;
                              if (tempQuery != null)
                              {
                                  DriveDetectorEventArgs e = new DriveDetectorEventArgs();
                                  e.Drive = mCurrentDrive;        // drive which is hooked
                                  tempQuery(this, e);

                                  // If the client wants to cancel, let Windows know
                                  if (e.Cancel)
                                  {
                                      m.Result = (IntPtr)BROADCAST_QUERY_DENY;
                                  }
                                  else
                                  {
                                      // Change 28.10.2007: Unregister the notification, this will
                                      // close the handle to file or root directory also. 
                                      // We have to close it anyway to allow the removal so
                                      // even if some other app cancels the removal we would not know about it...                                    
                                      RegisterForDeviceChange(false, null);   // will also close the mFileOnFlash
                                  }

                              }
                          }
                          break;


                      //
                      // Device has been removed
                      //
                      case DBT_DEVICEREMOVECOMPLETE:

                          devType = Marshal.ReadInt32(m.LParam, 4);
                          if (devType == DBT_DEVTYP_VOLUME)
                          {
                              devType = Marshal.ReadInt32(m.LParam, 4);
                              if (devType == DBT_DEVTYP_VOLUME)
                              {
                                  DEV_BROADCAST_VOLUME vol;
                                  vol = (DEV_BROADCAST_VOLUME)
                                      Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));
                                  c = DriveMaskToLetter(vol.dbcv_unitmask);

                                  //
                                  // Call the client event handler
                                  //
                                  DriveDetectorEventHandler tempDeviceRemoved = DeviceRemoved;
                                  if (tempDeviceRemoved != null)
                                  {
                                      DriveDetectorEventArgs e = new DriveDetectorEventArgs();
                                      e.Drive = c + ":\\";
                                      tempDeviceRemoved(this, e);
                                  }

                                  // TODO: we could unregister the notify handle here if we knew it is the
                                  // right drive which has been just removed
                                  //RegisterForDeviceChange(false, null);
                              }
                          }
                          break;
                  }

              }
              */
            }
        }
        #endregion

    }
}
