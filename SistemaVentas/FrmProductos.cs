using System;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace SistemaVentas
{
    public partial class FrmProductos : Form
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

        DataGridView dgvProductos;
        TextBox txtNombre, txtDescripcion, txtPrecio, txtBuscar;
        NumericUpDown nudStock;
        ComboBox cmbCategoria;
        Button btnGuardar, btnNuevo, btnEliminar;
        Label lblModo, lblStockInfo;
        int idSeleccionado = -1;

        public FrmProductos()
        {
            InitializeComponent();
            ConstruirUI();
            CargarCategorias();
            CargarProductos();
        }

        void ConstruirUI()
        {
            this.Text = "Gestión de Productos";
            this.Size = new Size(1050, 660);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = C_BG_DARK;
            this.ForeColor = C_TEXT_PRI;
            this.Font = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // HEADER
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = C_ACCENT };
            var lblH = new Label { Text = "  ◈  Gestión de Productos / Inventario", ForeColor = Color.White, Font = new Font("Segoe UI", 13f, FontStyle.Bold), Dock = DockStyle.Fill, TextAlign = System.Drawing.ContentAlignment.MiddleLeft, Padding = new Padding(16, 0, 0, 0) };
            pnlHeader.Controls.Add(lblH);
            this.Controls.Add(pnlHeader);

            // PANEL FORMULARIO
            var pnlForm = new Panel { Location = new Point(16, 72), Size = new Size(300, 550), BackColor = C_BG_CARD };
            this.Controls.Add(pnlForm);

            lblModo = new Label { Text = "Nuevo producto", ForeColor = C_TEXT_SEC, Font = new Font("Segoe UI", 9f, FontStyle.Bold), AutoSize = true, Location = new Point(14, 14) };
            pnlForm.Controls.Add(lblModo);

            int y = 40;
            txtNombre = AgregarCampo(pnlForm, "Nombre *", y); y += 64;

            var lblCat = new Label { Text = "Categoría *", ForeColor = C_TEXT_SEC, Font = new Font("Segoe UI", 8f, FontStyle.Bold), AutoSize = true, Location = new Point(14, y) };
            cmbCategoria = new ComboBox { Location = new Point(14, y + 18), Size = new Size(270, 28), DropDownStyle = ComboBoxStyle.DropDownList, BackColor = C_BG_PANEL, ForeColor = C_TEXT_PRI, FlatStyle = FlatStyle.Flat };
            pnlForm.Controls.AddRange(new Control[] { lblCat, cmbCategoria }); y += 64;

            txtDescripcion = AgregarCampo(pnlForm, "Descripción", y, 70); y += 96;

            var lblPrecio = new Label { Text = "Precio (Bs.) *", ForeColor = C_TEXT_SEC, Font = new Font("Segoe UI", 8f, FontStyle.Bold), AutoSize = true, Location = new Point(14, y) };
            txtPrecio = new TextBox { Location = new Point(14, y + 18), Size = new Size(130, 28), BackColor = C_BG_PANEL, ForeColor = C_TEXT_PRI, BorderStyle = BorderStyle.FixedSingle };
            pnlForm.Controls.AddRange(new Control[] { lblPrecio, txtPrecio });

            var lblStock = new Label { Text = "Stock inicial", ForeColor = C_TEXT_SEC, Font = new Font("Segoe UI", 8f, FontStyle.Bold), AutoSize = true, Location = new Point(154, y) };
            nudStock = new NumericUpDown { Location = new Point(154, y + 18), Size = new Size(130, 28), Minimum = 0, Maximum = 99999, BackColor = C_BG_PANEL, ForeColor = C_TEXT_PRI, BorderStyle = BorderStyle.FixedSingle };
            pnlForm.Controls.AddRange(new Control[] { lblStock, nudStock }); y += 64;

            lblStockInfo = new Label { Text = "", ForeColor = C_DANGER, Font = new Font("Segoe UI", 8f, FontStyle.Bold), AutoSize = true, Location = new Point(14, y) }; y += 24;
            pnlForm.Controls.Add(lblStockInfo);

            btnGuardar = CrearBtn("Guardar", C_ACCENT, C_ACCENT2, 14, y + 20);
            btnGuardar.Click += BtnGuardar_Click;
            pnlForm.Controls.Add(btnGuardar);

            btnNuevo = CrearBtn("Limpiar", C_BG_PANEL, C_BG_PANEL, 154, y + 20);
            btnNuevo.FlatAppearance.BorderColor = C_BORDER;
            btnNuevo.FlatAppearance.BorderSize = 1;
            btnNuevo.Click += (s, e) => LimpiarFormulario();
            pnlForm.Controls.Add(btnNuevo);

            // PANEL TABLA
            var pnlTabla = new Panel { Location = new Point(328, 72), Size = new Size(700, 550), BackColor = C_BG_CARD };
            this.Controls.Add(pnlTabla);

            var lblBus = new Label { Text = "Buscar:", ForeColor = C_TEXT_SEC, AutoSize = true, Location = new Point(14, 16) };
            pnlTabla.Controls.Add(lblBus);

            txtBuscar = new TextBox { Location = new Point(70, 12), Size = new Size(200, 28), BackColor = C_BG_PANEL, ForeColor = C_TEXT_PRI, BorderStyle = BorderStyle.FixedSingle };
            txtBuscar.TextChanged += (s, e) => CargarProductos(txtBuscar.Text);
            pnlTabla.Controls.Add(txtBuscar);

            btnEliminar = CrearBtn("Eliminar", C_DANGER, Color.FromArgb(180, 30, 30), 546, 10);
            btnEliminar.Enabled = false;
            btnEliminar.Click += BtnEliminar_Click;
            pnlTabla.Controls.Add(btnEliminar);

            dgvProductos = new DataGridView
            {
                Location = new Point(0, 48),
                Size = new Size(700, 502),
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
            dgvProductos.DefaultCellStyle.BackColor = C_BG_CARD;
            dgvProductos.DefaultCellStyle.ForeColor = C_TEXT_PRI;
            dgvProductos.DefaultCellStyle.SelectionBackColor = C_ACCENT;
            dgvProductos.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvProductos.DefaultCellStyle.Padding = new Padding(4);
            dgvProductos.ColumnHeadersDefaultCellStyle.BackColor = C_BG_PANEL;
            dgvProductos.ColumnHeadersDefaultCellStyle.ForeColor = C_TEXT_SEC;
            dgvProductos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            dgvProductos.ColumnHeadersHeight = 36;
            dgvProductos.RowTemplate.Height = 36;
            dgvProductos.EnableHeadersVisualStyles = false;
            dgvProductos.SelectionChanged += DgvProductos_SelectionChanged;
            dgvProductos.CellFormatting += DgvProductos_CellFormatting;
            pnlTabla.Controls.Add(dgvProductos);
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

        void CargarCategorias()
        {
            try
            {
                var db = new ConexionBD(); var conn = db.Abrir();
                if (conn == null) return;
                using var cmd = new NpgsqlCommand("SELECT id_categoria, nombre FROM categoria ORDER BY nombre", conn);
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    cmbCategoria.Items.Add(new ComboItem(rdr.GetInt32(0), rdr.GetString(1)));
                if (cmbCategoria.Items.Count > 0) cmbCategoria.SelectedIndex = 0;
                db.Cerrar();
            }
            catch { }
        }

        void CargarProductos(string filtro = "")
        {
            try
            {
                var db = new ConexionBD(); var conn = db.Abrir();
                if (conn == null) return;

                string sql = @"SELECT p.id_producto AS ""ID"", p.nombre AS ""Nombre"",
                    c.nombre AS ""Categoría"", p.precio AS ""Precio (Bs.)"", p.stock AS ""Stock""
                    FROM producto p JOIN categoria c ON c.id_categoria = p.id_categoria
                    WHERE LOWER(p.nombre) LIKE @f ORDER BY p.nombre";

                var da = new NpgsqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@f", "%" + filtro.ToLower() + "%");
                var dt = new System.Data.DataTable();
                da.Fill(dt);
                dgvProductos.DataSource = dt;
                db.Cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void DgvProductos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvProductos.Rows[e.RowIndex];
            if (row.Cells["Stock"].Value != null && Convert.ToInt32(row.Cells["Stock"].Value) < 5)
                row.DefaultCellStyle.ForeColor = C_DANGER;
        }

        void DgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow == null) return;
            var row = dgvProductos.CurrentRow;
            idSeleccionado = Convert.ToInt32(row.Cells["ID"].Value);
            txtNombre.Text = row.Cells["Nombre"].Value?.ToString();
            txtPrecio.Text = row.Cells["Precio (Bs.)"].Value?.ToString();
            nudStock.Value = Convert.ToDecimal(row.Cells["Stock"].Value);
            lblModo.Text = "Editar producto";
            btnEliminar.Enabled = true;
            lblStockInfo.Text = Convert.ToInt32(row.Cells["Stock"].Value) < 5 ? "⚠ Stock bajo — reponer pronto" : "";
            string cat = row.Cells["Categoría"].Value?.ToString();
            foreach (ComboItem item in cmbCategoria.Items)
                if (item.Nombre == cat) { cmbCategoria.SelectedItem = item; break; }
        }

        void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text) || cmbCategoria.SelectedItem == null)
            {
                MessageBox.Show("Nombre, Categoría y Precio son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
            {
                MessageBox.Show("El precio debe ser un número mayor a 0.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                var db = new ConexionBD(); var conn = db.Abrir();
                var cat = (ComboItem)cmbCategoria.SelectedItem;
                if (idSeleccionado == -1)
                {
                    using var cmd = new NpgsqlCommand("INSERT INTO producto (id_categoria,nombre,descripcion,precio,stock) VALUES (@ic,@n,@d,@p,@s)", conn);
                    AgregarParams(cmd, cat.Id, precio); cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    using var cmd = new NpgsqlCommand("UPDATE producto SET id_categoria=@ic,nombre=@n,descripcion=@d,precio=@p,stock=@s WHERE id_producto=@id", conn);
                    AgregarParams(cmd, cat.Id, precio);
                    cmd.Parameters.AddWithValue("@id", idSeleccionado);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                db.Cerrar(); LimpiarFormulario(); CargarProductos();
            }
            catch (Exception ex) { MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        void AgregarParams(NpgsqlCommand cmd, int catId, decimal precio)
        {
            cmd.Parameters.AddWithValue("@ic", catId);
            cmd.Parameters.AddWithValue("@n", txtNombre.Text.Trim());
            cmd.Parameters.AddWithValue("@d", txtDescripcion.Text.Trim());
            cmd.Parameters.AddWithValue("@p", precio);
            cmd.Parameters.AddWithValue("@s", (int)nudStock.Value);
        }

        void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == -1) return;
            if (MessageBox.Show("¿Eliminar este producto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                var db = new ConexionBD(); var conn = db.Abrir();
                using var cmd = new NpgsqlCommand("DELETE FROM producto WHERE id_producto=@id", conn);
                cmd.Parameters.AddWithValue("@id", idSeleccionado);
                cmd.ExecuteNonQuery(); db.Cerrar();
                MessageBox.Show("Producto eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarFormulario(); CargarProductos();
            }
            catch (Exception ex) { MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        void LimpiarFormulario()
        {
            idSeleccionado = -1;
            txtNombre.Text = txtDescripcion.Text = txtPrecio.Text = "";
            nudStock.Value = 0; lblModo.Text = "Nuevo producto"; lblStockInfo.Text = "";
            btnEliminar.Enabled = false; dgvProductos.ClearSelection();
            if (cmbCategoria.Items.Count > 0) cmbCategoria.SelectedIndex = 0;
        }
        public class ComboItem
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public ComboItem(int id, string nombre) { Id = id; Nombre = nombre; }
            public override string ToString() { return Nombre; }
        }
    }
}