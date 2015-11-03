using System;
using System.Collections.Generic;
using System.Linq;
using SeguridadTesauro.DAO;
using SeguridadTesauro.DAO.Impl;
using SeguridadTesauro.TO;

namespace SeguridadTesauro.Fachada
{
    public class FachadaST
    {
        public List<UsuarioTO> ObtenUsuarios()
        {
            UsuarioDAO Dao = new TesauroUsuarioDAOImpl();
            return Dao.ObtenUsuarios();
        }
        public bool ValidaUsuario(String usuario, String Pass)
        {
            UsuarioDAO Dao = new TesauroUsuarioDAOImpl();
            return Dao.VerificaUsuario(usuario, Pass);
        }
        public int ObtenIdUsuario(String usuario)
        {
            UsuarioDAO Dao = new TesauroUsuarioDAOImpl();
            return Dao.ObtenerId(usuario);
        }
        public List<MateriaTO> ObtenMaterias(int Usuario)
        {
            UsuarioDAO Dao = new TesauroUsuarioDAOImpl();
            return Dao.ObtenMaterias(Usuario);
        }

        public List<PermisoTO> ObtenPermisos(int Usuario)
        {
            UsuarioDAO Dao = new TesauroUsuarioDAOImpl();
            return Dao.ObtenPermisos(Usuario);
        }
        public int ObtenRol(String usuario)
        {
            UsuarioDAO Dao = new TesauroUsuarioDAOImpl();
            return Dao.ObtenRol(usuario);
        }

        public List<UsuarioTO> ObtenTodosUsuarios()
        {
            UsuarioDAO Dao = new TesauroUsuarioDAOImpl();
            return Dao.ObtenTodosUsuarios();
        }

        public List<PerfilTO> ObtenPerfiles()
        {
            UsuarioDAO Dao = new TesauroUsuarioDAOImpl();
            return Dao.ObtenPerfiles();
        }
    }
}
