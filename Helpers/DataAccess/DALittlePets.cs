using LittlePets.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LittlePets.Helpers.DataAccess
{
    public class DALittlePets
    {

        private static string CONEXION_KASSAB = "Data Source = localhost; Initial Catalog = LittlePets; Integrated Security = SSPI";
        public static string CONEXION_LUIS = "Data Source = (LocalDB)\\Prueba; Initial Catalog = LittlePets; Integrated Security = SSPI";
        public static List<Mascota> ObtenerMascota()
        {
            List<Mascota> mascota = null;

            try
            {
                SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);
                SqlCommand comando = new SqlCommand("Select * From TablaMascota", conexion);
                conexion.Open();
                SqlDataReader dr = comando.ExecuteReader();
                if (dr.HasRows)
                {
                    mascota = new List<Mascota>();
                    while (dr.Read())
                    {
                        Mascota m = new Mascota();
                        m.IdMascota = Convert.ToInt32(dr["IdMascota"]);
                        m.Dueño = Convert.ToString(dr["NombreDueño"]);
                        m.Nombre = Convert.ToString(dr["NombreMascota"]);
                        m.Especie = Convert.ToString(dr["Especie"]);
                        m.Raza = Convert.ToString(dr["Raza"]);
                        mascota.Add(m);
                    }
                    mascota.TrimExcess();
                }
                conexion.Close();
            }
            catch
            {
            }

            return mascota;
        }
        public static MascotaDetalle ObtenerMascotaDetalle(int idmascota)
        {
            MascotaDetalle mascota = null;
            try
            {
                SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);
                using (conexion)
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand("spObtenerMascotaDetalle", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@IdMascota", idmascota);
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            if (lector.HasRows)
                            {
                                mascota = new MascotaDetalle();
                                if (lector.Read())
                                {
                                    mascota.IdMascota = Convert.ToInt32(lector["IdMascota"]);
                                    mascota.Dueño = Convert.ToString(lector["NombreDueño"]);
                                    mascota.Cuidador = Convert.ToString(lector["NombreCuidador"]);
                                    mascota.Nombre = Convert.ToString(lector["Nombre"]);
                                    mascota.Especie = Convert.ToString(lector["Especie"]);
                                    mascota.Raza = Convert.ToString(lector["Raza"]);
                                    mascota.Color = Convert.ToString(lector["Color"]);
                                    mascota.Peso = Convert.ToDecimal(lector["Peso"]);
                                    mascota.Sexo = Convert.ToString(lector["Sexo"]);
                                    mascota.Edad = Convert.ToInt32(lector["Edad"]);
                                }
                            }
                        }
                    }
                    conexion.Close();
                }
            }
            catch
            {
                throw;
            }
            return mascota;
        }
        public static void EliminarMascota(int idmascota)
        {
            try
            {
                SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);
                using (conexion)
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand("DELETE FROM tblMascota WHERE IdMascota = @IdMascota", conexion))
                    {
                        comando.Parameters.Add(new SqlParameter("@IdMascota", idmascota));
                        comando.ExecuteReader();
                    }
                }
                conexion.Close();
            }
            catch
            {
                throw;
            }
        }
        public static void EditarMascota(int idmascota, string color, float peso)
        {
            SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);

            string query = "update tblMascota SET Peso = @Peso, Color = @Color  where  IdMascota = @IdMascota";
            try
            {
                using (conexion)
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {

                        comando.CommandTimeout = 600;
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.AddWithValue("@IdMascota", idmascota);
                        comando.Parameters.AddWithValue("@Peso", peso);
                        comando.Parameters.AddWithValue("@Color", color);

                        conexion.Open();
                        comando.ExecuteReader();
                        conexion.Close();
                    }
                }



            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Error interno. Por favor, inténtelo más tarde."));
            }
            finally
            {
                if (conexion != null && conexion.State != ConnectionState.Closed)
                {
                    conexion.Close();
                }
            }

        }
        public static List<Mascota> BuscarMascota(string nombre)
        {
            List<Mascota> mascota = null;
            try
            {
                SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);
                using (conexion)
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand("spBuscarMascota", conexion))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@Nombre", nombre);
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            if (lector.HasRows)
                            {
                                mascota = new List<Mascota>();
                                while (lector.Read())
                                {
                                    Mascota m = new Mascota();
                                    m.IdMascota = Convert.ToInt32(lector["IdMascota"]);
                                    m.Dueño = Convert.ToString(lector["NombreDueño"]);
                                    m.Nombre = Convert.ToString(lector["NombreMascota"]);
                                    m.Especie = Convert.ToString(lector["Especie"]);
                                    m.Raza = Convert.ToString(lector["Raza"]);
                                    mascota.Add(m);
                                }
                            }
                        }
                    }
                }
                conexion.Close();
            }
            catch
            {
                throw;
            }
            return mascota;
        }
        public static List<Analisis> ObtenerAnalisis()
        {
            List<Analisis> analisis = new List<Analisis>();
            CentroEstudio centro = new CentroEstudio();

            SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);

            string query = "SELECT A.IdAnalisis,A.Nombre AS Analisis, A.Precio, C.Nombre AS Centro, C.Domicilio, C.Telefono FROM tblAnalisis A " +
                           "JOIN tblAnalisisCentro AC ON A.IdAnalisis = AC.IdAnalisis " +
                           "JOIN tblCentroEstudio C ON C.IdCentro = AC.IdCentro";
            try
            {
                using (conexion)
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {

                        //comando.CommandTimeout = 600;
                        comando.CommandType = CommandType.Text;
                        conexion.Open();
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            if (lector.HasRows)
                            {
                                while (lector.Read())
                                {


                                    analisis.Add(new Analisis()
                                    {

                                        IdAnalisis = Int32.Parse(lector["IdAnalisis"].ToString()),
                                        Nombre = lector["Analisis"].ToString(),
                                        Precio = Double.Parse(lector["Precio"].ToString()),
                                        Centro = lector["Centro"].ToString(),
                                        Domicilio = lector["Domicilio"].ToString(),
                                        Telefono = lector["Telefono"].ToString(),

                                    });

                                }
                            }
                            conexion.Close();
                        }
                    }
                }

            }
            catch
            {
                analisis = null;
            }
            finally
            {
                if (conexion != null && conexion.State != ConnectionState.Closed)
                {
                    conexion.Close();
                }
            }

            return analisis;
        }
        public static List<Producto> ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>();
            SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);

            string query = "select * from tblProducto order by Nombre";
            try
            {
                using (conexion)
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {

                        //comando.CommandTimeout = 600;
                        comando.CommandType = CommandType.Text;
                        //comando.Parameters.AddWithValue("@IdMensaje", idMensaje);
                        conexion.Open();
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {
                            if (lector.HasRows)
                            {
                                while (lector.Read())
                                {

                                    productos.Add(new Producto()
                                    {

                                        IdProducto = Int32.Parse(lector["IdProducto"].ToString()),
                                        Nombre = lector["Nombre"].ToString(),
                                        Cantidad = Int32.Parse(lector["Cantidad"].ToString()),
                                        Precio = Double.Parse(lector["Precio"].ToString())
                                    });

                                }
                            }
                            conexion.Close();
                        }
                    }
                }

            }
            catch
            {
                productos = null;
            }
            finally
            {
                if (conexion != null && conexion.State != ConnectionState.Closed)
                {
                    conexion.Close();
                }
            }

            return productos;
        }
        public static void ActualizarInventario(int idProducto, int cantidad)
        {


            SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);

            string query = "update tblProducto SET Cantidad = @Cantidad  where  IdProducto = @idProducto";
            try
            {
                using (conexion)
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {

                        comando.CommandTimeout = 600;
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.AddWithValue("@IdProducto", idProducto);
                        comando.Parameters.AddWithValue("@Cantidad", cantidad);

                        conexion.Open();
                        using (SqlDataReader lector = comando.ExecuteReader())
                        {

                            conexion.Close();
                        }
                    }
                }



            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Error interno. Por favor, inténtelo más tarde."));
            }
            finally
            {
                if (conexion != null && conexion.State != ConnectionState.Closed)
                {
                    conexion.Close();
                }
            }
        }
        //public static List<TablaPrincipal> ObtenerTablaPrincipal()
        //{
        //    List<TablaPrincipal> tablaprincipal = null;

        //    string query = "SELECT * FROM GraficaMascotas";
        //    try
        //    {
        //        SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);
        //        using (conexion)
        //        {
        //            using (SqlCommand comando = new SqlCommand(query, conexion))
        //            {
        //                conexion.Open();
        //                using (SqlDataReader lector = comando.ExecuteReader())
        //                {
        //                    if (lector.HasRows)
        //                    {
        //                        tablaprincipal = new List<TablaPrincipal>();
        //                        while (lector.Read())
        //                        {
        //                            TablaPrincipal t = new TablaPrincipal();
        //                            t.Especie = Convert.ToString(lector["Especie"]);
        //                            t.Cantidad = Convert.ToInt32(lector["Numero"]);
        //                            tablaprincipal.Add(t);
        //                        }
        //                    }
        //                    conexion.Close();
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }



        //    return tablaprincipal;
        //}
        public static void AgregarMascota(string duenio, string nombre, int cuidador, string especie, string raza, string color, float peso, string sexo, string nacimiento)
        {
            string query = "INSERT INTO tblMascota(IdCliente, IdEmpleado, Nombre, Especie, Raza, Color, Peso, Sexo, FNacimiento) VALUES((SELECT TOP 1 c.IdCliente FROM tblCliente c WHERE c.Nombre like '%' + @NombreCliente + '%'), @IdEmpleado, @Nombre, @Especie, @Raza, @Color, @Peso, @Sexo, convert(datetime, @FNacimiento, 20))";
            try
            {
                SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);
                using (conexion)
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.CommandTimeout = 600;
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.AddWithValue("@NombreCliente", duenio);
                        comando.Parameters.AddWithValue("@Nombre", nombre);
                        comando.Parameters.AddWithValue("@IdEmpleado", cuidador);
                        comando.Parameters.AddWithValue("@Especie", especie);
                        comando.Parameters.AddWithValue("@Raza", raza);
                        comando.Parameters.AddWithValue("@Color", color);
                        comando.Parameters.AddWithValue("@Peso", peso);
                        comando.Parameters.AddWithValue("@Sexo", sexo);
                        comando.Parameters.AddWithValue("@FNacimiento", nacimiento);

                        conexion.Open();
                        comando.ExecuteReader();
                        conexion.Close();
                    }
                }
            }
            catch
            {
            }
        }
        public static List<Cliente> ObtenerCliente()
        {
            List<Cliente> cliente = null;
            try
            {
                SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);
                SqlCommand comando = new SqlCommand("SELECT * FROM tblCliente", conexion);
                conexion.Open();
                SqlDataReader dr = comando.ExecuteReader();
                if (dr.HasRows)
                {
                    cliente = new List<Cliente>();
                    while (dr.Read())
                    {
                        Cliente c = new Cliente();
                        c.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                        c.NombreCliente = Convert.ToString(dr["Nombre"]);
                        c.Telefono = Convert.ToString(dr["Teléfono"]);
                        c.Correo = Convert.ToString(dr["Correo"]);

                        cliente.Add(c);
                    }
                    cliente.TrimExcess();
                }
                conexion.Close();
            }
            catch
            {
            }

            return cliente;
        }
        public static void EliminarCliente(int idcliente)
        {
            try
            {
                SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);
                using (conexion)
                {
                    conexion.Open();
                    using (SqlCommand comando = new SqlCommand("DELETE FROM tblCliente WHERE IdCliente = @IdCliente", conexion))
                    {
                        comando.Parameters.Add(new SqlParameter("@IdCliente", idcliente));
                        comando.ExecuteReader();
                    }
                }
                conexion.Close();
            }
            catch
            {
                throw;
            }
        }
        public static void AgregarCliente(string nombre, string direccion, string telefono, string email)
        {
            string query = "INSERT INTO tblCliente(Nombre, Domicilio, Teléfono, Correo) VALUES(@Nombre, @Direccion, @Telefono, @Correo)";
            try
            {
                SqlConnection conexion = new SqlConnection(CONEXION_KASSAB);
                using (conexion)
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.CommandTimeout = 600;
                        comando.CommandType = CommandType.Text;
                        comando.Parameters.AddWithValue("@Nombre", nombre);
                        comando.Parameters.AddWithValue("@Direccion", direccion);
                        comando.Parameters.AddWithValue("@Telefono", telefono);
                        comando.Parameters.AddWithValue("@Correo", email);

                        conexion.Open();
                        comando.ExecuteReader();
                        conexion.Close();
                    }
                }
            }
            catch
            {
            }
        }
    }
}