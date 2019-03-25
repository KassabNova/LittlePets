using LittlePets.Entidades;
using LittlePets.Helpers.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LittlePets.Controllers
{
    public class PrincipalController : Controller
    {
        // GET: Principal
        public ActionResult Index()
        {
           
            ViewBag.Title = "Mascotas";
            ViewBag.Opcion = "Registradas";
            List<Mascota> mascota = new List<Mascota>();
            mascota = DALittlePets.ObtenerMascota();
            return View("Mascota", mascota);
        }
        public ActionResult Cliente()
        {

            ViewBag.Title = "Clientes";
            List<Cliente> cliente = DALittlePets.ObtenerCliente();
            return View("Cliente", cliente);
        }
        public ActionResult ObtenerMascotaDetalle(string idmascota)
        {
            int idMascota = Convert.ToInt32(idmascota);
            MascotaDetalle mascota = new MascotaDetalle();
            mascota = DALittlePets.ObtenerMascotaDetalle(idMascota);
            return View("MascotaDetalle", mascota);
        }
        public ActionResult ConfirmarEliminar()
        {
            return PartialView("_Confirmar");
        }
        public ActionResult EliminarMascota(int idmascota)
        {
            ViewBag.Title = "Mascotas";
            ViewBag.Opcion = "Registradas";
            DALittlePets.EliminarMascota(idmascota);
            List<Mascota> mascota = new List<Mascota>();
            mascota = DALittlePets.ObtenerMascota();
            return View("Mascota", mascota);
        }
        public ActionResult BuscarMascota(string nombre)
        {
            ViewBag.Title = "Resultados";
            ViewBag.Opcion = "Encontradas";
            List<Mascota> mascota = new List<Mascota>();
            mascota = DALittlePets.BuscarMascota(nombre);
            return View("Mascota", mascota);
        }
        public ActionResult EditarMascota(string idmascota, string color, float peso)
        {
            int Idmascota = Convert.ToInt32(idmascota);
            DALittlePets.EditarMascota(Idmascota, color, peso);

            ViewBag.Title = "Mascotas";
            ViewBag.Opcion = "Registradas";
            List<Mascota> mascota = new List<Mascota>();
            mascota = DALittlePets.ObtenerMascota();
            return View("Mascota", mascota);
        }
        public ActionResult AgregarMascota()
        {
            return View("AgregarMascota");
        }
        public ActionResult AgregarMascotaDB(string duenio, string nombre, int cuidador, string especie, string raza, string color, float peso, string sexo, string nacimiento)
        {
            ViewBag.Title = "Mascotas";
            ViewBag.Opcion = "Registradas";
            DALittlePets.AgregarMascota(duenio, nombre, cuidador, especie, raza, color, peso, sexo, nacimiento);
            List<Mascota> mascota = new List<Mascota>();
            mascota = DALittlePets.ObtenerMascota();
            return View("Mascota", mascota);

        }
        public ActionResult EliminarCliente(int idcliente)
        {
            ViewBag.Title = "Clientes";
            DALittlePets.EliminarCliente(idcliente);
            List<Cliente> cliente = DALittlePets.ObtenerCliente();
            return View("Cliente", cliente);
        }
        public ActionResult AgregarCliente(string nombre, string direccion, string telefono, string email)
        {
            ViewBag.Title = "Clientes";
            DALittlePets.AgregarCliente(nombre, direccion, telefono, email);
            List<Cliente> cliente = DALittlePets.ObtenerCliente();
            return View("Cliente", cliente);
        }
    }
}