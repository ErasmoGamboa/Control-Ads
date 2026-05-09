using System;
using System.Drawing;
using System.Windows.Forms;

namespace AppLogin
{
    public class LoginForm : Form
    {
        // Declaración de controles
        private PictureBox pbLogo;
        private Label lblUser;
        private Label lblPass;
        private TextBox txtUser;
        private TextBox txtPassword;
        private Button btnLogin;

        public LoginForm()
        {
            // Configuración básica de la ventana
            this.Text = "Acceso al Sistema - Compurobotik";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            ConfigurarInterfaz();
        }

        private void ConfigurarInterfaz()
        {
            // 1. Logo de la App (En el medio superior)
            pbLogo = new PictureBox();
            pbLogo.Size = new Size(200, 150);
            pbLogo.Location = new Point(100, 30);
            pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
            // Si tienes el archivo de imagen, descomenta la siguiente línea:
            // pbLogo.Image = Image.FromFile("logo.png"); 
            pbLogo.BorderStyle = BorderStyle.None;
            pbLogo.BackColor = Color.LightGray; // Color temporal para ver el área del logo

            // 2. Etiqueta y campo de Usuario
            lblUser = new Label() { Text = "Usuario:", Location = new Point(50, 210), Width = 300, Font = new Font("Arial", 10, FontStyle.Bold) };
            txtUser = new TextBox() { Location = new Point(50, 235), Width = 280, Font = new Font("Arial", 12) };

            // 3. Etiqueta y campo de Contraseña
            lblPass = new Label() { Text = "Contraseña:", Location = new Point(50, 280), Width = 300, Font = new Font("Arial", 10, FontStyle.Bold) };
            txtPassword = new TextBox() { Location = new Point(50, 305), Width = 280, Font = new Font("Arial", 12), PasswordChar = '●' };

            // 4. Botón de Entrar
            btnLogin = new Button();
            btnLogin.Text = "INICIAR SESIÓN";
            btnLogin.Location = new Point(50, 370);
            btnLogin.Width = 280;
            btnLogin.Height = 45;
            btnLogin.BackColor = Color.DodgerBlue;
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Arial", 10, FontStyle.Bold);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += btnLogin_Click;

            // Agregar controles al Formulario
            this.Controls.Add(pbLogo);
            this.Controls.Add(lblUser);
            this.Controls.Add(txtUser);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);

            // Hacer que el botón funcione al presionar la tecla Enter
            this.AcceptButton = btnLogin;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Validación de credenciales
            if (txtUser.Text == "admin" && txtPassword.Text == "compurobotik")
            {
                MessageBox.Show("Acceso exitoso. ¡Bienvenido!", "Compurobotik C.A.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                

            
                MainForm menuPrincipal = new MainForm();
                menuPrincipal.Show(); // Abre el menú de clientes
                // Aquí cerramos este form y abriríamos el siguiente módulo
                this.Hide();
                // FormMenuPrincipal menu = new FormMenuPrincipal();
                // menu.Show();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Error de acceso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtUser.Focus();
            }
        }
    }
}