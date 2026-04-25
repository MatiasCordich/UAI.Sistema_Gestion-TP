namespace UAI.Sistema_Gestion_TP
{
    partial class MainForm
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
            this.menuStrMain = new System.Windows.Forms.MenuStrip();
            this.menuAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuListarUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProfesor = new System.Windows.Forms.ToolStripMenuItem();
            this.verCursosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verEntregasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verMateriasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAlumno = new System.Windows.Forms.ToolStripMenuItem();
            this.subirEntregasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verMateriasToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnCerrarSesion = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.PlUserInfo = new System.Windows.Forms.Panel();
            this.menuStrMain.SuspendLayout();
            this.PlUserInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrMain
            // 
            this.menuStrMain.Enabled = false;
            this.menuStrMain.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrMain.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAdmin,
            this.menuProfesor,
            this.menuAlumno,
            this.menuLogout});
            this.menuStrMain.Location = new System.Drawing.Point(0, 0);
            this.menuStrMain.Name = "menuStrMain";
            this.menuStrMain.Padding = new System.Windows.Forms.Padding(12, 4, 0, 4);
            this.menuStrMain.Size = new System.Drawing.Size(1580, 40);
            this.menuStrMain.TabIndex = 1;
            this.menuStrMain.Text = "menuSistemaGestion";
            this.menuStrMain.Visible = false;
            // 
            // menuAdmin
            // 
            this.menuAdmin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuListarUsuarios});
            this.menuAdmin.Name = "menuAdmin";
            this.menuAdmin.Size = new System.Drawing.Size(160, 32);
            this.menuAdmin.Text = "Administracion";
            this.menuAdmin.Visible = false;
            // 
            // MenuListarUsuarios
            // 
            this.MenuListarUsuarios.Name = "MenuListarUsuarios";
            this.MenuListarUsuarios.Size = new System.Drawing.Size(288, 36);
            this.MenuListarUsuarios.Text = "Gestión de Usuarios";
            this.MenuListarUsuarios.Click += new System.EventHandler(this.MenuListarUsuarios_Click);
            // 
            // menuProfesor
            // 
            this.menuProfesor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verCursosToolStripMenuItem,
            this.verEntregasToolStripMenuItem,
            this.verMateriasToolStripMenuItem});
            this.menuProfesor.Name = "menuProfesor";
            this.menuProfesor.Size = new System.Drawing.Size(119, 32);
            this.menuProfesor.Text = "Profesores";
            this.menuProfesor.Visible = false;
            // 
            // verCursosToolStripMenuItem
            // 
            this.verCursosToolStripMenuItem.Name = "verCursosToolStripMenuItem";
            this.verCursosToolStripMenuItem.Size = new System.Drawing.Size(222, 36);
            this.verCursosToolStripMenuItem.Text = "Ver Cursos";
            // 
            // verEntregasToolStripMenuItem
            // 
            this.verEntregasToolStripMenuItem.Name = "verEntregasToolStripMenuItem";
            this.verEntregasToolStripMenuItem.Size = new System.Drawing.Size(222, 36);
            this.verEntregasToolStripMenuItem.Text = "Ver Entregas";
            // 
            // verMateriasToolStripMenuItem
            // 
            this.verMateriasToolStripMenuItem.Name = "verMateriasToolStripMenuItem";
            this.verMateriasToolStripMenuItem.Size = new System.Drawing.Size(222, 36);
            this.verMateriasToolStripMenuItem.Text = "Ver Materias";
            // 
            // menuAlumno
            // 
            this.menuAlumno.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subirEntregasToolStripMenuItem,
            this.verMateriasToolStripMenuItem1});
            this.menuAlumno.Name = "menuAlumno";
            this.menuAlumno.Size = new System.Drawing.Size(97, 32);
            this.menuAlumno.Text = "Alumno";
            this.menuAlumno.Visible = false;
            // 
            // subirEntregasToolStripMenuItem
            // 
            this.subirEntregasToolStripMenuItem.Name = "subirEntregasToolStripMenuItem";
            this.subirEntregasToolStripMenuItem.Size = new System.Drawing.Size(240, 36);
            this.subirEntregasToolStripMenuItem.Text = "Subir Entregas";
            // 
            // verMateriasToolStripMenuItem1
            // 
            this.verMateriasToolStripMenuItem1.Name = "verMateriasToolStripMenuItem1";
            this.verMateriasToolStripMenuItem1.Size = new System.Drawing.Size(240, 36);
            this.verMateriasToolStripMenuItem1.Text = "Ver Materias";
            // 
            // menuLogout
            // 
            this.menuLogout.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnCerrarSesion});
            this.menuLogout.Name = "menuLogout";
            this.menuLogout.Size = new System.Drawing.Size(85, 32);
            this.menuLogout.Text = "Sesión";
            // 
            // BtnCerrarSesion
            // 
            this.BtnCerrarSesion.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.BtnCerrarSesion.Name = "BtnCerrarSesion";
            this.BtnCerrarSesion.Size = new System.Drawing.Size(270, 36);
            this.BtnCerrarSesion.Text = "Cerrar Sesión";
            this.BtnCerrarSesion.Click += new System.EventHandler(this.BtnCerrarSesion_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Bienvenido/a";
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblUserInfo.Location = new System.Drawing.Point(145, 9);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(108, 37);
            this.lblUserInfo.TabIndex = 1;
            this.lblUserInfo.Text = "label2";
            // 
            // PlUserInfo
            // 
            this.PlUserInfo.Controls.Add(this.lblUserInfo);
            this.PlUserInfo.Controls.Add(this.label1);
            this.PlUserInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PlUserInfo.Location = new System.Drawing.Point(0, 735);
            this.PlUserInfo.Name = "PlUserInfo";
            this.PlUserInfo.Size = new System.Drawing.Size(1580, 50);
            this.PlUserInfo.TabIndex = 5;
            this.PlUserInfo.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1580, 785);
            this.Controls.Add(this.PlUserInfo);
            this.Controls.Add(this.menuStrMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrMain;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UAI - Sistema Gestión TP";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrMain.ResumeLayout(false);
            this.menuStrMain.PerformLayout();
            this.PlUserInfo.ResumeLayout(false);
            this.PlUserInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrMain;
        private System.Windows.Forms.ToolStripMenuItem menuAdmin;
        private System.Windows.Forms.ToolStripMenuItem MenuListarUsuarios;
        private System.Windows.Forms.ToolStripMenuItem menuProfesor;
        private System.Windows.Forms.ToolStripMenuItem verCursosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verEntregasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verMateriasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuAlumno;
        private System.Windows.Forms.ToolStripMenuItem subirEntregasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verMateriasToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuLogout;
        private System.Windows.Forms.ToolStripMenuItem BtnCerrarSesion;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel PlUserInfo;
    }
}

