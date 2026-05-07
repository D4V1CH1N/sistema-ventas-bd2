using System;
using Npgsql;
using System.Windows.Forms;

namespace SistemaVentas
{
    public class ConexionBD
    {
        private readonly string cadenaConexion = "Host=localhost;Port=5432;Database=sistema_ventas;Username=postgres;Password=1234;";
        private NpgsqlConnection conexion;

        public ConexionBD()
        {
            conexion = new NpgsqlConnection(cadenaConexion);
        }

        public NpgsqlConnection Abrir()
        {
            try
            {
                if (conexion.State == System.Data.ConnectionState.Closed)
                {
                    conexion.Open();
                }
                return conexion;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar a la Base de Datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public void Cerrar()
        {
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                conexion.Close();
            }
        }
    }
}