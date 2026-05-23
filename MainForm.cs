using System;
using System.Drawing;
using System.Windows.Forms;
using PatisserieMS.Database;
using PatisserieMS.Forms;

namespace PatisserieMS.Forms
{
    public class MainForm : Form
    {
        private static readonly Color Gold      = Color.FromArgb(212, 175, 55);
        private static readonly Color DarkBrown = Color.FromArgb(62, 39, 35);
        private static readonly Color Cream     = Color.FromArgb(255, 253, 245);
        private static readonly Color LightGold = Color.FromArgb(255, 248, 220);

        private Label lblTodayOrders, lblTodayRevenue;
        private string _userRole = "admin";

        public MainForm(string role = "admin")
        {
            _userRole = role;
            InitializeComponent();
            LoadTodayStats();
        }

        private void InitializeComponent()
        {
            this.Text            = "La Patisserie – Management System";
            this.Size            = new Size(900, 640);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.BackColor       = Cream;
            this.Font            = new Font("Segoe UI", 9.5f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;

            // Header
            var pnlHeader = new Panel { Dock=DockStyle.Top, Height=110, BackColor=DarkBrown };
            var lblTitle = new Label { Text="🥐  La Patisserie", Font=new Font("Georgia",26f,FontStyle.Bold), ForeColor=Gold, AutoSize=true, Location=new Point(30,12) };
            var lblSub   = new Label { Text="Management System", Font=new Font("Segoe UI",11f,FontStyle.Italic), ForeColor=Color.FromArgb(200,180,140), AutoSize=true, Location=new Point(36,60) };
            var lblDate  = new Label { Text=DateTime.Now.ToString("dddd, MMMM dd, yyyy"), Font=new Font("Segoe UI",10f), ForeColor=Color.FromArgb(180,160,120), AutoSize=true, Location=new Point(36,82) };
            var lblRole  = new Label
            {
                Text      = _userRole == "admin" ? "👤 Admin" : "🛍️ Customer",
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = _userRole == "admin" ? Color.FromArgb(255,200,50) : Color.FromArgb(150,255,150),
                AutoSize  = true,
                Location  = new Point(760, 14)
            };
            pnlHeader.Controls.AddRange(new Control[]{lblTitle,lblSub,lblDate,lblRole});

            // Stats
            var pnlStats = new Panel { Location=new Point(0,110), Size=new Size(900,90), BackColor=LightGold };
            var card1 = MakeCard("Today's Orders","0",new Point(30,12));
            lblTodayOrders = (Label)card1.Controls["val"];
            pnlStats.Controls.Add(card1);

            if (_userRole == "admin")
            {
                var card2 = MakeCard("Today's Revenue","Rs. 0",new Point(280,12));
                lblTodayRevenue = (Label)card2.Controls["val"];
                pnlStats.Controls.Add(card2);
            }

            // Nav buttons
            var pnlBtns = new Panel { Location=new Point(0,200), Size=new Size(900,380), BackColor=Cream };
            pnlBtns.Controls.Add(new Label { Text="Quick Actions", Font=new Font("Segoe UI",11f,FontStyle.Bold), ForeColor=DarkBrown, AutoSize=true, Location=new Point(30,20) });

            if (_userRole == "admin")
            {
                var b1 = MakeNavBtn("📋","View Menu","Browse pastries & beverages",new Point(30,55));
                var b2 = MakeNavBtn("🛒","Place Order","Take order & generate bill",new Point(300,55));
                var b3 = MakeNavBtn("📊","Daily Sales","View sales summary",new Point(570,55));
                b1.Click += (s,e) => { new MenuViewForm().ShowDialog(this); LoadTodayStats(); };
                b2.Click += (s,e) => { new OrderForm().ShowDialog(this); LoadTodayStats(); };
                b3.Click += (s,e) => { new DailySalesForm().ShowDialog(this); };
                pnlBtns.Controls.AddRange(new Control[]{b1,b2,b3});
            }
            else
            {
                var b1 = MakeNavBtn("📋","View Menu","Browse our full menu",new Point(130,55));
                var b2 = MakeNavBtn("🛒","Place Order","Order and get your bill",new Point(490,55));
                b1.Click += (s,e) => { new MenuViewForm().ShowDialog(this); };
                b2.Click += (s,e) => { new OrderForm().ShowDialog(this); };
                pnlBtns.Controls.AddRange(new Control[]{b1,b2});
            }

            // Footer
            var pnlFoot = new Panel { Dock=DockStyle.Bottom, Height=32, BackColor=DarkBrown };
            pnlFoot.Controls.Add(new Label { Text="La Patisserie Management System  |  CS-412 Visual Programming", Font=new Font("Segoe UI",8.5f), ForeColor=Color.FromArgb(160,140,100), AutoSize=true, Location=new Point(12,8) });

            this.Controls.AddRange(new Control[]{pnlHeader,pnlStats,pnlBtns,pnlFoot});
        }

        private Panel MakeCard(string title, string value, Point loc)
        {
            var p = new Panel { Size=new Size(220,66), Location=loc, BackColor=Color.White, BorderStyle=BorderStyle.FixedSingle };
            p.Controls.Add(new Label { Text=title, Font=new Font("Segoe UI",9f), ForeColor=Color.Gray, AutoSize=true, Location=new Point(12,8) });
            var v = new Label { Name="val", Text=value, Font=new Font("Segoe UI",18f,FontStyle.Bold), ForeColor=DarkBrown, AutoSize=true, Location=new Point(10,28) };
            p.Controls.Add(v);
            return p;
        }

        private Button MakeNavBtn(string emoji, string title, string desc, Point loc)
        {
            var btn = new Button { Size=new Size(240,220), Location=loc, BackColor=Color.White, FlatStyle=FlatStyle.Flat, Cursor=Cursors.Hand, Text="" };
            btn.FlatAppearance.BorderColor = Color.FromArgb(220,200,160);
            btn.FlatAppearance.MouseOverBackColor = LightGold;
            var e = new Label { Text=emoji, Font=new Font("Segoe UI Emoji",32f), AutoSize=false, Size=new Size(240,75), Location=new Point(0,15), TextAlign=ContentAlignment.MiddleCenter, BackColor=Color.Transparent };
            var t = new Label { Text=title, Font=new Font("Segoe UI",13f,FontStyle.Bold), ForeColor=DarkBrown, AutoSize=false, Size=new Size(220,28), Location=new Point(10,95), TextAlign=ContentAlignment.MiddleCenter, BackColor=Color.Transparent };
            var d = new Label { Text=desc, Font=new Font("Segoe UI",8.5f), ForeColor=Color.DimGray, AutoSize=false, Size=new Size(210,45), Location=new Point(15,128), TextAlign=ContentAlignment.TopCenter, BackColor=Color.Transparent };
            e.Click += (s,ev) => btn.PerformClick();
            t.Click += (s,ev) => btn.PerformClick();
            d.Click += (s,ev) => btn.PerformClick();
            btn.Controls.AddRange(new Control[]{e,t,d});
            return btn;
        }

        private void LoadTodayStats()
        {
            var (orders, revenue) = DatabaseHelper.GetTodaysSales();
            lblTodayOrders.Text = orders.ToString();
            if (_userRole == "admin" && lblTodayRevenue != null)
                lblTodayRevenue.Text = $"Rs. {revenue:N0}";
        }
    }
}
