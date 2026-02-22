using ENTITY;
using System;
using System.Windows.Forms;
using UAI.Sistema_Gestion_TP.Login;
using UAI.Sistema_Gestion_TP.UI.Usuarios;
using UAI.Sistema_Gestion_TP.UserSesion;

namespace UAI.Sistema_Gestion_TP
{
    /*--------------------------------------------------------------------------------
    Se encarga de manejar la lógica de la interfaz de usuario del Formulario Principal.
    ----------------------------------------------------------------------------------*/
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /* Evento Load donde se setea siempre el Formulario de Login y se muestra */
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoginFrm loginForm = new LoginFrm();

            loginForm.MdiParent = this;

            loginForm.Show();

        }

        /* Función para setear el Menú dependiendo del rol de usuario*/
        public void SetMenuByRol()
        {

            /* El Menú principal se activa y se vuelve visible */
            this.menuStrMain.Visible = true;
            this.menuStrMain.Enabled = true;

            /* Obtenemos el usuario que se logueó */
            var user = Sesion.UsuarioLogueado;

            /* Dependiendo del usuario, se muestra las opciones de Menú dependiendo del Rol */
            this.menuAdmin.Visible = (user is Administrador);
            this.menuProfesor.Visible = (user is Profesor);
            this.menuAlumno.Visible = (user is Alumno);

            /* Se muestra el nombre y apellido del Usuario logueado */
            this.PlUserInfo.Visible = true;
            this.lblUserInfo.Text = $"{user.Nombre} {user.Apellido} - {user.Rol}";
        }

        /* Botón para cerrar sesión */
        private void BtnCerrarSesion_Click_1(object sender, EventArgs e)
        {

            /* Invocamos la función para desloguarse y setear el usuario a Null */
            Sesion.Logout();

            /* Cerramos todos los formularios que esten abiertos */
            foreach (Form formHijos in this.MdiChildren)
            {
                formHijos.Close();
            }

            /* Desactivamos y ocultamos el menú*/
            this.menuStrMain.Visible = false;
            this.menuStrMain.Enabled = false;

            /* Ocultamos y vacíamos la información de usuario logueado */
            this.PlUserInfo.Visible = false;
            this.lblUserInfo.Text = string.Empty;

            /* Invocamos la función que activa el evento Load del Formulario Principal*/
            MainForm_Load(null, null);

        }

        private void MenuListarUsuarios_Click(object sender, EventArgs e)
        {
            /* Verificamos que el formulario no esté abierto */
            foreach (Form formH in this.MdiChildren)
            {
                if (formH is ListarUsuariosForm)
                {
                    formH.BringToFront();
                    return;
                }
            }
            ListarUsuariosForm listarUsuariosForm = new ListarUsuariosForm();

            listarUsuariosForm.MdiParent = this;

            listarUsuariosForm.Show();
        }
    }
}
