using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace TPFInalNivel2_LuduenaGomez
{
    public partial class frmProducto : Form
    {

        private List<Producto> listaProductos;

        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            cargar();
        }

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvProductos.CurrentRow != null)
            {
                Producto seleccionado = (Producto)dgvProductos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            } 
        }

        private void cargar()
        {
            ProductoNegocio negocio = new ProductoNegocio();

            try
            {
                listaProductos = negocio.listar();
                dgvProductos.DataSource = listaProductos;
                ocultarColumnas();
                pbxProducto.Load(listaProductos[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void ocultarColumnas()
        {
            dgvProductos.Columns["ImagenUrl"].Visible = false;
            dgvProductos.Columns["Id"].Visible = false;
            dgvProductos.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxProducto.Load(imagen);
            }
            catch (Exception)
            {

                pbxProducto.Load("https://img.freepik.com/vector-gratis/ilustracion-icono-galeria_53876-27002.jpg");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaProducto alta = new frmAltaProducto();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Producto seleccionado;
            seleccionado = (Producto)dgvProductos.CurrentRow.DataBoundItem;

            frmAltaProducto alta = new frmAltaProducto(seleccionado);
            alta.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            ProductoNegocio negocio = new ProductoNegocio();
            Producto producto;
            try
            {
                DialogResult rta = MessageBox.Show("¿Desea eliminar este producto permanentemente?", "Eliminar producto", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(rta == DialogResult.Yes)
                {
                    producto = (Producto)dgvProductos.CurrentRow.DataBoundItem;
                    negocio.eliminarProducto(producto.Id);
                    cargar();
                }
                    

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            List<Producto> listafiltrada;
            string filtro = txtBuscar.Text;

            if (filtro != "")
                listafiltrada = listaProductos.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()) || x.Codigo.ToLower().Contains(filtro.ToLower()) || x.MarcaProducto.Descripcion.ToLower().Contains(filtro) || x.CategoriaProducto.Descripcion.ToLower().Contains(filtro));
            else
            {
                listafiltrada = listaProductos;
            }

            dgvProductos.DataSource = null;
            dgvProductos.DataSource = listafiltrada;
            ocultarColumnas();
        }
    }
}
