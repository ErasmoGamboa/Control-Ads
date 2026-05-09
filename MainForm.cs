using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AppLogin
{
    public class MainForm : Form
    {
        private DataGridView dgvClientes;
        private Panel panelSuperior;
        private Panel panelInferior;
        private Label lblTasaTitulo;
        private TextBox txtTasaDolar;

        private Label lblPlanStarter;
        private Label lblPlanPymes;
        private Label lblPlanCorporativo;

        public MainForm()
        {
            this.Text = "Panel de Control - Compurobotik C.A.";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            ConfigurarComponentes();
            CargarDatosDesdeBD();
            
            // Cargamos la tasa guardada al iniciar
            // --- Configuración avanzada del cuadro de Tasa ---
            txtTasaDolar = new TextBox() {
               Location = new Point(820, 18),
               Width = 120,
               Font = new Font("Arial", 11, FontStyle.Bold),
               TextAlign = HorizontalAlignment.Center
            };

            // 1. Bloquear letras (Solo permite números, un separador decimal y teclas de control como borrar)
            txtTasaDolar.TextChanged += (object sender, EventArgs e) => {
                // Solo permite números y un separador decimal (punto o coma)
                string original = txtTasaDolar.Text;
                string filtrado = Regex.Replace(original, @"[^0-9.,]", "");

                if (original != filtrado)
                {
               
                   txtTasaDolar.Text = filtrado;
                   // Ponemos el cursor al final para que no salte al inicio al borrar
                   txtTasaDolar.SelectionStart = txtTasaDolar.Text.Length;
                }

                ActualizarPreciosPlanes();
            };


            txtTasaDolar.KeyDown += (object sender, KeyEventArgs e) => {
               if (e.KeyCode == Keys.Enter) {
                  ActualizarTasa();
                  // Quita el foco del textbox para que se dispare el formato visual
                  this.ActiveControl = null;
                  e.SuppressKeyPress = true; // Evita el sonido de "beep" de Windows
                }
            };

            txtTasaDolar.Leave += (object sender, EventArgs e) => {
                  ActualizarTasa();
            };

            txtTasaDolar.KeyUp += (object sender, KeyEventArgs e) => {
            // Esto fuerza la actualización de los precios cada vez que presionas y sueltas una tecla
            ActualizarPreciosPlanes();
        };

            // Agregar al panel
            panelSuperior.Controls.Add(lblTasaTitulo);
            panelSuperior.Controls.Add(txtTasaDolar);
        }

        private void ActualizarTasa()
        {
            if (string.IsNullOrWhiteSpace(txtTasaDolar.Text)) return;
            {
               // Normalizamos el texto (cambiar coma por punto para el cálculo)
               string textoLimpio = txtTasaDolar.Text.Replace(',', '.');
               
               if (decimal.TryParse(textoLimpio, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal res))
               {
                  // Guardamos en el XML
                  TasaDolarManager.GuardarTasa(res.ToString(System.Globalization.CultureInfo.InvariantCulture));

                  // Mostramos en el cuadro con 4 decimales exactos
                  txtTasaDolar.Text = res.ToString("F4", CultureInfo.CurrentCulture);

                  MessageBox.Show("Tasa actualizada correctamente", "Compurobotik", MessageBoxButtons.OK, MessageBoxIcon.Information);
                  ActualizarPreciosPlanes();
               }

               else
               {
                   // Si el número es un desastre, lo reseteamos a la tasa anterior
                   txtTasaDolar.Text = TasaDolarManager.ObtenerTasa();
                   ActualizarPreciosPlanes();
        
               }
            }
        }


        private void ConfigurarComponentes()
        {
            // Panel Superior
            panelSuperior = new Panel() { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(45, 45, 45) };

            // --- Planes a la izquierda ---
            lblPlanStarter = new Label() {
                Text = "Plan Starter: 0Bs",
                ForeColor = Color.LightGreen,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(15, 5),
                AutoSize = true
            };

            lblPlanPymes = new Label() {
                Text = "Plan Pymes: 0Bs",
                ForeColor = Color.LightBlue,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(15, 22),
                AutoSize = true
            };

            lblPlanCorporativo = new Label() {
                Text = "Plan Corporativo: 0Bs",
                ForeColor = Color.Gold,
                Font = new Font("Arial", 9, FontStyle.Bold),
                Location = new Point(15, 39),
                AutoSize = true
            };

            panelSuperior.Controls.Add(lblPlanStarter);
            panelSuperior.Controls.Add(lblPlanPymes);
            panelSuperior.Controls.Add(lblPlanCorporativo);

            lblTasaTitulo = new Label() {
                Text = "Tasa Dólar (Bs):",
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(700, 20),
                AutoSize = true
            };

            panelSuperior.Controls.Add(lblTasaTitulo);
            panelSuperior.Controls.Add(txtTasaDolar);

            // Tabla (Centro)
            dgvClientes = new DataGridView() {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                BorderStyle = BorderStyle.None
            };
            dgvClientes.Columns.Add("Nombre", "Nombre Cliente");
            dgvClientes.Columns.Add("Pagina", "Nombre Página");
            dgvClientes.Columns.Add("Tel", "Teléfono");
            dgvClientes.Columns.Add("Plan", "Plan Contratado");
            dgvClientes.Columns.Add("Dias", "Días Restantes");
            dgvClientes.Columns.Add("Sector", "Sector");

            // Panel Inferior (Botones abajo al centro)
            panelInferior = new Panel() { Dock = DockStyle.Bottom, Height = 100, BackColor = Color.FromArgb(240, 240, 240) };

            Button btnAñadir = new Button() {
                Text = "Añadir Cliente",
                Size = new Size(180, 50),
                BackColor = Color.ForestGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(310, 25) // Ajustado para el centro aproximado
            };
            btnAñadir.Click += BtnAñadir_Click;

            Button btnBorrar = new Button() {
                Text = "Borrar Cliente",
                Size = new Size(180, 50),
                BackColor = Color.Firebrick,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(510, 25)
            };
            btnBorrar.Click += BtnBorrar_Click;

            panelInferior.Controls.Add(btnAñadir);
            panelInferior.Controls.Add(btnBorrar);

            this.Controls.Add(dgvClientes);
            this.Controls.Add(panelSuperior);
            this.Controls.Add(panelInferior);
        }

        private void ActualizarPreciosPlanes()
        {
            // Si la casilla está vacía, mostramos 0 directamente y salimos del método
            if (string.IsNullOrWhiteSpace(txtTasaDolar.Text))
            {
                lblPlanStarter.Text = "Plan Starter: 0.00 Bs";
                lblPlanPymes.Text = "Plan Pymes: 0.00 Bs";
                lblPlanCorporativo.Text = "Plan Corporativo: 0.00 Bs";
                return;
            }

            // Limpiamos el texto de la tasa por si tiene comas
            string tasaTexto = txtTasaDolar.Text.Replace(',', '.');

            if (decimal.TryParse(tasaTexto, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal tasa))
            {
                lblPlanStarter.Text = string.Format("Plan Starter: {0:N2} Bs", 30 * tasa);
                lblPlanPymes.Text = string.Format("Plan Pymes: {0:N2} Bs", 45 * tasa);
                lblPlanCorporativo.Text = string.Format("Plan Corporativo: {0:N2} Bs", 60 * tasa);
            }
        }

        private void CargarDatosDesdeBD()
        {
            try {
                DataTable dt = DatabaseManager.ObtenerClientes();
                dgvClientes.Rows.Clear();
                foreach (DataRow row in dt.Rows) {
                    dgvClientes.Rows.Add(row["Nombre"], row["Pagina"], row["Telefono"], row["Plan"], row["Dias"], row["Sector"]);
                }
            } catch (Exception ex) {
                MessageBox.Show("Error al cargar clientes: " + ex.Message);
            }
        }

        private void BtnAñadir_Click(object sender, EventArgs e)
        {
            using (var formAlta = new FormAñadirCliente()) {
                if (formAlta.ShowDialog() == DialogResult.OK) {
                    DatabaseManager.GuardarCliente(formAlta.Nombre, formAlta.Pagina, formAlta.Telefono, formAlta.Plan, int.Parse(formAlta.Dias), formAlta.Sector);
                    CargarDatosDesdeBD();
                }
            }
        }

        private void BtnBorrar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0) {
                string nombre = dgvClientes.SelectedRows[0].Cells["Nombre"].Value.ToString();
                if (MessageBox.Show($"¿Borrar a {nombre}?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    DatabaseManager.EliminarCliente(nombre);
                    CargarDatosDesdeBD();
                }
            }
        }
    }
}