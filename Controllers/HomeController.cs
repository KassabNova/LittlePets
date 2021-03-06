﻿using LittlePets.Entidades;
using System;
using System.IO;
using System.Collections.Generic;
using LittlePets.Helpers.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LittlePets.Controllers
{
    public class HomeController : Controller
    {
        // Pagina principal
        public ActionResult Index()
        {
            List<TablaPrincipal> tablaprincipal = null;
            //tablaprincipal = DALittlePets.ObtenerTablaPrincipal();

            return View("Index", tablaprincipal);
        }
        public ActionResult Productos()
        {
            List<Producto> productos = new List<Producto>();
            ViewBag.Title = "Productos LittlePets";
            ViewBag.Message = "Inventario";
            productos = DALittlePets.ObtenerProductos();
            if (productos == null)
            {
                productos = new List<Producto>();
                Producto vacio = new Producto();
                vacio.IdProducto = 123;
                vacio.Nombre = "Vacio";
                vacio.Precio = 123;
                vacio.Cantidad = 9;
                productos.Add(vacio);
            }
            return PartialView("Productos", productos);
        }
        public ActionResult ActualizarInventario(string idProducto, string cantidad, string inventario, string accion)
        {
            int total = 0;
            if (accion == "inventario")
            {
                total = Int32.Parse(inventario) + Int32.Parse(cantidad);
            }
            else
            {
                total = Int32.Parse(inventario) - Int32.Parse(cantidad);

            }

            string data;
            try
            {

                DALittlePets.ActualizarInventario(Int32.Parse(idProducto), total);
                return Json(new { title = "¡Actualización exitosa!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { title = "Error", data = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ActualizarProductos()
        {
            List<Producto> productos = new List<Producto>();
            productos = DALittlePets.ObtenerProductos();
            if (productos == null)
            {
                productos = new List<Producto>();
                Producto vacio = new Producto();
                vacio.IdProducto = 123;
                vacio.Nombre = "Vacio";
                vacio.Precio = 123;
                vacio.Cantidad = 9;
                productos.Add(vacio);
            }
            return Json(new { data = RenderRazorViewToString("~/Views/Home/_ProductosParciales.cshtml", productos) }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Analisis()
        {
            List<Analisis> analisis = new List<Analisis>();
            analisis = DALittlePets.ObtenerAnalisis();
            if (analisis == null)
            {
                CentroEstudio centro = new CentroEstudio();

                analisis = new List<Analisis>();
                Analisis vacio = new Analisis();
                vacio.IdAnalisis = 123;
                vacio.Nombre = "Vacio";
                vacio.Precio = 0;
                vacio.Centro = "VACIO";
                vacio.Domicilio = "VACIO";
                vacio.Telefono = "VACIO";
                analisis.Add(vacio);
            }

            return View(analisis);
        }
        private string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}