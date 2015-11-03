using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.electoral_common.context;
using mx.gob.scjn.ius_common.TO;
using System.Data.Common;
using System.Data;
using mx.gob.scjn.electoral_common.utils;
using System.Net.Mail;
using System.Diagnostics;
using System.Net;

namespace mx.gob.scjn.electoral_common.DAO.impl
{
    class GuardarExpresionDAOImpl:GuardarExpresionDAO
    {
        /// <summary>
        /// El manejadro de contexto para la BD
        /// </summary>
        public DBContext contextoBD;

        private IUSApplicationContext contexto { get; set; }
        /// <summary>
        /// Constructor por omisión
        /// </summary>
        public GuardarExpresionDAOImpl()
        {
            contexto = new IUSApplicationContext();
            contextoBD = (DBContext)contexto.getInitialContext().GetObject("dataSource");
        }
        #region GuardarExpresionDAO Members
        /// <summary>
        /// Obtiene el usuario
        /// </summary>
        /// <param name="usuario">nombre del usuario</param>
        /// <param name="Passwd">Password a validar</param>
        /// <returns>Los datos del usuario.</returns>
        public UsuarioTO ObtenUsuario(string usuario, string Passwd)
        {
            try
            {
                DbConnection conexion = contextoBD.RegistroUsuariosConnection;
                DataAdapter query = contextoBD.RegistroDataAdapter("Select usuario, passwd, Nombre, Apellidos, Enviar " +
                    "from Usuarios where Usuario = '" + usuario + "' AND passwd = '" + Passwd + "'", conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<UsuarioTO> lista = new List<UsuarioTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    UsuarioTO usuarioActual = new UsuarioTO();
                    usuarioActual.Nombre = "" + fila["Nombre"];
                    usuarioActual.Apellidos = "" + fila["Apellidos"];
                    usuarioActual.Enviar = (bool)fila["Enviar"];
                    usuarioActual.Passwd = "" + fila["Passwd"];
                    usuarioActual.Usuario = "" + fila["Usuario"];
                    lista.Add(usuarioActual);
                }
                conexion.Close();
                return lista.ElementAt(0);
            }
            catch (Exception e)
            {
                return new UsuarioTO();
            }
        }
        /// <summary>
        /// Obtiene el usuario
        /// </summary>
        /// <param name="usuario">nombre del usuario</param>
        /// <returns>Los datos del usuario.</returns>
        public UsuarioTO ObtenUsuario(string usuario)
        {
            try
            {
                DbConnection conexion = contextoBD.RegistroUsuariosConnection;
                DataAdapter query = contextoBD.RegistroDataAdapter("Select usuario, passwd, Nombre, Apellidos, Enviar " +
                    "from Usuarios where Usuario = '" + usuario + "'", conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<UsuarioTO> lista = new List<UsuarioTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {
                    UsuarioTO usuarioActual = new UsuarioTO();
                    usuarioActual.Nombre = "" + fila["Nombre"];
                    usuarioActual.Apellidos = "" + fila["Apellidos"];
                    usuarioActual.Enviar = (bool)fila["Enviar"];
                    usuarioActual.Passwd = "" + fila["Passwd"];
                    usuarioActual.Usuario = "" + fila["Usuario"];
                    lista.Add(usuarioActual);
                }
                conexion.Close();
                return lista.ElementAt(0);

            }
            catch (Exception e)
            {
                return new UsuarioTO();
            }
        }
        /// <summary>
        /// Obtiene todas las búsquedas de un usuario.
        /// </summary>
        /// <param name="usuario">El usuario del que se quiere las búsquedas</param>
        /// <returns>La lista de las búsquedas.</returns>
        public List<BusquedaAlmacenadaTO> ObtenBusquedas(string usuario)
        {
            try
            {
                DbConnection conexion = contextoBD.RegistroUsuariosConnection;
                DataAdapter query = contextoBD.RegistroDataAdapter("Select tipo_busqueda, usuario, nombre, valorBusqueda, id, expresion " +
                    "from busqueda where id>0 and Usuario = '" + usuario + "'", conexion);
                DataSet datos = new DataSet();
                query.Fill(datos);
                List<BusquedaAlmacenadaTO> lista = new List<BusquedaAlmacenadaTO>();
                DataTable tabla = datos.Tables[0];
                foreach (DataRow fila in tabla.Rows)
                {BusquedaAlmacenadaTO  busquedaActual = new BusquedaAlmacenadaTO();
                    busquedaActual.id = (int) fila["id"];
                    busquedaActual.Nombre = "" + fila["Nombre"];
                    busquedaActual.TipoBusqueda = (int)fila["tipo_busqueda"];
                    busquedaActual.ValorBusqueda = "" + fila["valorBusqueda"];
                    busquedaActual.Expresion = "" + fila["expresion"];
                    //busquedaActual.Usuario = usuario;
                    lista.Add(busquedaActual);
                }
                foreach (BusquedaAlmacenadaTO busqueda in lista)
                {
                    query = contextoBD.RegistroDataAdapter("Select Epoca from busquedaEpocas where id_busqueda = " + busqueda.id, conexion);
                    datos = new DataSet();
                    query.Fill(datos);
                    tabla = datos.Tables[0];
                    int[] epocas = new int[tabla.Rows.Count];
                    int contador = 0;
                    foreach (DataRow fila in tabla.Rows)
                    {
                        epocas[contador] = (int)fila["Epoca"];
                        contador++;
                    }
                    query = contextoBD.RegistroDataAdapter("Select Registro from busquedaRegistro where idBusqueda = " + busqueda.id, conexion);
                    datos = new DataSet();
                    query.Fill(datos);
                    tabla = datos.Tables[0];
                    int[] registro = new int[tabla.Rows.Count];
                    contador = 0;
                    foreach (DataRow fila in tabla.Rows)
                    {
                        registro[contador] = (int)fila["Registro"];
                    }
                    query = contextoBD.RegistroDataAdapter("Select Expresion, Operador, IsJuris, idExpresionBusqueda from ExpresionBusqueda where idBusqueda = " + busqueda.id, conexion);
                    datos = new DataSet();
                    query.Fill(datos);
                    int ExpresionActual = 0;
                    tabla = datos.Tables[0];
                    List<ExpresionBusqueda> expresiones = new List<ExpresionBusqueda>();
                    foreach (DataRow fila in tabla.Rows)
                    {
                        ExpresionBusqueda expresion = new ExpresionBusqueda();
                        expresion.Expresion = ""+fila["Expresion"];
                        expresion.Operador = (int)fila["Operador"];
                        expresion.IsJuris = (int)fila["IsJuris"];
                        ExpresionActual = (int)fila["IdExpresionBusqueda"];
                        DataAdapter expresionesCampos = contextoBD.RegistroDataAdapter("Select IdExpresionBusqueda, Campo from CampoExpresion where idExpresionBusqueda =" + ExpresionActual, conexion);
                        DataSet expresionesDataSet = new DataSet();
                        expresionesCampos.Fill(expresionesDataSet);
                        expresion.Campos = new List<int>();
                        foreach (DataRow campoActual in expresionesDataSet.Tables[0].Rows)
                        {
                            expresion.Campos.Add((int)campoActual["campo"]);
                        }
                        expresiones.Add(expresion);
                    }
                    List<FiltrosTO> filtros = new List<FiltrosTO>();
                    query = contextoBD.RegistroDataAdapter("Select idBusqueda, idFiltro, TipoFiltro,"
                                                            + " ValorFiltro, ValorAdicional  from filtros"
                                                            + " Where idBusqueda = "+busqueda.id, conexion);
                    datos = new DataSet();
                    query.Fill(datos);
                    tabla = datos.Tables[0];
                    foreach (DataRow fila in tabla.Rows)
                    {
                        FiltrosTO filtroActual = new FiltrosTO();
                        filtroActual.TipoFiltro=(int)fila["TipoFiltro"];
                        filtroActual.ValorAdicional = (int)fila["ValorAdicional"];
                        filtroActual.ValorFiltro = (int)fila["ValorFiltro"];
                        filtros.Add(filtroActual);
                    }
                    busqueda.Epocas = epocas;
                    busqueda.BusquedaRegistro = registro;
                    busqueda.Expresiones = expresiones;
                    busqueda.Filtros = filtros;
                }
                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                return new List<BusquedaAlmacenadaTO>();
            }
        }
        /// <summary>
        /// Registra un usuario del IUS Internet para que
        /// este pueda utilizar las funcionalidades de las Búsquedas Almacenadas.
        /// </summary>
        /// <param name="usuario">Los datos del usuario</param>
        public void RegistrarUsuario(UsuarioTO usuario)
        {
            try
            {
                DbConnection conexion = contextoBD.RegistroUsuariosConnection;
                String sql = "Insert into Usuarios(Apellidos, Enviar, Nombre, Passwd, Usuario, eMail) values('" +
                    usuario.Apellidos + "', " + usuario.Enviar + ", '" + usuario.Nombre + "', '" +
                    usuario.Passwd + "', '" + usuario.Usuario + "', '" + usuario.Correo + "' )";
                DbCommand command = contextoBD.CommandRegister(sql, conexion);
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                command.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                return;
            }
        }
        /// <summary>
        /// Almacena una búsqueda de un determinado usuario.
        /// </summary>
        /// <param name="busqueda">Los parámetros de la Búsqueda.</param>
        /// <param name="usuario">El usuario que guarda la búsqueda.</param>
        public int RegistrarBusqueda(BusquedaAlmacenadaTO busqueda, string usuario)
        {
            try
            {
                int[] registros = busqueda.BusquedaRegistro;
                int[] epocas = busqueda.Epocas;
                List<ExpresionBusqueda> expresiones = busqueda.Expresiones;
                String nombre = busqueda.Nombre;
                int tipoBusqueda = busqueda.TipoBusqueda;
                String valor=busqueda.ValorBusqueda;
                /*******************************************************************/
                /****** Empezamos guardando la búsqueda y después sus **************/
                /****** componentes.                                  **************/
                /*******************************************************************/
                string sqlId = "select max(id)+1 from busqueda";
                DbConnection conexion = contextoBD.RegistroUsuariosConnection;
                DataAdapter adaptador = contextoBD.RegistroDataAdapter(sqlId, conexion);
                DataSet dataSetId = new DataSet();
                adaptador.Fill(dataSetId);
                DataTable tablaId = dataSetId.Tables[0];
                DataRow resultado = tablaId.Rows[0];
                int Id = (int)resultado[0];
                String sql = "Insert into busqueda(tipo_busqueda, usuario, nombre, valorBusqueda, id, Expresion) values (" +
                    tipoBusqueda + ", '" + usuario + "', '" + nombre + "', '" +
                    valor + "', "+ Id+", '"+EncuentraExpresion(busqueda)+"')";
                DbCommand command = contextoBD.CommandRegister(sql, conexion);
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                command.ExecuteNonQuery();
                /**********************************************************/
                /******* Ahora se guardan los datos de los registros,******/
                /******* epocas y expresiones.                       ******/
                /**********************************************************/
                if ((registros!=null)&&(registros.Length > 0))
                {
                    foreach (int numeroRegistro in registros)
                    {
                        sql = "Insert into BusquedaRegistro(idBusqueda, registro, idRegistro) values " +
                            "( " + Id + ", "  + numeroRegistro + ", select max(idRegistro)+1 from BusquedaRegistro)";
                        command = contextoBD.CommandRegister(sql, conexion);
                        if (command.Connection.State != ConnectionState.Open)
                        {
                            command.Connection.Open();
                        }
                        command.ExecuteNonQuery();
                    }
                }
                if ((epocas != null) && (epocas.Length > 0))
                {
                    foreach (int numeroEpoca in epocas)
                    {
                        sql = "Insert into BusquedaEpocas(id_busqueda, epoca) values " +
                            "( " + Id + ", " + numeroEpoca + ")";
                        command = contextoBD.CommandRegister(sql, conexion);
                        if (command.Connection.State != ConnectionState.Open)
                        {
                            command.Connection.Open();
                        }
                        command.ExecuteNonQuery();
                    }
                }
                if ((expresiones!=null)&&(expresiones.Count > 0))
                {
                    foreach (ExpresionBusqueda item in expresiones)
                    {
                        sqlId = "select max(idExpresionBusqueda)+1 from ExpresionBusqueda";
                        DataAdapter adaptador2 = contextoBD.RegistroDataAdapter(sqlId, conexion);
                        dataSetId = new DataSet();
                        adaptador2.Fill(dataSetId);
                        tablaId = dataSetId.Tables[0];
                        resultado = tablaId.Rows[0];
                        int IdExpr = (int)resultado[0];
                        sql = "Insert into ExpresionBusqueda(idBusqueda, Expresion, "+
                            "operador, IsJuris, idExpresionBusqueda) values " +
                            "( " + Id + ", '" + item.Expresion + "', " +
                            item.Operador +"," + item.IsJuris + "," + IdExpr + " )";
                        command = contextoBD.CommandRegister(sql, conexion);
                        if (command.Connection.State != ConnectionState.Open)
                        {
                            command.Connection.Open();
                        }
                        command.ExecuteNonQuery();
                        foreach (int campo in item.Campos)
                        {
                            sql = "insert into CampoExpresion (idExpresionBusqueda, campo) values " +
                                "( " + IdExpr + ", " + campo + ")";
                            command = contextoBD.CommandRegister(sql, conexion);
                            if (command.Connection.State != ConnectionState.Open)
                            {
                                command.Connection.Open();
                            }
                            command.ExecuteNonQuery();
                        }
                    }
                }
                //if ((busqueda.Filtros != null) && (busqueda.Filtros.Count > 0))
                //{
                //    sqlId = "Select max(idFiltro)+1 from filtros";
                //    dataSetId = new DataSet();
                //    adaptador.Fill(dataSetId);
                //    tablaId = dataSetId.Tables[0];
                //    resultado = tablaId.Rows[0];
                //    int IdExpr = (int)resultado[0];
                //    foreach (FiltrosTO item in busqueda.Filtros)
                //    {
                //        sql = "Insert into Filtros(idBusqueda, idFiltro, TipoFiltro, ValorFiltro, ValorAdicional) values " +
                //                "( " + busqueda.id+ ", '" + IdExpr + "', " + item.TipoFiltro+ "," + item.ValorFiltro+ "," + item.ValorAdicional+ " )";
                //        command = contextoBD.CommandRegister(sql, conexion);
                //        if (command.Connection.State != ConnectionState.Open)
                //        {
                //            command.Connection.Open();
                //        }
                //        command.ExecuteNonQuery();
                //        IdExpr++;
                //    }
                //}
                conexion.Close();
                return IUSConstants.NO_ERROR;
            }
            catch (Exception e)
            {
                return IUSConstants.ERROR_ALMACENA;
            }
        }

        private string EncuentraExpresion(BusquedaAlmacenadaTO busqueda)
        {
            if ((busqueda.TipoBusqueda == IUSConstants.BUSQUEDA_TESIS_TEMATICA))
            {
                int lugar = busqueda.Nombre.IndexOf(": ");
                return "Sin expresiones: "+busqueda.Nombre.Substring(0,lugar);
            }
            if (busqueda.TipoBusqueda == IUSConstants.BUSQUEDA_ESPECIALES)
            {              
                return "Sin expresiones: " + busqueda.Expresion;
            }
            if (busqueda.Expresiones.Count > 1)
            {
                return "Búsqueda con " + busqueda.Expresiones.Count + " expresiones";
            }
            else if (busqueda.Expresiones.Count == 1)
            {
                return busqueda.Expresiones[0].Expresion;
            }
            if (busqueda.TipoBusqueda == IUSConstants.BUSQUEDA_ESPECIALES)
            {
                return "Busquedas Especiales ";
            }
            return "Busqueda irreconocible";
        }
        public void EliminaBusqueda(BusquedaAlmacenadaTO busqueda)
        {
            try
            {
                DbConnection conexion = contextoBD.RegistroUsuariosConnection;
                /*******************************************************************/
                /****** Empezamos borrando los anexos de la busqueda  **************/
                /****** la búsqueda y después sus componentes.        **************/
                /*******************************************************************/
                string sql = "delete from filtros where idBusqueda = " + busqueda.id;
                DataAdapter adaptador = contextoBD.RegistroDataAdapter(sql, conexion);
                DataSet dataSetId = new DataSet();
                adaptador.Fill(dataSetId);
                
                DbCommand command = contextoBD.CommandRegister(sql, conexion);
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                command.ExecuteNonQuery();
                sql = "delete from busquedaEpocas where id_Busqueda = " + busqueda.id;
                command = contextoBD.CommandRegister(sql, conexion);
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                command.ExecuteNonQuery();
                sql = "delete from busquedaRegistro where idBusqueda = " + busqueda.id;
                command = contextoBD.CommandRegister(sql, conexion);
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                command.ExecuteNonQuery();
                /*******************************************************************/
                /*********    Buscamos las expresiones para poder ******************/
                /*********    quitar los campos.                  ******************/
                /*******************************************************************/
                sql = "Select idExpresionBusqueda from ExpresionBusqueda where idBusqueda = " + busqueda.id;
                adaptador = contextoBD.RegistroDataAdapter(sql, conexion);
                adaptador.Fill(dataSetId);
                DataTable tablaExpresiones = dataSetId.Tables[0];
                foreach (DataRow fila in tablaExpresiones.Rows)
                {
                    sql = "delete from CampoExpresion where idExpresionBusqueda = " + fila["idExpresionBusqueda"];
                    command = contextoBD.CommandRegister(sql, conexion);
                    if (command.Connection.State != ConnectionState.Open)
                    {
                        command.Connection.Open();
                    }
                    command.ExecuteNonQuery();
                }
                command.ExecuteNonQuery();
                sql = "delete from ExpresionBusqueda where idBusqueda = " + busqueda.id;
                command = contextoBD.CommandRegister(sql, conexion);
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                command.ExecuteNonQuery();
                /**********************************************************/
                /******* Ahora se borran los datos de los registros  ******/
                /******* de las busquedas.                       ******/
                /**********************************************************/
                command.ExecuteNonQuery();
                sql = "delete from busqueda where id = " + busqueda.id;
                command = contextoBD.CommandRegister(sql, conexion);
                if (command.Connection.State != ConnectionState.Open)
                {
                    command.Connection.Open();
                }
                command.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception e)
            {
                return;
            }
        }
        public BusquedaAlmacenadaTO ObtenBusqueda(int p)
        {
            DbConnection conexion = contextoBD.RegistroUsuariosConnection;
            BusquedaAlmacenadaTO resultado = new BusquedaAlmacenadaTO();
            resultado.id = p;
            String sql = "select id, tipo_busqueda, nombre, valorBusqueda, expresion from busqueda where id = " + p;
            DataAdapter adaptador = contextoBD.RegistroDataAdapter(sql, conexion);
            DataSet dataSetId = new DataSet();
            adaptador.Fill(dataSetId);
            DataTable tabla = dataSetId.Tables[0];
            foreach (DataRow fila in tabla.Rows)
            {
                resultado.id = (int)fila["id"];
                resultado.Expresion = (string)fila["expresion"];
                resultado.Nombre = (String)fila["nombre"];
                resultado.TipoBusqueda = (int)fila["tipo_busqueda"];
                resultado.ValorBusqueda = (String) fila["valorBusqueda"];
            }
            sql = "select id_busqueda, epoca from busquedaEpocas where id_busqueda = " + p;
            adaptador = contextoBD.RegistroDataAdapter(sql, conexion);
            dataSetId = new DataSet();
            adaptador.Fill(dataSetId);
            tabla = dataSetId.Tables[0];
            resultado.Epocas = new int[tabla.Rows.Count];
            int contador = 0;
            foreach (DataRow fila in tabla.Rows)
            {
                resultado.Epocas[contador] = (int)fila["epoca"];
                contador ++;
            }
            sql = "select idBusqueda, Expresion, Operador, isJuris, idExpresionbusqueda from expresionBusqueda where idBusqueda = " + p;
            adaptador = contextoBD.RegistroDataAdapter(sql, conexion);
            dataSetId = new DataSet();
            adaptador.Fill(dataSetId);
            tabla = dataSetId.Tables[0];
            resultado.Expresiones = new List<ExpresionBusqueda>();
            foreach (DataRow fila in tabla.Rows)
            {
                ExpresionBusqueda expresion = new ExpresionBusqueda();
                expresion.Operador = (int)fila["Operador"];
                expresion.IsJuris = (int)fila["isJuris"];
                expresion.Expresion = (String)fila["Expresion"];
                String sqlExpresion = "Select idExpresionBusqueda, campo from campoExpresion where idExpresionBusqueda = "
                    + fila["idExpresionbusqueda"];
                DataAdapter adaptadorCampo = contextoBD.RegistroDataAdapter(sqlExpresion, conexion);
                DataSet dataSetCampo = new DataSet();
                adaptadorCampo.Fill(dataSetCampo);
                DataTable tablaCampo = dataSetCampo.Tables[0];
                expresion.Campos=new List<int>();
                foreach (DataRow itemCampo in tablaCampo.Rows)
                {
                    expresion.Campos.Add((int)itemCampo["campo"]);
                }
                resultado.Expresiones.Add(expresion);
                contador++;
            }
            conexion.Close();
            return resultado;
        }
        public void RecuperaUsuario(string correo)
        {
            DbConnection Conexion = contextoBD.RegistroUsuariosConnection;
            String SQL = "Select eMail, usuario, passwd, nombre, Apellidos from usuarios where email = '" + correo + "'";
            DataAdapter Adaptador = contextoBD.RegistroDataAdapter(SQL, Conexion);
            DataSet Datos = new DataSet();
            Adaptador.Fill(Datos);
            DataTable Tabla = Datos.Tables[0];
            String NombreCompleto = "";
            String Usuario = "";
            String Contrasena = "";
            foreach (DataRow item in Tabla.Rows)
            {
                NombreCompleto = item["nombre"] + " " + item["Apellidos"];
                Usuario = (String)item["usuario"];
                Contrasena = (String)item["passwd"];
            }
            String MensajeHtml = "<HTML>\n<BODY>\n";
            MensajeHtml += "<IMG SRC=\"http://www.scjn.gob.mx/PortalSCJN/images/top_001.jpg\"><BR>"+
                "Estimado <B> " + NombreCompleto + "</b>:<br>";
            MensajeHtml +="Este correo es enviado por solicitud suya para poder recuperar su contraseña.<Br>\n Su usuario es<BR >";
            MensajeHtml += "<B> Usuario: </B> " + Usuario + "<BR>";
            MensajeHtml += "<B> Contrase&ntilde;a: </B> " + Contrasena + "<BR>";
            MensajeHtml += "<i>Le recordamos que el usuario y password son sensibles a mayúsculas, por lo que tendrán que ser escritos tal cual aparecen.</i>";
            MailMessage mensaje = new MailMessage(IUSConstants.DIRECCION_CORREO, correo);
            mensaje.IsBodyHtml = true;
            mensaje.Body = MensajeHtml;
            mensaje.Subject = "Recuperación de usuario y contraseña para IUS";
            SmtpClient clienteEnvio = new SmtpClient(IUSConstants.IUS_SMTP_SERVER);
            clienteEnvio.UseDefaultCredentials = false;
            NetworkCredential credenciales = new NetworkCredential(IUSConstants.IUS_SMTP_USER,
                IUSConstants.IUS_SMTP_PASSWD, IUSConstants.IUS_SMTP_DOMAIN);
            clienteEnvio.Credentials = credenciales;
            try
            {
                clienteEnvio.Send(mensaje);
            }
            catch (Exception exc)
            {
                EventLog Log = new EventLog("Application", ".","IUSWeb");
                Log.WriteEntry("Error al enviar correo a: " + correo + " " + exc.Message,
                    EventLogEntryType.Error);
            }
        }
        #endregion
    }
}
