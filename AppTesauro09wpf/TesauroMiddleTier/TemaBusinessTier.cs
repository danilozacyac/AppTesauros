using System;
using System.Collections.Generic;
using System.Linq;
using TesauroTO;
using TesauroUtilities;
using mx.gob.scjn.ius_common.TO;

namespace TesauroMiddleTier
{
    public class TemaBusinessTier
    {
        public const int STATUS_NUEVO = 1;
        public const int STATUS_OBSERVADO = 2;
        public const int STATUS_ATENDIDO = 3;
        public const int STATUS_ACEPTADO = 4;

        public List<TemaTO> getTemas()
        {
            TesauroDAC dac = new TesauroDAC();
            return dac.GetAllTemasOrder("Temas.Descripcion");
        }

        /// <summary>
        /// Devuelve un listado solo con los temas de la materia o materias que le corresponden a cada usuario
        /// </summary>
        /// <param name="materias">Materias asignadas a este usuario</param>
        /// <returns></returns>
        public List<TemaTO> getTemas(List<int> materias)
        {
            TesauroDAC dac = new TesauroDAC();
            return dac.GetAllTemasOrder("Temas.Descripcion",materias);
        }

        internal List<SinonimoTO> GetSinonimos(int p, int idMateria)
        {
            TesauroDAC dac = new TesauroDAC();
            return dac.GetSinonimosForTema(p, "Sinonimos.Descripcion",idMateria);
        }

        internal List<SinonimoTO> GetRP(int p, int idMateria)
        {
            TesauroDAC dac = new TesauroDAC();
            return dac.GetRPForTema(p, "Sinonimos.Descripcion",idMateria);
        }

        internal TemaTO GetIA(int p)
        {
            TesauroDAC dac = new TesauroDAC();
            return dac.GetIAForTema(p);
        }

        public int ActualizaPadre(TemaTO temaTO)
        {
            TesauroDAC dac = new TesauroDAC();
            return dac.ActualizaPadreTema(temaTO);
        }

        public List<int> GetTesisPorTema(TemaTO tema)
        {
            TesauroDAC dac = new TesauroDAC();
            return dac.GetTesisPorTemaOrder(tema, "ConsecIndx");
        }

        public void ActualizaTema(TemaTO Datos)
        {
            TesauroDAC dac = new TesauroDAC();
            dac.ActualizaTema(Datos);
        }

        internal int GeneraNuevoTema(TemaTO datos)
        {
            TesauroDAC dac = new TesauroDAC();
            int maxTema = dac.ObtenMaximoIDtema(datos.Materia);

            if (maxTema == -1)
                return maxTema;

            datos.IDTema = maxTema ;
            dac.GeneraNuevoTema(datos);
            String strExpr = String.Empty;
            switch (datos.Materia)
            {
                case ConstantsMT.MATERIA_ADMINISTRATIVA:
                    strExpr = "Administrativa";
                    break;
                case ConstantsMT.MATERIA_CIVIL:
                    strExpr = "Civil";
                    break;
                case ConstantsMT.MATERIA_COMUN:
                    strExpr = "Comun";
                    break;
                case ConstantsMT.MATERIA_DH:
                    strExpr = "\"Derechos Humanos\"";
                    break;
                case ConstantsMT.MATERIA_LABORAL:
                    strExpr = "Laboral";
                    break;
                case ConstantsMT.MATERIAS_CONSTITUCIONAL:
                    strExpr = "Constitucional";
                    break;
                case ConstantsMT.MATERIAS_PENAL:
                    strExpr = "Penal";
                    break;
            }
            int[] campos = new int[1];
            campos[0] = ConstantsMT.BUSQUEDA_PALABRA_CAMPO_LOC;
            //ExpresionTO Expresion = new ExpresionTO(datos.IDTema, strExpr, TesauroDAC.UsuarioActual,
            //    DateTime.Now, DateTime.Now, dac.ObtenMaximoIDExpresion() + 1,
            //    ConstantsMT.BUSQUEDA_PALABRA_OP_Y, campos);
            //dac.InsertaExpresion(Expresion);
            return datos.IDTema;
        }

