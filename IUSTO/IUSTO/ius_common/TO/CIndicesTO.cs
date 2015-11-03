using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mx.gob.scjn.ius_common.TO
{
    /**
     * Este objeto representa la forma en que se generarán los
     * índices y su estructura de datos dentro del manejador de datos.
     * @author Carlos de Luna Saenz
     *
     */
    public class CIndicesTO
    {
        /**
         * Identificador del índice.
         */
        private int idInd;
        /**
         * Identificador del nivel del índice dentro del
         * árbol de contenido.
         */
        private int numeroNivel;
        /**
         * Descripción del índice.
         */
        private String cadenaDesc;
        /**
         * Etiqueta que tendrá el índice.
         */
        private String cadenaTag;
        /**
         * Llave de búsqueda del indice:<BR>
         * Cuando tiene un * al principio se trata de un menú.<BR>
         * Una M indica que es una materia.<BR>
         * Una S una búsqueda por instancia y época.
         */
        private String cadenaKey;
        /**
         * El id del identificador del padre.<BR>
         * Si el id del padre es  18 se trata de TCC, por lo que existen
         * hijos que ya no se deberán buscar en la tabla de cIndices, sino
         * en la de tribcol.
         */
        private int numeroIdPadre;
        /**
         * La imagen que se tiene para mostrar (por compatibilidad
         * con el Ius antiguo).
         */
        private String cadenaImagen;
        /**
         * @return the idInd
         */
        public int getIdInd()
        {
            return idInd;
        }
        /**
         * @param idInd the idInd to set
         */
        public void setIdInd(int idInd)
        {
            this.idInd = idInd;
        }
        /**
         * @return the nNivel
         */
        public int getNumeroNivel()
        {
            return numeroNivel;
        }
        /**
         * @param nivel the nNivel to set
         */
        public void setNumeroNivel(int nivel)
        {
            numeroNivel = nivel;
        }
        /**
         * @return the cDesc
         */
        public String getCadenaDesc()
        {
            return cadenaDesc;
        }
        /**
         * @param desc the cDesc to set
         */
        public void setCadenaDesc(String desc)
        {
            cadenaDesc = desc;
        }
        /**
         * @return the cTag
         */
        public String getCadenaTag()
        {
            return cadenaTag;
        }
        /**
         * @param tag the cTag to set
         */
        public void setCadenaTag(String tag)
        {
            cadenaTag = tag;
        }
        /**
         * @return the cKey
         */
        public String getCadenaKey()
        {
            return cadenaKey;
        }
        /**
         * @param key the cKey to set
         */
        public void setCadenaKey(String key)
        {
            cadenaKey = key;
        }
        /**
         * @return the nIdPadre
         */
        public int getNumeroIdPadre()
        {
            return numeroIdPadre;
        }
        /**
         * @param idPadre the nIdPadre to set
         */
        public void setNumeroIdPadre(int idPadre)
        {
            numeroIdPadre = idPadre;
        }
        /**
         * @return the cImagen
         */
        public String getCadenaImagen()
        {
            return cadenaImagen;
        }
        /**
         * @param imagen the cImagen to set
         */
        public void setCadenaImagen(String imagen)
        {
            cadenaImagen = imagen;
        }

    }
}
