using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PatisserieMS.Database;
using PatisserieMS.Models;

namespace PatisserieMS.Forms
{
    public class OrderForm : Form
    {
        private static readonly Color Gold      = Color.FromArgb(212, 175, 55);
        private static readonly Color DarkBrown = Color.FromArgb(62, 39, 35);
        private static readonly Color Cream     = Color.FromArgb(255, 253, 245);
        private static readonly Color LightGold = Color.FromArgb(255, 248, 220);

        private DataGridView dgvMenu, dgvCart;
        private DataTable _menuTable;
        private List<OrderItem> _cart = new List<OrderItem>();
        private Label lblSubtotal, lblTax, lblTotal;
        private TextBox txtName;
        private FlowLayoutPanel flpCat;
        private const double TaxRate = 0.08;

        public OrderForm()
        {
            this.Text            = "Place Order – La Patisserie";
            this.Size            = new Size(1150, 720);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.BackColor       = Cream;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;

            // Header
            var pnlH = new Panel { Dock=DockStyle.Top, Height=75, BackColor=DarkBrown };
            pnlH.Controls.Add(new Label { Text="🛒  New Order", Font=new Font("Georgia",18f,FontStyle.Bold), ForeColor=Gold, AutoSize=true, Location=new Point(20,8) });
            pnlH.Controls.Add(new Label { Text="Customer Name:", Font=new Font("Segoe UI",9f), ForeColor=Color.FromArgb(200,180,140), AutoSize=true, Location=new Point(20,48) });
            txtName = new TextBox { Text="Walk-in Customer", Font=new Font("Segoe UI",9.5f), Size=new Size(250,24), Location=new Point(148,44), BackColor=Color.FromArgb(80,55,50), ForeColor=Color.White, BorderStyle=BorderStyle.FixedSingle };
            pnlH.Controls.Add(txtName);

            // Left - Menu
            var pnlLeft = new Panel { Location=new Point(0,75), Size=new Size(600,610), BackColor=Cream };
            pnlLeft.Controls.Add(new Label { Text="Menu", Font=new Font("Segoe UI",11f,FontStyle.Bold), ForeColor=DarkBrown, AutoSize=true, Location=new Point(10,10) });
            flpCat = new FlowLayoutPanel { Location=new Point(10,36), Size=new Size(578,70), BackColor=Color.Transparent, FlowDirection=FlowDirection.LeftToRight, WrapContents=true };
            pnlLeft.Controls.Add(flpCat);

            dgvMenu = MakeGrid(new Point(10,112), new Size(578,420));
            dgvMenu.DoubleClick += (s,e) => AddToCart();
            pnlLeft.Controls.Add(dgvMenu);

            var btnAdd = new Button { Text="➕  Add to Cart", Size=new Size(180,38), Location=new Point(10,544), BackColor=DarkBrown, ForeColor=Color.White, FlatStyle=FlatStyle.Flat, Font=new Font("Segoe UI",10f,FontStyle.Bold), Cursor=Cursors.Hand };
            btnAdd.FlatAppearance.BorderSize=0;
            btnAdd.Click += (s,e) => AddToCart();
            pnlLeft.Controls.Add(btnAdd);
            pnlLeft.Controls.Add(new Label { Text="Double-click or press Add to Cart", Font=new Font("Segoe UI",8f), ForeColor=Color.Gray, AutoSize=true, Location=new Point(200,554) });

            // Right - Cart
            var pnlRight = new Panel { Location=new Point(602,75), Size=new Size(540,610), BackColor=Cream };
            pnlRight.Controls.Add(new Label { Text="Your Order", Font=new Font("Segoe UI",11f,FontStyle.Bold), ForeColor=DarkBrown, AutoSize=true, Location=new Point(10,10) });

            dgvCart = MakeGrid(new Point(10,40), new Size(518,280));
            dgvCart.AutoGenerateColumns = false;
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { Name="Name", HeaderText="Item", FillWeight=40 });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { Name="Qty", HeaderText="Qty", FillWeight=12, DefaultCellStyle={ Alignment=DataGridViewContentAlignment.MiddleCenter } });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { Name="Price", HeaderText="Unit Price", FillWeight=24, DefaultCellStyle={ Alignment=DataGridViewContentAlignment.MiddleRight } });
            dgvCart.Columns.Add(new DataGridViewTextBoxColumn { Name="Sub", HeaderText="Subtotal", FillWeight=24, DefaultCellStyle={ Alignment=DataGridViewContentAlignment.MiddleRight } });
            pnlRight.Controls.Add(dgvCart);

            var btnRem = MakeBtn("🗑  Remove", new Point(10,332), Color.FromArgb(180,50,40));
            var btnClr = MakeBtn("✖  Clear Cart", new Point(170,332), Color.FromArgb(140,100,60));
            btnRem.Click += (s,e) => { if(dgvCart.CurrentRow!=null && _cart.Count>0){ _cart.RemoveAt(dgvCart.CurrentRow.Index); RefreshCart(); } };
            btnClr.Click += (s,e) => { _cart.Clear(); RefreshCart(); };
            pnlRight.Controls.Add(btnRem);
            pnlRight.Controls.Add(btnClr);

            var pnlSum = new Panel { Location=new Point(10,378), Size=new Size(518,130), BackColor=LightGold, BorderStyle=BorderStyle.FixedSingle };
            lblSubtotal = new Label { Text="Subtotal:        Rs. 0", Font=new Font("Segoe UI",10f), ForeColor=Color.FromArgb(80,60,30), AutoSize=true, Location=new Point(12,12) };
            lblTax      = new Label { Text="Tax (8%):        Rs. 0", Font=new Font("Segoe UI",10f), ForeColor=Color.FromArgb(80,60,30), AutoSize=true, Location=new Point(12,44) };
            lblTotal    = new Label { Text="Total Due:  Rs. 0", Font=new Font("Segoe UI",13f,FontStyle.Bold), ForeColor=DarkBrown, AutoSize=true, Location=new Point(12,80) };
            pnlSum.Controls.AddRange(new Control[]{lblSubtotal,lblTax,lblTotal});
            pnlRight.Controls.Add(pnlSum);

            var btnOrder = new Button { Text="✔  Place Order & Generate Bill", Size=new Size(518,50), Location=new Point(10,520), BackColor=Color.FromArgb(76,153,0), ForeColor=Color.White, FlatStyle=FlatStyle.Flat, Font=new Font("Segoe UI",11f,FontStyle.Bold), Cursor=Cursors.Hand };
            btnOrder.FlatAppearance.BorderSize=0;
            btnOrder.Click += PlaceOrder;
            pnlRight.Controls.Add(btnOrder);

            this.Controls.AddRange(new Control[]{pnlH,pnlLeft,pnlRight});
            _menuTable = DatabaseHelper.GetMenuItems();
            BindMenu(_menuTable);
            LoadCategoryButtons();
        }

        private DataGridView MakeGrid(Point loc, Size size)
        {
            var g = new DataGridView { Location=loc, Size=size, ReadOnly=true, AllowUserToAddRows=false, AllowUserToDeleteRows=false, SelectionMode=DataGridViewSelectionMode.FullRowSelect, BackgroundColor=Cream, BorderStyle=BorderStyle.None, GridColor=Color.FromArgb(235,220,185), RowHeadersVisible=false, ColumnHeadersHeight=34, AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill, Font=new Font("Segoe UI",9.5f), MultiSelect=false };
            g.ColumnHeadersDefaultCellStyle.BackColor=DarkBrown; g.ColumnHeadersDefaultCellStyle.ForeColor=Gold; g.ColumnHeadersDefaultCellStyle.Font=new Font("Segoe UI",9.5f,FontStyle.Bold);
            g.ColumnHeadersBorderStyle=DataGridViewHeaderBorderStyle.None; g.EnableHeadersVisualStyles=false;
            g.AlternatingRowsDefaultCellStyle.BackColor=LightGold; g.RowTemplate.Height=32;
            return g;
        }

        private Button MakeBtn(string text, Point loc, Color color)
        {
            var b = new Button { Text=text, Size=new Size(148,32), Location=loc, BackColor=color, ForeColor=Color.White, FlatStyle=FlatStyle.Flat, Font=new Font("Segoe UI",9f), Cursor=Cursors.Hand };
            b.FlatAppearance.BorderSize=0; return b;
        }

        private void LoadCategoryButtons()
        {
            flpCat.Controls.Clear();
            foreach (var cat in DatabaseHelper.GetCategories())
            {
                var btn = new Button { Text=cat, Tag=cat, Size=new Size(90,28), FlatStyle=FlatStyle.Flat, Cursor=Cursors.Hand, Font=new Font("Segoe UI",8.5f), BackColor=(cat=="All")?DarkBrown:Color.White, ForeColor=(cat=="All")?Color.White:DarkBrown, Margin=new Padding(2,2,2,2) };
                btn.FlatAppearance.BorderColor=DarkBrown;
                btn.Click += (s,e) =>
                {
                    var b=(Button)s; string c=b.Tag.ToString();
                    DataTable dt=c=="All"?_menuTable:Filter(c);
                    BindMenu(dt);
                    foreach(Button x in flpCat.Controls){ x.BackColor=x.Tag.ToString()==c?DarkBrown:Color.White; x.ForeColor=x.Tag.ToString()==c?Color.White:DarkBrown; }
                };
                flpCat.Controls.Add(btn);
            }
        }

        private DataTable Filter(string cat)
        {
            var dt=_menuTable.Clone();
            foreach(DataRow r in _menuTable.Rows) if(r["Category"].ToString()==cat) dt.ImportRow(r);
            return dt;
        }

        private void BindMenu(DataTable dt)
        {
            dgvMenu.DataSource=dt;
            if(dgvMenu.Columns.Contains("ItemID"))      dgvMenu.Columns["ItemID"].Visible=false;
            if(dgvMenu.Columns.Contains("IsAvailable")) dgvMenu.Columns["IsAvailable"].Visible=false;
            if(dgvMenu.Columns.Contains("Description")) dgvMenu.Columns["Description"].Visible=false;
            if(dgvMenu.Columns.Contains("Name"))     { dgvMenu.Columns["Name"].HeaderText="Item Name"; dgvMenu.Columns["Name"].FillWeight=50; }
            if(dgvMenu.Columns.Contains("Category")) { dgvMenu.Columns["Category"].HeaderText="Category"; dgvMenu.Columns["Category"].FillWeight=25; }
            if(dgvMenu.Columns.Contains("Price"))    { dgvMenu.Columns["Price"].HeaderText="Price (PKR)"; dgvMenu.Columns["Price"].DefaultCellStyle.Format="N0"; dgvMenu.Columns["Price"].DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleRight; dgvMenu.Columns["Price"].FillWeight=25; }
        }

        private void AddToCart()
        {
            if(dgvMenu.CurrentRow==null) return;
            int id=Convert.ToInt32(dgvMenu.CurrentRow.Cells["ItemID"].Value);
            string name=dgvMenu.CurrentRow.Cells["Name"].Value.ToString();
            double price=Convert.ToDouble(dgvMenu.CurrentRow.Cells["Price"].Value);
            var ex=_cart.FirstOrDefault(i=>i.ItemID==id);
            if(ex!=null) ex.Quantity++;
            else _cart.Add(new OrderItem{ItemID=id,Name=name,UnitPrice=price,Quantity=1});
            RefreshCart();
        }

        private void RefreshCart()
        {
            dgvCart.Rows.Clear();
            double sub=0;
            foreach(var i in _cart){ dgvCart.Rows.Add(i.Name,i.Quantity,$"Rs. {i.UnitPrice:N0}",$"Rs. {i.Subtotal:N0}"); sub+=i.Subtotal; }
            double tax=sub*TaxRate;
            lblSubtotal.Text=$"Subtotal:        Rs. {sub:N0}";
            lblTax.Text     =$"Tax (8%):        Rs. {tax:N0}";
            lblTotal.Text   =$"Total Due:  Rs. {sub+tax:N0}";
        }

        private void PlaceOrder(object sender, EventArgs e)
        {
            if(_cart.Count==0){ MessageBox.Show("Cart is empty!","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning); return; }
            string name=txtName.Text.Trim(); if(string.IsNullOrEmpty(name)) name="Walk-in Customer";
            double sub=_cart.Sum(i=>i.Subtotal); double total=sub+(sub*TaxRate);
            if(MessageBox.Show($"Confirm order for {name}?\nTotal: Rs. {total:N0}","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes) return;
            try
            {
                int orderId=DatabaseHelper.PlaceOrder(name,_cart);
                new BillForm(orderId,name,_cart,sub,total).ShowDialog(this);
                _cart.Clear(); RefreshCart(); txtName.Text="Walk-in Customer";
            }
            catch(Exception ex){ MessageBox.Show($"Error: {ex.Message}","Error",MessageBoxButtons.OK,MessageBoxIcon.Error); }
        }
    }
}
