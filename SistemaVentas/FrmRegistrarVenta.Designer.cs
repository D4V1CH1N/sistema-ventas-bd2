namespace SistemaVentas
{
    partial class FrmRegistrarVenta
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtIdCliente = new TextBox();
            txtIdProducto = new TextBox();
            txtCantidad = new TextBox();
            txtPrecioUnitario = new TextBox();
            btnGuardarVenta = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // txtIdCliente
            // 
            txtIdCliente.Location = new Point(50, 40);
            txtIdCliente.Name = "txtIdCliente";
            txtIdCliente.Size = new Size(155, 23);
            txtIdCliente.TabIndex = 0;
            // 
            // txtIdProducto
            // 
            txtIdProducto.Location = new Point(50, 86);
            txtIdProducto.Name = "txtIdProducto";
            txtIdProducto.Size = new Size(155, 23);
            txtIdProducto.TabIndex = 1;
            // 
            // txtCantidad
            // 
            txtCantidad.Location = new Point(50, 141);
            txtCantidad.Name = "txtCantidad";
            txtCantidad.Size = new Size(155, 23);
            txtCantidad.TabIndex = 2;
            // 
            // txtPrecioUnitario
            // 
            txtPrecioUnitario.Location = new Point(50, 196);
            txtPrecioUnitario.Name = "txtPrecioUnitario";
            txtPrecioUnitario.Size = new Size(155, 23);
            txtPrecioUnitario.TabIndex = 3;
            // 
            // btnGuardarVenta
            // 
            btnGuardarVenta.Location = new Point(290, 256);
            btnGuardarVenta.Name = "btnGuardarVenta";
            btnGuardarVenta.Size = new Size(128, 23);
            btnGuardarVenta.TabIndex = 4;
            btnGuardarVenta.Text = "Guardar Venta";
            btnGuardarVenta.UseVisualStyleBackColor = true;
            btnGuardarVenta.Click += btnGuardarVenta_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(270, 48);
            label1.Name = "label1";
            label1.Size = new Size(75, 15);
            label1.TabIndex = 5;
            label1.Text = "ID del cliente";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(270, 94);
            label2.Name = "label2";
            label2.Size = new Size(89, 15);
            label2.TabIndex = 6;
            label2.Text = "ID del producto";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(270, 149);
            label3.Name = "label3";
            label3.Size = new Size(55, 15);
            label3.TabIndex = 7;
            label3.Text = "Cantidad";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(270, 199);
            label4.Name = "label4";
            label4.Size = new Size(93, 15);
            label4.TabIndex = 8;
            label4.Text = "Precio (Unitario)";
            // 
            // FrmRegistrarVenta
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnGuardarVenta);
            Controls.Add(txtPrecioUnitario);
            Controls.Add(txtCantidad);
            Controls.Add(txtIdProducto);
            Controls.Add(txtIdCliente);
            Name = "FrmRegistrarVenta";
            Text = "FrmRegistrarVenta";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtIdCliente;
        private TextBox txtIdProducto;
        private TextBox txtCantidad;
        private TextBox txtPrecioUnitario;
        private Button btnGuardarVenta;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}