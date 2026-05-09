using System;
using System.Drawing;
using System.Windows.Forms;

namespace AppLogin
{
    public class FormAñadirCliente : Form
    {
        // Campos que devolverán los datos al formulario principal
        public string Nombre { get; set; }
        public string Pagina { get; set; }
        public string Telefono { get; set; }
        public string Plan { get; set; }
        public string Dias { get; set; }
        public string Sector { get; set; }

        private TextBox[] inputs = new TextBox[6];
        private string[] etiquetas = { "Nombre Cliente", "Nombre Pagina", "Teléfono", "Plan Contratado", "Días Restantes", "Sector" };

        public FormAñadirCliente()
        {
            this.Text = "Registrar Nuevo Cliente";
            this.Size = new Size(350, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            CrearInterfaz();
        }

        private void CrearInterfaz()
        {
            int yPos = 20;
            for (int i = 0; i < etiquetas.Length; i++)
            {
                Label lbl = new Label() { Text = etiquetas[i], Location = new Point(30, yPos), AutoSize = true, Font = new Font("Arial", 9, FontStyle.Bold) };
                inputs[i] = new TextBox() { Location = new Point(30, yPos + 20), Width = 270 };
                this.Controls.Add(lbl);
                this.Controls.Add(inputs[i]);
                yPos += 55;
            }

            Button btnGuardar = new Button() { 
                Text = "GUARDAR CLIENTE", 
                Location = new Point(30, yPos + 10), 
                Width = 270, Height = 45, 
                BackColor = Color.ForestGreen, ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat, Font = new Font("Arial", 10, FontStyle.Bold)
            };

            btnGuardar.Click += (s, e) => {
                Nombre = inputs[0].Text;
                Pagina = inputs[1].Text;
                Telefono = inputs[2].Text;
                Plan = inputs[3].Text;
                Dias = inputs[4].Text;
                Sector = inputs[5].Text;

                if (!string.IsNullOrWhiteSpace(Nombre)) {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                } else {
                    MessageBox.Show("El nombre es obligatorio.");
                }
            };

            this.Controls.Add(btnGuardar);
        }
    }
}