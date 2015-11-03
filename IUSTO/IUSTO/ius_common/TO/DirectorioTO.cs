
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using mx.gob.scjn.ius_common.TO;

namespace mx.gob.scjn.ius_common.TO
{

    //[DataContract]
    public class DirectorioTO
    {

        List<DirectorioPersonasTO> ListaPersonas = new List<DirectorioPersonasTO>();

        public DirectorioTO()
        {

        }

        #region

        //    //[DataMember]
        //    //public DirectorioPersonasTO[] setListaPersonas { get { return this.ListaPersonas(); } set { this.setListaPersonas(value); } }
        //    //List<DirectorioPersonasTO> ListaPersonas = new List<DirectorioPersonasTO>();

        //[DataMember]
        //public List<DirectorioPersonasTO> ListaPersonas { get { return this.getListaPersonas(); } set; } }
        //private List<DirectorioPersonasTO> ListaPersonas = new List<DirectorioPersonasTO>();

        //            public List<DirectorioPersonasTO> getListaPersonas() { return ListaPersonas; }
        //        public void setListaPersonas(List<DirectorioPersonasTO> ListaPersonas) { (List<DirectorioPersonasTO>)this.ListaPersonas = ListaPersonas; }

        ////        [DataMember]
        //        //public int IdPersona { get { return this.getIdPersona(); } set { this.setIdLey(value); } }
        //        //private int idPersona;

        //    //public void setListaPersonas(DirectorioPersonasTO ListaPersonas) { this.ListaPersonas = ListaPersonas; }
        //    //private  List<DirectorioPersonasTO> getListaPersonas() { return ListaPersonas; }

        //private void LlenaElementos() {

        //    int i;

        //    for (i = 0; i < 10; i++) {

        //            DirectorioPersonasTO Personas = new DirectorioPersonasTO();

        //            Personas.NombrePersona = "Nombre: " + i.ToString();
        //            Personas.DomPersona  = "Domicilio: " + i.ToString();
        //            Personas.TelPersona  = "Teléfono: " + i.ToString();

        //            ListaPersonas.Add(Personas);

        //    }

        //}

        #endregion

    }
}
