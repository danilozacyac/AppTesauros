using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Xml;
using System.IO;
//using System.Windows;
using System.Reflection;

namespace mx.gob.scjn.ius_common.utils
{

    ///<summary>
    /// Constantes relacionadas con el IUS, en cualquiera de sus dos versiones.
    /// </summary>
    /// <remarks author="Carlos de Luna Sáenz"/>
    /// 
    public class IUSConstants
    {
        public const String TESIS_RUBRO = "TESIS_RUBRO";
        public const String TESIS_TEXTO = "TESIS_TEXTO";
        public const String TESIS_LOC = "TESIS_LOC";
        public const String TESIS_PREC = "TESIS_PREC";
        public const String EJE_TEXTO = "EJE_TEXTO";
        public const String EJE_ASUNTO = "EJE_ASUNTO";
        public const String EJE_PREC = "EJE_PREC";
        public const String EJE_TEMA = "EJE_TEMA";
        public const String VOTO_LOC = "VOTO_LOC";
        public const String VOTO_EMI = "VOTO_EMI";
        public const String VOTO_TEX = "VOTO_TEX";
        public const String VOTO_ASU = "VOTO_ASU";
        public const String ACUE_LOC = "ACUE_LOC";
        public const String ACUE_TEM = "ACUE_TEM";
        public const String ACUE_TEX = "ACUE_TEX";
        public const String PIE_PAGINA = "PIE_PAGINA";
        public const String PONER_ENCABEZADO = "PONER_ENCABEZADO";
        public const String TAM_LETRA = "TAM_LETRA";
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
        /// Define cuantos registros devuelve el paginador en referencia a una consulta determinada.
        /// </summary>
        public static String IUS_RUTA_ANEXOS { get { return GetIusRutaAnexos(); } }

        private static String GetIusRutaAnexos()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("RUTAANEXOS");
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
        ///     Las tablas de tematico que son manuales.
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        public const String TEMATICA_MANUAL = "ELE IJA SAR DRH";
        /// <summary>
        ///     Representa el valor entero cuando hay una búsqueda almacenada
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        public const int BUSQUEDA_PALABRA_ALMACENADA = 3;
        /// <summary>
        /// El largo del panel de epocas.
        /// </summary>
        /// 
        public const int EPOCAS_LARGO = 10;
        /// <summary>
        /// El ancho del panel de epocas.
        /// </summary>
        public const int EPOCAS_ANCHO = 7;
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
        public const int APENDICES_LARGO = 6;

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
            String PathDirectorio = (String)clase.GetValue("DIRECCION_INDEXER");
            return PathDirectorio;
        }
        /// <summary>
        /// Dirección donde se encuentra el Indizado para los acuerdos.
        /// </summary>
        public static String DIRECCION_INDEX_ACUERDOS { get { return GetDireccionIndexAcuerdos(); } }

