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
using UAI.Sistema_Gestion_TP.Usuarios;

namespace UAI.Sistema_Gestion_TP.UI.Usuarios
{
    public partial class ListarUsuariosForm : Form
    {
        /* Instanciamos la BLL */
        private readonly UsuarioBLL _usuarioBll = new UsuarioBLL();

        public ListarUsuariosForm()
        {
            InitializeComponent();
        }

        private void ListarUsuariosForm_Load(object sender, EventArgs e)
        {
            SetearCombos();
            ActualizarGrilla();
        }

       
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            ActualizarGrilla();
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            TxtFiltroDNI.Clear();
            SetearCombos();
            ActualizarGrilla();
        }

        private void BtnAltaForm_Click(object sender, EventArgs e)
        {
            // 1. Buscamos entre los hijos del MainForm si ya existe una instancia de frmCrearUsuario
            foreach (Form formH in this.MdiParent.MdiChildren)
            {
                if (formH is AltaUsuarioForm)
                {
                    // Si ya está abierto, lo traemos al frente y salimos del método
                    formH.BringToFront();
                    return;
                }
            }

            // 2. Si el bucle termina y no lo encontró, recién ahí lo creamos
            AltaUsuarioForm frmAlta = new AltaUsuarioForm();
            frmAlta.MdiParent = this.MdiParent;
            frmAlta.Show();

        }

        // ---------------------------------------------------------
        // MÉTODOS AUXILIARES
        // ---------------------------------------------------------

        private void SetearCombos()
        {
            /* Setemos los valores de los ComboBox a sus valores Iniciales */ 
            CmbFiltroEstado.SelectedIndex = 0;
            CmbFiltroRol.SelectedIndex = 0;
        }

        public void ActualizarGrilla()
        {
            try
            {

                /* Obtenemos los valores de los filtros */
                string dni = TxtFiltroDNI.Text.Trim();
                string rol = CmbFiltroRol.SelectedItem.ToString();
                string estado = CmbFiltroEstado.SelectedItem.ToString();

                /*Obtenemos la lista de usuarios llamando a la BLL */
                List <Usuario> lista = _usuarioBll.ListarUsuarios(dni, rol, estado);

                /* Limpiamos el DataGrid para volvera poblar con datos actualizados */
                DgvUsuarios.DataSource = null;
                DgvUsuarios.DataSource = lista;

                /* Ocultamos la columna de contraseña */
                DgvUsuarios.Columns["Password"].Visible = false;
                DgvUsuarios.Columns["Id"].Visible = false;
                DgvUsuarios.Columns["Activo"].Visible = false;

                /* Llamamos al método para formatear las filas según el estado del usuario */
                FormatearFilasPorEstado();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la lista: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatearFilasPorEstado()
        {
            /* Por cada fila del DataGrid asignamos un color según el estado del usuario */
            foreach (DataGridViewRow row in DgvUsuarios.Rows)
            {
                if (row.DataBoundItem is Usuario usuario)
                {
                    if (!usuario.Activo)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }

       
    }
}