        internal void actualizaSinonimo(SinonimoTO sinonimoActual, int idMateria)
        {
            TesauroDAC dac = new TesauroDAC();
            dac.ActualizaSinonimo(sinonimoActual,idMateria);
        }

        public bool VerifyExistence(String texto, int idpadre,int idMateria)
        {
            TesauroDAC dac = new TesauroDAC();
            return dac.VerifyExistence( texto,  idpadre,idMateria);
        }

        internal int GeneraNuevoSinonimo(SinonimoTO sinonimoActual, int idMateria)
        {
            TesauroDAC dac = new TesauroDAC();
            //int maxTema = dac.ObtenMaximoIDtema();
            
            sinonimoActual.IDTema = dac.ObtenMaximoIdRelaciones("Sinonimos");
            dac.GeneraNuevoSinonimo(sinonimoActual,idMateria);
            

            TemaTO tema = dac.ObtenTema(sinonimoActual.IDPadre);
            String strExpr = String.Empty;
            switch (tema.Materia)
            {
                case ConstantsMT.MATERIA_ADMINISTRATIVA:
                    strExpr = "Administrativa";
                    break;
                case ConstantsMT.MATERIA_CIVIL:
                    strExpr = "Civil";
                    break;
                case ConstantsMT.MATERIA_COMUN:
                    strExpr = "Comun";
                    break;
                case ConstantsMT.MATERIA_DH:
                    strExpr = "\"Derechos Humanos\"";
                    break;
                case ConstantsMT.MATERIA_LABORAL:
                    strExpr = "Laboral";
                    break;
                case ConstantsMT.MATERIAS_CONSTITUCIONAL:
                    strExpr = "Constitucional";
                    break;
                case ConstantsMT.MATERIAS_PENAL:
                    strExpr = "Penal";
                    break;
            }
            int[] campos = new int[1];
            campos[0] = ConstantsMT.BUSQUEDA_PALABRA_CAMPO_LOC;
            ExpresionTO Expresion = new ExpresionTO(sinonimoActual.IDTema, strExpr, TesauroDAC.UsuarioActual,
                DateTime.Now, DateTime.Now, dac.ObtenMaximoIDExpresion() + 1,
                ConstantsMT.BUSQUEDA_PALABRA_OP_Y, campos);
            dac.InsertaExpresion(Expresion,idMateria);

            return sinonimoActual.IDTema;
        }

        public void EliminarSinonimo(SinonimoTO actual, int idMateria)
        {
            TesauroDAC dac = new TesauroDAC();
            int id = actual.IDTema;
            int valorActual = actual.Tipo + 3;
            dac.ActualizaTipoSinonimo(id, valorActual,idMateria);
        }

        public List<ExpresionTO> GetExpresiones(int tema, int idMateria)
        {
            TesauroDAC dac = new TesauroDAC();
            return dac.ObtenExpresiones(tema,idMateria);
        }

        public void InsertaExpresiones(ExpresionTO ExpresionActual, int idMateria)
        {
            TesauroDAC dac = new TesauroDAC();
            dac.InsertaExpresion(ExpresionActual,idMateria);
        }

        public void actualizaExpresion(ExpresionTO ExpresionActual, int idMateria)
        {
            TesauroDAC dac = new TesauroDAC();
            dac.ActualizaExpresion(ExpresionActual,idMateria);
        }

        public void EliminaExpresion(int id)
        {
            TesauroDAC dac = new TesauroDAC();
            dac.EliminarExpresioni(id);
        }

