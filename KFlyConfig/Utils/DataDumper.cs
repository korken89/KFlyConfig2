using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace KFly
{
    public static class DataDumper
    {
        

        public static Boolean ToXml(String filename, object data)
        {
            Boolean res = false;
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(data.GetType());
            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            try
            {
                writer.Serialize(file, data);
                res = true;
            }
            finally
            {
                file.Close();
            }
            return res;
        }

        public static Boolean ToCsv(String filename, object data)
        {
            Boolean res = false;
            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            try
            {
                var csv = new CsvWriter(file);
                if (data is System.Collections.IEnumerable)
                {
                    csv.WriteRecords(data as System.Collections.IEnumerable);
                }
                else
                {
                    csv.WriteRecord(data.GetType(), data);
                }
                res = true;
            }
            finally
            {
                file.Close();
            }
            return res;
        }
    }
}
