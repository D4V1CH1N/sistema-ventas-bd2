using System;
using System.Drawing;
using System.Windows.Forms;
using Npgsql;

namespace SistemaVentas
{
    public partial class FrmRegistrarVenta : Form
    {
        static readonly Color C_BG_DARK = Color.FromArgb(15, 15, 30);
        static readonly Color C_BG_PANEL = Color.FromArgb(22, 33, 62);
        static readonly Color C_BG_CARD = Color.FromArgb(26, 40, 75);
        static readonly Color C_ACCENT = Color.FromArgb(79, 70, 229);
        static readonly Color C_ACCENT2 = Color.FromArgb(99, 90, 255);
        static readonly Color C_TEXT_PRI = Color.FromArgb(230, 230, 250);
        static readonly Color C_TEXT_SEC = Color.FromArgb(140, 146, 176);
        static readonly Color C_SUCCESS = Color.FromArgb(34, 197, 94);
        static readonly Color C_BORDER = Color.FromArgb(50, 60, 100);

        ComboBox cmbCliente, cmbEmpleado, cmbProducto, cmbMetodoPago, cmbEstado;
        NumericUpDown nudCantidad;
        TextBox txtPrecioUnitario, txtSubtotal;
        Button btnGuardar, btnCancelar;
        Panel pnlResumen;
        Label lblTotalFinal;

        public FrmRegistrarVenta()
        {
            InitializeComponent();
            ConstruirUI();
            CargarDatos();
        }

        void ConstruirUI()
        {
            // EL TRUCO PARA LAPTOPS
            this.AutoScaleMode = AutoScaleMode.None;
            this.ClientSize = new Size(680, 620); // Tamaño interno exacto

            this.Text = "Registrar Nueva Venta";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = C_BG_DARK;
            this.ForeColor = C_TEXT_PRI;
            this.Font = new Font("Segoe UI", 9f);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // HEADER (Barra superior morada de lado a lado)
            var pnlHeader = new Panel { Location = new Point(0, 0), Size = new Size(680, 60), BackColor = C_ACCENT };
            var lblHeader = new Label { Text = "+  Registrar Nueva Venta", ForeColor = Color.White, Font = new Font("Segoe UI", 13f, FontStyle.Bold), AutoSize = true, Location = new Point(20, 18) };
            pnlHeader.Controls.Add(lblHeader);
            this.Controls.Add(pnlHeader);

            // CARD PRINCIPAL (Caja oscura central)
            var pnlCard = new Panel { Location = new Point(20, 80), Size = new Size(640, 460), BackColor = C_BG_CARD };
            this.Controls.Add(pnlCard);

            int xL = 20, xR = 330, yS = 20, gap = 75;

            AgregarLabel(pnlCard, "Cliente *", xL, yS);
            cmbCliente = AgregarCombo(pnlCard, xL, yS + 25, 290);

            AgregarLabel(pnlCard, "Empleado *", xR, yS);
            cmbEmpleado = AgregarCombo(pnlCard, xR, yS + 25, 290);

            AgregarLabel(pnlCard, "Método de pago *", xL, yS + gap);
            cmbMetodoPago = AgregarCombo(pnlCard, xL, yS + gap + 25, 290);
            cmbMetodoPago.Items.AddRange(new object[] { "Efectivo", "Tarjeta debito", "Tarjeta credito", "Transferencia" });
            cmbMetodoPago.SelectedIndex = 0;

            AgregarLabel(pnlCard, "Estado", xR, yS + gap);
            cmbEstado = AgregarCombo(pnlCard, xR, yS + gap + 25, 290);
            cmbEstado.Items.AddRange(new object[] { "Pendiente", "Pagada", "Anulada" });
            cmbEstado.SelectedIndex = 0;

            // LÍNEA SEPARADORA
            var sep = new Label { Location = new Point(20, yS + gap * 2 + 5), Size = new Size(600, 1), BackColor = C_BORDER };
            pnlCard.Controls.Add(sep);

            var lblDet = new Label { Text = "Detalle del producto", ForeColor = C_TEXT_SEC, Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), AutoSize = true, Location = new Point(xL, yS + gap * 2 + 15) };
            pnlCard.Controls.Add(lblDet);

            int y2 = 230;

            AgregarLabel(pnlCard, "Producto *", xL, y2);
            cmbProducto = AgregarCombo(pnlCard, xL, y2 + 25, 600);
            cmbProducto.SelectedIndexChanged += CmbProducto_Changed;