        public void InsertaIA(string Ascendente, int IDTema, int idMateria)
        {
            TesauroDAC dac = new TesauroDAC();
            dac.InsertaIA(Ascendente, IDTema);
            TemaTO tema = dac.ObtenTema(IDTema);

            String strExpr = String.Empty;
            switch (tema.Materia)
            {
                case ConstantsMT.MATERIA_ADMINISTRATIVA:
                    strExpr = "Admonistrativa";
                    break;
                case ConstantsMT.MATERIA_CIVIL:
                    strExpr = "Civil";
                    break;
                case ConstantsMT.MATERIA_COMUN:
                    strExpr = "Comun";
                    break;
                case ConstantsMT.MATERIA_DH:
                    strExpr = "\"Derechos Humanos\"";
                    break;
                case ConstantsMT.MATERIA_LABORAL:
                    strExpr = "Laboral";
                    break;
                case ConstantsMT.MATERIAS_CONSTITUCIONAL:
                    strExpr = "Constitucional";
                    break;
                case ConstantsMT.MATERIAS_PENAL:
                    strExpr = "Penal";
                    break;
            }
            int[] campos = new int[1];
            tema = dac.GetIAForTema(tema.IDTema);
            campos[0] = ConstantsMT.BUSQUEDA_PALABRA_CAMPO_LOC;
            ExpresionTO Expresion = new ExpresionTO(tema.IDTema, strExpr, TesauroDAC.UsuarioActual,
                DateTime.Now, DateTime.Now, dac.ObtenMaximoIDExpresion() + 1,
                ConstantsMT.BUSQUEDA_PALABRA_OP_Y, campos);
            dac.InsertaExpresion(Expresion,idMateria);
        }

        public void ActualizaIA(TemaTO datosIA)
        {
            TesauroDAC dac = new TesauroDAC();
            dac.ActualizaIA(datosIA);
        }

        public void ActualizaMateria(TemaTO temaQueActualizar, int materia)
        {
            TesauroDAC dac = new TesauroDAC();

            dac.ActualizaMateria(temaQueActualizar, materia);
            
            foreach (TemaTO item in temaQueActualizar.SubTemas)
            {
                this.ActualizaMateria(item, materia);
            }
        }

        public int copiaTema(int id, int padre, int materia, int idMateria)
        {
            int resultado = 0;
            TesauroDAC dac = new TesauroDAC();
            TemaTO temaActual = dac.ObtenTema(id);
            temaActual.Materia = materia;
            temaActual.IDPadre = padre;
            temaActual.IDTema = dac.ObtenMaximoIDtema(idMateria);
            dac.GeneraNuevoTema(temaActual);
            List<int> hijos = dac.ObtenHijos(id);
            List<SinonimoTO> sinonimos = dac.GetSinonimosForTema(id, "Sinonimos.Descripcion",idMateria);
            List<SinonimoTO> rps = dac.GetRPForTema(id, "Sinonimos.Descripcion",idMateria);
            foreach (SinonimoTO item in sinonimos)
            {
                SinonimoTO nuevoItem = new SinonimoTO(dac.ObtenMaximoIdRelaciones("Sinonimos"), temaActual.IDTema,
                    item.Descripcion, item.Tipo, item.DescripcionStr, item.IDUser, DateTime.Now,
                    DateTime.Now, item.Nota, item.Observaciones);
                dac.GeneraNuevoSinonimo(nuevoItem,idMateria);
            }
            foreach (SinonimoTO item in rps)
            {
                SinonimoTO nuevoItem = new SinonimoTO(dac.ObtenMaximoIdRelaciones("Sinonimos"), temaActual.IDTema,
                    item.Descripcion, item.Tipo, item.DescripcionStr, item.IDUser, DateTime.Now,
                    DateTime.Now, item.Nota, item.Observaciones);
                dac.GeneraNuevoSinonimo(nuevoItem,idMateria);
            }
            foreach (int item in hijos)
            {
                copiaTema(item, temaActual.IDTema, materia,idMateria);
            }
            return resultado;
        }

        public List<TemaTO> GetTemas(String temaBuscado)
        {
            TesauroDAC dac = new TesauroDAC();
            return dac.ObtenTemas(temaBuscado);
        }

