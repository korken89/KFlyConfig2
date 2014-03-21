using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFly
{
    public static class DataDumper
    {
        public static void Dump(String filename, object data)
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(data.GetType());
            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            try
            {
                writer.Serialize(file, data);
            }
            finally
            {
                file.Close();
            }
        }
    }
}
