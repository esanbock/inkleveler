using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InkLeveler
{
    public partial class AddPrinter : Form
    {
        public AddPrinter()
        {
            InitializeComponent();
        }


        private void cboDevice_DropDown(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "loading...";

            try
            {
                cboDevice.Items.Clear();
                InklevelHost host = new InklevelHost(textServer.Text, 8080);
                cboDevice.Items.AddRange(host.GetPrinters());
            }
            catch (Exception err)
            {
                toolStripStatusLabel1.Text = err.Message;
            }
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        public Printer SelectedPrinter
        {
            get
            {
                return (Printer) cboDevice.SelectedItem;
            }
        }

        private void cboDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboDevice.SelectedIndex > -1)
                acceptButton.Enabled = true;
        }
    }
}
