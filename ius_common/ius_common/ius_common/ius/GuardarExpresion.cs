using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.DAO;
using mx.gob.scjn.ius_common.context;
using mx.gob.scjn.ius_common.TO;
using mx.gob.scjn.ius_common.utils;

namespace mx.gob.scjn.ius_common.ius
{
    public class GuardarExpresion
    {
        	private IUSApplicationContext contexto;
	/// <summary>
	/// Constructor por omisión.
	/// </summary>
	public GuardarExpresion(){
        try{
		    this.contexto = new IUSApplicationContext();
		} catch (Exception e ) {
			System.Console.Write("Fallo al iniciar el contexto, la excepcion es:"+e.Message);
			System.Console.Write(e.StackTrace);
		}
	}
        public bool VerificaUsuario(String usuario, String password)
        {
            bool resultado = false;
            GuardarExpresionDAO guardar = (GuardarExpresionDAO)contexto.GetObject("GuardarExpresionDAO");
            UsuarioTO usuarioDatos = guardar.ObtenUsuario(usuario, password);
            resultado = !((usuarioDatos.Nombre==null)||(usuarioDatos.Nombre.Equals("")));
            return resultado;
        }

        public UsuarioTO ObtenDatosUsuario(string usuario)
        {
            GuardarExpresionDAO guardar = (GuardarExpresionDAO)contexto.GetObject("GuardarExpresionDAO");
            UsuarioTO usuarioDatos = guardar.ObtenUsuario(usuario);
            return usuarioDatos;
        }
        public List<BusquedaAlmacenadaTO> getBusquedasAlmacenadas(String usuario)
        {
            GuardarExpresionDAO guardar = (GuardarExpresionDAO)contexto.GetObject("GuardarExpresionDAO");
            List<BusquedaAlmacenadaTO> resultado = guardar.ObtenBusquedas(usuario);
            return resultado;
        }

        public int RegistrarUsuario(UsuarioTO usuario)
        {
            GuardarExpresionDAO guardar = (GuardarExpresionDAO)contexto.GetObject("GuardarExpresionDAO");
            UsuarioTO usuarioEncontrado = guardar.ObtenUsuario(usuario.Usuario);
            if ((usuarioEncontrado.Nombre == null) || (usuarioEncontrado.Nombre.Equals("")))
            {
                guardar.RegistrarUsuario(usuario);
                return IUSConstants.NO_ERROR;
            }
            else
            {
                return IUSConstants.USUARIO_EXISTENTE;
            }
        }
        public int RegistrarBusqueda(BusquedaAlmacenadaTO busqueda, String usuario)
        {
            GuardarExpresionDAO guardar = (GuardarExpresionDAO)contexto.GetObject("GuardarExpresionDAO");
            UsuarioTO usuarioEncontrado = guardar.ObtenUsuario(usuario);
            return guardar.RegistrarBusqueda(busqueda, usuario);
        }

        public int EliminarBusqueda(BusquedaAlmacenadaTO busqueda)
        {
            GuardarExpresionDAO guardar = (GuardarExpresionDAO)contexto.GetObject("GuardarExpresionDAO");
            guardar.EliminaBusqueda(busqueda);
            return IUSConstants.NO_ERROR;
        }
        public void RecuperaUsuario(string correo)
        {
            GuardarExpresionDAO guardar = (GuardarExpresionDAO)contexto.GetObject("GuardarExpresionDAO");
            guardar.RecuperaUsuario(correo);
        }
    }
}
