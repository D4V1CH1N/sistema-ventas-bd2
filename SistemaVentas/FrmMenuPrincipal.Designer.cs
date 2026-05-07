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
            btnEmpleados = new Button();
            pnlSidebar = new Panel();
            pnlTop = new Panel();
            pnlContent = new Panel();
            lblTitle = new Label();
            lblUser = new Label();
            lblConexion = new Label();
            dgvVentas = new DataGridView();
            button1 = new Button();
            cardVentas = new Panel();
            cardIngresos = new Panel();
            cardClientes = new Panel();
            cardStock = new Panel();
            pnlSidebar.SuspendLayout();
            pnlTop.SuspendLayout();
            pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvVentas).BeginInit();
            SuspendLayout();
            // 
            // btnClientes
            // 
            btnClientes.Location = new Point(38, 163);
            btnClientes.Margin = new Padding(3, 4, 3, 4);
            btnClientes.Name = "btnClientes";
            btnClientes.Size = new Size(158, 31);
            btnClientes.TabIndex = 0;
            btnClientes.Text = "Módulo Clientes";
            btnClientes.UseVisualStyleBackColor = true;
            // 
            // btnProductos
            // 
            btnProductos.Location = new Point(38, 76);
            btnProductos.Margin = new Padding(3, 4, 3, 4);
            btnProductos.Name = "btnProductos";
            btnProductos.Size = new Size(157, 31);
            btnProductos.TabIndex = 1;
            btnProductos.Text = "Módulo Productos";
            btnProductos.UseVisualStyleBackColor = true;
            btnProductos.Click += btnProductos_Click_1;
            // 
            // btnVentas
            // 
            btnVentas.Location = new Point(38, 202);
            btnVentas.Margin = new Padding(3, 4, 3, 4);
            btnVentas.Name = "btnVentas";
            btnVentas.Size = new Size(157, 32);
            btnVentas.TabIndex = 2;
            btnVentas.Text = "Registrar Venta";
            btnVentas.UseVisualStyleBackColor = true;
            btnVentas.Click += btnVentas_Click;
            // 
            // btnEmpleados
            // 
            btnEmpleados.Location = new Point(38, 114);
            btnEmpleados.Name = "btnEmpleados";
            btnEmpleados.Size = new Size(157, 29);
            btnEmpleados.TabIndex = 3;
            btnEmpleados.Text = "Modulo Empleados";
            btnEmpleados.UseVisualStyleBackColor = true;
            // 
            // pnlSidebar
            // 
            pnlSidebar.Controls.Add(btnProductos);
            pnlSidebar.Controls.Add(btnEmpleados);
            pnlSidebar.Controls.Add(btnClientes);
            pnlSidebar.Controls.Add(btnVentas);
            pnlSidebar.Location = new Point(12, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(250, 553);
            pnlSidebar.TabIndex = 4;
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(lblTitle);
            pnlTop.Controls.Add(lblUser);
            pnlTop.Location = new Point(265, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(647, 73);
            pnlTop.TabIndex = 5;
            // 
            // pnlContent
            // 
            pnlContent.Controls.Add(button1);
            pnlContent.Controls.Add(dgvVentas);
            pnlContent.Location = new Point(268, 202);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(634, 351);
            pnlContent.TabIndex = 6;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(22, 26);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(35, 20);
            lblTitle.TabIndex = 7;
            lblTitle.Text = "title";
            // 
            // lblUser
            // 
            lblUser.AutoSize = true;
            lblUser.Location = new Point(579, 26);
            lblUser.Name = "lblUser";
            lblUser.Size = new Size(38, 20);
            lblUser.TabIndex = 8;
            lblUser.Text = "User";
            // 
            // lblConexion
            // 
            lblConexion.AutoSize = true;
            lblConexion.Location = new Point(12, 571);
            lblConexion.Name = "lblConexion";
            lblConexion.Size = new Size(71, 20);
            lblConexion.TabIndex = 9;
            lblConexion.Text = "Conexion";
            // 
            // dgvVentas
            // 
            dgvVentas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVentas.Location = new Point(19, 89);
            dgvVentas.Name = "dgvVentas";
            dgvVentas.RowHeadersWidth = 51;
            dgvVentas.Size = new Size(595, 243);
            dgvVentas.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(520, 22);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // cardVentas
            // 
            cardVentas.Location = new Point(287, 92);
            cardVentas.Name = "cardVentas";
            cardVentas.Size = new Size(115, 75);
            cardVentas.TabIndex = 10;
            // 
            // cardIngresos
            // 
            cardIngresos.Location = new Point(436, 92);
            cardIngresos.Name = "cardIngresos";
            cardIngresos.Size = new Size(115, 75);
            cardIngresos.TabIndex = 11;
            // 
            // cardClientes
            // 
            cardClientes.Location = new Point(586, 92);
            cardClientes.Name = "cardClientes";
            cardClientes.Size = new Size(119, 75);
            cardClientes.TabIndex = 12;
            // 
            // cardStock
            // 
            cardStock.Location = new Point(736, 92);
            cardStock.Name = "cardStock";
            cardStock.Size = new Size(121, 75);
            cardStock.TabIndex = 13;
            // 
            // FrmMenuPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(cardStock);
            Controls.Add(cardClientes);
            Controls.Add(cardIngresos);
            Controls.Add(cardVentas);
            Controls.Add(lblConexion);
            Controls.Add(pnlContent);
            Controls.Add(pnlTop);
            Controls.Add(pnlSidebar);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrmMenuPrincipal";
            Text = "Form1";
            Load += FrmMenuPrincipal_Load;
            pnlSidebar.ResumeLayout(false);
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvVentas).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnClientes;
        private Button btnProductos;
        private Button btnVentas;
        private Button btnEmpleados;
        private Panel pnlSidebar;
        private Panel pnlTop;
        private Panel pnlContent;
        private Label lblTitle;
        private Label lblUser;
        private Label lblConexion;
        private Button button1;
        private DataGridView dgvVentas;
        private Panel cardVentas;
        private Panel cardIngresos;
        private Panel cardClientes;
        private Panel cardStock;
    }
}
