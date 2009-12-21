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
    public partial class ColorBar : ProgressBar
    {
        private Color _fontColor = Color.White;

        public ColorBar()
            :base()
        {
            SetStyle(ControlStyles.UserPaint, true);
            //InitializeComponent();
        }

        public Color TextColor
        {
            get
            {
                return _fontColor;
            }
            set
            {
                _fontColor = value;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {

            Brush brush = new SolidBrush(ForeColor);

            Rectangle drawRect = pe.ClipRectangle;
            //drawRect.Inflate(-1, -1);

            // draw outline
            pe.Graphics.DrawRectangle(Pens.Black, drawRect.Left, drawRect.Top, pe.ClipRectangle.Width, pe.ClipRectangle.Height);
            
            // draw fill bar
            float width = (float)drawRect.Width * ((float)Value / (float)Maximum);
            drawRect.Width = (int)width;
            drawRect.Inflate(-1, -1);
            pe.Graphics.FillRectangle(brush, drawRect);


                      

            Brush fontBrush = new SolidBrush(TextColor);
            int x = 2;
            SizeF fontSize = pe.Graphics.MeasureString(Text, Font);
            float y = (pe.ClipRectangle.Height - fontSize.Height) / 2;
            pe.Graphics.DrawString(Text, Font, fontBrush, new PointF(x, y));

        }
    }
}
