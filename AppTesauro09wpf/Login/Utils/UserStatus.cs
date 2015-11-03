using System;
using System.Collections.Generic;
using System.Linq;
using SeguridadTesauro.TO;

namespace Login.Utils
{
    public class UserStatus
    {
        public static int IdActivo { get; set; }
        public static int RolActivo { get; set; }
        public static List<MateriaTO> MateriasUser { get; set; }
    }
}
