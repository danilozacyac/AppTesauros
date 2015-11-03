using System;
using System.Collections.Generic;
using System.Linq;
using SeguridadTesauro.TO;

namespace SeguridadTesauro.DAO
{
    public interface UsuarioDAO
    {
        List<UsuarioTO> ObtenUsuarios();
        Boolean VerificaUsuario(String usuario, String Pass);
        int ObtenerId(String Usuario);

        List<MateriaTO> ObtenMaterias(int Usuario);

        List<PermisoTO> ObtenPermisos(int Usuario);

        int ObtenRol(string usuario);

        List<UsuarioTO> ObtenTodosUsuarios();

        List<PerfilTO> ObtenPerfiles();
    }
}
