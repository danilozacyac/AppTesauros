using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.electoral_common.DAO
{
    public interface GuardarExpresionDAO
    {
        /// <summary>
        /// Obtiene los datos del usuario si es que el usuario y passwd son correctos.
        /// </summary>
        /// <param name="usuario">Usuario que se busca</param>
        /// <param name="Passwd">Contraseña</param>
        /// <returns>Si el password es incorrecto se da un objeto vacio (no uno nulo), en caso de existir se devuelven todos los datos llenos.</returns>
        UsuarioTO ObtenUsuario(String usuario, String Passwd);
        /// <summary>
        /// Se obtienen las búsquedas que tiene un usuario.
        /// </summary>
        /// <param name="usuario">El usuario</param>
        /// <returns>La lista de sus búsquedas.</returns>
        List<BusquedaAlmacenadaTO> ObtenBusquedas(String usuario);
        /// <summary>
        /// Obtiene los datos del usuario
        /// </summary>
        /// <param name="usuario">El usuario</param>
        /// <returns>Los datos del usuario</returns>
        UsuarioTO ObtenUsuario(string usuario);
        /// <summary>
        /// Registra un usuario nuevo con los datos dados.
        /// </summary>
        /// <param name="usuario">El usuario a registrar</param>
        void RegistrarUsuario(UsuarioTO usuario);
        /// <summary>
        /// Guarda en la base de datos la búsqueda almacenada.
        /// </summary>
        /// <param name="busqueda">La busqueda a guardar</param>
        /// <param name="usuario">El usuario al que pertenece la busqueda.</param>
        int RegistrarBusqueda(BusquedaAlmacenadaTO busqueda, string usuario);
        /// <summary>
        /// Elimina una determinada Busqueda
        /// </summary>
        /// <param name="busqueda">Busqueda a borrar</param>
        void EliminaBusqueda(BusquedaAlmacenadaTO busqueda);
        /// <summary>
        /// obtiene una determinada búsqueda de acuerdo al identificador proporcionado
        /// </summary>
        /// <param name="p">El identificador de la búsqueda a obtener</param>
        /// <returns>La búsqueda encontrada</returns>
        BusquedaAlmacenadaTO ObtenBusqueda(int p);
        /// <summary>
        /// Envía un correo para la recuperación de usuario/contraseña.
        /// </summary>
        /// <param name="correo">El correo del que se solicita el usuario y contraseña.</param>
        void RecuperaUsuario(string correo);
    }
}
