using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mx.gob.scjn.ius_common.TO
{
    /// <summary>
    /// Mantiene los datos para la transferencia de un objeto determinado
    /// a lo largo de la aplicación.
    /// </summary>
    public class AcuerdoPartesTO
    {
        /// <summary>
        ///Identificador del acuerdo
        /// </summary>
        private int id;
        /// <summary>
        /// Número de parte del acuerdo
        /// </summary>
        private String parte;
        /// <summary>
        /// Texto de la parte del acuerdo
        /// </summary>
        private String txtParte;
        /// <summary>
        /// Establece la parte a la que pertenece el texto del acuerdo.
        /// </summary>
        /// <param name="parametro">El número de parte</param>
        public void setParte(String parametro)
        {
            this.parte = parametro;
        }
        /// <summary>
        /// Devuelve el número de parte
        /// </summary>
        /// <returns>El número de parte</returns>
        public String getParte()
        {
            return this.parte;
        }
        /// <summary>
        /// Establece el texto en el documento de Acuerdo.
        /// </summary>
        /// <param name="parametro">El texto del documento</param>
        public void setTxtParte(String parametro)
        {
            this.txtParte = parametro;
        }
        /// <summary>
        /// Obtiene el texto del documento
        /// </summary>
        /// <returns>El texto de la parte</returns>
        public String getTxtParte()
        {
            return this.txtParte;
        }
        /// <summary>
        /// Establece el Valor del Id
        /// </summary>
        /// <param name="nuevoId">El nuevo Id</param>
        public void setId(int nuevoId)
        {
            this.id = nuevoId;
        }
        /// <summary>
        /// Devuelve el valor del Id
        /// </summary>
        /// <returns>El valor del Id de la parte del cacuerdo</returns>
        public int getId()
        {
            return this.id;
        }
    }
}
