using System;
using System.Linq;

namespace SeguridadTesauro.TO
{
    public class UsuarioTO
    {
        public int Id { get; set; }
        public String UserName { get; set; }
        public String Pwd { get; set; }
        public int TipoUsuario { get; set; }
        public int Perfil { get; set; }
        public int Rol { get; set; }
    }
}
