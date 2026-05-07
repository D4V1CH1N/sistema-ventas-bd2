using System;
using System.Drawing;
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

        Panel pnlSidebar, pnlTop, pnlContent;
        Label lblTitle, lblUser, lblConexion;
        Button btnMenuVentas, btnMenuClientes, btnMenuProductos, btnMenuEmpleados;
        Panel cardVentas, cardIngresos, cardClientes, cardStock;
        DataGridView dgvVentas;

        public FrmMenuPrincipal()
        {
            InitializeComponent();
            ConstruirUI();
            CargarEstadisticas();
            CargarUltimasVentas();
        }

        void ConstruirUI()
        {
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new Size(1100, 700); // Forzamos el tamaño exacto interno
            this.Text = "SistemaVentas — Panel Principal";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = C_BG_DARK;
            this.ForeColor = C_TEXT_PRI;
            this.Font = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // =========================================================
            // EL TRUCO DEFINITIVO: POSICIONES ABSOLUTAS SIN "DOCK"
            // =========================================================

            // 1. SIDEBAR: Pegado a la izquierda, desde arriba hasta abajo
            pnlSidebar = new Panel { Location = new Point(0, 0), Size = new Size(220, 700), BackColor = C_BG_PANEL, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left };
            this.Controls.Add(pnlSidebar);

            // 2. TOPBAR: Empieza justo donde termina el sidebar (X=220)
            pnlTop = new Panel { Location = new Point(220, 0), Size = new Size(880, 70), BackColor = C_BG_PANEL, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            this.Controls.Add(pnlTop);

            // 3. CONTENIDO: Empieza abajo del Topbar (Y=70) y a la derecha del Sidebar (X=220)
            // Es imposible que se esconda debajo de nada ahora.
            pnlContent = new Panel { Location = new Point(220, 70), Size = new Size(880, 630), BackColor = C_BG_DARK, Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right };
            this.Controls.Add(pnlContent);

            // =========================================================

            // ELEMENTOS DEL SIDEBAR
            var pnlLogo = new Panel { Location = new Point(0, 0), Size = new Size(220, 70), BackColor = C_ACCENT };
            var lblLogo = new Label { Text = "◈  SistemaVentas", ForeColor = Color.White, Font = new Font("Segoe UI", 12f, FontStyle.Bold), AutoSize = true, Location = new Point(16, 24) };
            pnlLogo.Controls.Add(lblLogo);
            pnlSidebar.Controls.Add(pnlLogo);

            btnMenuVentas = CrearBotonSidebar("  +   Registrar Venta", 80);
            btnMenuClientes = CrearBotonSidebar("  ◎  Clientes", 130);
            btnMenuProductos = CrearBotonSidebar("  ◈  Productos", 180);
            btnMenuEmpleados = CrearBotonSidebar("  ◉  Empleados", 230);

            btnMenuVentas.Click += (s, e) => AbrirFormulario(new FrmRegistrarVenta());
            btnMenuClientes.Click += (s, e) => AbrirFormulario(new FrmClientes());
            btnMenuProductos.Click += (s, e) => AbrirFormulario(new FrmProductos());

            lblConexion = new Label { Text = "● BD Conectada", ForeColor = C_SUCCESS, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), AutoSize = true, Location = new Point(16, 650) };
            pnlSidebar.Controls.Add(lblConexion);

            // ELEMENTOS DEL TOPBAR
            lblTitle = new Label { Text = "Dashboard", ForeColor = C_TEXT_PRI, Font = new Font("Segoe UI", 14f, FontStyle.Bold), AutoSize = true, Location = new Point(24, 20) };
            pnlTop.Controls.Add(lblTitle);

            lblUser = new Label { Text = "Genesis Flores  |  Vendedora", ForeColor = C_TEXT_SEC, Font = new Font("Segoe UI", 9.5f), AutoSize = true, Location = new Point(620, 26), Anchor = AnchorStyles.Top | AnchorStyles.Right };
            pnlTop.Controls.Add(lblUser);

            // ELEMENTOS DEL CONTENIDO (El tamaño disponible es 880 de ancho x 630 de alto)
            cardVentas = CrearCard("Ventas hoy", "0", C_ACCENT, 20, 24);
            cardIngresos = CrearCard("Ingresos", "Bs. 0", C_WARNING, 235, 24);
            cardClientes = CrearCard("Clientes", "0", C_SUCCESS, 450, 24);
            cardStock = CrearCard("Stock bajo", "0", C_DANGER, 665, 24);

            var lblSec = new Label { Text = "Últimas ventas", ForeColor = C_TEXT_PRI, Font = new Font("Segoe UI", 12f, FontStyle.Bold), AutoSize = true, Location = new Point(20, 145) };
            pnlContent.Controls.Add(lblSec);

            var btnNuevaVenta = CrearBotonAccion("+ Nueva venta", 710, 135);
            btnNuevaVenta.Click += (s, e) => AbrirFormulario(new FrmRegistrarVenta());
            pnlContent.Controls.Add(btnNuevaVenta);

            dgvVentas = new DataGridView
            {
                Location = new Point(20, 185),
                Size = new Size(840, 420), // Exacto hasta el borde del botón
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
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
                Font = new Font("Segoe UI", 9.5f),
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            dgvVentas.DefaultCellStyle.BackColor = C_BG_CARD;
            dgvVentas.DefaultCellStyle.ForeColor = C_TEXT_PRI;
            dgvVentas.DefaultCellStyle.SelectionBackColor = C_ACCENT;
            dgvVentas.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvVentas.DefaultCellStyle.Padding = new Padding(6);
            dgvVentas.ColumnHeadersDefaultCellStyle.BackColor = C_BG_PANEL;
            dgvVentas.ColumnHeadersDefaultCellStyle.ForeColor = C_TEXT_SEC;
            dgvVentas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            dgvVentas.ColumnHeadersHeight = 40;
            dgvVentas.RowTemplate.Height = 40;
            dgvVentas.EnableHeadersVisualStyles = false;
            pnlContent.Controls.Add(dgvVentas);
        }

        Button CrearBotonSidebar(string texto, int top)
        {
            var btn = new Button
            {
                Text = texto,
                Location = new Point(0, top),
                Size = new Size(220, 50),
                FlatStyle = FlatStyle.Flat,
                ForeColor = C_TEXT_SEC,
                BackColor = C_BG_PANEL,
                Font = new Font("Segoe UI", 10f),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(40, 79, 70, 229);
            btn.MouseEnter += (s, e) => btn.ForeColor = Color.White;
            btn.MouseLeave += (s, e) => btn.ForeColor = C_TEXT_SEC;
            pnlSidebar.Controls.Add(btn);
            return btn;
        }

        Panel CrearCard(string titulo, string valor, Color acento, int x, int y)
        {
            var card = new Panel { Location = new Point(x, y), Size = new Size(195, 95), BackColor = C_BG_CARD };
            var top = new Panel { Location = new Point(0, 0), Size = new Size(195, 4), BackColor = acento };
            var lbl = new Label { Text = titulo, ForeColor = C_TEXT_SEC, Font = new Font("Segoe UI", 9.5f), AutoSize = true, Location = new Point(14, 16) };
            var val = new Label { Text = valor, ForeColor = Color.White, Font = new Font("Segoe UI", 18f, FontStyle.Bold), AutoSize = true, Location = new Point(12, 42) };
            card.Controls.AddRange(new Control[] { top, lbl, val });
            pnlContent.Controls.Add(card);
            return card;
        }

        Button CrearBotonAccion(string texto, int x, int y)
        {
            var btn = new Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(150, 36),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = C_ACCENT,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
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

                using (var cmd = new Npgsql.NpgsqlCommand("SELECT COUNT(*) FROM venta WHERE DATE(fecha_venta) = CURRENT_DATE", conn))
                    ActualizarCard(cardVentas, cmd.ExecuteScalar()?.ToString() ?? "0");

                using (var cmd = new Npgsql.NpgsqlCommand("SELECT COALESCE(SUM(total),0) FROM venta WHERE DATE(fecha_venta) = CURRENT_DATE AND estado='Pagada'", conn))
                    ActualizarCard(cardIngresos, "Bs. " + (cmd.ExecuteScalar()?.ToString() ?? "0"));

                using (var cmd = new Npgsql.NpgsqlCommand("SELECT COUNT(*) FROM cliente", conn))
                    ActualizarCard(cardClientes, cmd.ExecuteScalar()?.ToString() ?? "0");

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

                string sql = @"SELECT v.id_venta AS ""#"",
                    c.nombre || ' ' || c.apellido AS ""Cliente"",
                    v.fecha_venta::DATE AS ""Fecha"",
                    v.metodo_pago AS ""Método"",
                    v.estado AS ""Estado"",
                    v.total AS ""Total (Bs.)""
                    FROM venta v
                    JOIN cliente c ON c.id_cliente = v.id_cliente
                    ORDER BY v.fecha_venta DESC LIMIT 20";

                var da = new Npgsql.NpgsqlDataAdapter(sql, conn);
                var dt = new System.Data.DataTable();
                da.Fill(dt);
                dgvVentas.DataSource = dt;

                foreach (DataGridViewRow row in dgvVentas.Rows)
                {
                    string estado = row.Cells["Estado"].Value?.ToString() ?? "";
                    row.DefaultCellStyle.ForeColor = estado == "Pagada" ? C_SUCCESS :
                                                     estado == "Anulada" ? C_DANGER : C_WARNING;
                }
                db.Cerrar();
            }
            catch { }
        }

        void AbrirFormulario(Form frm)
        {
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            CargarEstadisticas();
            CargarUltimasVentas();
        }
    }
}