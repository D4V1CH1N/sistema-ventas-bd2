using System;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace SistemaVentas
{
    public partial class FrmClientes : Form
    {
        static readonly Color C_BG_DARK = Color.FromArgb(15, 15, 30);
        static readonly Color C_BG_PANEL = Color.FromArgb(22, 33, 62);
        static readonly Color C_BG_CARD = Color.FromArgb(26, 40, 75);
        static readonly Color C_ACCENT = Color.FromArgb(79, 70, 229);
        static readonly Color C_ACCENT2 = Color.FromArgb(99, 90, 255);
        static readonly Color C_TEXT_PRI = Color.FromArgb(230, 230, 250);
        static readonly Color C_TEXT_SEC = Color.FromArgb(140, 146, 176);
        static readonly Color C_DANGER = Color.FromArgb(220, 38, 38);
        static readonly Color C_BORDER = Color.FromArgb(50, 60, 100);

        DataGridView dgvClientes;
        TextBox txtNombre, txtApellido, txtDni, txtTelefono, txtEmail, txtDireccion, txtBuscar;
        Button btnGuardar, btnNuevo, btnEliminar;
        Label lblModo;
        int idSeleccionado = -1;

        public FrmClientes()
        {
            InitializeComponent();
            ConstruirUI();
            CargarClientes();
        }

        void ConstruirUI()
        {
            this.Text = "Gestión de Clientes";
            this.Size = new Size(1000, 640);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = C_BG_DARK;
            this.ForeColor = C_TEXT_PRI;
            this.Font = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // HEADER
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = C_ACCENT };
            var lblH = new Label { Text = "  ◎  Gestión de Clientes", ForeColor = Color.White, Font = new Font("Segoe UI", 13f, FontStyle.Bold), Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft, Padding = new Padding(16, 0, 0, 0) };
            pnlHeader.Controls.Add(lblH);
            this.Controls.Add(pnlHeader);

            // PANEL FORMULARIO
            var pnlForm = new Panel { Location = new Point(16, 72), Size = new Size(300, 530), BackColor = C_BG_CARD };
            this.Controls.Add(pnlForm);

            lblModo = new Label { Text = "Nuevo cliente", ForeColor = C_TEXT_SEC, Font = new Font("Segoe UI", 9f, FontStyle.Bold), AutoSize = true, Location = new Point(14, 14) };
            pnlForm.Controls.Add(lblModo);

            int y = 40;
            txtNombre = AgregarCampo(pnlForm, "Nombre *", y); y += 64;
            txtApellido = AgregarCampo(pnlForm, "Apellido *", y); y += 64;
            txtDni = AgregarCampo(pnlForm, "DNI *", y); y += 64;
            txtTelefono = AgregarCampo(pnlForm, "Teléfono", y); y += 64;
            txtEmail = AgregarCampo(pnlForm, "Email", y); y += 64;
            txtDireccion = AgregarCampo(pnlForm, "Dirección", y, 80); y += 102;

            btnGuardar = CrearBtn("Guardar", C_ACCENT, C_ACCENT2, 14, y);
            btnGuardar.Click += BtnGuardar_Click;
            pnlForm.Controls.Add(btnGuardar);

            btnNuevo = CrearBtn("Limpiar", C_BG_PANEL, C_BG_PANEL, 154, y);
            btnNuevo.FlatAppearance.BorderColor = C_BORDER;
            btnNuevo.FlatAppearance.BorderSize = 1;
            btnNuevo.Click += (s, e) => LimpiarFormulario();
            pnlForm.Controls.Add(btnNuevo);

            // PANEL TABLA
            var pnlTabla = new Panel { Location = new Point(328, 72), Size = new Size(648, 530), BackColor = C_BG_CARD };
            this.Controls.Add(pnlTabla);

            var lblBus = new Label { Text = "Buscar:", ForeColor = C_TEXT_SEC, AutoSize = true, Location = new Point(14, 16) };
            pnlTabla.Controls.Add(lblBus);

            txtBuscar = new TextBox { Location = new Point(70, 12), Size = new Size(200, 28), BackColor = C_BG_PANEL, ForeColor = C_TEXT_PRI, BorderStyle = BorderStyle.FixedSingle };
            txtBuscar.TextChanged += (s, e) => CargarClientes(txtBuscar.Text);
            pnlTabla.Controls.Add(txtBuscar);

            btnEliminar = CrearBtn("Eliminar", C_DANGER, Color.FromArgb(180, 30, 30), 490, 10);
            btnEliminar.Enabled = false;
            btnEliminar.Click += BtnEliminar_Click;
            pnlTabla.Controls.Add(btnEliminar);

            dgvClientes = new DataGridView
            {
                Location = new Point(0, 48),
                Size = new Size(648, 482),
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
            dgvClientes.DefaultCellStyle.BackColor = C_BG_CARD;
            dgvClientes.DefaultCellStyle.ForeColor = C_TEXT_PRI;
            dgvClientes.DefaultCellStyle.SelectionBackColor = C_ACCENT;
            dgvClientes.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvClientes.DefaultCellStyle.Padding = new Padding(4);
            dgvClientes.ColumnHeadersDefaultCellStyle.BackColor = C_BG_PANEL;
            dgvClientes.ColumnHeadersDefaultCellStyle.ForeColor = C_TEXT_SEC;
            dgvClientes.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            dgvClientes.ColumnHeadersHeight = 36;
            dgvClientes.RowTemplate.Height = 36;
            dgvClientes.EnableHeadersVisualStyles = false;
            dgvClientes.SelectionChanged += DgvClientes_SelectionChanged;
            pnlTabla.Controls.Add(dgvClientes);
        }

        TextBox AgregarCampo(Panel parent, string label, int y, int alto = 28)
        {
            var lbl = new Label { Text = label, ForeColor = C_TEXT_SEC, Font = new Font("Segoe UI", 8f, FontStyle.Bold), AutoSize = true, Location = new Point(14, y) };
            var txt = new TextBox { Location = new Point(14, y + 18), Size = new Size(270, alto), BackColor = C_BG_PANEL, ForeColor = C_TEXT_PRI, BorderStyle = BorderStyle.FixedSingle };
            if (alto > 28) { txt.Multiline = true; txt.ScrollBars = ScrollBars.Vertical; }
            parent.Controls.AddRange(new Control[] { lbl, txt });
            return txt;
        }

        Button CrearBtn(string texto, Color bg, Color hover, int x, int y)
        {
            var btn = new Button { Text = texto, Location = new Point(x, y), Size = new Size(130, 36), FlatStyle = FlatStyle.Flat, ForeColor = Color.White, BackColor = bg, Font = new Font("Segoe UI", 9f, FontStyle.Bold), Cursor = Cursors.Hand };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = hover;
            return btn;
        }

        void CargarClientes(string filtro = "")
        {
            try
            {
                var db = new ConexionBD();
                var conn = db.Abrir();
                if (conn == null) return;

                string sql = @"SELECT id_cliente AS ""ID"", nombre AS ""Nombre"", apellido AS ""Apellido"",
                    dni AS ""DNI"", telefono AS ""Teléfono"", email AS ""Email"", fecha_registro AS ""Registro""
                    FROM cliente WHERE LOWER(nombre || ' ' || apellido || ' ' || dni) LIKE @f ORDER BY nombre";

                var da = new NpgsqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@f", "%" + filtro.ToLower() + "%");
                var dt = new System.Data.DataTable();
                da.Fill(dt);
                dgvClientes.DataSource = dt;
                db.Cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar clientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void DgvClientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClientes.CurrentRow == null) return;
            var row = dgvClientes.CurrentRow;
            idSeleccionado = Convert.ToInt32(row.Cells["ID"].Value);
            txtNombre.Text = row.Cells["Nombre"].Value?.ToString();
            txtApellido.Text = row.Cells["Apellido"].Value?.ToString();
            txtDni.Text = row.Cells["DNI"].Value?.ToString();
            txtTelefono.Text = row.Cells["Teléfono"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();
            lblModo.Text = "Editar cliente";
            btnEliminar.Enabled = true;
        }

        void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text) || string.IsNullOrWhiteSpace(txtDni.Text))
            {
                MessageBox.Show("Nombre, Apellido y DNI son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var db = new ConexionBD();
                var conn = db.Abrir();
                if (idSeleccionado == -1)
                {
                    using var cmd = new NpgsqlCommand("INSERT INTO cliente (nombre,apellido,dni,telefono,email,direccion) VALUES (@n,@a,@d,@t,@e,@dir)", conn);
                    AgregarParams(cmd); cmd.ExecuteNonQuery();
                    MessageBox.Show("Cliente registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    using var cmd = new NpgsqlCommand("UPDATE cliente SET nombre=@n,apellido=@a,dni=@d,telefono=@t,email=@e,direccion=@dir WHERE id_cliente=@id", conn);
                    AgregarParams(cmd);
                    cmd.Parameters.AddWithValue("@id", idSeleccionado);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cliente actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                db.Cerrar(); LimpiarFormulario(); CargarClientes();
            }
            catch (Exception ex) { MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        void AgregarParams(NpgsqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@n", txtNombre.Text.Trim());
            cmd.Parameters.AddWithValue("@a", txtApellido.Text.Trim());
            cmd.Parameters.AddWithValue("@d", txtDni.Text.Trim());
            cmd.Parameters.AddWithValue("@t", txtTelefono.Text.Trim());
            cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@dir", txtDireccion.Text.Trim());
        }

        void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == -1) return;
            if (MessageBox.Show("¿Eliminar este cliente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                var db = new ConexionBD(); var conn = db.Abrir();
                using var cmd = new NpgsqlCommand("DELETE FROM cliente WHERE id_cliente=@id", conn);
                cmd.Parameters.AddWithValue("@id", idSeleccionado);
                cmd.ExecuteNonQuery(); db.Cerrar();
                MessageBox.Show("Cliente eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario(); CargarClientes();
            }
            catch (Exception ex) { MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        void LimpiarFormulario()
        {
            idSeleccionado = -1;
            txtNombre.Text = txtApellido.Text = txtDni.Text = txtTelefono.Text = txtEmail.Text = txtDireccion.Text = "";
            lblModo.Text = "Nuevo cliente"; btnEliminar.Enabled = false; dgvClientes.ClearSelection();
        }
    }
}