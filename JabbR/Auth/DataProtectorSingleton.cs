using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JabbR.Auth
{
    //TODO: inject through kernel
    public class DataProtectorSingleton
    {
        private static DataProtectorSingleton instance;
        public DpapiDataProtectionProvider ProtectionProvider {get; private set;}

        private DataProtectorSingleton()
        {
            this.ProtectionProvider = new DpapiDataProtectionProvider("Jabbr");
        }

        public static DataProtectorSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataProtectorSingleton();
            }

            return instance;
        }
    }
}