        private static String GetDireccionIndexAcuerdos()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("DIRECCION_INDEX_ACUERDOS");
            return PathDirectorio;
        }
        /// <summary>
        /// Dirección del Indizado de las ejecutorias.
        /// </summary>
        public static String DIRECCION_INDEX_EJECUTORIAS { get { return GetDireccionIndexEjecutorias(); } }

        private static String GetDireccionIndexEjecutorias()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("DIRECCION_INDEX_EJECUTORIAS");
            return PathDirectorio;
        }
        /// <summary>
        /// Dirección del Indizado de las ejecutorias.
        /// </summary>
        public static String DIRECCION_INDEX_VOTOS { get { return GetDireccionIndexVotos(); } }

        private static String GetDireccionIndexVotos()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("DIRECCION_INDEX_VOTOS");
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
        /// <summary>
        /// Establecimiento de las conexionaes a la base de datos Access
        /// </summary>
        public static String IUS_DATABASE_ACCESS { get { return GetIusDatabaseAccess(); } }

        private static String GetIusDatabaseAccess()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("IUS_DATABASE_ACCESS");
            if ((PathDirectorio == null) || (PathDirectorio.Equals("")))
            {
                return IUSConstants.CADENA_VACIA;
            }
            String Directorio = PathDirectorio.Substring(PathDirectorio.IndexOf("Data Source=") + 12);
            Directorio = Directorio.Substring(0, Directorio.Length - 3)+"mdw";
            /***************************************************************************************************
             * *************************        IUS                *********************************************
             * *************************************************************************************************/
            PathDirectorio += ";User Id=LECTOR;Password=JURISPRUDENCIA;Jet OLEDB:System database=" + Directorio;
            /***************************************************************************************************
             * *************************       Migracion Temático  *********************************************
             * *************************************************************************************************/
            //PathDirectorio += ";User Id=Desarrollo;Password=PROGRAMADORES2;Jet OLEDB:System database=" + Directorio;
            return PathDirectorio;
        }
        public static String IUS_DATABASE_ACCESS_EVA { get { return GetIusDatabaseAccessEVA(); } }
        /// <summary>
        /// Base de datos para Ejecutoria, votos y acuerdos
        /// </summary>
        /// <returns></returns>
        private static string GetIusDatabaseAccessEVA()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("IUS_DATABASE_ACCESS_EVA");
            if ((PathDirectorio == null) || (PathDirectorio.Equals("")))
            {
                return IUSConstants.CADENA_VACIA;
            }
            String Directorio = PathDirectorio.Substring(PathDirectorio.IndexOf("Data Source=") + 12);
            Directorio = Directorio.Substring(0, Directorio.Length - 3) + "mdw";
            PathDirectorio += ";User Id=LECTOR;Password=JURISPRUDENCIA;Jet OLEDB:System database=" + Directorio;
            return PathDirectorio;
        }
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
            RegistryKey clase = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER, true);
            String PathDirectorio = (String)clase.GetValue("USUARIOS_DATABASE_ACCESS");
            if (PathDirectorio.ToLower().EndsWith("busquedasalmacenadas.mdb"))
            {
                PathDirectorio = PathDirectorio.ToLower().Replace("busquedasalmacenadas.mdb", "busquedasalmacenadas2012.mdb");
                clase.SetValue("USUARIOS_DATABASE_ACCESS", PathDirectorio);
            }
            clase.Close();
            return PathDirectorio;
        }
        private static String GetUsuariosDatabaseSql()
        {
            RegistryKey clase = Registry.CurrentUser.OpenSubKey(IUSConstants.IUS_REGISTRY_USER);
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
#if IUS_FOR_APPS
            String FileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //String Directory = (new FileInfo(FileName)).DirectoryName;
            FileStream Archivo = new FileStream(FileName+"\\BD.xml",FileMode.Open,FileAccess.Read);
            XmlReader rdr = new XmlTextReader(Archivo);
            rdr.Read();
            return rdr.ReadElementString("BD");
#else
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("IUS_DATABASE_SQL");
            return PathDirectorio;
#endif
        }

        /// <summary>
        /// Establecimiento de las conexionesa a BD del paginador
        /// </summary>
        public static String PAGINADOR_BD { get { return GetPaginadorBD(); } }
        /// <summary>
        /// Base de datos de paginador cuando se conecta por SQL
        /// </summary>
        /// <returns></returns>
        private static String GetPaginadorBD()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("PAGINADOR_BD");
            return PathDirectorio;
        }

        /// <summary>
        /// Establecimiento denumero de registros para guardar con el paginador
        /// </summary>
        public static int PAGINADOR_INSERT_RECS { get { return GetPaginadorInsertRecs(); } }
        /// <summary>
        /// Base de datos cuando se conecta por SQL
        /// </summary>
        /// <returns></returns>
        private static int GetPaginadorInsertRecs()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("PAGINADOR_INSERT_RECS");
            return Int32.Parse(PathDirectorio);
        }

        /// <summary>
        /// La llave del registro de Windows donde esta los datos de configuración
        /// personalizable del IUS
        /// </summary>
        /******************************* IUS USB ****************************************/
        //public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\ius2012-2-USB\General";
        /******************************* Apendice 2010 **********************************/
        //public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\APENDICE2010\GENERAL";
        /******************************* IUS CJF *********************************/
        public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\ius2011-1-08\General";
        /******************************* IUS WCF ****************************************/
        //public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\IUS2009\General";
        /*************************** IUS DVD Diciembre 2012*********************************************/
        //public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\ius2012-2\General";
        /*************************** IUS DVD Pruebas*********************************************/
        //public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\ius2012-2-02\General";
        /*************************** Migracion Tematico ******************************************/
        //public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\ius2011-1\General";
        /*************************** IUS Pruebas*********************************************/
        //public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\Pruebas\General";
        /// <summary>
        /// Llave del registro donde se guarda la configuración del usuario
        /// </summary>
        public const String IUS_REGISTRY_USER = @"SOFTWARE\SCJN\ius2009-1\General";
        //public const String IUS_REGISTRY_ENTRY = @"SOFTWARE\SCJN\ius2009-1\General";
        /// <summary>
        /// El archivo .off para tematica
        /// </summary>
        public static String IUS_TEMATICA_FILE { get { return GetTematicaFile(); } }
        /// <summary>
        /// Nombre del archivo .OFF para temática.
        /// <Remarks>
        /// Fuera de uso para el IUS, pero necesario para los programas de mantenimiento final a la BD.
        /// </Remarks>
        /// </summary>
        /// <returns></returns>
        private static String GetTematicaFile()
        {
            RegistryKey clase = Registry.LocalMachine.OpenSubKey(IUSConstants.IUS_REGISTRY_ENTRY);
            String PathDirectorio = (String)clase.GetValue("IUS_TEMATICA_FILE");
            return PathDirectorio;
        }
        /// <summary>
        /// Busqueda por Indices
        /// </summary>
        public const int BUSQUEDA_INDICES = 5;
        /// <summary>
        /// Busqueda por temas.
        /// </summary>
        public const int BUSQUEDA_TESIS_TEMATICA = 6;

        /**
         * Establece el nombre de la imagen para las contradicciones. Apagado
         */
        public static String IUS_IMAGEN_CONTRADICCION = "/ius/images/contradicion.jpg";
        /**
         * Establece el nombre de la imagen para las controversias. apagado.
         */
        public static String IUS_IMAGEN_CONTROVERSIAS = "/ius/images/controversias.jpg";
        /**
         * Establece el nombre para la imagen de las reiteraciones. apagado
         */
        public static String IUS_IMAGEN_REITERACION = "/ius/images/reiteraciones.jpg";
        /**
         * Establece el nombre para la imagen de las acciones. apagado.
         */
        public static String IUS_IMAGEN_ACCIONES = "/ius/images/acciones.jpg";
        /**
         * Etiqueta de inicio que buscaran las tablas de resultado
         */
        public static String ETIQUETA_INICIO = "inicio";
        /**
         * Una cadena vacia.
         */
        public static String CADENA_VACIA = "";
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
        public const String FILTRADO_POR_TESIS_AISLADAS_VALOR = "0";
        public const String FILTRADO_POR_TESIS_AISLADAS_COLUMNA = "tpoTesis";
        public const String FILTRADO_POR_CONTRADICCION_VALOR = "2";
        public const String FILTRADO_POR_CONTRADICCION_COLUMNA = "tpoTesis";
        public const String FILTRADO_POR_CONTROVERSIAS_VALOR = "4";
        public const String FILTRADO_POR_CONTROVERSIAS_COLUMNA = "tpoTesis";
        public const String FILTRADO_POR_REITERACIONES_VALOR = "6";
        public const String FILTRADO_POR_REITERACIONES_COLUMNA = "tpoTesis";
        public const String FILTRADO_POR_ACCIONES_VALOR = "8";
        public const String FILTRADO_POR_ACCIONES_COLUMNA = "tpoTesis";
        public const String FILTRADO_POR_TESIS_AISLADAS_CONSTANTE = "TesisAislada";
        public const String FILTRADO_POR_CONTRADICCION_CONSTANTE = "Contradiccion";
        public const String FILTRADO_POR_CONTROVERSIAS_CONSTANTE = "Controversias";
        public const String FILTRADO_POR_ACCIONES_CONSTANTE = "Accion";
        public const String FILTRADO_POR_REITERACIONES_CONSTANTE = "Reiteracion";
        public const String FILTRADO_JURISPRUDENCIA = "Jurisprudencia";
        public const String FILTRADO_NINGUNO = "Original";
        public const int BUSQUEDA_TESIS_SIMPLE = 1;
        public const int BUSQUEDA_ACUERDO = 2;
        public const int BUSQUEDA_OTROS = 8;
        public const int BUSQUEDA_EJECUTORIAS = 3;
        public const int BUSQUEDA_VOTOS = 4;
        public const int BUSQUEDA_POR_OMISION = 0;
    }
}
