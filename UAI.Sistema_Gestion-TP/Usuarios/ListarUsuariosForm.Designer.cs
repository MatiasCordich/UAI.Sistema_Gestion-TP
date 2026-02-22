namespace UAI.Sistema_Gestion_TP.UI.Usuarios
{
    partial class ListarUsuariosForm
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
            this.GboxFiltros = new System.Windows.Forms.GroupBox();
            this.BtnLimpiar = new System.Windows.Forms.Button();
            this.BtnBuscar = new System.Windows.Forms.Button();
            this.CmbFiltroEstado = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CmbFiltroRol = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtFiltroDNI = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DgvUsuarios = new System.Windows.Forms.DataGridView();
            this.GbxAcciones = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.BtnAltaForm = new System.Windows.Forms.Button();
            this.GbxListaUsuarios = new System.Windows.Forms.GroupBox();
            this.GboxFiltros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvUsuarios)).BeginInit();
            this.GbxAcciones.SuspendLayout();
            this.GbxListaUsuarios.SuspendLayout();
            this.SuspendLayout();
            // 
            // GboxFiltros
            // 
            this.GboxFiltros.Controls.Add(this.BtnLimpiar);
            this.GboxFiltros.Controls.Add(this.BtnBuscar);
            this.GboxFiltros.Controls.Add(this.CmbFiltroEstado);
            this.GboxFiltros.Controls.Add(this.label3);
            this.GboxFiltros.Controls.Add(this.CmbFiltroRol);
            this.GboxFiltros.Controls.Add(this.label2);
            this.GboxFiltros.Controls.Add(this.TxtFiltroDNI);
            this.GboxFiltros.Controls.Add(this.label1);
            this.GboxFiltros.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GboxFiltros.ForeColor = System.Drawing.Color.Maroon;
            this.GboxFiltros.Location = new System.Drawing.Point(12, 29);
            this.GboxFiltros.Name = "GboxFiltros";
            this.GboxFiltros.Size = new System.Drawing.Size(1505, 100);
            this.GboxFiltros.TabIndex = 0;
            this.GboxFiltros.TabStop = false;
            this.GboxFiltros.Text = "Búsqueda";
            // 
            // BtnLimpiar
            // 
            this.BtnLimpiar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.BtnLimpiar.ForeColor = System.Drawing.Color.White;
            this.BtnLimpiar.Location = new System.Drawing.Point(1302, 35);
            this.BtnLimpiar.Name = "BtnLimpiar";
            this.BtnLimpiar.Size = new System.Drawing.Size(168, 44);
            this.BtnLimpiar.TabIndex = 8;
            this.BtnLimpiar.Text = "Limpiar Filtros";
            this.BtnLimpiar.UseVisualStyleBackColor = false;
            this.BtnLimpiar.Click += new System.EventHandler(this.BtnLimpiar_Click);
            // 
            // BtnBuscar
            // 
            this.BtnBuscar.BackColor = System.Drawing.Color.LemonChiffon;
            this.BtnBuscar.Location = new System.Drawing.Point(1085, 35);
            this.BtnBuscar.Name = "BtnBuscar";
            this.BtnBuscar.Size = new System.Drawing.Size(168, 44);
            this.BtnBuscar.TabIndex = 7;
            this.BtnBuscar.Text = "Buscar";
            this.BtnBuscar.UseVisualStyleBackColor = false;
            this.BtnBuscar.Click += new System.EventHandler(this.BtnBuscar_Click);
            // 
            // CmbFiltroEstado
            // 
            this.CmbFiltroEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbFiltroEstado.FormattingEnabled = true;
            this.CmbFiltroEstado.Items.AddRange(new object[] {
            "Todos",
            "Activos",
            "Inactivos"});
            this.CmbFiltroEstado.Location = new System.Drawing.Point(869, 43);
            this.CmbFiltroEstado.Name = "CmbFiltroEstado";
            this.CmbFiltroEstado.Size = new System.Drawing.Size(172, 31);
            this.CmbFiltroEstado.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(779, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Estado";
            // 
            // CmbFiltroRol
            // 
            this.CmbFiltroRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbFiltroRol.FormattingEnabled = true;
            this.CmbFiltroRol.Items.AddRange(new object[] {
            "Todos",
            "ADMIN",
            "PROFESOR",
            "ALUMNO"});
            this.CmbFiltroRol.Location = new System.Drawing.Point(475, 43);
            this.CmbFiltroRol.Name = "CmbFiltroRol";
            this.CmbFiltroRol.Size = new System.Drawing.Size(172, 31);
            this.CmbFiltroRol.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(429, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Rol";
            // 
            // TxtFiltroDNI
            // 
            this.TxtFiltroDNI.Location = new System.Drawing.Point(87, 43);
            this.TxtFiltroDNI.Name = "TxtFiltroDNI";
            this.TxtFiltroDNI.Size = new System.Drawing.Size(240, 32);
            this.TxtFiltroDNI.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(36, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "DNI";
            // 
            // DgvUsuarios
            // 
            this.DgvUsuarios.AllowUserToAddRows = false;
            this.DgvUsuarios.AllowUserToDeleteRows = false;
            this.DgvUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvUsuarios.Location = new System.Drawing.Point(6, 34);
            this.DgvUsuarios.MultiSelect = false;
            this.DgvUsuarios.Name = "DgvUsuarios";
            this.DgvUsuarios.ReadOnly = true;
            this.DgvUsuarios.RowHeadersWidth = 62;
            this.DgvUsuarios.RowTemplate.Height = 28;
            this.DgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvUsuarios.Size = new System.Drawing.Size(1421, 424);
            this.DgvUsuarios.TabIndex = 1;
            // 
            // GbxAcciones
            // 
            this.GbxAcciones.Controls.Add(this.button2);
            this.GbxAcciones.Controls.Add(this.BtnAltaForm);
            this.GbxAcciones.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GbxAcciones.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.GbxAcciones.Location = new System.Drawing.Point(12, 135);
            this.GbxAcciones.Name = "GbxAcciones";
            this.GbxAcciones.Size = new System.Drawing.Size(721, 140);
            this.GbxAcciones.TabIndex = 9;
            this.GbxAcciones.TabStop = false;
            this.GbxAcciones.Text = "Acciones";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.MistyRose;
            this.button2.Location = new System.Drawing.Point(370, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(313, 73);
            this.button2.TabIndex = 10;
            this.button2.Text = "🚫 Gestionar Baja/Estado";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // BtnAltaForm
            // 
            this.BtnAltaForm.BackColor = System.Drawing.Color.PaleGreen;
            this.BtnAltaForm.Location = new System.Drawing.Point(38, 39);
            this.BtnAltaForm.Name = "BtnAltaForm";
            this.BtnAltaForm.Size = new System.Drawing.Size(313, 73);
            this.BtnAltaForm.TabIndex = 9;
            this.BtnAltaForm.Text = "➕Alta Usuario ";
            this.BtnAltaForm.UseVisualStyleBackColor = false;
            this.BtnAltaForm.Click += new System.EventHandler(this.BtnAltaForm_Click);
            // 
            // GbxListaUsuarios
            // 
            this.GbxListaUsuarios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GbxListaUsuarios.Controls.Add(this.DgvUsuarios);
            this.GbxListaUsuarios.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GbxListaUsuarios.Location = new System.Drawing.Point(12, 281);
            this.GbxListaUsuarios.Name = "GbxListaUsuarios";
            this.GbxListaUsuarios.Size = new System.Drawing.Size(1450, 471);
            this.GbxListaUsuarios.TabIndex = 10;
            this.GbxListaUsuarios.TabStop = false;
            this.GbxListaUsuarios.Text = "Lista de Usuarios";
            // 
            // ListarUsuariosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1542, 764);
            this.Controls.Add(this.GbxListaUsuarios);
            this.Controls.Add(this.GbxAcciones);
            this.Controls.Add(this.GboxFiltros);
            this.Name = "ListarUsuariosForm";
            this.Text = "UAI - Gestion de Usuarios";
            this.Load += new System.EventHandler(this.ListarUsuariosForm_Load);
            this.GboxFiltros.ResumeLayout(false);
            this.GboxFiltros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvUsuarios)).EndInit();
            this.GbxAcciones.ResumeLayout(false);
            this.GbxListaUsuarios.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GboxFiltros;
        private System.Windows.Forms.ComboBox CmbFiltroRol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtFiltroDNI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnBuscar;
        private System.Windows.Forms.ComboBox CmbFiltroEstado;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView DgvUsuarios;
        private System.Windows.Forms.Button BtnLimpiar;
        private System.Windows.Forms.GroupBox GbxAcciones;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button BtnAltaForm;
        private System.Windows.Forms.GroupBox GbxListaUsuarios;
    }
}