using System;
using System.Linq;

namespace mx.gob.scjn.ius_common.TO
{
    public class DocumentoADesplegarTO
    {
        public String Articulo { get; set; }
        public String DescLey { get; set; }
        public DocumentoADesplegarTO(DocumentoLeyTO principal)
        {
            this.DescLey = principal.Ley.DescLey;
            if (principal.Articulo[0].Info.Length > 200)
            {
                this.Articulo = principal.Articulo[0].Info.Substring(0, 200) + " <más informacion>";
            }
            else
            {
                this.Articulo = principal.Articulo[0].Info + " <más informacion>";
            }
        }
    }
}
