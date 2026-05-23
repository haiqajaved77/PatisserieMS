using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PatisserieMS.Models;

namespace PatisserieMS.Forms
{
    public class BillForm : Form
    {
        private static readonly Color Gold      = Color.FromArgb(212, 175, 55);
        private static readonly Color DarkBrown = Color.FromArgb(62, 39, 35);
        private static readonly Color Cream     = Color.FromArgb(255, 253, 245);

        private readonly int             _orderId;
        private readonly string          _customerName;
        private readonly List<OrderItem> _items;
        private readonly double          _subtotal;
        private readonly double          _total;
        private const double             TaxRate = 0.08;
        private RichTextBox rtbBill;

        public BillForm(int orderId, string customerName,
                        List<OrderItem> items, double subtotal, double total)
        {
            _orderId      = orderId;
            _customerName = customerName;
            _items        = items;
            _subtotal     = subtotal;
            _total        = total;

            this.Text            = $"Bill – Order #{_orderId}";
            this.Size            = new Size(480, 680);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.BackColor       = Cream;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;

            var pnlH = new Panel { Dock=DockStyle.Top, Height=56, BackColor=DarkBrown };
            pnlH.Controls.Add(new Label { Text=$"🧾  Receipt – Order #{_orderId}", Font=new Font("Georgia",14f,FontStyle.Bold), ForeColor=Gold, AutoSize=true, Location=new Point(16,14) });

            rtbBill = new RichTextBox
            {
                Location=new Point(16,68), Size=new Size(446,520),
                Font=new Font("Courier New",10f),
                BackColor=Color.White, ForeColor=Color.Black,
                ReadOnly=true, BorderStyle=BorderStyle.FixedSingle,
                ScrollBars=RichTextBoxScrollBars.Vertical
            };

            var btnPrint = new Button { Text="🖨  Print Receipt", Size=new Size(160,36), Location=new Point(16,598), BackColor=DarkBrown, ForeColor=Color.White, FlatStyle=FlatStyle.Flat, Font=new Font("Segoe UI",10f,FontStyle.Bold), Cursor=Cursors.Hand };
            btnPrint.FlatAppearance.BorderSize=0;
            btnPrint.Click += (s,e) => MessageBox.Show("Printing feature coming soon!","Print",MessageBoxButtons.OK,MessageBoxIcon.Information);

            var btnClose = new Button { Text="Close", Size=new Size(100,36), Location=new Point(362,598), BackColor=Color.FromArgb(120,100,80), ForeColor=Color.White, FlatStyle=FlatStyle.Flat, Font=new Font("Segoe UI",10f), Cursor=Cursors.Hand };
            btnClose.FlatAppearance.BorderSize=0;
            btnClose.Click += (s,e) => this.Close();

            this.Controls.AddRange(new Control[]{pnlH,rtbBill,btnPrint,btnClose});
            RenderBill();
        }

        private void RenderBill()
        {
            string div  = new string('-', 46);
            string tdiv = new string('=', 46);
            var lines = new List<string>();
            lines.Add(tdiv);
            lines.Add(Center("LA PATISSERIE"));
            lines.Add(Center("Fine Pastries & Confections"));
            lines.Add(Center("Karachi, Pakistan"));
            lines.Add(Center("Tel: 021-1234567"));
            lines.Add(tdiv);
            lines.Add($"Order #  : {_orderId}");
            lines.Add($"Date     : {DateTime.Now:dd-MMM-yyyy  HH:mm:ss}");
            lines.Add($"Customer : {_customerName}");
            lines.Add(div);
            lines.Add($"{"Item",-26}{"Qty",4}{"Price",8}{"Total",8}");
            lines.Add(div);
            foreach(var i in _items)
            {
                string n = i.Name.Length>24 ? i.Name.Substring(0,22)+".." : i.Name;
                lines.Add($"{n,-26}{i.Quantity,4}{i.UnitPrice,8:N0}{i.Subtotal,8:N0}");
            }
            lines.Add(div);
            double tax = _subtotal * TaxRate;
            lines.Add($"{"Subtotal",-34}Rs.{_subtotal,7:N0}");
            lines.Add($"{"Tax (8%)",-34}Rs.{tax,7:N0}");
            lines.Add(tdiv);
            lines.Add($"{"TOTAL",-34}Rs.{_total,7:N0}");
            lines.Add(tdiv);
            lines.Add("");
            lines.Add(Center("Thank you for visiting!"));
            lines.Add(Center("*** Enjoy your treats! ***"));
            rtbBill.Text = string.Join("\n", lines);
        }

        private string Center(string text)
        {
            if(text.Length>=46) return text;
            int p=(46-text.Length)/2;
            return new string(' ',p)+text;
        }
    }
}
