using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UAI.Sistema_Gestion_TP.UI.Usuarios;

namespace UAI.Sistema_Gestion_TP.Usuarios
{
    public partial class AltaUsuarioForm : Form
    {
        /* Instanciams la BLL de Usuaio */
        private readonly UsuarioBLL _usuarioBll = new UsuarioBLL();

        /* Esto es un 'flag' -> 0 = Alta, > 0 = Modifiación */
        private int _usuarioId = 0;
        public AltaUsuarioForm()
        {
            InitializeComponent();
        }

        private void AltaUsuarioForm_Load(object sender, EventArgs e)
        {
            /* Setemos un valor por defecto */
            CbxRol.SelectedIndex = 0;
        }

        private void TxtDni_KeyPress(object sender, KeyPressEventArgs e)
        {
            /* Seteamos que el input de DNI sólo se ingresen números */
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BtnAltaUsuario_Click(object sender, EventArgs e)
        {

            try
            {
                /* Instanciamos el nuevo Usuario */
                Usuario nuevoUsuario;

                /* Creamos un tipo de Usuario determinado según el valor del ComboBox */
                string rolSeleccionado = CbxRol.SelectedItem.ToString();
                switch (rolSeleccionado)
                {
                    case "ADMIN": nuevoUsuario = new Administrador(); 
                        break;
                    case "PROFESOR": nuevoUsuario = nuevoUsuario = new Profesor(); 
                        break;
                    default: nuevoUsuario = new Alumno();
                        break;
                }


                /* Cargamos el nuevo usuario con los datos */
                nuevoUsuario.Id = _usuarioId; // Usamos nuestro flag
                nuevoUsuario.DNI = TxtDni.Text.Trim();
                nuevoUsuario.Nombre = TxtNombre.Text.Trim();
                nuevoUsuario.Apellido = TxtApellido.Text.Trim();
                nuevoUsuario.Email = TxtEmail.Text.Trim();
                nuevoUsuario.Password = TxtPassword.Text;
                nuevoUsuario.Rol = rolSeleccionado;

                /* Llamamos a la BLL */
                _usuarioBll.CrearUsuario(nuevoUsuario);

                /* Mostramos un mensaje de éxito dinámico según el flag */
                string msjExito; 
                
                /* Antes de hacer alguna acción, al setear el Id */
                if (_usuarioId == 0)
                {
                    msjExito = "Usuario creado exitosamente.";
                }
                else
                {
                    msjExito = "Usuario modificado exitosamente.";
                }

                MessageBox.Show(msjExito, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                /* Refrescamos el listado de usuarios */
                this.RefrescarListado();

                /* Cerramos el formulario */
                this.Close();
            }
            catch (Exception ex)
            {
                /* Aquí atrapamos todos los mensjes de error escristo en la BLL */
                MessageBox.Show(ex.Message, "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            
        }

        private void BtnCancelarForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        // ---------------------------------------------------------
        // MÉTODOS AUXILIARES
        // ---------------------------------------------------------
        private void RefrescarListado()
        {
            /* Buscamos en los formularios hijos del MainForm, si hay un formulario de listar Usuarios */
            foreach (Form formularios in this.MdiParent.MdiChildren)
            {
                if (formularios is ListarUsuariosForm formListar)
                {
                    /* Si hay un formulario de listar usuarios, le decimos que nos actualice la grilla */
                    formListar.ActualizarGrilla();
                    break;
                }
            }
        }

        
    }
}
