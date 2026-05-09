using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace AppLogin
{
    public static class DatabaseManager
    {
        // El archivo se guardará como un simple archivo de texto XML
        private static string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clientes_datos.xml");

        public static void InicializarBaseDeDatos()
        {
            if (!File.Exists(xmlPath))
            {
                // Si no existe, creamos la estructura inicial
                DataTable dt = new DataTable("Clientes");
                dt.Columns.Add("Nombre");
                dt.Columns.Add("Pagina");
                dt.Columns.Add("Telefono");
                dt.Columns.Add("Plan");
                dt.Columns.Add("Dias", typeof(int));
                dt.Columns.Add("Sector");
                dt.WriteXml(xmlPath);
            }
        }

        public static void GuardarCliente(string nom, string pag, string tel, string plan, int dias, string sec)
        {
            DataTable dt = ObtenerClientes();
            dt.Rows.Add(nom, pag, tel, plan, dias, sec);
            dt.WriteXml(xmlPath);
        }

        public static DataTable ObtenerClientes()
        {
            DataTable dt = new DataTable("Clientes");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Pagina");
            dt.Columns.Add("Telefono");
            dt.Columns.Add("Plan");
            dt.Columns.Add("Dias", typeof(int));
            dt.Columns.Add("Sector");

            if (File.Exists(xmlPath))
            {
                dt.ReadXml(xmlPath);
            }
            return dt;
        }

        public static void EliminarCliente(string nombre)
        {
            DataTable dt = ObtenerClientes();
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (dt.Rows[i]["Nombre"].ToString() == nombre)
                {
                    dt.Rows.RemoveAt(i);
                }
            }
            dt.WriteXml(xmlPath);
        }
    }
}