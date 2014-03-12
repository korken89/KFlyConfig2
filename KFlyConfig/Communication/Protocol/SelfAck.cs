using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace KFly
{

    /// <summary>
    /// Defines that no ack request is needed on this KFlyCommand
    /// instead use the response of the message as an ack
    /// </summary>
    public class SelfAck : Attribute
    {
        public static Boolean AppliesTo(KFlyCommandType type)
        {
            FieldInfo fi = type.GetType().GetField(type.ToString());
            if (fi != null)
            {
                return (fi.GetCustomAttributes(typeof(SelfAck), false).Length > 0);
            }
			return false;
        }
    }
}