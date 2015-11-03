using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TesauroTO
{
    public class SinonimoTO
    {
        private int _IDTema;
        public int IDTema
        {
            get { return _IDTema; }
            set { _IDTema = value; }
        }

        private int _IDPadre;
        public int IDPadre
        {
            get { return _IDPadre; }
            set { _IDPadre = value; }
        }

        private string _Descripcion;
        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }


        private int _Tipo;
        public int Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

        private string _DescripcionStr;
        public string DescripcionStr
        {
            get { return _DescripcionStr; }
            set { _DescripcionStr = value; }
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

        private String _Nota;
        public String Nota
        {
            get { return _Nota; }
            set { _Nota = value; }
        }

        private String _Observaciones;
        public String Observaciones
        {
            get { return _Observaciones; }
            set { _Observaciones = value; }
        }


        public SinonimoTO(int IDTema, int IDPadre, string Descripcion, int Tipo,
            string DescripcionStr, int IDUser, DateTime Fecha, DateTime Hora, 
            String nota, String observaciones)
        {
            _IDTema = IDTema;
            _IDPadre = IDPadre;
            _Descripcion = Descripcion;
            _Tipo = Tipo;
            _DescripcionStr = DescripcionStr;
            _IDUser = IDUser;
            _Fecha = Fecha;
            _Hora = Hora;
            _Nota = nota;
            _Observaciones = observaciones;
        }


        //int IDTema, int IDPadre, string Descripcion, int Tipo,
        //string DescripcionStr, int IDUser, DateTime Fecha, DateTime Hora

        //SELECT Sinonimos.IDTema, Sinonimos.IDPadre, Sinonimos.Descripcion, Sinonimos.Tipo, Sinonimos.DescripcionStr, Sinonimos.IDUser, Sinonimos.Fecha, Sinonimos.Hora
        //FROM Sinonimos
    }
}
