using System;
using System.Windows.Forms;

namespace SistemaVentas
{
    public partial class FrmMenuPrincipal : Form
    {
        public FrmMenuPrincipal()
        {
            InitializeComponent();
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {

            FrmRegistrarVenta frmVenta = new FrmRegistrarVenta();
            frmVenta.ShowDialog();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo en construcción...", "Aviso");
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Módulo en construcción...", "Aviso");
        }
    }
}