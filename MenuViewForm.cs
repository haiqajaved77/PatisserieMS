using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using PatisserieMS.Database;

namespace PatisserieMS.Forms
{
    public class MenuViewForm : Form
    {
        private static readonly Color Gold      = Color.FromArgb(212, 175, 55);
        private static readonly Color DarkBrown = Color.FromArgb(62, 39, 35);
        private static readonly Color Cream     = Color.FromArgb(255, 253, 245);
        private static readonly Color LightGold = Color.FromArgb(255, 248, 220);

        private DataGridView dgvMenu;
        private DataTable _fullMenu;
        private FlowLayoutPanel flpCat;
        private Label lblCount;

        public MenuViewForm()
        {
            this.Text            = "Menu – La Patisserie";
            this.Size            = new Size(860, 660);
            this.StartPosition   = FormStartPosition.CenterParent;
            this.BackColor       = Cream;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;

            var pnlH = new Panel { Dock=DockStyle.Top, Height=60, BackColor=DarkBrown };
            pnlH.Controls.Add(new Label { Text="🥐  Our Menu", Font=new Font("Georgia",20f,FontStyle.Bold), ForeColor=Gold, AutoSize=true, Location=new Point(20,14) });

            var pnlF = new Panel { Location=new Point(0,60), Size=new Size(860,60), BackColor=LightGold };
            pnlF.Controls.Add(new Label { Text="Category:", Font=new Font("Segoe UI",9f,FontStyle.Bold), ForeColor=DarkBrown, AutoSize=true, Location=new Point(12,20) });
            flpCat = new FlowLayoutPanel { Location=new Point(90,8), Size=new Size(760,48), BackColor=Color.Transparent, FlowDirection=FlowDirection.LeftToRight, WrapContents=true };
            pnlF.Controls.Add(flpCat);

            dgvMenu = new DataGridView
            {
                Location=new Point(10,128), Size=new Size(836,460),
                ReadOnly=true, AllowUserToAddRows=false, AllowUserToDeleteRows=false,
                SelectionMode=DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor=Cream, BorderStyle=BorderStyle.None,
                GridColor=Color.FromArgb(235,220,185), RowHeadersVisible=false,
                ColumnHeadersHeight=34, AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,
                Font=new Font("Segoe UI",9.5f)
            };
            dgvMenu.ColumnHeadersDefaultCellStyle.BackColor = DarkBrown;
            dgvMenu.ColumnHeadersDefaultCellStyle.ForeColor = Gold;
            dgvMenu.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI",10f,FontStyle.Bold);
            dgvMenu.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvMenu.EnableHeadersVisualStyles = false;
            dgvMenu.AlternatingRowsDefaultCellStyle.BackColor = LightGold;
            dgvMenu.RowTemplate.Height = 34;

            var pnlBot = new Panel { Dock=DockStyle.Bottom, Height=30, BackColor=Color.FromArgb(240,235,210) };
            lblCount = new Label { Font=new Font("Segoe UI",8.5f), ForeColor=Color.DimGray, AutoSize=true, Location=new Point(12,8) };
            pnlBot.Controls.Add(lblCount);

            this.Controls.AddRange(new Control[]{pnlH,pnlF,dgvMenu,pnlBot});

            _fullMenu = DatabaseHelper.GetMenuItems();
            ApplyFilter("All");
            LoadCategoryButtons();
        }

        private void LoadCategoryButtons()
        {
            flpCat.Controls.Clear();
            foreach (var cat in DatabaseHelper.GetCategories())
            {
                var btn = new Button
                {
                    Text=cat, Tag=cat, Size=new Size(90,28),
                    FlatStyle=FlatStyle.Flat, Cursor=Cursors.Hand,
                    Font=new Font("Segoe UI",8.5f),
                    BackColor=(cat=="All")?DarkBrown:Color.White,
                    ForeColor=(cat=="All")?Color.White:DarkBrown,
                    Margin=new Padding(2,2,2,2)
                };
                btn.FlatAppearance.BorderColor = DarkBrown;
                btn.Click += (s,e) =>
                {
                    var b=(Button)s; string c=b.Tag.ToString();
                    ApplyFilter(c);
                    foreach (Button x in flpCat.Controls)
                    { x.BackColor=x.Tag.ToString()==c?DarkBrown:Color.White; x.ForeColor=x.Tag.ToString()==c?Color.White:DarkBrown; }
                };
                flpCat.Controls.Add(btn);
            }
        }

        private void ApplyFilter(string category)
        {
            DataTable filtered = _fullMenu.Clone();
            if (category=="All") { foreach (DataRow r in _fullMenu.Rows) filtered.ImportRow(r); }
            else { foreach (DataRow r in _fullMenu.Rows) if (r["Category"].ToString()==category) filtered.ImportRow(r); }

            dgvMenu.DataSource = filtered;
            if (dgvMenu.Columns.Contains("ItemID"))      dgvMenu.Columns["ItemID"].Visible=false;
            if (dgvMenu.Columns.Contains("IsAvailable")) dgvMenu.Columns["IsAvailable"].Visible=false;
            if (dgvMenu.Columns.Contains("Name"))        { dgvMenu.Columns["Name"].HeaderText="Item Name"; dgvMenu.Columns["Name"].FillWeight=30; }
            if (dgvMenu.Columns.Contains("Category"))    { dgvMenu.Columns["Category"].HeaderText="Category"; dgvMenu.Columns["Category"].FillWeight=15; }
            if (dgvMenu.Columns.Contains("Price"))       { dgvMenu.Columns["Price"].HeaderText="Price (PKR)"; dgvMenu.Columns["Price"].DefaultCellStyle.Format="N0"; dgvMenu.Columns["Price"].FillWeight=10; dgvMenu.Columns["Price"].DefaultCellStyle.Alignment=DataGridViewContentAlignment.MiddleRight; }
            if (dgvMenu.Columns.Contains("Description")) { dgvMenu.Columns["Description"].HeaderText="Description"; dgvMenu.Columns["Description"].FillWeight=45; }
            lblCount.Text = $"Showing {filtered.Rows.Count} item(s)";
        }
    }
}
