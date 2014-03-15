using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace KFly
{

    /// <summary>
    /// Tells the owning data object to check this property IsInSync in its own IsInSync method
    /// </summary>
    public class KFlySyncCheck : Attribute
    {
        public static List<PropertyInfo> GetAllSyncProperties(Type type)
        {
            List<PropertyInfo> pis = new List<PropertyInfo>();
            PropertyInfo[] all = type.GetProperties();
            foreach (PropertyInfo pi in all)
            {
                if (pi.GetCustomAttribute(typeof(KFlySyncCheck)) != null)
                    pis.Add(pi);
            }
            return pis;
         }
    }
}