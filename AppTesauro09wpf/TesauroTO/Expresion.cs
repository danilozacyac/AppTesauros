using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TesauroTO
{
    public class ExpresionTO
    {

        private int _IDTema;
        public int IDTema
        {
            get { return _IDTema; }
            set { _IDTema = value; }
        }

        private string _Descripcion;
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        private int _IDUser;
        public int IDUser
        {
            get { return _IDUser; }
            set { _IDUser = value; }
        }


        private DateTime _Fecha;
        public DateTime Fecha
        {
            get { return _Fecha; }
            set { _Fecha = value; }
        }

        private DateTime _Hora;
        public DateTime Hora
        {
            get { return _Hora; }
            set { _Hora = value; }
        }

        private int _Id;
        public int Id
        {
            get { return _Id; }
            set { _Id = value; }
        }

        private int _Operador;
        public int Operador
        {
            get { return _Operador; }
            set { _Operador = value; }
        }

        private int[] _Campos;
        public int[] Campos
        {
            get { return _Campos; }
            set { _Campos = value; }
        }

        public ExpresionTO(int IDTema, string Descripcion,
            int IDUser, DateTime Fecha, DateTime Hora, int id, int Operador,
            int[] Campos)
        {
            _Campos = Campos;
            _IDTema = IDTema;
            _Descripcion = Descripcion;
            _IDUser = IDUser;
            _Fecha = Fecha;
            _Hora = Hora;
            _Id = id;
            _Operador = Operador;
        }
    }
}
