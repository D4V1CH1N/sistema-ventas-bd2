using System;
using System.Windows.Forms;
using Npgsql;

namespace SistemaVentas
{
    public partial class FrmRegistrarVenta : Form
    {
        ConexionBD conexionObj = new ConexionBD();

        public FrmRegistrarVenta()
        {
            InitializeComponent();
        }

        private void btnGuardarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                int idCliente = Convert.ToInt32(txtIdCliente.Text);
                int idProducto = Convert.ToInt32(txtIdProducto.Text);
                int cantidad = Convert.ToInt32(txtCantidad.Text);
                decimal precio = Convert.ToDecimal(txtPrecioUnitario.Text);

                decimal totalVenta = cantidad * precio;

                NpgsqlConnection con = conexionObj.Abrir();
                if (con == null) return;

                string queryVenta = "INSERT INTO venta (id_cliente, id_empleado, estado, metodo_pago, total) " +
                                    "VALUES (@cliente, 1, 'Pagada', 'Efectivo', @total) RETURNING id_venta;";

                NpgsqlCommand cmdVenta = new NpgsqlCommand(queryVenta, con);
                cmdVenta.Parameters.AddWithValue("@cliente", idCliente);
                cmdVenta.Parameters.AddWithValue("@total", totalVenta);

                int idVentaGenerada = Convert.ToInt32(cmdVenta.ExecuteScalar());

                string queryDetalle = "INSERT INTO detalle_venta (id_venta, id_producto, cantidad, precio_unitario) " +
                                      "VALUES (@venta, @producto, @cant, @precio);";

                NpgsqlCommand cmdDetalle = new NpgsqlCommand(queryDetalle, con);
                cmdDetalle.Parameters.AddWithValue("@venta", idVentaGenerada);
                cmdDetalle.Parameters.AddWithValue("@producto", idProducto);
                cmdDetalle.Parameters.AddWithValue("@cant", cantidad);
                cmdDetalle.Parameters.AddWithValue("@precio", precio);

                cmdDetalle.ExecuteNonQuery();

                conexionObj.Cerrar();
                MessageBox.Show("Venta registrada con éxito.\nTotal cobrado: " + totalVenta.ToString("0.00") + " Bs.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtIdCliente.Clear();
                txtIdProducto.Clear();
                txtCantidad.Clear();
                txtPrecioUnitario.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar la venta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conexionObj.Cerrar(); 
            }
        }
    }
}