            AgregarLabel(pnlCard, "Cantidad", xL, y2 + gap);
            nudCantidad = new NumericUpDown { Location = new Point(xL, y2 + gap + 25), Size = new Size(120, 30), Minimum = 1, Maximum = 9999, Value = 1, BackColor = C_BG_PANEL, ForeColor = C_TEXT_PRI, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10f) };
            nudCantidad.ValueChanged += (s, e) => ActualizarSubtotal();
            pnlCard.Controls.Add(nudCantidad);

            AgregarLabel(pnlCard, "Precio unitario (Bs.)", 160, y2 + gap);
            txtPrecioUnitario = AgregarTextBox(pnlCard, 160, y2 + gap + 25, 200);
            txtPrecioUnitario.ReadOnly = true;
            txtPrecioUnitario.BackColor = C_BG_PANEL;

            AgregarLabel(pnlCard, "Subtotal (Bs.)", 380, y2 + gap);
            txtSubtotal = AgregarTextBox(pnlCard, 380, y2 + gap + 25, 240);
            txtSubtotal.ReadOnly = true;
            txtSubtotal.BackColor = C_BG_PANEL;
            txtSubtotal.ForeColor = C_SUCCESS;
            txtSubtotal.Font = new Font("Segoe UI", 11f, FontStyle.Bold);

            // BOTONES (Ubicados dentro del Card)
            btnCancelar = new Button { Text = "Cancelar", Location = new Point(130, 400), Size = new Size(180, 40), FlatStyle = FlatStyle.Flat, ForeColor = C_TEXT_SEC, BackColor = C_BG_DARK, Font = new Font("Segoe UI", 10f), Cursor = Cursors.Hand };
            btnCancelar.FlatAppearance.BorderSize = 1;
            btnCancelar.FlatAppearance.BorderColor = C_BORDER;
            btnCancelar.Click += (s, e) => this.Close();
            pnlCard.Controls.Add(btnCancelar);

            btnGuardar = new Button { Text = "Guardar Venta", Location = new Point(330, 400), Size = new Size(180, 40), FlatStyle = FlatStyle.Flat, ForeColor = Color.White, BackColor = C_BG_DARK, Font = new Font("Segoe UI", 10f, FontStyle.Bold), Cursor = Cursors.Hand };
            btnGuardar.FlatAppearance.BorderSize = 1;
            btnGuardar.FlatAppearance.BorderColor = C_BORDER;
            btnGuardar.FlatAppearance.MouseOverBackColor = C_ACCENT;
            btnGuardar.Click += BtnGuardar_Click;
            pnlCard.Controls.Add(btnGuardar);

            // TOTAL BAR (Barra morada pegada matemáticamente al fondo)
            pnlResumen = new Panel { Location = new Point(0, 560), Size = new Size(680, 60), BackColor = C_ACCENT };
            var lblTotalTxt = new Label { Text = "TOTAL A PAGAR:", ForeColor = Color.White, Font = new Font("Segoe UI", 11f, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20) };

            lblTotalFinal = new Label { Text = "Bs. 0.00", ForeColor = Color.White, Font = new Font("Segoe UI", 16f, FontStyle.Bold), AutoSize = false, Size = new Size(200, 30), TextAlign = ContentAlignment.MiddleRight, Location = new Point(460, 15) };

