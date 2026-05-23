using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PatisserieMS.Database;

namespace PatisserieMS.Forms
{
    public class DailySalesForm : Form
    {
        private static readonly Color Gold      = Color.FromArgb(212, 175, 55);
        private static readonly Color DarkBrown = Color.FromArgb(62, 39, 35);
        private static readonly Color Cream     = Color.FromArgb(255, 253, 245);
        private static readonly Color LightGold = Color.FromArgb(255, 248, 220);

        private DataGridView dgvSales;
        private Label lblOrders, lblRevenue;

        public DailySalesForm()
        {
            this.Text            = "Daily Sales – La Patisserie";
            this.Size            = new Size(700, 580);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.BackColor       = Cream;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;

            var pnlH = new Panel { Dock=DockStyle.Top, Height=60, BackColor=DarkBrown };
            pnlH.Controls.Add(new Label { Text="📊  Daily Sales Report", Font=new Font("Georgia",18f,FontStyle.Bold), ForeColor=Gold, AutoSize=true, Location=new Point(20,14) });

            var pnlS = new Panel { Location=new Point(0,60), Size=new Size(700,80), BackColor=LightGold };
            pnlS.Controls.Add(new Label { Text="TODAY'S ORDERS:", Font=new Font("Segoe UI",9f,FontStyle.Bold), ForeColor=Color.FromArgb(100,80,40), AutoSize=true, Location=new Point(20,10) });
            lblOrders = new Label { Text="0", Font=new Font("Segoe UI",22f,FontStyle.Bold), ForeColor=DarkBrown, AutoSize=true, Location=new Point(20,28) };
            pnlS.Controls.Add(lblOrders);
            pnlS.Controls.Add(new Label { Text="TODAY'S REVENUE:", Font=new Font("Segoe UI",9f,FontStyle.Bold), ForeColor=Color.FromArgb(100,80,40), AutoSize=true, Location=new Point(220,10) });
            lblRevenue = new Label { Text="Rs. 0", Font=new Font("Segoe UI",22f,FontStyle.Bold), ForeColor=Color.FromArgb(46,125,50), AutoSize=true, Location=new Point(220,28) };
            pnlS.Controls.Add(lblRevenue);

            var lblH = new Label { Text="Sales History", Font=new Font("Segoe UI",10f,FontStyle.Bold), ForeColor=DarkBrown, AutoSize=true, Location=new Point(16,152) };

            dgvSales = new DataGridView
            {
                Location=new Point(16,172), Size=new Size(668,320),
                ReadOnly=true, AllowUserToAddRows=false, AllowUserToDeleteRows=false,
                SelectionMode=DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor=Cream, BorderStyle=BorderStyle.None,
                GridColor=Color.FromArgb(235,220,185), RowHeadersVisible=false,
                ColumnHeadersHeight=36, AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,
                Font=new Font("Segoe UI",10f)
            };
            dgvSales.ColumnHeadersDefaultCellStyle.BackColor=DarkBrown;
            dgvSales.ColumnHeadersDefaultCellStyle.ForeColor=Gold;
            dgvSales.ColumnHeadersDefaultCellStyle.Font=new Font("Segoe UI",10.5f,FontStyle.Bold);
            dgvSales.ColumnHeadersBorderStyle=DataGridViewHeaderBorderStyle.None;
            dgvSales.EnableHeadersVisualStyles=false;
            dgvSales.AlternatingRowsDefaultCellStyle.BackColor=LightGold;
            dgvSales.RowTemplate.Height=36;

            var pnlBot = new Panel { Dock=DockStyle.Bottom, Height=44, BackColor=Color.FromArgb(240,235,210) };
            var btnRef = new Button { Text="🔄  Refresh", Size=new Size(110,30), Location=new Point(12,8), BackColor=DarkBrown, ForeColor=Color.White, FlatStyle=FlatStyle.Flat, Font=new Font("Segoe UI",9f), Cursor=Cursors.Hand };
            btnRef.FlatAppearance.BorderSize=0;
            btnRef.Click += (s,e) => LoadData();
            var btnClose = new Button { Text="Close", Size=new Size(90,30), Location=new Point(596,8), BackColor=Color.FromArgb(120,100,80), ForeColor=Color.White, FlatStyle=FlatStyle.Flat, Font=new Font("Segoe UI",9f), Cursor=Cursors.Hand };
            btnClose.FlatAppearance.BorderSize=0;
            btnClose.Click += (s,e) => this.Close();
            pnlBot.Controls.AddRange(new Control[]{btnRef,btnClose});

            this.Controls.AddRange(new Control[]{pnlH,pnlS,lblH,dgvSales,pnlBot});
            LoadData();
        }

        private void LoadData()
        {
            var (orders, revenue) = DatabaseHelper.GetTodaysSales();
            lblOrders.Text  = orders.ToString();
            lblRevenue.Text = $"Rs. {revenue:N0}";

            DataTable dt = DatabaseHelper.GetDailySales();
            dgvSales.DataSource = dt;

            if(dgvSales.Columns.Contains("SaleDate"))    { dgvSales.Columns["SaleDate"].HeaderText="Date"; dgvSales.Columns["SaleDate"].FillWeight=35; }
            if(dgvSales.Columns.Contains("TotalOrders")) { dgvSales.Columns["TotalOrders"].HeaderText="Orders"; dgvSales.Columns["TotalOrders"].FillWeight=25; dgvSales.Columns["TotalOrders"].DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleCenter; }
            if(dgvSales.Columns.Contains("TotalRevenue")){ dgvSales.Columns["TotalRevenue"].HeaderText="Revenue (PKR)"; dgvSales.Columns["TotalRevenue"].FillWeight=40; dgvSales.Columns["TotalRevenue"].DefaultCellStyle.Format="N0"; dgvSales.Columns["TotalRevenue"].DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleRight; }

            string today = DateTime.Now.ToString("yyyy-MM-dd");
            foreach(DataGridViewRow row in dgvSales.Rows)
            {
                if(row.Cells["SaleDate"].Value?.ToString()==today)
                {
                    row.DefaultCellStyle.BackColor=Color.FromArgb(220,255,220);
                    row.DefaultCellStyle.ForeColor=Color.FromArgb(20,100,20);
                    row.DefaultCellStyle.Font=new Font("Segoe UI",10f,FontStyle.Bold);
                }
            }
        }
    }
}
