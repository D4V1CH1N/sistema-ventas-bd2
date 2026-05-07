using System;
using System.Windows.Forms;

namespace SistemaVentas
{
    public partial class FrmMenuPrincipal : Form
    {
        static readonly Color C_BG_DARK = Color.FromArgb(15, 15, 30);
        static readonly Color C_BG_PANEL = Color.FromArgb(22, 33, 62);
        static readonly Color C_BG_CARD = Color.FromArgb(26, 40, 75);
        static readonly Color C_ACCENT = Color.FromArgb(79, 70, 229);
        static readonly Color C_ACCENT2 = Color.FromArgb(99, 90, 255);
        static readonly Color C_TEXT_PRI = Color.FromArgb(230, 230, 250);
        static readonly Color C_TEXT_SEC = Color.FromArgb(140, 146, 176);
        static readonly Color C_SUCCESS = Color.FromArgb(34, 197, 94);
        static readonly Color C_WARNING = Color.FromArgb(234, 179, 8);
        static readonly Color C_DANGER = Color.FromArgb(220, 38, 38);
        public FrmMenuPrincipal()
        {

            InitializeComponent();
            CargarEstadisticas();
            CargarUltimasVentas();
        }
        void InitializeComponent()
        {
            this.Text = "SistemaVentas — Panel Principal";
            this.Size = new Size(1100, 680);
            this.MinimumSize = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = C_BG_DARK;
            this.ForeColor = C_TEXT_PRI;
            this.Font = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // ── SIDEBAR ─────────────────────────────────────────────────
            pnlSidebar = new Panel
            {
                Width = 200,
                Dock = DockStyle.Left,
                BackColor = C_BG_PANEL
            };
            this.Controls.Add(pnlSidebar);

            // Logo / nombre sistema
            var pnlLogo = new Panel { Height = 70, Dock = DockStyle.Top, BackColor = C_ACCENT, Padding = new Padding(14, 16, 0, 0) };
            var lblLogo = new Label { Text = "◈  SistemaVentas", ForeColor = Color.White, Font = new Font("Segoe UI", 11f, FontStyle.Bold), AutoSize = true, Location = new Point(14, 22) };
            pnlLogo.Controls.Add(lblLogo);
            pnlSidebar.Controls.Add(pnlLogo);

            // Botones sidebar
            btnVentas = CrearBotonSidebar("  +   Registrar Venta", 72);
            btnClientes = CrearBotonSidebar("  ◎  Clientes", 122);
            btnProductos = CrearBotonSidebar("  ◈  Productos", 172);
            btnEmpleados = CrearBotonSidebar("  ◉  Empleados", 222);

            btnVentas.Click += (s, e) => AbrirFormulario(new FrmRegistrarVenta());
            btnClientes.Click += (s, e) => AbrirFormulario(new FrmClientes());
            btnProductos.Click += (s, e) => AbrirFormulario(new FrmProductos());

            // Indicador de conexión
            lblConexion = new Label
            {
                Text = "● BD Conectada",
                ForeColor = C_SUCCESS,
                Font = new Font("Segoe UI", 8f),
                AutoSize = true,
                Location = new Point(14, 560)
            };
            pnlSidebar.Controls.Add(lblConexion);

            pnlTop = new Panel
            {
                Height = 52,
                Dock = DockStyle.Top,
                BackColor = C_BG_PANEL,
                Padding = new Padding(20, 0, 20, 0)
            };
            this.Controls.Add(pnlTop);

            lblTitle = new Label
            {
                Text = "Dashboard",
                ForeColor = C_TEXT_PRI,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 14)
            };
            pnlTop.Controls.Add(lblTitle);

            lblUser = new Label
            {
                Text = "Genesis Flores  |  Vendedora",
                ForeColor = C_TEXT_SEC,
                Font = new Font("Segoe UI", 9f),
                AutoSize = true,
                Location = new Point(700, 18)
            };
            pnlTop.Controls.Add(lblUser);

            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = C_BG_DARK,
                Padding = new Padding(20)
            };
            this.Controls.Add(pnlContent);

            cardVentas = CrearCard("Ventas hoy", "0", C_ACCENT, 20, 20);
            cardIngresos = CrearCard("Ingresos", "Bs. 0", C_WARNING, 200, 20);
            cardClientes = CrearCard("Clientes", "0", C_SUCCESS, 380, 20);
            cardStock = CrearCard("Stock bajo", "0", C_DANGER, 560, 20);

