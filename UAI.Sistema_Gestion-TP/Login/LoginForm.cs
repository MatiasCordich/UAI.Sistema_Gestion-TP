using BLL;
using ENTITY;
using System;
using System.Windows.Forms;
using UAI.Sistema_Gestion_TP.UserSesion;


namespace UAI.Sistema_Gestion_TP.Login
{
    /*---------------------------------------------------------------------
    Se encarga de manejar la lógica de la interfaz de usuario del Login. 
    -----------------------------------------------------------------------*/
    public partial class LoginFrm : Form
    {
        readonly LoginBLL loginBLL = new LoginBLL();
        public LoginFrm()
        {
            InitializeComponent();
        }

        /* Logica de Login */
        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                /* Llamamos a la función Login para obtener un usuario */
                Usuario user = loginBLL.Login(txtDNI.Text, txtPassword.Text);

                /* Si se obutuvo un usuario, se setea para la sesión */
                Sesion.UsuarioLogueado = user;

                /* El Formulario Principal ni bien inicia el programa es el Container */
                MainForm mainForm = (MainForm)this.MdiParent;

                /* Invocamos la función donde setea el menú del Formulario Principal según rol */
                mainForm.SetMenuByRol();

                /* Se cierra el formulario de Login */
                this.Close();

            }
            catch (Exception ex)
            {
                /* Si hay error, mostrarlo*/
                LblError.Text = ex.Message;
                LblError.Visible = true;
            }
        }

        /* Lógica para mostrar la contraseña */
        private void BtnShowPass_Click(object sender, EventArgs e)
        {
            /* Dependiendo si el input tiene algún caracter de constraseña, setear la lógica de mostrar u ocultar. */
            if(txtPassword.PasswordChar == '*')
            {
                txtPassword.PasswordChar = '\0';
                BtnShowPass.Text = "🛇";
            } else
            {
                txtPassword.PasswordChar = '*';
                BtnShowPass.Text = "👁";
            }
        }

        
    }
}

               
