using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Printing;

using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Soap;
using System.IO;

namespace InkLeveler
{
    public partial class InkPopup : Form
    {
        private List<Printer> _printers;
        private string configFileName = Application.LocalUserAppDataPath + "InkLeveler.xml";


        public InkPopup()
        {
            InitializeComponent();
            WindowState = FormWindowState.Minimized;
        }

        List<Printer> printers
        {
            get
            {
                if (_printers == null)
                    _printers = new List<Printer>(LoadPrinters());

                return _printers;
            }
        }

        protected Printer[] LoadPrinters()
        {
            if (!File.Exists(configFileName))
                return new Printer[]{};

            FileStream f = new FileStream(configFileName, FileMode.Open);
            SoapFormatter formatter = new SoapFormatter();
            Printer[] printers = (Printer[])formatter.Deserialize(f);
            f.Close();

            return printers;
        }

        protected void SavePrinters( Printer[] printers )
        {
            FileStream f = new FileStream(configFileName, FileMode.Create);
            SoapFormatter formatter = new SoapFormatter();
            formatter.Serialize(f, printers);
            f.Close();
        }

        private void InkPopup_Load(object sender, EventArgs e)
        {
            this.Visible = false;

        }

        private void AddPrinters()
        {
            SuspendLayout();
            int prevTop = 0;
            Height = 0;

            foreach (Printer printer in printers )
            {
                FlowLayoutPanel printerPanel = new FlowLayoutPanel();
                printerPanel.FlowDirection = FlowDirection.LeftToRight;
                printerPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                
                Label lblPrinterName = new Label();
                lblPrinterName.Text = printer.Name;
                lblPrinterName.AutoSize = true;
                printerPanel.SetFlowBreak( lblPrinterName, true );
                printerPanel.Controls.Add(lblPrinterName);
                int rows = 1;
                try
                {
                    InkTank[] tanks = printer.GetTanks(printer.Server, printer.Port, printer.Device);
                    foreach (InkTank tank in tanks)
                    {
                        addTank(printerPanel, tank);
                    }
                    rows += tanks.Count();
                }
                catch (Exception e)
                {
                    Label error = new Label();
                    error.AutoSize = true;
                    error.Text = e.Message;
                    printerPanel.Controls.Add(error);
                    rows += 2 ;
                }

                printerPanel.Top = prevTop;
                printerPanel.Width = this.Width;
                printerPanel.Height = 20 * rows;
                Height = rows * 20;
                Controls.Add(printerPanel);
                
                prevTop = printerPanel.Bottom + 1;
            }
            ResumeLayout();
        }

        private void MoveWindow()
        {
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - this.Width,
                Screen.PrimaryScreen.WorkingArea.Height - this.Height);
        }

        private static void addTank(FlowLayoutPanel printerPanel, InkTank tank)
        {
            // add label
            string tankName = tank.Color.Name;
            if (tank.IsPhoto)
                tankName = "Photo " + tankName;
            // add progress bar
            ColorBar barTank = new ColorBar();

            if (tank.IsPhoto)
            {
                //Color.FromArgb(0, tank.Color); 
                const int upVal = 100;
                barTank.ForeColor = Color.FromArgb(
                    Math.Min(255, tank.Color.R + upVal),
                    Math.Min(255, tank.Color.G + upVal),
                    Math.Min(255, tank.Color.B + upVal));
            }
            else
            {
                barTank.ForeColor = tank.Color;
            }
            barTank.TextColor = Color.FromArgb(255 - barTank.ForeColor.R,
                255 - barTank.ForeColor.G,
                255 - barTank.ForeColor.B);
            barTank.Height = 15;
            barTank.Width = printerPanel.Width - barTank.Left * 2;
            barTank.Value = (int)tank.Pct;
            barTank.Text = tankName + " (" + tank.Pct + "%)";
            printerPanel.SetFlowBreak(barTank, true);
            printerPanel.Controls.Add(barTank);
        }


        private void InkPopup_Deactivate(object sender, EventArgs e)
        {
            //WindowState = FormWindowState.Minimized;
            Visible = false;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // show the popup
                Controls.Clear();
                AddPrinters();
                Show();
                WindowState = FormWindowState.Normal;
                MoveWindow();
                //Activate();
            }
        }

        private void toolStripExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GPLAboutBox().ShowDialog();
        }

        private void toolStripAddPrinter_Click(object sender, EventArgs e)
        {
            AddPrinter addPrinterDialog = new AddPrinter();
            addPrinterDialog.ShowDialog();
            if (addPrinterDialog.DialogResult == DialogResult.OK)
            {
                printers.Add(addPrinterDialog.SelectedPrinter);

                // save file
                SavePrinters(printers.ToArray());
            }
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            removeToolStripMenuItemRemovePrinter.DropDownItems.Clear();
            foreach( Printer printer in printers )
            {
                removeToolStripMenuItemRemovePrinter.DropDownItems.Add(printer.Name, null, removePrinter_Click);
            }
        }
        private void removePrinter_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (ToolStripItem) sender;
            for( int i=0; i < printers.Count; i++ )
            {
                if (printers[i].Name == item.Text)
                {
                    // remove the selected printer
                    printers.RemoveAt(i);
                    // save settings
                    SavePrinters(printers.ToArray());
                    // we're done here
                    return;
                }
            }
            // darn it
            throw new InvalidOperationException("removePrinter_Click: item not found");
        }
    }
}