            var lblSec = new Label
            {
                Text = "Últimas ventas",
                ForeColor = C_TEXT_PRI,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 135)
            };
            pnlContent.Controls.Add(lblSec);

            var btnNuevaVenta = CrearBotonAccion("+ Nueva venta", 620, 130);
            btnNuevaVenta.Click += (s, e) => AbrirFormulario(new FrmRegistrarVenta());
            pnlContent.Controls.Add(btnNuevaVenta);

            dgvVentas = new DataGridView
            {
                Location = new Point(20, 165),
                Size = new Size(760, 380),
                BackgroundColor = C_BG_CARD,
                ForeColor = C_TEXT_PRI,
                GridColor = Color.FromArgb(40, 50, 80),
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9f),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };

            dgvVentas.DefaultCellStyle.BackColor = C_BG_CARD;
            dgvVentas.DefaultCellStyle.ForeColor = C_TEXT_PRI;
            dgvVentas.DefaultCellStyle.SelectionBackColor = C_ACCENT;
            dgvVentas.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvVentas.DefaultCellStyle.Padding = new Padding(4);
            dgvVentas.ColumnHeadersDefaultCellStyle.BackColor = C_BG_PANEL;
            dgvVentas.ColumnHeadersDefaultCellStyle.ForeColor = C_TEXT_SEC;
            dgvVentas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            dgvVentas.ColumnHeadersHeight = 36;
            dgvVentas.RowTemplate.Height = 38;
            dgvVentas.EnableHeadersVisualStyles = false;

            pnlContent.Controls.Add(dgvVentas);
        }
        Button CrearBotonAccion(string texto, int x, int y)
        {
            var btn = new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(150, 34),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = C_ACCENT,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = C_ACCENT2;
            return btn;
        }

        void CargarEstadisticas()
        {
            try
            {
                var db = new ConexionBD();
                var conn = db.Abrir();
                if (conn == null) return;

                // Ventas de hoy
                using (var cmd = new Npgsql.NpgsqlCommand(
                    "SELECT COUNT(*) FROM venta WHERE DATE(fecha_venta) = CURRENT_DATE", conn))
                    ActualizarCard(cardVentas, cmd.ExecuteScalar()?.ToString() ?? "0");

                // Ingresos
                using (var cmd = new Npgsql.NpgsqlCommand(
                    "SELECT COALESCE(SUM(total),0) FROM venta WHERE DATE(fecha_venta) = CURRENT_DATE AND estado='Pagada'", conn))
                    ActualizarCard(cardIngresos, "Bs. " + cmd.ExecuteScalar()?.ToString() ?? "0");

                // Clientes
                using (var cmd = new Npgsql.NpgsqlCommand("SELECT COUNT(*) FROM cliente", conn))
                    ActualizarCard(cardClientes, cmd.ExecuteScalar()?.ToString() ?? "0");

                // Stock bajo (menos de 5 unidades)
                using (var cmd = new Npgsql.NpgsqlCommand("SELECT COUNT(*) FROM producto WHERE stock < 5", conn))
                    ActualizarCard(cardStock, cmd.ExecuteScalar()?.ToString() ?? "0");

                db.Cerrar();
            }
            catch { }
        }

        void ActualizarCard(Panel card, string valor)
        {
            foreach (Control c in card.Controls)
                if (c is Label lbl && lbl.Font.Size > 14)
                    lbl.Text = valor;
        }

        void CargarUltimasVentas()
        {
            try
            {
                var db = new ConexionBD();
                var conn = db.Abrir();
                if (conn == null) return;

                string sql = @"
                    SELECT v.id_venta AS ""#"",
                           c.nombre || ' ' || c.apellido AS ""Cliente"",
                           v.fecha_venta::DATE AS ""Fecha"",
                           v.metodo_pago AS ""Método"",
                           v.estado AS ""Estado"",
                           v.total AS ""Total (Bs.)""
                    FROM venta v
                    JOIN cliente c ON c.id_cliente = v.id_cliente
                    ORDER BY v.fecha_venta DESC
                    LIMIT 20";

                var da = new Npgsql.NpgsqlDataAdapter(sql, conn);
                var dt = new System.Data.DataTable();
                da.Fill(dt);
                dgvVentas.DataSource = dt;

                // Color filas por estado
                foreach (DataGridViewRow row in dgvVentas.Rows)
                {
                    string estado = row.Cells["Estado"].Value?.ToString() ?? "";
                    row.DefaultCellStyle.ForeColor = estado switch
                    {
                        "Pagada" => C_SUCCESS,
                        "Anulada" => C_DANGER,
                        _ => C_WARNING
                    };
                }
                db.Cerrar();
            }
            catch { }
        }

        void AbrirFormulario(Form frm)
        {
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            // Refrescar al volver
            CargarEstadisticas();
            CargarUltimasVentas();
        }
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

        private void FrmMenuPrincipal_Load(object sender, EventArgs e)
        {

        }

        private void btnProductos_Click_1(object sender, EventArgs e)
        {

        }
    }
}