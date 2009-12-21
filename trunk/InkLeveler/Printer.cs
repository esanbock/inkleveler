/* Printer.cs
 *
 * (c) 2009 Douglas Esanbock
 *
 * This software is licensed under the terms of the GPL.
  */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace InkLeveler
{
    [Serializable]
    public class InkTank
    {
        private Color _color;
        private float _pct;
        private bool _isPhoto;


        public InkTank(Color color, float pct, bool isPhoto)
        {
            _color = color;
            _pct = pct;
            _isPhoto = isPhoto;
        }

        public Color Color
        {
            get
            {
                return _color;
            }
        }

        public float Pct
        {
            get
            {
                return _pct;
            }
        
        }
        public bool IsPhoto
        {
            get { return _isPhoto; }
        }
    }

    [Serializable]
    public class Printer
    {
        private string _name;
        private string _device;
        private string _server;
        private int _port;

        public Printer(string server, int port, string name, string device)
        {
            _name = name;
            _device = device;
            _server = server;
            _port = port;
        }


        private static Random _rand = new Random();

        public InkTank[] GetTanks( string server, int port, string deviceName )
        {
            string BaseURL = "http://" + server + ":" + port + "/GetPrinterInk";
            string parameter = "?printer=" + deviceName;
            XmlDocument response = new XmlDocument();

            response.Load(BaseURL + parameter);
            Program.ThrowError(response);

            // response
            List<InkTank> tanks = new List<InkTank>();
            // load from xml
            foreach (XmlElement tank in response["PrinterLevels"])
            {
                bool isPhoto = tank.Name.Contains("PHOTO");
                string tankName = tank.Name.Replace("CARTRIDGE_", "");
                tankName = tankName.Replace("PHOTO", "");
                Color color = Color.FromName(tankName);
                float level = float.Parse(tank.InnerText);
                tanks.Add(new InkTank(color, level, isPhoto));
            }
            return tanks.ToArray();
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Device
        {
            get
            {
                return _device;
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }
        }

        public string Server
        {
            get
            {
                return _server;
            }
        }

        public override string ToString()
        {
            return Name + " [" + Device + "]";
        }
    }
}
