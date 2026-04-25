namespace UAI.Sistema_Gestion_TP.Usuarios
{
    partial class AltaUsuarioForm
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
            this.GbxAltaUsuario = new System.Windows.Forms.GroupBox();
            this.BtnCancelarForm = new System.Windows.Forms.Button();
            this.LblError = new System.Windows.Forms.Label();
            this.BtnAltaUsuario = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.CbxRol = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtPassword = new System.Windows.Forms.TextBox();
            this.TxtEmail = new System.Windows.Forms.TextBox();
            this.TxtApellido = new System.Windows.Forms.TextBox();
            this.TxtNombre = new System.Windows.Forms.TextBox();
            this.TxtDni = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GbxAltaUsuario.SuspendLayout();
            this.SuspendLayout();
            // 
            // GbxAltaUsuario
            // 
            this.GbxAltaUsuario.Controls.Add(this.BtnCancelarForm);
            this.GbxAltaUsuario.Controls.Add(this.LblError);
            this.GbxAltaUsuario.Controls.Add(this.BtnAltaUsuario);
            this.GbxAltaUsuario.Controls.Add(this.label6);
            this.GbxAltaUsuario.Controls.Add(this.CbxRol);
            this.GbxAltaUsuario.Controls.Add(this.label5);
            this.GbxAltaUsuario.Controls.Add(this.label4);
            this.GbxAltaUsuario.Controls.Add(this.label3);
            this.GbxAltaUsuario.Controls.Add(this.label2);
            this.GbxAltaUsuario.Controls.Add(this.TxtPassword);
            this.GbxAltaUsuario.Controls.Add(this.TxtEmail);
            this.GbxAltaUsuario.Controls.Add(this.TxtApellido);
            this.GbxAltaUsuario.Controls.Add(this.TxtNombre);
            this.GbxAltaUsuario.Controls.Add(this.TxtDni);
            this.GbxAltaUsuario.Controls.Add(this.label1);
            this.GbxAltaUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GbxAltaUsuario.Location = new System.Drawing.Point(13, 42);
            this.GbxAltaUsuario.Name = "GbxAltaUsuario";
            this.GbxAltaUsuario.Size = new System.Drawing.Size(528, 705);
            this.GbxAltaUsuario.TabIndex = 0;
            this.GbxAltaUsuario.TabStop = false;
            this.GbxAltaUsuario.Text = "Alta Usuario";
            // 
            // BtnCancelarForm
            // 
            this.BtnCancelarForm.BackColor = System.Drawing.Color.Gray;
            this.BtnCancelarForm.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BtnCancelarForm.Location = new System.Drawing.Point(6, 617);
            this.BtnCancelarForm.Name = "BtnCancelarForm";
            this.BtnCancelarForm.Size = new System.Drawing.Size(507, 71);
            this.BtnCancelarForm.TabIndex = 14;
            this.BtnCancelarForm.Text = "Cancelar";
            this.BtnCancelarForm.UseVisualStyleBackColor = false;
            this.BtnCancelarForm.Click += new System.EventHandler(this.BtnCancelarForm_Click);
            // 
            // LblError
            // 
            this.LblError.AutoSize = true;
            this.LblError.ForeColor = System.Drawing.Color.Red;
            this.LblError.Location = new System.Drawing.Point(54, 470);
            this.LblError.Name = "LblError";
            this.LblError.Size = new System.Drawing.Size(59, 25);
            this.LblError.TabIndex = 13;
            this.LblError.Text = "Error";
            this.LblError.Visible = false;
            // 
            // BtnAltaUsuario
            // 
            this.BtnAltaUsuario.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.BtnAltaUsuario.Location = new System.Drawing.Point(6, 525);
            this.BtnAltaUsuario.Name = "BtnAltaUsuario";
            this.BtnAltaUsuario.Size = new System.Drawing.Size(507, 71);
            this.BtnAltaUsuario.TabIndex = 12;
            this.BtnAltaUsuario.Text = "Crear Usuario";
            this.BtnAltaUsuario.UseVisualStyleBackColor = false;
            this.BtnAltaUsuario.Click += new System.EventHandler(this.BtnAltaUsuario_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(23, 397);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 25);
            this.label6.TabIndex = 11;
            this.label6.Text = "Rol";
            // 
            // CbxRol
            // 
            this.CbxRol.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CbxRol.FormattingEnabled = true;
            this.CbxRol.Items.AddRange(new object[] {
            "ADMIN",
            "PROFESOR",
            "ALUMNO"});
            this.CbxRol.Location = new System.Drawing.Point(165, 389);
            this.CbxRol.Name = "CbxRol";
            this.CbxRol.Size = new System.Drawing.Size(297, 37);
            this.CbxRol.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(23, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 25);
            this.label5.TabIndex = 9;
            this.label5.Text = "Nombre";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(23, 211);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Apellido";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(23, 269);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "Email";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(23, 330);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Contraseña";
            // 
            // TxtPassword
            // 
            this.TxtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtPassword.Location = new System.Drawing.Point(165, 325);
            this.TxtPassword.Name = "TxtPassword";
            this.TxtPassword.Size = new System.Drawing.Size(297, 35);
            this.TxtPassword.TabIndex = 5;
            // 
            // TxtEmail
            // 
            this.TxtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtEmail.Location = new System.Drawing.Point(165, 264);
            this.TxtEmail.Name = "TxtEmail";
            this.TxtEmail.Size = new System.Drawing.Size(297, 35);
            this.TxtEmail.TabIndex = 4;
            // 
            // TxtApellido
            // 
            this.TxtApellido.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtApellido.Location = new System.Drawing.Point(165, 206);
            this.TxtApellido.Name = "TxtApellido";
            this.TxtApellido.Size = new System.Drawing.Size(297, 35);
            this.TxtApellido.TabIndex = 3;
            // 
            // TxtNombre
            // 
            this.TxtNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtNombre.Location = new System.Drawing.Point(165, 142);
            this.TxtNombre.Name = "TxtNombre";
            this.TxtNombre.Size = new System.Drawing.Size(297, 35);
            this.TxtNombre.TabIndex = 2;
            // 
            // TxtDni
            // 
            this.TxtDni.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtDni.Location = new System.Drawing.Point(165, 85);
            this.TxtDni.MaxLength = 8;
            this.TxtDni.Name = "TxtDni";
            this.TxtDni.Size = new System.Drawing.Size(297, 35);
            this.TxtDni.TabIndex = 1;
            this.TxtDni.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtDni_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(23, 90);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "DNI";
            // 
            // AltaUsuarioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 759);
            this.Controls.Add(this.GbxAltaUsuario);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "AltaUsuarioForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UAI - Alta Formulario";
            this.Load += new System.EventHandler(this.AltaUsuarioForm_Load);
            this.GbxAltaUsuario.ResumeLayout(false);
            this.GbxAltaUsuario.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GbxAltaUsuario;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtPassword;
        private System.Windows.Forms.TextBox TxtEmail;
        private System.Windows.Forms.TextBox TxtApellido;
        private System.Windows.Forms.TextBox TxtNombre;
        private System.Windows.Forms.TextBox TxtDni;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CbxRol;
        private System.Windows.Forms.Label LblError;
        private System.Windows.Forms.Button BtnAltaUsuario;
        private System.Windows.Forms.Button BtnCancelarForm;
    }
}