using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace KFly
{

    /// <summary>
    /// Defines that if sent, this will affect settings on card,
    /// (Needs a save to flash to store the settings later)
    /// </summary>
    public class AffectSettingsOnCard : Attribute
    {
        public static Boolean AppliesTo(KFlyCommandType type)
        {
            FieldInfo fi = type.GetType().GetField(type.ToString());
            if (fi != null)
            {
                return (fi.GetCustomAttributes(typeof(AffectSettingsOnCard), false).Length > 0);
            }
			return false;
        }
    }
}