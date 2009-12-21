/* Program.cs
 *
 * (c) 2009 Douglas Esanbock
 *
 * This software is licensed under the terms of the GPL.
  */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace InkLeveler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            InkPopup popupWindow = new InkPopup();
            Application.Run(popupWindow);
        }

        public static void ThrowError(XmlDocument response)
        {
            if (response["Error"] != null)
                throw new ApplicationException(response["Error"].InnerXml);
        }

    }
}
