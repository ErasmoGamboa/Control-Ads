using System;
using System.Data;
using System.IO;
using System.Globalization;

namespace AppLogin
{
    public static class TasaDolarManager
    {
        private static string xmlPathTasa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "configuracion.xml");

        public static void GuardarTasa(string valor)
        {
            // Validamos que sea un número válido antes de guardar
            if (decimal.TryParse(valor.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal resultado))
            {
                DataTable dt = new DataTable("Config");
                dt.Columns.Add("Tasa");
                // Guardamos con formato de 4 decimales
                dt.Rows.Add(resultado.ToString("F4", CultureInfo.InvariantCulture));
                dt.WriteXml(xmlPathTasa);
            }
        }

        public static string ObtenerTasa()
        {
            if (File.Exists(xmlPathTasa))
            {
                DataTable dt = new DataTable("Config");
                dt.Columns.Add("Tasa");
                dt.ReadXml(xmlPathTasa);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Tasa"].ToString();
                }
            }
            return "0.0000";
        }
    }
}