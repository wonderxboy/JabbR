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
        public IDataProtector Protector { get; private set; }

        private DataProtectorSingleton()
        {
            this.Protector = new MachineKeyProtectionProvider().Create("Jabbr");
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