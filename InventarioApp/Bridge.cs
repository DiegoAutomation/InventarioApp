////using System;
////using System.Collections.Generic;
////using System.Text;

////namespace InventarioApp
////{
////    internal class Bridge
////    {
////    }
////}
//using System;
//using System.Runtime.InteropServices;
//using System.Text.Json;

//namespace InventarioApp
//{
//    [ClassInterface(ClassInterfaceType.AutoDispatch)]
//    [ComVisible(true)]
//    public class Bridge
//    {
//        private SQL db = new SQL();

//        public string ObtenerMateriales()
//        {
//            return JsonSerializer.Serialize(db.ObtenerTodosLosMateriales());
//        }

//        public string AgregarMaterial(string np, string desc, int cant, string proyecto, string equipo, string marca, string usuario)
//        {
//            try
//            {
//                var mat = new Material { NumeroParte = np, Descripcion = desc, Cantidad = cant, Marca = marca };
//                db.GuardarMaterialMultiplo(mat, equipo, proyecto, usuario);
//                return "OK";
//            }
//            catch (Exception ex)
//            {
//                return "ERROR: " + ex.Message;
//            }
//        }

//        public string RetirarMaterial(int id, int cantidad, string usuario)
//        {
//            try
//            {
//                db.RegistrarRetiro(id, cantidad, usuario);
//                return "OK";
//            }
//            catch (Exception ex)
//            {
//                return "ERROR: " + ex.Message;
//            }
//        }
//        //public string ActualizarMaterial(int id, string descripcion, int cantidad, string proyecto, string equipo, string marca, string usuario)
//        //{
//        //    try
//        //    {
//        //        db.ActualizarMaterial(id, descripcion, cantidad, proyecto, equipo, marca, usuario);
//        //        return "OK";
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        return "ERROR: " + ex.Message;
//        //    }
//        //}
//        public string EliminarMaterial(int id, string usuario)
//        {
//            try
//            {
//                db.EliminarMaterial(id, usuario);
//                return "OK";
//            }
//            catch (Exception ex)
//            {
//                return "ERROR: " + ex.Message;
//            }
//        }

//        public string ObtenerHistorial()
//        {
//            return JsonSerializer.Serialize(db.ObtenerHistorialMovimientos());
//        }

//        public string ObtenerEstadisticas()
//        {
//            return JsonSerializer.Serialize(db.ObtenerEstadisticas());
//        }
//        // ===== USUARIOS =====

//        public string ActualizarMaterial(int id, string descripcion, int cantidad, string proyecto, string equipo, string marca, string usuario)
//        {
//            try
//            {
//                SQL.ActualizarMaterial(id, descripcion, cantidad, proyecto, equipo, marca, usuario);
//                return "OK";
//            }
//            catch (Exception ex)
//            {
//                return "ERROR:" + ex.Message;
//            }
//        }

//        public string ValidarUsuario(string nombre, string contrasena)
//        {
//            try
//            {
//                bool valido = SQL.ValidarUsuario(nombre, contrasena);
//                if (valido)
//                {
//                    var usuario = SQL.ObtenerUsuarioPorNombre(nombre);
//                    if (usuario != null)
//                    {
//                        return usuario.Id + "|" + usuario.Nombre + "|" + usuario.Rol;
//                    }
//                    return "ERROR:Usuario no encontrado";
//                }
//                else
//                {
//                    return "ERROR:Usuario o contraseña incorrectos";
//                }
//            }
//            catch (Exception ex)
//            {
//                return "ERROR:" + ex.Message;
//            }
//        }

//        public string CrearUsuario(string nombre, string contrasena, string rol)
//        {
//            try
//            {
//                SQL.CrearUsuario(nombre, contrasena, rol);
//                return "OK";
//            }
//            catch (Exception ex)
//            {
//                return "ERROR:" + ex.Message;
//            }
//        }

//        public void InicializarUsuarios()
//        {
//            try
//            {
//                SQL.CrearTablaUsuarios();
//            }
//            catch (Exception ex)
//            {
//                System.Windows.Forms.MessageBox.Show("Error inicializando usuarios: " + ex.Message);
//            }
//        }
//    }
//}
using System;

namespace InventarioApp
{
    public class Bridge
    {
        private SQL SQL;

        public Bridge()
        {
            SQL = new SQL();
        }

        // Métodos existentes que ya tienes...
        public string ObtenerMateriales()
        {
            try
            {
                var materiales = SQL.ObtenerTodosLosMateriales();
                // Convertir a JSON manualmente
                var json = "[";
                foreach (var m in materiales)
                {
                    json += $"{{\"Id\":{m.Id},\"NumeroParte\":\"{m.NumeroParte}\",\"Descripcion\":\"{m.Descripcion}\",\"Cantidad\":{m.Cantidad},\"Proyecto\":\"{m.Proyecto}\",\"Equipo\":\"{m.Equipo}\",\"Marca\":\"{m.Marca}\",\"Cambio\":\"{m.Cambio}\"}},";
                }
                json = json.TrimEnd(',') + "]";
                return json;
            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message;
            }
        }

        public string ObtenerHistorial()
        {
            try
            {
                var historial = SQL.ObtenerHistorialMovimientos();
                var json = "[";
                foreach (var h in historial)
                {
                    json += $"{{\"NumeroParte\":\"{h.NumeroParte}\",\"Cantidad\":{h.Cantidad},\"Fecha\":\"{h.Fecha}\",\"Tipo\":\"{h.Tipo}\",\"Usuario\":\"{h.Usuario}\"}},";
                }
                json = json.TrimEnd(',') + "]";
                return json;
            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message;
            }
        }

        public string AgregarMaterial(string numeroParte, string descripcion, int cantidad, string proyecto, string equipo, string marca, string usuario)
        {
            try
            {
                var material = new Material
                {
                    NumeroParte = numeroParte,
                    Descripcion = descripcion,
                    Cantidad = cantidad,
                    Marca = marca
                };
                SQL.GuardarMaterialMultiplo(material, equipo, proyecto, usuario);
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message;
            }
        }

        public string RetirarMaterial(int id, int cantidad, string usuario)
        {
            try
            {
                SQL.RegistrarRetiro(id, cantidad, usuario);
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message;
            }
        }

        public string EliminarMaterial(int id, string usuario)
        {
            try
            {
                SQL.EliminarMaterial(id, usuario);
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message;
            }
        }

        // ===== NUEVOS MÉTODOS =====

        public string ActualizarMaterial(int id, string descripcion, int cantidad, string proyecto, string equipo, string marca, string usuario)
        {
            try
            {
                SQL.ActualizarMaterial(id, descripcion, cantidad, proyecto, equipo, marca, usuario);
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message;
            }
        }

        public string ValidarUsuario(string nombre, string contrasena)
        {
            try
            {
                bool valido = SQL.ValidarUsuario(nombre, contrasena);
                if (valido)
                {
                    var usuario = SQL.ObtenerUsuarioPorNombre(nombre);
                    if (usuario != null)
                    {
                        return usuario.Id + "|" + usuario.Nombre + "|" + usuario.Rol;
                    }
                    return "ERROR:Usuario no encontrado";
                }
                else
                {
                    return "ERROR:Usuario o contraseña incorrectos";
                }
            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message;
            }
        }

        public string CrearUsuario(string nombre, string contrasena, string rol)
        {
            try
            {
                SQL.CrearUsuario(nombre, contrasena, rol);
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERROR:" + ex.Message;
            }
        }

        public void InicializarUsuarios()
        {
            try
            {
                SQL.CrearTablaUsuarios();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error inicializando usuarios: " + ex.Message);
            }
        }
    }
}