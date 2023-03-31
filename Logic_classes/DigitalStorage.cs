using System;

namespace Trippin_Website.Logic_classes
{
    public static class DigitalStorage
    {
        public static string ConvertToDigitalStorageSize(this float bytes)
        {
            float rezultatInFloat;
            string rezultat;

            if (bytes < 1073741824)
            {
                rezultatInFloat = bytes / 1024 / 1024;

                rezultat = Convert.ToInt16(rezultatInFloat).ToString();

                return rezultat + " MB";
            }
            else
            {
                rezultatInFloat = bytes / 1024 / 1024 / 1024;
                rezultat = string.Format("{0:0.00}", rezultatInFloat.ToString());

                return rezultat + "GB";

            }
        }
    }
}