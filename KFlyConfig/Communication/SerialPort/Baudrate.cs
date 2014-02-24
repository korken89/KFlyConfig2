using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KFly.Utils;

namespace KFly.Communication
{
    public enum Baudrate
    {
        [DisplayValue("1000000")]
        Baud_1000000,
        [DisplayValue("250000")]
        Baud_250000,
        [DisplayValue("115200")]
        Baud_115200,
        [DisplayValue("57600")]
        Baud_57600,
        [DisplayValue("19200")]
        Baud_19200
    }
}
