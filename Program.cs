using System;
using System.Windows.Forms;

namespace AppLogin
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // 1. Configuraciones básicas de Windows Forms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // 2. Intentamos conectar a la base de datos
                // Si aquí falta una DLL, el bloque 'catch' nos lo dirá
                DatabaseManager.InicializarBaseDeDatos();

                // 3. Si la BD cargó bien, lanzamos el Login
                Application.Run(new LoginForm());
            }
            catch (TypeInitializationException ex)
            {
                // Este error suele ocurrir cuando falta la DLL de SQLite
                MessageBox.Show("Error de inicialización (Posible falta de SQLite.Interop.dll): " + ex.Message, 
                                "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Cualquier otro error (permisos, rutas, etc.)
                MessageBox.Show("Se produjo un error al arrancar: " + ex.Message, 
                                "Error de Aplicación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 