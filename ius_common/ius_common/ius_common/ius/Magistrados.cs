using mx.gob.scjn.ius_common.context;
using System.Collections.Generic;
using mx.gob.scjn.ius_common.TO;
using System;
using mx.gob.scjn.ius_common.DAO;
using log4net;

namespace mx.gob.scjn.ius_common.ius
{
    ///<summary>
    /// Obtiene los datos de los magistrados para el caso
    ///  de la consulta en las noticias históricas.
    ///  </summary>
    ///  <remarks author="Carlos de Luna Saenz"></remarks>
    ///  
    public class Magistrados
    {
        private ILog log = LogManager.GetLogger("mx.gob.scjn.iuscommon.ius.Ejecutorias");

        private IUSApplicationContext contexto;

        ///<summary>
        /// Inicia el objeto obteniendo los contextos necesarios.
        /// </summary>
        public Magistrados()
        {
            try
            {
                this.contexto = new IUSApplicationContext();
            }
            catch (Exception e)
            {
                log.Debug("Problemas al iniciar el contexto" + e.Message);
            }
        }
        /// <summary>
        ///  Obtiene una lista de las fechas de acuerdo a la seleccion de
        ///  la sala (instancia) y epoca que vienen en el objeto de parametro.
        ///  </summary>
        ///  <param name="parametro"> Los criterios de búsqueda mencionados.</param>
        ///  <return> La lista de coincidencias.</return>
        /// 

        public List<EpocaMagistradoTO> getFechasMagistrados(EpocaMagistradoTO parametro)
        {
            MinistrosDAO magis = (MinistrosDAO)contexto.getInitialContext().GetObject("MinistrosDAO");
            return magis.getFechasMagistrados(parametro);
        }

        /// <summary>
        /// Obtiene la lista de funcionarios que estuvieron en activo en
        /// una determinada fecha.
        /// </summary>
        /// <param name="id">el identificador fecha/sala/epoca</param>
        /// <return>La lista de los funcionarios</return> 
        /// 

        public List<FuncionariosTO> getFuncionarios(String id)
        {
            MinistrosDAO magis = (MinistrosDAO)contexto.getInitialContext().GetObject("MinistrosDAO");
            return magis.getFuncionarios(id);
        }
        public String getActualizadoA()
        {
            MinistrosDAO magis = (MinistrosDAO)contexto.getInitialContext().GetObject("MinistrosDAO");
            return magis.getActualizadoA();
        }
    }
}