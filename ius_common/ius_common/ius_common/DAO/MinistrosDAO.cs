using System.Collections.Generic;
using mx.gob.scjn.ius_common.TO;
using System;
namespace mx.gob.scjn.ius_common.DAO
{

    ///<summary>
    /// Define las consultas para la vista detalle/maestro de
    /// los ministros.
    /// </summary>
    /// <remarks author="Carlos de Luna Sáenz."></remarks>
    ///
    
    public interface MinistrosDAO
    {
        ///<summary>
        /// Obtiene una lista de fechas que concuerdan
        /// con una sala y una epoca determinada. 
        ///
        List<EpocaMagistradoTO> getFechasMagistrados(EpocaMagistradoTO parametros);
        /**
         * Obtiene una lista de los funcionarios que estuvieron en un determinado
         * momento.
         */
        List<FuncionariosTO> getFuncionarios(String id);
        /// <summary>
        /// Regresa el string en el que se define la fecha de actualización
        /// del sistema
        /// </summary>
        /// <returns>La cadena que indica hasta cuando está actualizado</returns>
        String getActualizadoA();
    }
}