            pnlResumen.Controls.AddRange(new Control[] { lblTotalTxt, lblTotalFinal });
            this.Controls.Add(pnlResumen);
        }

        Label AgregarLabel(Control parent, string texto, int x, int y)
        {
            var lbl = new Label { Text = texto, ForeColor = C_TEXT_SEC, Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) };
            parent.Controls.Add(lbl);
            return lbl;
        }

        ComboBox AgregarCombo(Control parent, int x, int y, int ancho)
        {
            var cmb = new ComboBox { Location = new Point(x, y), Size = new Size(ancho, 30), DropDownStyle = ComboBoxStyle.DropDownList, BackColor = C_BG_PANEL, ForeColor = C_TEXT_PRI, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10f) };
            parent.Controls.Add(cmb);
            return cmb;
        }

        TextBox AgregarTextBox(Control parent, int x, int y, int ancho)
        {
            var txt = new TextBox { Location = new Point(x, y), Size = new Size(ancho, 30), BackColor = C_BG_PANEL, ForeColor = C_TEXT_PRI, BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 10f) };
            parent.Controls.Add(txt);
            return txt;
        }

        void CargarDatos()
        {
            try
            {
                var db = new ConexionBD();
                var conn = db.Abrir();
                if (conn == null) return;

                using (var cmd = new NpgsqlCommand("SELECT id_cliente, nombre || ' ' || apellido FROM cliente ORDER BY nombre", conn))
                using (var rdr = cmd.ExecuteReader())
                    while (rdr.Read())
                        cmbCliente.Items.Add(new ComboItem(rdr.GetInt32(0), rdr.GetString(1)));

                using (var cmd = new NpgsqlCommand("SELECT id_empleado, nombre || ' ' || apellido || ' (' || cargo || ')' FROM empleado ORDER BY nombre", conn))
                using (var rdr = cmd.ExecuteReader())
                    while (rdr.Read())
                        cmbEmpleado.Items.Add(new ComboItem(rdr.GetInt32(0), rdr.GetString(1)));

                using (var cmd = new NpgsqlCommand("SELECT id_producto, nombre, precio, stock FROM producto ORDER BY nombre", conn))
                using (var rdr = cmd.ExecuteReader())
                    while (rdr.Read())
                        cmbProducto.Items.Add(new ProductoItem(rdr.GetInt32(0), rdr.GetString(1), rdr.GetDecimal(2), rdr.GetInt32(3)));

                if (cmbCliente.Items.Count > 0) cmbCliente.SelectedIndex = 0;
                if (cmbEmpleado.Items.Count > 0) cmbEmpleado.SelectedIndex = 0;
                if (cmbProducto.Items.Count > 0) { cmbProducto.SelectedIndex = 0; ActualizarSubtotal(); }

                db.Cerrar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void CmbProducto_Changed(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedItem is ProductoItem p)
            {
                txtPrecioUnitario.Text = p.Precio.ToString("0.00");
                nudCantidad.Maximum = p.Stock;
                ActualizarSubtotal();
            }
        }

        void ActualizarSubtotal()
        {
            if (decimal.TryParse(txtPrecioUnitario.Text, out decimal precio))
            {
                decimal sub = precio * (int)nudCantidad.Value;
                txtSubtotal.Text = sub.ToString("0.00");
                lblTotalFinal.Text = "Bs. " + sub.ToString("0.00");
            }
        }

        void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedItem == null || cmbEmpleado.SelectedItem == null || cmbProducto.SelectedItem == null)
            {
                MessageBox.Show("Por favor completa todos los campos obligatorios (*).", "Campos requeridos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var cliente = (ComboItem)cmbCliente.SelectedItem;
            var empleado = (ComboItem)cmbEmpleado.SelectedItem;
            var producto = (ProductoItem)cmbProducto.SelectedItem;

            if ((int)nudCantidad.Value > producto.Stock)
            {
                MessageBox.Show($"Stock insuficiente. Solo hay {producto.Stock} unidades disponibles.", "Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Confirmar venta?\n\nCliente:  {cliente}\nProducto: {producto}\nCantidad: {(int)nudCantidad.Value}\nTotal:    Bs. {txtSubtotal.Text}",
                "Confirmar venta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                var db = new ConexionBD();
                var conn = db.Abrir();

                long idVenta;
                using (var cmd = new NpgsqlCommand(@"INSERT INTO venta (id_cliente, id_empleado, metodo_pago, estado, total) VALUES (@ic,@ie,@mp,@est,@tot) RETURNING id_venta", conn))
                {
                    cmd.Parameters.AddWithValue("@ic", cliente.Id);
                    cmd.Parameters.AddWithValue("@ie", empleado.Id);
                    cmd.Parameters.AddWithValue("@mp", cmbMetodoPago.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@est", cmbEstado.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@tot", decimal.Parse(txtSubtotal.Text));
                    idVenta = (long)cmd.ExecuteScalar();
                }

                using (var cmd = new NpgsqlCommand(@"INSERT INTO detalle_venta (id_venta, id_producto, cantidad, precio_unitario) VALUES (@iv,@ip,@cant,@pu)", conn))
                {
                    cmd.Parameters.AddWithValue("@iv", idVenta);
                    cmd.Parameters.AddWithValue("@ip", producto.Id);
                    cmd.Parameters.AddWithValue("@cant", (int)nudCantidad.Value);
                    cmd.Parameters.AddWithValue("@pu", producto.Precio);
                    cmd.ExecuteNonQuery();
                }

                db.Cerrar();
                MessageBox.Show($"Venta #{idVenta} registrada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la venta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    class ComboItem
    {
        public int Id { get; }
        public string Nombre { get; }
        public ComboItem(int id, string nombre) { Id = id; Nombre = nombre; }
        public override string ToString() => Nombre;
    }

    class ProductoItem
    {
        public int Id { get; }
        public string Nombre { get; }
        public decimal Precio { get; }
        public int Stock { get; }
        public ProductoItem(int id, string nombre, decimal precio, int stock) { Id = id; Nombre = nombre; Precio = precio; Stock = stock; }
        public override string ToString() => $"{Nombre}  (Stock: {Stock})";
    }
}