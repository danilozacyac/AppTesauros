using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace mx.gob.scjn.electoral_common.utils
{
    ///<summary>
    /// Constantes relacionadas con el IUS, en cualquiera de sus dos versiones.
    /// </summary>
    /// <remarks author="Carlos de Luna Sáenz"/>
    public class IUSConstants
    {
        public const String DIRECCION_CORREO = "soporte_ius@mail.scjn.gob.mx";
        /// <summary>
        /// Define cuantos registros devuelve el paginador en referencia a una consulta determinada.
        /// </summary>
        public static int BLOQUE_PAGINADOR { get { return GetBloquePaginador(); } }

        private static int GetBloquePaginador()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            Int32 PathDirectorio = Int32.Parse((String)clase.GetValue("BLOQUE_PAGINADOR"));
            return PathDirectorio;
        }
        /// <summary>
        /// Tiempo de vida de una consulta dentro del listado de consultas activas.
        /// </summary>
        public TimeSpan VIDA_DE_LA_CONSULTA
        {
            get
            {
                long tiempo = 6000000000;
                return new TimeSpan(tiempo);//"0.0:10:00"; //10 MINUTOS
            }
        }
        /// <summary>
        ///     Clausulas que debe aceptar el query mandado a la busqueda por palabras.
        /// </summary>
        /// <remarks>
        ///     Necesario para queja evision 
        /// </remarks>
        public const int CLAUSULAS = 4096;
        /// <summary>
        /// Especificador para establecer con que pueden empezar o terminar las palabras (comodín al principio o al final
        /// </summary>
        public static String EMPIEZA_CON = "+";
        /// <summary>
        /// Especificador para establecer con que pueden empezar o terminar las palabras (comodín al principio o al final
        /// </summary>
        public static String TERMINA_CON = "-";
        /// <summary>
        ///     Los separadores para juntar campos en busqueda por palabras
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        public static String[] SEPARADOR_FRASES {get{String[] resultado = new String[1]; resultado[0]=" &&& ";return resultado;}}
        /// <summary>
        ///     Representa el valor entero cuando hay una búsqueda almacenada
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        public const int BUSQUEDA_PALABRA_ALMACENADA = 3;
        /// <summary>
        /// Define si un bloque de búsqueda está o no vacio;
        /// </summary>
        public const int BLOQUE_VACIO=-1;
        /// <summary>
        /// Error de usuario ya existente
        /// </summary>
        public const int USUARIO_EXISTENTE = 2;
        /// <summary>
        /// No hubo error en la operación
        /// </summary>
        public const int NO_ERROR = 0;
        public const int ERROR_ALMACENA = 1;

        public static String IUS_SMTP_SERVER { get { return GetSmtpServer(); } }

        private static string GetSmtpServer()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("IUS_SMTP_SERVER");
            return PathDirectorio;
        }
        public static String IUS_SMTP_USER { get { return GetSmtpUser(); } }

        private static string GetSmtpUser()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("IUS_SMTP_APP_USER");
            return PathDirectorio;
        }
        public static String IUS_SMTP_PASSWD { get { return GetSmtpPasswd(); } }

        private static string GetSmtpPasswd()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("IUS_SMTP_APP_PASSWD");
            return PathDirectorio;
        }
        public static String IUS_SMTP_DOMAIN { get { return GetSmtpDomain(); } }

        private static string GetSmtpDomain()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("IUS_SMTP_APP_DOMAIN");
            return PathDirectorio;
        }
        /// <summary>
        /// Dirección donde se encuentra el Indizado.
        /// </summary>
        
        public static  String DIRECCION_INDEXER { get {return GetDireccionIndexer(); } }

        private static String GetDireccionIndexer()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("DIRECCION_INDEXER_ELE");
            return PathDirectorio;
        }
        /// <summary>
        /// Dirección donde se encuentra el Indizado para los acuerdos.
        /// </summary>
        public static String DIRECCION_INDEX_ACUERDOS { get { return GetDireccionIndexAcuerdos(); } }

        private static String GetDireccionIndexAcuerdos()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("DIRECCION_INDEX_ACUERDOS_ELE");
            return PathDirectorio;
        }
        /// <summary>
        /// Dirección del Indizado de las ejecutorias.
        /// </summary>
        public static String DIRECCION_INDEX_EJECUTORIAS { get { return GetDireccionIndexEjecutorias(); } }

        private static String GetDireccionIndexEjecutorias()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("DIRECCION_INDEX_EJECUTORIAS_ELE");
            return PathDirectorio;
        }
        /// <summary>
        /// Dirección del Indizado de las ejecutorias.
        /// </summary>
        public static String DIRECCION_INDEX_VOTOS { get { return GetDireccionIndexVotos(); } }

        private static String GetDireccionIndexVotos()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("DIRECCION_INDEX_VOTOS_ELE");
            return PathDirectorio;
        }
        /// <summary>
        /// Esta constante define cuando una tabla pertenece a una ejecutoria.
        /// </summary>
        public const int TABLA_EJECUTORIAS = 2;
        /// <summary>
        /// Esta constante define cuando una tabla pertenece a un voto.
        /// </summary>
        public const int TABLA_VOTOS = 3;
        /// <summary>
        /// Esta constante define cuando una tabla pertenece a un acuerdo.
        /// </summary>
        public const int TABLA_ACUERDOS = 4;
        /// <summary>
        /// En el tesauro constitucional establece si dentro de la tabla
        /// de sinonimos del tesauro que este es un sinonimo.
        /// </summary>
        public const int TIPO_SINONIMO_CONSTITUCIONAL = 1;
        /// <summary>
        /// En el tesauro constitucional establece si dentro del la tabal
        /// de sinonimos del tesauro que este es una proximidad.
        /// </summary>
        public const int TIPO_PROXIMIDAD_CONSTITUCIONAL = 0;
        /// <summary>
        /// Identificador y busqueda para el tesauro.
        /// </summary>
        public const string BUSQUEDA_TESIS_THESAURO = "THE_TESIS";
        /// <summary>
        /// Identificador del tipo de búsqueda para las búsquedas especiales de tesis
        /// </summary>
        public const int BUSQUEDA_ESPECIALES = 7;
        public static String IUS_DATABASE_ACCESS_ELE { get { return GetIusDatabaseAccessELE(); } }
        /// <summary>
        /// Base de datos para Elecoral
        /// </summary>
        /// <returns></returns>
        private static string GetIusDatabaseAccessELE()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("IUS_DATABASE_ACCESS_ELE");
            if ((PathDirectorio == null) || (PathDirectorio.Equals("")))
            {
                return IUSConstants.CADENA_VACIA;
            }
            String Directorio = PathDirectorio.Substring(PathDirectorio.IndexOf("Data Source=") + 12);
            Directorio = Directorio.Substring(0, Directorio.Length - 3) + "mdw";
            PathDirectorio += ";User Id=LECTOR;Password=JURISPRUDENCIA;Jet OLEDB:System database=" + Directorio;
            return PathDirectorio;
        }
        /// <summary>
        /// Base de datos que tiene las búsquedas almacenadas.
        /// </summary>
        public static String USUARIOS_DATABASE_ACCESS { get { return GetUsuariosDatabaseAccess(); } }
        public static String USUARIOS_DATABASE_SQL { get { return GetUsuariosDatabaseSql(); } }

        private static String GetUsuariosDatabaseAccess()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("USUARIOS_DATABASE_ACCESS");
            return PathDirectorio;
        }
        private static String GetUsuariosDatabaseSql()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("USUARIOS_DATABASE_SQL");
            return PathDirectorio;
        }

        //public const String IUS_DATABASE_SQL = "Data Source=CTINF-21152-CLS\\SQLEXPRESS;Initial Catalog=IUS_SqlDVD08;Integrated Security=True";
        /// <summary>
        /// Establecimiento de las conexionesa a SQLServer
        /// </summary>
        public static String IUS_DATABASE_SQL { get { return GetIusDataBaseSQL(); } }
        /// <summary>
        /// Base de datos cuando se conecta por SQL
        /// </summary>
        /// <returns></returns>
        private static String GetIusDataBaseSQL()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("IUS_DATABASE_SQL_ELE");
            return PathDirectorio;
        }
        /// <summary>
        /// La llave del registro de Windows donde esta la ruta
        /// de los archivos .OFF
        /// </summary>
        public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\ius2009-2\General";
        //public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\ius2009\GeneralPrueba20092";
        //public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\IUS2009\General";
        /// <summary>
        /// Busqueda por Indices
        /// </summary>
        public const int BUSQUEDA_INDICES = 5;
        /// <summary>
        /// Busqueda por temas.
        /// </summary>
        public const int BUSQUEDA_TESIS_TEMATICA = 6;
        /**
         * Etiqueta de inicio que buscaran las tablas de resultado
         */
        public static String ETIQUETA_INICIO = "inicio";
        /**
         * Una cadena vacia.
         */
        public static String CADENA_VACIA = String.Empty;
        /**
         * Especifica cuando se mando una orden a la tabla para hacer un ordenamiento.
         */
        public static String ORDER_BY = "orderBy";
        /**
         * Especifica si se realizará un filtrado por alguna condición.
         */
        public static String FILTER_BY = "filterBy";
        /**
         * Especifica el ordenamiento por omisión.
         */
        public static String ORDER_DEFAULT = "ConsecIndx";
        /// <summary>
        /// Ordenamiento por rubro
        /// </summary>
        public const String ORDER_RUBRO = "rubro";
        /**
         * Especifica como se ordenarán las columnas.
         */
        public static String ORDER_TYPE_DEFAULT = "asc";
        /**
         * Especifica la etiqueta de inicio.
         */
        public static String BEGIN_LABEL = "inicio";
        /**
         * Especifica la etiqueta de fin.
         */
        public static String LAST_LABEL = "fin";
        /**
         * Especifica la etiqueta del siguiente registro.
         */
        public static String NEXT_LABEL = "siguiente";
        /**
         * Etiqueta del registro anterior.
         */
        public static String PREV_LABEL = "anterior";
        /**
         * Orden recibida para ir a una página.
         */
        public static String TABLE_GO_PAGE = "goPage";
        /**
         * Orden recibida para cambiar el numero de registros
         * por página
         */
        public static String CHANGE_ROW_SCROLL = "changeRowScroll";
        /**
         * Propiedad de la forma que contiene el nuevo scroll row number.
         */
        public static String NEW_SCROLL_ROW_NUMBER = "NewScrollRowNumber";
        /**
         * Define cual es el la propiedad que tiene los datos en que la sesion guarda
         * el Scroll row number definido por el usuario
         */
        public static String USER_SCROLL_ROW_NUMER = "userScrollNumber";
        /**
         * Orden de ir a un registro en específico.
         */
        public static String GO_REGISTER = "goRegister";
        /**
         * Añadidura al campo de GO_REGISTER
         */
        public static String GO_REGISTER_FIELD_POSTFIX = "GoRegister";
        /**
         * Prefijo para busqueda por palabra.
         */
        public static String BUSCA_PALABRA_PREFIX = "BP";
        /**
         * Prefijo de una consulta de tesis.
         */
        public static String BUSCA_TESIS_PREFIX = "T";
        /**
         * Prefijo de una busqueda por Id.
         */
        public String BUSCA_ID_PREFIX = "BPI";
        /**
         * Propiedad de sesión para guardar la palabra buscada.
         */
        public static String PALABRA_BUSCADA = "resultadoTesisPalabra";
        /**
         * Propiedad de sesion para guardar la palabra con las secciones a buscar.
         */
        public static String PALABRA_BUSCADOR = "tesisPalabrasParaBuscador";
        /**
         * Nombre del parámetro de la búsqueda especial
         */
        public static String BUSQUEDA_ESPECIAL_TESIS = "consultaEspecial";
        /**
         * Prefijo para indicar la Búsqueda Especial.
         */
        public static String BUSQUEDA_ESPECIAL_PREFIX = "BE";
        /**
         * Tipo de Búsqueda.
         */
        public static String TIPO_BUSQUEDA_PREFIX = "TB";
        /**
         * Párametro para la búsqueda por Indices.
         */
        public static String PARAMETRO_BUSQUEDA_INDICE = "cKey";
        /**
         * Ordenamiento por omisión cuando la búsqueda es por Indice.
         */
        public static String ORDER_INDICE = "consecInst";
        /**
         * Ordenamiento por omisión cuando la búsqueda es por Indice.
         */
        public static String ORDER_INDICE_LETRA = "consecLetra";
        /**
         * Tipo de búsqueda por Indices.
         */
        public static String TIPO_BUSQUEDA_INDICES_PREFIX = "TBIND";
        /**
         * Busqueda de Indices Secuenciales.
         */
        public static String BUSQUEDA_INDICES_SECUENCIAL = "secuencial:";
        /**
         * Búsqueda de Índices Materias
         */
        public static String BUSQUEDA_INDICES_MATERIA = "materia:";
        /**
         * Búsqueda de Índices Tribunales Colegiados
         */
        public static String BUSQUEDA_INDICES_TRIBUNALES = "tribunal:";
        /**
         * Búsqueda de Índices Tribunales Colegiados, el prefijo del ID
         */
        public static String BUSQUEDA_INDICES_TRIBUNALES_PREFIX = "T";
        /**
         * Cadena tipo Jason para iniciar un objeto "ResultSet"
         */
        public static String JSON_RESULT_SET = "{\"ResultSet\":";
        /**
         * Cadena de un ResultSet vacio en JSON
         */
        public static String JSON_RESULT_SET_VACIO = "{ResultSet:\"\\n\"}";
        /**
         * Cadena que indica el frame de la derecha donde exista.
         */
        public static String FRAME_DERECHA = "rightFrame";
        /**
         * En la Busqueda de Indice el prefijo de Secuencial
         */
        public static String BUSQUEDA_INDICES_SECUENCIAL_PREFIX = "S";
        /**
         * En la Búsqueda de Indice el prefijo de Materia
         */
        public static String BUSQUEDA_INDICES_MATERIA_PREFIX = "M";
        /**
         * URL para el resultado cuando es una busqueda por indice
         */
        public static String BUSQUEDA_INDICES_URL = "/faces/TablaResultados.jsp?tipoBusquedaTipoDocumento=exito&tipoDocumento=1&tipoBusqueda=1&cKey=";

        public const int BUSQUEDA_PALABRA_TESIS = 2;
        public const int BUSQUEDA_PALABRA_JURIS = 1;
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
        public const int BUSQUEDA_TESIS_SIMPLE = 1;
        public const int BUSQUEDA_ACUERDO = 2;
        public const int BUSQUEDA_EJECUTORIAS = 3;
        public const int BUSQUEDA_VOTOS = 4;
        public const int BUSQUEDA_POR_OMISION = 0;
    }
}
