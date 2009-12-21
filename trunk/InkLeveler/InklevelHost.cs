/* InklevelHost.cs
 *
 * (c) 2009 Douglas Esanbock
 *
 * This software is licensed under the terms of the GPL.
  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace InkLeveler
{
    public class InklevelHost
    {
        private string _hostName; 
        private int _port;

        public InklevelHost(string hostName, int port)
        {
            _hostName = hostName;
            _port = port;
        }

        public Printer[] GetPrinters()
        {
            string url = "http://" + _hostName + ":" + _port + "/GetPrinters";
            XmlDocument response = new XmlDocument();
            response.Load(url);
            Program.ThrowError(response);

            List<Printer> printers = new List<Printer>();

            foreach (XmlElement printerElement in response["Printers"])
            {
                Printer printer = new Printer(_hostName, _port, printerElement["Name"].InnerText, printerElement["Device"].InnerText);
                printers.Add(printer);
            }

            return printers.ToArray();
        }


    }
}
