using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KFly.GUI
{
    public static class GUIDataDumper
    {
        public static Boolean DumpDataToFile(object data)
        {
            if (data != null)
            {
                // Configure save file dialog box
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "calibrate_rawdatadump"; // Default file name
                dlg.DefaultExt = ".csv"; // Default file extension
                dlg.Filter = "CSV documents (.csv)|*.csv|XML documents (.xml)|*.xml"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> result = dlg.ShowDialog();

                // Process save file dialog box results
                if (result == true)
                {
                    if (Path.GetExtension(dlg.FileName).ToLower() == ".csv")
                    {
                        return DataDumper.ToCsv(dlg.FileName, data);
                    }
                    else
                    {
                        return DataDumper.ToXml(dlg.FileName, data);
                    }
                }
            }
            return false;
        }

    }
}
