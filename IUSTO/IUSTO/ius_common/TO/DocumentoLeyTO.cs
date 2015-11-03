using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace mx.gob.scjn.ius_common.TO
{
    [DataContract]
    public class DocumentoLeyTO
    {
        [DataMember]
        public LeyTO Ley { get { return this.getLey(); } set { this.setLey(value); } }
        private LeyTO ley;
        [DataMember]
        public List<ArticulosTO> Articulo { get { return this.getArticulo(); } set { this.setArticulo(value); } }
        private List<ArticulosTO> articulo;
        /**
         * La ley a la que pertenece el artículo.
         * @return el valor de la ley a la que pertenece el artículo.
         */
        public LeyTO getLey()
        {
            return ley;
        }
        /**
         * Define la ley a la que pertenece el artículo.
         * @param ley the ley to set
         */
        public void setLey(LeyTO ley)
        {
            this.ley = ley;
        }
        /**
         * @return the articulo
         */
        public List<ArticulosTO> getArticulo()
        {
            return articulo;
        }
        /**
         * @param articulo the articulo to set
         */
        public void setArticulo(List<ArticulosTO> articulo)
        {
            this.articulo = articulo;
        }

    }
}
