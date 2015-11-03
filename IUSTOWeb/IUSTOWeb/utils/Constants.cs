using System;
using System.Linq;
using Microsoft.Win32;

namespace mx.gob.scjn.ius_common.utils
{
    public class Constants
    {
#if STAND_ALONE
        public static String VistaPreliminar="Vista preliminar";

        public static int FONTSIZE
        {
            get
            {
                return getFontSize();
            }
        }

        private static int getFontSize()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
            int resultado = 12;// Int32.Parse((String)reg.GetValue(IUSConstants.TAM_LETRA));
            return resultado;
        }

#else
        public const int FONTSIZE =11;
#endif
        public const int ANCHO_BOTONES = 150;
        public const String VOTO_CONCURRENTE = "7";
        public const String VOTO_PARALELO = "8";
        public const String VOTO_MINORITARIO = "3";
        public const String VOTO_PARTICULAR = "4";
        /// <summary>
        /// Etiqueta para el boton de "Todos"
        /// </summary>
        public const String TODOS_LABEL = "Todo";
        /// <summary>
        /// El número de tesis que el servicio o fachada deberán regresar por cada bloque de paginación
        /// </summary>
        public const int NUMERO_TESIS_PAGINA = 150;
        /// <summary>
        /// Titulo para la ventana de Consultas Almacenadas
        /// </summary>
        public const String CONSULTAS_ALMACENADAS_TITULO = "Almacenar consulta";
        /// <summary>
        ///     En una expresión, para poder unir los campos se requiere definir la separación de frases, en este
        ///     caso es esta.
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        public const String SEPARADOR_FRASES = " &&& ";
        /// <summary>
        ///     Tamaño mínimo de Font permitido
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        public const int FONT_MENOR = 5;
        /// <summary>
        ///     Tamaño máximo del Font permitido
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        public const int FONT_MAYOR = 40;
        /// <summary>
        ///     El font para los reportes
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        public const String FONT_USAR = "Arial";
        public static double FONT_SIZE { get; set; }
        public static String BLOQUE_PAGINADOR{ get; set; }
        public static String TESIS_RUBRO{ get; set; }
        public static String TESIS_LOC{ get; set; }
        public static String TESIS_PREC{ get; set; }
        public static String TESIS_TEXTO{ get; set; }
        public static String EJE_ASUNTO{ get; set; }
        public static String EJE_PREC{ get; set; }
        public static String EJE_TEMA{ get; set; }
        public static String EJE_TEXTO{ get; set; }
        public static String ACUE_LOC{ get; set; }
        public static String ACUE_TEM{ get; set; }
        public static String ACUE_TEX{ get; set; }
        public static String VOTO_ASU{ get; set; }
        public static String VOTO_EMI{ get; set; }
         public static String VOTO_LOC{ get; set; }
         public static String VOTO_TEX { get; set; }
        public const String ENCABEZADO_OMISION = "Suprema Corte de Justicia de la Nación";
#if STAND_ALONE
        public static String PIEPAGINA_OMISION
        {
            get
            {
                RegistryKey reg = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, false);
                return (String)reg.GetValue(IUSConstants.PIE_PAGINA);
            }
            set { }
        }
#else
        public static String PIEPAGINA_OMISION = "Jurisprudencias y Tesis Aisladas IUS";
