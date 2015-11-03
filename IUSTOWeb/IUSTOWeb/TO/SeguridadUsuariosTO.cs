using System;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO
{
    public class SeguridadUsuariosTO
    {
        public static UsuarioTO UsuarioActual{get{
            if(usuarioActual==null) usuarioActual=new UsuarioTO();
            return usuarioActual;
        }
            set { usuarioActual = value; }
        } 
        private static UsuarioTO usuarioActual{get;set;}
        public String passwd { get; set; }
        public bool TieneBusqueda { get; set; }
    }
}
