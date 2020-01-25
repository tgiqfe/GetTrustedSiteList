using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace GetTrustedSiteList
{
    class Program
    {
        const string DOMAINS_KEY = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings\ZoneMap\Domains";

        static void Main(string[] args)
        {
            List<string> trustedSiteList = new List<string>();
            using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(DOMAINS_KEY, false))
            {
                foreach (string subKeyName in regKey.GetSubKeyNames())
                {
                    using (RegistryKey subRegKey = regKey.OpenSubKey(subKeyName, false))
                    {
                        foreach (string subSubKeyName in subRegKey.GetSubKeyNames())
                        {
                            using (RegistryKey subSubRegKey = subRegKey.OpenSubKey(subSubKeyName, false))
                            {
                                foreach (string valueName in subSubRegKey.GetValueNames())
                                {
                                    trustedSiteList.Add(
                                        string.Format("{0}://{1}.{2}", valueName, subSubKeyName, subKeyName));
                                }
                            }
                        }
                    }
                }
            }
            foreach (string trustedSite in trustedSiteList)
            {
                Console.WriteLine(trustedSite);
            }
            Console.ReadLine();
        }
    }
}