#endif
        /// <summary>
        ///     La descripción  para la búsqueda Almacenada
        /// </summary>
        /// <remarks>
        ///   
        /// </remarks>
        public const String BUSQUEDA_ALMACENADA = "Búsqueda Almacenada";
        /// <summary>
        /// La llave del registro de Windows donde esta la ruta
        /// de los archivos .OFF
        /// </summary>
        public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\ius2009-2\General";
        //public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\ius2009\General";
        //public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\ius2009\GeneralPrueba20092";
        /// <summary>
        /// Filtro en la tabla de resultado sobre si es o no jurisprudencia y que tipo
        /// de jurisprudencia es
        /// </summary>
        public const int IUS_FILTRO_JURIS = 1;
        /// <summary>
        /// Filtro en la tabla de resultados sobre un asunto determinado.
        /// </summary>
        public const int IUS_FILTRO_ASUNTO = 2;
        /// <summary>
        /// Filtro sobre un ponente.
        /// </summary>
        public const int IUS_FILTRO_PONENTE = 3;

        
        /// <summary>
        /// El campo por el cual se tiene que ordenar cuando se busca que el
        /// orden sea por promovente
        /// </summary>
        public const String ORDENAR_PROMOVENTE = "promovente";
        /// <summary>
        /// El campo por el cual se tiene que ordenar cuando se busca el orden por
        /// el consecutivo.
        /// </summary>
        public const String ORDENAR_CONSEC = "consecindx";
        /// <summary>
        /// El campo por el cual se tiene que ordenar cuando se busca el orden por
        /// el asunto.
        /// </summary>
        public const String ORDENAR_ASUNTO = "rubro";
        /// <summary>
        /// El campo por el cual se tiene que ordenar cuando se busca el orden por
        /// el identificador.
        /// </summary>
        public const String ORDENAR_ID = "id";
        /// <summary>
        /// Error de usuario ya existente
        /// </summary>
        public const int USUARIO_EXISTENTE = 2;
        /// <summary>
        /// No hubo error en la operación
        /// </summary>
        public const int NO_ERROR = 0;
        /// <summary>
        /// Hubo un error al llamar a la fachada
        /// </summary>
        public const int ERROR_FACHADA = 1;
        /// <summary>
        /// Usuario utilizado en la aplicación stand Alone
        /// </summary>
        public const String USUARIO_OMISION = "defaultIUS";
        /// <summary>
        /// Caracteres no permitidos en una búsqueda por palabra.
        /// </summary>
        public static String[] NO_PERMITIDOS = new String[] { "+", "=", "'", "&", "^", "$", "#", "@","-","\\",
                                 "!","¡","¿","?","<",">","~","¬","|","°",",",";",";","%","\n",
                                 "(",")","[","]","{","}","´","¨","_","`","¥","€"};
        public static String[] NO_PERMITIDOS_CORREO = new String[] { "+", "=", "'", "&", "^", "$", "#",
                                 "!","¡","¿","?","<",">","~","¬","|","°",",",";",";","%","\n",
                                 "(",")","[","]","{","}","´","¨","`","¥","€", "\""};
        /// <summary>
        /// Los comodines permiidos en la búsqueda.
        /// </summary>
        public static String[] COMODINES = { "*", "?" };
        public static String EMPIEZA_CON = "|";
        public static String TERMINA_CON = "'";
        /// <summary>
        /// Las palabras que no se incluyen en busquedas o que no deben ser pintadas en los
        /// resultados de las mismas.
        /// </summary>
        public static String[] STOPERS = new String[]{"el","la","las", "le","lo", "los", "no", ".", 
            "pero", "puede","se", "sus", "y", "o", "n","a", "al", "aquel", "aun", "cada", "como", "con", "cual", 
            "de", "debe", "deben", "del", "el", "en", "este", "esta", "la", "las", "le", "lo", "los", 
            "para", "pero", "por", "puede", "que", "se", "sin", "sus", "un", "una"};
        /// <summary>
        /// Los separadores comunes de las palabras.
        /// </summary>
        public static String[] SEPARADORES = new String[] { " ", ",", ".", "\n", "\"", ";", ":", "'", "´", "‘", ")", "(" };
        /// <summary>
        ///     Titulo para la etiqueta de guardar una consulta.
        /// </summary>
        public const String TITULO_CONSULTA = "Almacenar consulta";
        /// <summary>
        /// Representa la busqueda de una tesis.
        /// </summary>
        public const int BUSQUEDA_TESIS_SIMPLE = 1;
        /// <summary>
        /// Representa que lo que se busca en un panel es un acuerdo.
        /// </summary>
        public const int BUSQUEDA_ACUERDO = 2;
        /// <summary>
        /// Lo que se busca en el panel es la ejecutoria.
        /// </summary>
        public const int BUSQUEDA_EJECUTORIAS = 3;
        /// <summary>
        /// Lo que se busca en el panel es un voto.
        /// </summary>
        public const int BUSQUEDA_VOTOS = 4;
        /// <summary>
        /// Genera una búqsqueda por omisión.
        /// </summary>
        /// <remarks deprecated>Obsoleta</remarks>
        public const int BUSQUEDA_POR_OMISION = 0;
        /// <summary>
        /// Busqueda por Indices
        /// </summary>
        public const int BUSQUEDA_INDICES = 5;
        /// <summary>
        /// Busqueda por temas.
        /// </summary>
        public const int BUSQUEDA_TESIS_TEMATICA = 6;
        /// <summary>
        ///     La busqueda es por búsquedas especiales,
        ///     sirve principalmente para las búsquedas
        ///     almacenadas.
        /// </summary>
        public const int BUSQUEDA_ESPECIALES = 7;
        /// <summary>
        /// Busqueda de otro tipo de documentos dentro del panel.
        /// </summary>
        public const int BUSQUEDA_OTROS = 8;
        /// <summary>
        /// Identificador y busqueda para el tesauro.
        /// </summary>
        public const string BUSQUEDA_TESIS_THESAURO = "THE_TESIS";
        /// <summary>
        /// El largo del panel de epocas.
        /// </summary>
        /// 
        public const int EPOCAS_LARGO = 7;
        /// <summary>
        /// El ancho del panel de epocas.
        /// </summary>
        public const int EPOCAS_ANCHO = 6;
        /// <summary>
        /// El ancho del panel de acuerdos.
        /// </summary>
        public const int ACUERDOS_ANCHO = 7;
        /// <summary>
        /// El largo del panel de Acuerdos.
        /// </summary>
        public const int ACUERDOS_LARGO = 2;
        /// <summary>
        /// El ancho del panel de apéndices.
        /// </summary>
        public const int APENDICES_ANCHO = 8;
        /// <summary>
        /// El largo del panel de apéndices.
        /// </summary>
        public const int APENDICES_LARGO = 5;
        /// <summary>
        /// Representa una cadena vacia.
        /// </summary>
        public const String CADENA_VACIA = "";
        /// <summary>
        /// Seccion de las ligas de los rubros.
        /// </summary>
        public const int SECCION_LIGAS_RUBRO = 1;
        ///<summary>
        ///Seccion de Liga para texto.
        ///</summary>
        public const int SECCION_LIGAS_TEXTO = 2;
        /// <summary>
        /// Seccion de las ligas para los precedentes.
        /// </summary>
        public const int SECCION_LIGAS_PRECEDENTES = 4;
        ///<summary>
        /// Especifica cuando se mando una orden a la tabla para hacer un ordenamiento.
        ///</summary>
        public const String ORDER_BY = "orderBy";
        /// <summary>
        /// Especifica si se realizará un filtrado por alguna condición.
        ///</summary>
        public const String FILTER_BY = "filterBy";
        /// <summary>
        /// Especifica el ordenamiento por omisión.
        ///</summary>
        public const String ORDER_DEFAULT = "ConsecIndx";
        ///
        /// Especifica como se ordenarán las columnas.
        ///
        public const String ORDER_TYPE_DEFAULT = "asc";
        /// <summary>
        /// Constante para filtrar por Tesis Aisladas.
        /// </summary>
        public const String FILTER_AISLADAS = "TesisAislada";
        /// <summary>
        /// Constante para filtrar por Jurisprudencia
        /// </summary>
        public const String FILTER_JURISPRUDENCIA = "Jurisprudencia";
        /// <summary>
        /// Constante para el filtro por contradiccion de tesis.
        /// </summary>
        public const String FILTER_CONTRADICCION = "Contradiccion";
        /// <summary>
        /// Constante para el filtro por Acciones de Inconstitucionalidad.
        /// </summary>
        public const String FILTER_ACCIONES = "Accion";
        /// <summary>
        /// Constante para el filtro por reiteraciones
        /// </summary>
        public const String FILTER__REITERACIONES = "Reiteracion";
        /// <summary>
        /// Constante para el filtro por Controversias constitucionales.
        /// </summary>
        public const String FILTER_CONTROVERSIAS = "Controversias";
        /// <summary>
        /// Especifica la etiqueta de inicio.
        ///</summary>
        public const String BEGIN_LABEL = "inicio";
        /// <summary>
        /// Especifica la etiqueta de fin.
        ///</summary>
        public const String LAST_LABEL = "fin";
        ///<summary>
        /// Especifica la etiqueta del siguiente registro.
        ///</summary>
        public const String NEXT_LABEL = "siguiente";
        ///<summary>
        /// Etiqueta del registro anterior.
        ///</summary>
        public const String PREV_LABEL = "anterior";
        /**
         * Orden recibida para ir a una página.
         */
        public const String TABLE_GO_PAGE = "goPage";
        /**
         * Orden recibida para cambiar el numero de registros
         * por página
         */
        public const String CHANGE_ROW_SCROLL = "changeRowScroll";
        /**
         * Propiedad de la forma que contiene el nuevo scroll row number.
         */
        public const String NEW_SCROLL_ROW_NUMBER = "NewScrollRowNumber";
        /**
         * Define cual es el la propiedad que tiene los datos en que la sesion guarda
         * el Scroll row number definido por el usuario
         */
        public const String USER_SCROLL_ROW_NUMER = "userScrollNumber";
        /**
         * Orden de ir a un registro en específico.
         */
        public const String GO_REGISTER = "goRegister";
        /**
         * Añadidura al campo de GO_REGISTER
         */
        public const String GO_REGISTER_FIELD_POSTFIX = "GoRegister";
        /**
         * Prefijo para busqueda por palabra.
         */
        public const String BUSCA_PALABRA_PREFIX = "BP";
        /**
         * Prefijo de una consulta de tesis.
         */
        public const String BUSCA_TESIS_PREFIX = "T";
        /**
         * Prefijo de una busqueda por Id.
         */
        public const String BUSCA_ID_PREFIX = "BPI";
        /**
         * Propiedad de sesión para guardar la palabra buscada.
         */
        public const String PALABRA_BUSCADA = "resultadoTesisPalabra";
        /**
         * Propiedad de sesion para guardar la palabra con las secciones a buscar.
         */
        public const String PALABRA_BUSCADOR = "tesisPalabrasParaBuscador";
        /**
         * Nombre del parámetro de la búsqueda especial
         */
        public const String BUSQUEDA_ESPECIAL_TESIS = "consultaEspecial";
        /**
         * Prefijo para indicar la Búsqueda Especial.
         */
        public const String BUSQUEDA_ESPECIAL_PREFIX = "BE";
        /**
         * Tipo de Búsqueda.
         */
        public const String TIPO_BUSQUEDA_PREFIX = "TB";
        /**
         * Párametro para la búsqueda por Indices.
         */
        public const String PARAMETRO_BUSQUEDA_INDICE = "cKey";
        /**
         * Ordenamiento por omisión cuando la búsqueda es por Indice.
         */
        public const String ORDER_INDICE = "consecInst";
        /**
         * Ordenamiento por omisión cuando la búsqueda es por Indice.
         */
        public const String ORDER_INDICE_LETRA = "consecLetra";
        /**
         * Tipo de búsqueda por Indices.
         */
        public const String TIPO_BUSQUEDA_INDICES_PREFIX = "TBIND";
        /**
         * Busqueda de Indices Secuenciales.
         */
        public const String BUSQUEDA_INDICES_SECUENCIAL = "secuencial:";
        /**
         * Búsqueda de Índices Materias
         */
        public const String BUSQUEDA_INDICES_MATERIA = "materia:";
        /**
         * Búsqueda de Índices Tribunales Colegiados
         */
        public const String BUSQUEDA_INDICES_TRIBUNALES = "tribunal:";
        /**
         * Búsqueda de Índices Tribunales Colegiados, el prefijo del ID
         */
        public const String BUSQUEDA_INDICES_TRIBUNALES_PREFIX = "T";
        /**
         * Cadena tipo Jason para iniciar un objeto "ResultSet"
         */
        public const String JSON_RESULT_SET = "{\"ResultSet\":";
        /**
         * Cadena de un ResultSet vacio en JSON
         */
        public const String JSON_RESULT_SET_VACIO = "{ResultSet:\"\\n\"}";
        /**
         * Cadena que indica el frame de la derecha donde exista.
         */
        public const String FRAME_DERECHA = "rightFrame";
        /**
         * En la Busqueda de Indice el prefijo de Secuencial
         */
        public const String BUSQUEDA_INDICES_SECUENCIAL_PREFIX = "S";
        
        /// <summary>
        ///     En la Búsqueda de Indice el prefijo de Materia
        /// </summary>
        public const String BUSQUEDA_INDICES_MATERIA_PREFIX = "M";
        /// <summary>
        ///     Se usa en el campo de Jurisprudencia para las búsquedas por palabras e indica
        ///     que se trata de buscar únicamente Jurisprudencia.
        /// </summary>
        /// <remarks>
        ///     <seealso cref="BUSQUEDA_PALABRA_JURIS"/>
        ///     <seealso cref="BUSQUEDA_PALABRA_AMBAS"/>
        ///     <seealso cref="BUSQUEDA_PALABRAS_ALMACENADA"/>
        /// </remarks>
        public const int BUSQUEDA_PALABRA_TESIS = 2;
        /// <summary>
        ///     Se usa en el campo de Jurisprudencia para las búsquedas por palabras e indica
        ///     que se trata de buscar únicamente tesis.
        /// </summary>
        /// <remarks>
        ///     <seealso cref="BUSQUEDA_PALABRA_TESIS"/>
        ///     <seealso cref="BUSQUEDA_PALABRA_AMBAS"/>
        ///     <seealso cref="BUSQUEDA_PALABRAS_ALMACENADA"/>
        /// </remarks>
        public const int BUSQUEDA_PALABRA_JURIS = 1;
        /// <summary>
        ///     Se usa en el campo de Jurisprudencia para las búsquedas por palabras e indica
        ///     que se trata de una busqueda almacenada, en este caso en el campo "campos" irá el
        ///     identificador de la búsqueda almacenada.
        /// </summary>
        /// <remarks>
        ///     <seealso cref="BUSQUEDA_PALABRA_JURIS"/>
        ///     <seealso cref="BUSQUEDA_PALABRA_AMBAS"/>
        ///     <seealso cref="BUSQUEDA_PALABRAS_TESIS"/>
        /// </remarks>
        public const int BUSQUEDA_PALABRA_ALMACENADA = 3;
        /// <summary>
        ///     Se usa en el campo de Jurisprudencia para las búsquedas por palabras e indica
        ///     que se trata de buscar tanto jurisprudencias como tesis.
        /// </summary>
        /// <remarks>
        ///     <seealso cref="BUSQUEDA_PALABRA_JURIS"/>
        ///     <seealso cref="BUSQUEDA_PALABRA_TESIS"/>
        ///     <seealso cref="BUSQUEDA_PALABRAS_ALMACENADA"/>
        /// </remarks>
        public const int BUSQUEDA_PALABRA_AMBAS = 0;
        public const int BUSQUEDA_PALABRA_CAMPO_LOC = 1;
        public const int BUSQUEDA_PALABRA_CAMPO_TEXTO = 2;
        public const int BUSQUEDA_PALABRA_CAMPO_RUBRO = 3;
        public const int BUSQUEDA_PALABRA_CAMPO_PRECE = 4;
        public const int BUSQUEDA_PALABRA_CAMPO_ASUNTO = 5;
        public const int BUSQUEDA_PALABRA_CAMPO_TEMA = 6;
        public const int BUSQUEDA_PALABRA_CAMPO_EMISOR = 7;
        public const int BUSQUEDA_PALABRA_OP_Y = 1;
        public const int BUSQUEDA_PALABRA_OP_O = 2;
        public const int BUSQUEDA_PALABRA_OP_NO = 3;

        public const int TESIS_AISLADAS = 0;
        public const String TPO_TESIS_AISLADA = "0";
        public const int TESIS_JURIS = 1;
        public const String TESIS_ACCIONES = "8";
        public const int TESIS_ACCIONES_INT = 8;
        public const String TESIS_CONTROVERSIAS = "4";
        public const int TESIS_CONTROVERSIAS_INT = 4;
        public const String TESIS_REITERACIONES = "6";
        public const int TESIS_REITERACIONES_INT = 6;
        public const String TESIS_CONTRADICCION = "2";
        public const int TESIS_CONTRADICCION_INT = 2;
        public const String VISTA_PRELIMINAR_FUERA = "Salir de la vista preliminar";
        public const String VISTA_PRELIMINAR = "Ver vista preliminar de impresión";
        public const String TOOLTIP_LIGAS = "Esta liga abrirá una ventana interna";
        public const String URI_SCJN = "http://www.scjn.gob.mx";
    }
}
