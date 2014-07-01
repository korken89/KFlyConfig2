using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFly
{
    /**
     * @brief   Port type identifier for serial communication.
     */
    public enum KFlyPort
    {
        /**
        * @brief   USB identifier.
        */
        PORT_USB = 0,
        /**
         * @brief   AUX1 identifier.
         */
        PORT_AUX1,
        /**
         * @brief   AUX2 identifier.
         */
        PORT_AUX2,
        /**
         * @brief   AUX3 identifier.
         */
        PORT_AUX3,
        /**
         * @brief   AUX4 (CAN) identifier.
         */
        PORT_AUX4
    }
}
