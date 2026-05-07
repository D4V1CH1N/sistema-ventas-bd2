using System;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace SistemaVentas
{
    public partial class FrmEmpleados : Form
    {
        static readonly Color C_BG_DARK  = Color.FromArgb(15,  15,  30);
        static readonly Color C_BG_PANEL = Color.FromArgb(22,  33,  62);
        static readonly Color C_BG_CARD  = Color.FromArgb(26,  40,  75);
        static readonly Color C_ACCENT   = Color.FromArgb(79,  70, 229);
        static readonly Color C_ACCENT2  = Color.FromArgb(99,  90, 255);
        static readonly Color C_TEXT_PRI = Color.FromArgb(230, 230, 250);
        static readonly Color C_TEXT_SEC = Color.FromArgb(140, 146, 176);
        static readonly Color C_DANGER   = Color.FromArgb(220,  38,  38);
        static readonly Color C_BORDER   = Color.FromArgb(50,   60, 100);

        DataGridView dgvEmpleados;
        TextBox      txtNombre, txtApellido, txtTelefono, txtBuscar;
        ComboBox     cmbCargo;
        Button       btnGuardar, btnNuevo, btnEliminar;
        Label        lblModo;
        int          idSeleccionado = -1;

        public FrmEmpleados()
        {
            InitializeComponent();
            ConstruirUI();
            CargarEmpleados();
        }

        void ConstruirUI()
        {
            this.Text            = "Gestión de Empleados";
            this.Size            = new Size(1000, 600);
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.BackColor       = C_BG_DARK;
            this.ForeColor       = C_TEXT_PRI;
            this.Font            = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox     = false;

            // ── HEADER ───────────────────────────────────────────────────
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = C_ACCENT };
            var lblH = new Label
            {
                Text      = "  ◉  Gestión de Empleados",
                ForeColor = Color.White,
                Font      = new Font("Segoe UI", 13f, FontStyle.Bold),
                Dock      = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                Padding   = new Padding(16, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblH);
            this.Controls.Add(pnlHeader);

            // ── PANEL FORMULARIO ─────────────────────────────────────────
            var pnlForm = new Panel { Location = new Point(16, 72), Size = new Size(300, 480), BackColor = C_BG_CARD };
            this.Controls.Add(pnlForm);

            lblModo = new Label
            {
                Text     = "Nuevo empleado",
                ForeColor = C_TEXT_SEC,
                Font     = new Font("Segoe UI", 9f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(14, 14)
            };
            pnlForm.Controls.Add(lblModo);

            // Avatar decorativo
            var pnlAvatar = new Panel { Location = new Point(110, 40), Size = new Size(80, 80), BackColor = Color.FromArgb(50, 79, 70, 229) };
            var lblAvatar = new Label
            {
                Text      = "◉",
                ForeColor = C_ACCENT,
                Font      = new Font("Segoe UI", 28f),
                AutoSize  = false,
                Size      = new Size(80, 80),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            pnlAvatar.Controls.Add(lblAvatar);
            pnlForm.Controls.Add(pnlAvatar);

            int y = 140;
            txtNombre   = AgregarCampo(pnlForm, "Nombre *",   y); y += 64;
            txtApellido = AgregarCampo(pnlForm, "Apellido *", y); y += 64;

            // Cargo (combo)
            var lblCargo = new Label
            {
                Text     = "Cargo *",
                ForeColor = C_TEXT_SEC,
                Font     = new Font("Segoe UI", 8f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(14, y)
            };
            cmbCargo = new ComboBox
            {
                Location      = new Point(14, y + 18),
                Size          = new Size(270, 28),
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor     = C_BG_PANEL,
                ForeColor     = C_TEXT_PRI,
                FlatStyle     = FlatStyle.Flat
            };
            cmbCargo.Items.AddRange(new object[]
            {
                "Administrador", "Vendedor", "Vendedora",
                "Supervisor", "Almacenero", "Cajero", "Cajera"
            });
            cmbCargo.SelectedIndex = 0;
            pnlForm.Controls.AddRange(new Control[] { lblCargo, cmbCargo });
            y += 64;

            txtTelefono = AgregarCampo(pnlForm, "Teléfono", y); y += 64;

            // Botones
            btnGuardar = CrearBtn("Guardar", C_ACCENT, C_ACCENT2, 14, y + 10);
            btnGuardar.Click += BtnGuardar_Click;
            pnlForm.Controls.Add(btnGuardar);

            btnNuevo = CrearBtn("Limpiar", C_BG_PANEL, C_BG_PANEL, 154, y + 10);
            btnNuevo.FlatAppearance.BorderColor = C_BORDER;
            btnNuevo.FlatAppearance.BorderSize  = 1;
            btnNuevo.Click += (s, e) => LimpiarFormulario();
            pnlForm.Controls.Add(btnNuevo);

            // ── PANEL TABLA ──────────────────────────────────────────────
            var pnlTabla = new Panel { Location = new Point(328, 72), Size = new Size(648, 480), BackColor = C_BG_CARD };
            this.Controls.Add(pnlTabla);

            var lblBus = new Label { Text = "Buscar:", ForeColor = C_TEXT_SEC, AutoSize = true, Location = new Point(14, 16) };
            pnlTabla.Controls.Add(lblBus);

            txtBuscar = new TextBox
            {
                Location    = new Point(70, 12),
                Size        = new Size(200, 28),
                BackColor   = C_BG_PANEL,
                ForeColor   = C_TEXT_PRI,
                BorderStyle = BorderStyle.FixedSingle
            };
            txtBuscar.TextChanged += (s, e) => CargarEmpleados(txtBuscar.Text);
            pnlTabla.Controls.Add(txtBuscar);

            btnEliminar = CrearBtn("Eliminar", C_DANGER, Color.FromArgb(180, 30, 30), 490, 10);
            btnEliminar.Enabled = false;
            btnEliminar.Click  += BtnEliminar_Click;
            pnlTabla.Controls.Add(btnEliminar);

            dgvEmpleados = new DataGridView
            {
                Location              = new Point(0, 48),
                Size                  = new Size(648, 432),
                BackgroundColor       = C_BG_CARD,
                ForeColor             = C_TEXT_PRI,
                GridColor             = Color.FromArgb(40, 50, 80),
                BorderStyle           = BorderStyle.None,
                RowHeadersVisible     = false,
                AllowUserToAddRows    = false,
                AllowUserToDeleteRows = false,
                ReadOnly              = true,
                SelectionMode         = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode   = DataGridViewAutoSizeColumnsMode.Fill,
                Font                  = new Font("Segoe UI", 9f),
                CellBorderStyle       = DataGridViewCellBorderStyle.SingleHorizontal
            };
            dgvEmpleados.DefaultCellStyle.BackColor           = C_BG_CARD;
            dgvEmpleados.DefaultCellStyle.ForeColor           = C_TEXT_PRI;
            dgvEmpleados.DefaultCellStyle.SelectionBackColor  = C_ACCENT;
            dgvEmpleados.DefaultCellStyle.SelectionForeColor  = Color.White;
            dgvEmpleados.DefaultCellStyle.Padding             = new Padding(4);
            dgvEmpleados.ColumnHeadersDefaultCellStyle.BackColor = C_BG_PANEL;
            dgvEmpleados.ColumnHeadersDefaultCellStyle.ForeColor = C_TEXT_SEC;
            dgvEmpleados.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            dgvEmpleados.ColumnHeadersHeight      = 36;
            dgvEmpleados.RowTemplate.Height        = 38;
            dgvEmpleados.EnableHeadersVisualStyles = false;
            dgvEmpleados.SelectionChanged         += DgvEmpleados_SelectionChanged;
            pnlTabla.Controls.Add(dgvEmpleados);
        }

        // ── Helpers UI ───────────────────────────────────────────────────

        TextBox AgregarCampo(Panel parent, string label, int y, int alto = 28)
        {
            var lbl = new Label { Text = label, ForeColor = C_TEXT_SEC, Font = new Font("Segoe UI", 8f, FontStyle.Bold), AutoSize = true, Location = new Point(14, y) };
            var txt = new TextBox { Location = new Point(14, y + 18), Size = new Size(270, alto), BackColor = C_BG_PANEL, ForeColor = C_TEXT_PRI, BorderStyle = BorderStyle.FixedSingle };
            parent.Controls.AddRange(new Control[] { lbl, txt });
            return txt;
        }

        Button CrearBtn(string texto, Color bg, Color hover, int x, int y)
        {
            var btn = new Button
            {
                Text      = texto,
                Location  = new Point(x, y),
                Size      = new Size(130, 36),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = bg,
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = hover;
            return btn;
        }

        // ── Lógica ───────────────────────────────────────────────────────

        void CargarEmpleados(string filtro = "")
        {
            try
            {
                var db   = new ConexionBD();
                var conn = db.Abrir();
                if (conn == null) return;

                string sql = @"
                    SELECT id_empleado AS ""ID"",
                           nombre     AS ""Nombre"",
                           apellido   AS ""Apellido"",
                           cargo      AS ""Cargo"",
                           telefono   AS ""Teléfono""
                    FROM empleado
                    WHERE LOWER(nombre || ' ' || apellido || ' ' || cargo) LIKE @f
                    ORDER BY nombre";

                var da = new NpgsqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@f", "%" + filtro.ToLower() + "%");
                var dt = new System.Data.DataTable();
                da.Fill(dt);
                dgvEmpleados.DataSource = dt;
                db.Cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar empleados: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void DgvEmpleados_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEmpleados.CurrentRow == null) return;
            var row = dgvEmpleados.CurrentRow;
            idSeleccionado      = Convert.ToInt32(row.Cells["ID"].Value);
            txtNombre.Text      = row.Cells["Nombre"].Value?.ToString();
            txtApellido.Text    = row.Cells["Apellido"].Value?.ToString();
            txtTelefono.Text    = row.Cells["Teléfono"].Value?.ToString();
            lblModo.Text        = "Editar empleado";
            btnEliminar.Enabled = true;

            // Seleccionar cargo en combo
            string cargo = row.Cells["Cargo"].Value?.ToString();
            for (int i = 0; i < cmbCargo.Items.Count; i++)
                if (cmbCargo.Items[i].ToString() == cargo) { cmbCargo.SelectedIndex = i; break; }
        }

        void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("Nombre y Apellido son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var db   = new ConexionBD();
                var conn = db.Abrir();

                if (idSeleccionado == -1)
                {
                    using var cmd = new NpgsqlCommand(
                        "INSERT INTO empleado (nombre, apellido, cargo, telefono) VALUES (@n,@a,@c,@t)", conn);
                    AgregarParams(cmd);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Empleado registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    using var cmd = new NpgsqlCommand(
                        "UPDATE empleado SET nombre=@n, apellido=@a, cargo=@c, telefono=@t WHERE id_empleado=@id", conn);
                    AgregarParams(cmd);
                    cmd.Parameters.AddWithValue("@id", idSeleccionado);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Empleado actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                db.Cerrar();
                LimpiarFormulario();
                CargarEmpleados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void AgregarParams(NpgsqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@n", txtNombre.Text.Trim());
            cmd.Parameters.AddWithValue("@a", txtApellido.Text.Trim());
            cmd.Parameters.AddWithValue("@c", cmbCargo.SelectedItem?.ToString() ?? "");
            cmd.Parameters.AddWithValue("@t", txtTelefono.Text.Trim());
        }

        void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == -1) return;
            if (MessageBox.Show("¿Eliminar este empleado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                var db   = new ConexionBD();
                var conn = db.Abrir();
                using var cmd = new NpgsqlCommand("DELETE FROM empleado WHERE id_empleado=@id", conn);
                cmd.Parameters.AddWithValue("@id", idSeleccionado);
                cmd.ExecuteNonQuery();
                db.Cerrar();
                MessageBox.Show("Empleado eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario();
                CargarEmpleados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LimpiarFormulario()
        {
            idSeleccionado      = -1;
            txtNombre.Text      = txtApellido.Text = txtTelefono.Text = "";
            cmbCargo.SelectedIndex = 0;
            lblModo.Text        = "Nuevo empleado";
            btnEliminar.Enabled = false;
            dgvEmpleados.ClearSelection();
        }
    }
}