        public List<TemaTO> GetHijos(int Padre)
        {
            TesauroDAC dac = new TesauroDAC();
            List<int> hijos = dac.ObtenHijos(Padre);
            List<TemaTO> hijosVerdaderos = new List<TemaTO>();
            foreach (int id in hijos)
            {
                hijosVerdaderos.Add(dac.ObtenTema(id));
            }
            return hijosVerdaderos;
        }

        internal string ObtenRuta(TemaTO Tema)
        {
            TesauroDAC dac = new TesauroDAC();
            return dac.ObtenMateria(Tema.Materia) + dac.ObtenRuta(Tema);
        }

        internal void copiaTema(List<TemaTO> TemasCopiar, int IDPadre, int IDMateria)
        {
            TesauroDAC dac = new TesauroDAC();
            Dictionary<int, int> ids = new Dictionary<int, int>();
            Dictionary<int, TemaTO> temas = new Dictionary<int, TemaTO>();
            foreach (TemaTO item in TemasCopiar)
            {
                ids.Add(item.IDTema, item.IDTema);
                temas.Add(item.IDTema, item);
            }
            List<int> procesados = new List<int>();
            bool seguir = true;
            while (seguir)
            {
                List<int> aprocesar = EncuentraAProcesar(ids, procesados, TemasCopiar);
                seguir = !(aprocesar.Count == ids.Count);
                if (seguir)
                {
                    foreach (int item in aprocesar)
                    {
                        ///Verificar si existen sus padres, cambiar sus ids  y 
                        ///actualizar la lista de ids... insertandom los nuevos temas, sinonimos,
                        ///relaciones próximas y expresiones para cada uno de ellos.
                        if (!procesados.Contains(item))
                        {
                            procesados.Add(item);
                            int nuevoId = dac.ObtenMaximoIDtema(IDMateria);
                            int padre = IDPadre;
                            if (ids.ContainsKey(temas[item].IDPadre))
                            {
                                padre = ids[temas[item].IDPadre];
                            }
                            else
                            {
                                padre = IDPadre;
                            }
                            TemaTO temaInserta = copiaValoresTema(temas[item], nuevoId, padre, IDMateria);
                            dac.GeneraNuevoTema(temaInserta);
                            List<ExpresionTO> expresionesCopiar = dac.GetAllExpresionForTema(item, "Expresion.IdExpresion");
                            ;
                            foreach (ExpresionTO itemExpresion in expresionesCopiar)
                            {
                                itemExpresion.Id = dac.ObtenMaximoIDExpresion() + 3;
                                itemExpresion.IDTema = nuevoId;
                                dac.InsertaExpresion(itemExpresion,IDMateria);
                            }
                            List<SinonimoTO> sinonimosCopiar = dac.GetSinonimosForTema(item, "Sinonimos.Descripcion",IDMateria);
                            foreach (SinonimoTO itemSinonimos in sinonimosCopiar)
                            {
                                int sinonimoId = dac.ObtenMaximoIdRelaciones("Sinonimos");
                                itemSinonimos.IDTema = sinonimoId;
                                itemSinonimos.IDPadre = nuevoId;
                                dac.GeneraNuevoSinonimo(itemSinonimos,IDMateria);
                                expresionesCopiar = dac.GetAllExpresionForTema(item, "Expresion.IdExpresion");
                                ;
                                foreach (ExpresionTO itemExpresion in expresionesCopiar)
                                {
                                    itemExpresion.Id = dac.ObtenMaximoIDExpresion() + 3;
                                    itemExpresion.IDTema = nuevoId;
                                    dac.InsertaExpresion(itemExpresion,IDMateria);
                                }
                            }
                            List<SinonimoTO> relacionesCopiar = dac.GetRPForTema(item, "Sinonimos.Descripcion",IDMateria);
                            foreach (SinonimoTO itemSinonimos in relacionesCopiar)
                            {
                                int sinonimoId = dac.ObtenMaximoIdRelaciones("Sinonimos");
                                itemSinonimos.IDTema = sinonimoId;
                                itemSinonimos.IDPadre = nuevoId;
                                dac.GeneraNuevoSinonimo(itemSinonimos,IDMateria);
                                expresionesCopiar = dac.GetAllExpresionForTema(item, "Expresion.IdExpresion");
                                ;
                                foreach (ExpresionTO itemExpresion in expresionesCopiar)
                                {
                                    itemExpresion.Id = dac.ObtenMaximoIDExpresion() + 3;
                                    itemExpresion.IDTema = nuevoId;
                                    dac.InsertaExpresion(itemExpresion,IDMateria);
                                }
                            }
                            ids[item] = nuevoId;
                        }
                    }
                }
            }
        }

