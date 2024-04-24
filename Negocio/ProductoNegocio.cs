using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class ProductoNegocio
    {

        public List<Producto> listar()
        {
            List<Producto> lista = new List<Producto>();
            AccesoDatos datos = new AccesoDatos();
            try
            {

                datos.setearConsulta("select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, M.Descripcion as Marca ,C.Descripcion as Categoria, A.IdCategoria, A.IdMarca from ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id = A.IdCategoria and M.Id = A.IdMarca");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Producto producto = new Producto();
                    producto.Id = (int)datos.Lector["Id"];
                    producto.Codigo = (string)datos.Lector["Codigo"];
                    producto.Nombre = (string)datos.Lector["Nombre"];
                    producto.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        producto.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    if (!(datos.Lector["Precio"] is DBNull))
                        producto.Precio = (decimal)datos.Lector["Precio"];
                    producto.CategoriaProducto = new Categoria();
                    producto.CategoriaProducto.Id = (int)datos.Lector["IdCategoria"];
                    producto.CategoriaProducto.Descripcion = (string)datos.Lector["Categoria"];
                    producto.MarcaProducto = new Marca();
                    producto.MarcaProducto.Id = (int)datos.Lector["IdMarca"];
                    producto.MarcaProducto.Descripcion = (string)datos.Lector["Marca"];
                    
                    lista.Add(producto);
                }

                
                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }


        }

        public void agregarProducto(Producto producto)
        {
            AccesoDatos datos = new AccesoDatos();


            try
            {

                datos.setearConsulta("Insert into ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdCategoria, IdMarca, ImagenUrl)values('" + producto.Codigo + "','"+ producto.Nombre +"','" + producto.Descripcion + "','" + producto.Precio + "', @IdCategoria, @IdMarca, @ImagenUrl)");
                datos.setearParametro("@IdCategoria", producto.CategoriaProducto.Id);
                datos.setearParametro("@IdMarca", producto.MarcaProducto.Id);
                datos.setearParametro("@ImagenUrl", producto.ImagenUrl);
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public void modificarProducto(Producto producto)
        {

            AccesoDatos datos = new AccesoDatos();

            try
            {

                datos.setearConsulta("Update ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, ImagenUrl = @ImagenUrl, Precio = @Precio, IdCategoria = @IdCategoria, IdMarca = @IdMarca where id = @Id");
                datos.setearParametro("@Codigo", producto.Codigo);
                datos.setearParametro("@Nombre", producto.Nombre);
                datos.setearParametro("@Descripcion", producto.Descripcion);
                datos.setearParametro("@Imagenurl", producto.ImagenUrl);
                datos.setearParametro("@Precio", producto.Precio);
                datos.setearParametro("@IdCategoria", producto.CategoriaProducto.Id);
                datos.setearParametro("@IdMarca", producto.MarcaProducto.Id);
                datos.setearParametro("@Id", producto.Id);

                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void eliminarProducto(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();

                datos.setearConsulta("Delete from ARTICULOS where id = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
                

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
