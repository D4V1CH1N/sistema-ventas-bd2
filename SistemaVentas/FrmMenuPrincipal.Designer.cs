namespace SistemaVentas
{
    partial class FrmMenuPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnClientes = new Button();
            btnProductos = new Button();
            btnVentas = new Button();
            SuspendLayout();
            // 
            // btnClientes
            // 
            btnClientes.Location = new Point(133, 89);
            btnClientes.Name = "btnClientes";
            btnClientes.Size = new Size(138, 23);
            btnClientes.TabIndex = 0;
            btnClientes.Text = "Módulo Clientes";
            btnClientes.UseVisualStyleBackColor = true;
            // 
            // btnProductos
            // 
            btnProductos.Location = new Point(302, 89);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(137, 23);
            btnProductos.TabIndex = 1;
            btnProductos.Text = "Módulo Productos";
            btnProductos.UseVisualStyleBackColor = true;
            // 
            // btnVentas
            // 
            btnVentas.Location = new Point(143, 223);
            btnVentas.Name = "btnVentas";
            btnVentas.Size = new Size(119, 48);
            btnVentas.TabIndex = 2;
            btnVentas.Text = "Registrar Nueva Venta";
            btnVentas.UseVisualStyleBackColor = true;
            btnVentas.Click += btnVentas_Click;
            // 
            // FrmMenuPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnVentas);
            Controls.Add(btnProductos);
            Controls.Add(btnClientes);
            Name = "FrmMenuPrincipal";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button btnClientes;
        private Button btnProductos;
        private Button btnVentas;
    }
}