        private TemaTO copiaValoresTema(TemaTO temaTO, int nuevoId, int Padre, int IdMateria)
        {
            TemaTO resultado = new TemaTO(nuevoId, temaTO.Descripcion, temaTO.DescripcionStr,
                0, Padre, 0, DateTime.Now,DateTime.Now,temaTO.Nota,temaTO.Observaciones,IdMateria, STATUS_NUEVO,temaTO.IdOrigen);
            return resultado;
        }

        private List<int> EncuentraAProcesar(Dictionary<int, int> ids,
            List<int> procesados, List<TemaTO> temas)
        {
            List<int> resultado = new List<int>();
            foreach (TemaTO item in temas)
            {
                if (!ids.ContainsKey(item.IDPadre))
                {
                    resultado.Add(item.IDTema);
                }
                else
                {
                    if (ids[item.IDPadre] != item.IDPadre)
                    {
                        resultado.Add(item.IDTema);
                    }
                }
            }
            return resultado;
        }

        internal void EstableceUsuario(int usuario)
        {
            TesauroDAC.UsuarioActual = usuario;
        }

        public List<StatusTO> GetStatus()
        {
            TesauroDAC temas = new TesauroDAC();
            return temas.GetStatus();
        }

        internal void InsertaObservaciones(ObservacionTO obs)
        {
            TesauroDAC temas = new TesauroDAC();
            long Maximo = temas.ObtenMaximoObservacion();
            obs.Id = Maximo + 1;
            temas.InsertaObservacion(obs);
        }

        public List<ObservacionTO> ObtenObservaciones(int Id)
        {
            TesauroDAC temas = new TesauroDAC();
            return temas.ObtenObservaciones(Id);
        }

        public void NuevaRelacionTemaTesis(TemaTO tema, TesisTO tesis)
        {
            RelTemasTesisDac temas = new RelTemasTesisDac();
            temas.NuevaRelacionTemaTesis(tema, tesis);
        }

        public List<int> ObtenTesisRelacionadasPorTema(TemaTO tema)
        {
            RelTemasTesisDac tesis = new RelTemasTesisDac();
            return tesis.ObtenTesisRelacionadasPorTema(tema);
        }

        public void RemueveRelacionTemaTesis(TemaTO tema, int tesis)
        {
            RelTemasTesisDac dac = new RelTemasTesisDac();
            dac.RemueveRelacionTemaTesis(tema, tesis);
        }

        public void RemueveRelacionTemaTesis(TemaTO tema)
        {
            RelTemasTesisDac dac = new RelTemasTesisDac();
            dac.RemueveRelacionTemaTesis(tema);
        }

        public void SetTemaQueImporta(int idTema, int idMateria, int idTemaQueImporta)
        {
            TesauroDAC dac = new TesauroDAC();
            dac.SetTemaQueImporta(idTema, idMateria, idTemaQueImporta);
        }

        public int VerificaExistenciaRelacion(String registroIus, int idMateria)
        {
            RelTemasTesisDac dac = new RelTemasTesisDac();
            return dac.VerificaExistenciaRelacion(registroIus, idMateria);
        }

        public void SetBitacora(int idTema, int idSeccion, int idMovimiento, int idUsuario, String edoAnterior, String edoActual,int idMateria)
        {
            TesauroDAC dac = new TesauroDAC();
            dac.SetBitacora(idTema, idSeccion, idMovimiento, idUsuario, edoAnterior, edoActual,idMateria);
        }
    }
}