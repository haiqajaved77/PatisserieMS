using System;
using System.Drawing;
using System.Windows.Forms;

namespace PatisserieMS.Forms
{
    public class LoginForm : Form
    {
        private static readonly Color Gold      = Color.FromArgb(212, 175, 55);
        private static readonly Color DarkBrown = Color.FromArgb(62, 39, 35);
        private static readonly Color Cream     = Color.FromArgb(255, 253, 245);

        private TextBox txtUsername, txtPassword;
        private Button btnLogin;
        private Label lblError;

        public LoginForm()
        {
            this.Text            = "Login – La Patisserie";
            this.Size            = new Size(420, 540);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.BackColor       = Cream;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox     = false;

            var pnlTop = new Panel { Dock=DockStyle.Top, Height=150, BackColor=DarkBrown };
            var lblLogo  = new Label { Text="🥐", Font=new Font("Segoe UI Emoji",36f), ForeColor=Gold, AutoSize=true, Location=new Point(165,10) };
            var lblTitle = new Label { Text="La Patisserie", Font=new Font("Georgia",20f,FontStyle.Bold), ForeColor=Gold, AutoSize=true, Location=new Point(100,72) };
            var lblSub   = new Label { Text="Management System", Font=new Font("Segoe UI",10f,FontStyle.Italic), ForeColor=Color.FromArgb(200,180,140), AutoSize=true, Location=new Point(118,106) };
            pnlTop.Controls.AddRange(new Control[]{lblLogo,lblTitle,lblSub});

            var lblWelcome = new Label { Text="Welcome Back!", Font=new Font("Segoe UI",13f,FontStyle.Bold), ForeColor=DarkBrown, AutoSize=true, Location=new Point(130,168) };

            var lblUser = new Label { Text="Username", Font=new Font("Segoe UI",9f,FontStyle.Bold), ForeColor=DarkBrown, AutoSize=true, Location=new Point(50,215) };
            txtUsername = new TextBox { Size=new Size(310,32), Location=new Point(50,237), Font=new Font("Segoe UI",11f), BorderStyle=BorderStyle.FixedSingle, BackColor=Color.White };

            var lblPass = new Label { Text="Password", Font=new Font("Segoe UI",9f,FontStyle.Bold), ForeColor=DarkBrown, AutoSize=true, Location=new Point(50,278) };
            txtPassword = new TextBox { Size=new Size(310,32), Location=new Point(50,300), Font=new Font("Segoe UI",11f), BorderStyle=BorderStyle.FixedSingle, BackColor=Color.White, PasswordChar='*' };

            lblError = new Label { Text="", ForeColor=Color.Red, Font=new Font("Segoe UI",9f), AutoSize=true, Location=new Point(50,344) };

            btnLogin = new Button
            {
                Text="LOGIN", Size=new Size(310,44), Location=new Point(50,370),
                BackColor=DarkBrown, ForeColor=Color.White, FlatStyle=FlatStyle.Flat,
                Font=new Font("Segoe UI",12f,FontStyle.Bold), Cursor=Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize=0;
            btnLogin.Click += BtnLogin_Click;

            var lblHint = new Label
            {
                Text="Admin: admin / admin123" + Environment.NewLine + "Customer: customer / customer123",
                Font=new Font("Segoe UI",8f), ForeColor=Color.Gray,
                AutoSize=true, Location=new Point(90,428)
            };

            var pnlFoot = new Panel { Dock=DockStyle.Bottom, Height=32, BackColor=DarkBrown };
            pnlFoot.Controls.Add(new Label { Text="CS-412 Visual Programming", Font=new Font("Segoe UI",8.5f), ForeColor=Color.FromArgb(160,140,100), AutoSize=true, Location=new Point(120,8) });

            this.Controls.AddRange(new Control[]{pnlTop,lblWelcome,lblUser,txtUsername,lblPass,txtPassword,lblError,btnLogin,lblHint,pnlFoot});
            this.AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim().ToLower();
            string pass = txtPassword.Text.Trim();

            if (user == "admin" && pass == "admin123")
            {
                this.Hide();
                var main = new MainForm("admin");
                main.FormClosed += (s, args) => this.Close();
                main.Show();
            }
            else if (user == "customer" && pass == "customer123")
            {
                this.Hide();
                var main = new MainForm("customer");
                main.FormClosed += (s, args) => this.Close();
                main.Show();
            }
            else
            {
                lblError.Text = "❌ Invalid username or password!";
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}
