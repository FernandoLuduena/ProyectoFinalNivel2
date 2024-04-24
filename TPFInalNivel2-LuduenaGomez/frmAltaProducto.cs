using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using System.Configuration;
using System.Data.SqlTypes;

namespace TPFInalNivel2_LuduenaGomez
{
    public partial class frmAltaProducto : Form
    {
        private Producto producto = null;
        OpenFileDialog archivo = null;
        public frmAltaProducto()
        {
            InitializeComponent();
        }

        public frmAltaProducto(Producto producto)
        {
            InitializeComponent();
            this.producto = producto;
            Text = "Modificar Producto";
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            ProductoNegocio negocio = new ProductoNegocio();

            try
            {
                if(producto == null)
                    producto = new Producto();

                if (validarCarga())
                    return;

                string precio = txtPrecio.Text;

                producto.Codigo = txtCodigo.Text;
                producto.Nombre = txtNombre.Text;
                producto.Descripcion = txtDescripcion.Text;
                producto.ImagenUrl = txtUrlAlta.Text;
                producto.CategoriaProducto = (Categoria)cboCategoria.SelectedItem;
                producto.MarcaProducto = (Marca)cboMarca.SelectedItem;
                producto.Precio = decimal.Parse(precio);

                if(producto.Id != 0)
                {
                    negocio.modificarProducto(producto);
                    MessageBox.Show("Producto modificado exitosamente.");
                }
                else
                {
                    negocio.agregarProducto(producto);
                    MessageBox.Show("Producto agregado exitosamente.");
                }

                if (archivo != null && !(txtUrlAlta.Text.ToLower().Contains("http")))
                {
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["img-folder"] + archivo.SafeFileName);
                }

                Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarCarga()
        {

            if (txtCodigo.Text == "")
            {
                MessageBox.Show("Por favor ingrese el campo código.");
                return true;
            }
            if(txtNombre.Text == "")
            {
                MessageBox.Show("Por favor ingrese el campo nombre.");
                return true;
            }
            if (txtPrecio.Text == "")
            {
                MessageBox.Show("Ingrese el campo precio por favor");
                return true;
            }
            if (!(soloNumeros(txtPrecio.Text)))
            {
                MessageBox.Show("En el campo precio solo se pueden ingresar numeros.");
                return true;
            }
            

            return false;
        }

        private bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }


        private void frmAltaProducto_Load(object sender, EventArgs e)
        {

            CategoriaNegocio categoriaNegocio = new CategoriaNegocio();
            MarcaNegocio marcaNegocio = new MarcaNegocio();

            try
            {

                cboCategoria.DataSource = categoriaNegocio.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";
                cboMarca.DataSource = marcaNegocio.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";

                if (producto != null)
                {
                    
                    
                    txtCodigo.Text = producto.Codigo;
                    txtNombre.Text = producto.Nombre;
                    txtDescripcion.Text = producto.Descripcion;
                    txtUrlAlta.Text = producto.ImagenUrl;
                    txtPrecio.Text = producto.Precio.ToString();
                    cargarImagen(producto.ImagenUrl);
                    cboCategoria.SelectedValue = producto.CategoriaProducto.Id;
                    cboMarca.SelectedValue = producto.MarcaProducto.Id;

                }

            }
            catch (Exception ex) 
            {

                throw ex;
            }

        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlAlta.Text);
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

        private void btnImg_Click(object sender, EventArgs e)
        {

            archivo = new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg|png|*.png";
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlAlta.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
            }

        }
    }
}
