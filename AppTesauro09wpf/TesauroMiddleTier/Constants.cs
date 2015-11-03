using System;
using System.Linq;

namespace TesauroUtilities
{
    public class ConstantsMT
    {
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
        ///     que se trata de buscar tanto jurisprudencias como tesis.
        /// </summary>
        /// <remarks>
        ///     <seealso cref="BUSQUEDA_PALABRA_JURIS"/>
        ///     <seealso cref="BUSQUEDA_PALABRA_TESIS"/>
        ///     <seealso cref="BUSQUEDA_PALABRAS_ALMACENADA"/>
        /// </remarks>
        public const int BUSQUEDA_PALABRA_AMBAS = 0;

        public const int CANCELAR = 0;
        public const int MOVER = 1;
        public const int ACEPTAR = 1;
        public const int ATENDER = 2;
        public const int REVISAR = 4;
        public const int COPIAR = 2;
        public const int IGNORAR = 4;
        public const int STATUS_NUEVO = 1;
        public const int STATUS_OBSERVADO = 2;
        public const int STATUS_ATENDIDO = 3;
        public const int STATUS_ACEPTADO = 4;
        public const int PERMISOS_BUSQUEDA=0;
        public const int PERMISO_RESTAURAR =1;
        public const int PERMISO_TEMA_NUEVO=2;
        public const int PERMISO_TEMA_MODIFICA = 3;
        public const int PERMISO_TEMA_ELIMINA = 4;
        public const int PERMISO_SINONIMO_NUEVO = 5;
        public const int PERMISO_SINONIMO_MODIFICA = 6;
        public const int PERMISO_SINONIMO_ELIMINA = 7;
        public const int PERMISO_EXPRESION_NUEVA = 8;
        public const int PERMISO_EXPRESION_MODIFICA = 9;
        public const int PERMISO_EXPRESION_ELIMINA =10;
        public const int PERMISO_RP_NUEVO = 11;
        public const int PERMISO_RP_MODIFICA = 12;
        public const int PERMISO_RP_ELIMINA = 13;
        public const int PERMISO_EJECUTAR_BUSQUEDA = 14;
        public const int TODAS_MATERIAS = 128;
        public const int MATERIAS_CONSTITUCIONAL = 1;
        public const int MATERIAS_PENAL = 2;
        public const int MATERIA_CIVIL = 4;
        public const int MATERIA_ADMINISTRATIVA =8;
        public const int MATERIA_LABORAL =16;
        public const int MATERIA_COMUN = 32;
        public const int MATERIA_DH = 64;
        public const String MENSAJE_MOVER_TEMA = "¿Está seguro de que quiere mover el tema \n";
        public const String MENSAJE_ELIMINAR = "¿Esta seguro que desea eliminar: \n";
        public const String TITULO_GENERAL = "Mantenimiento de temas";
        public const int TIPO_SELECCION_TEMA =0;
        public const int TIPO_SELECCION_SINONIMO = 1;
        public const int TIPO_SELECCION_RP = 2;
        public const int TIPO_SELECCION_IA = 3;
        public const int TIPO_SINONIMO = 1;
        public const int TIPO_RP = 0;
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
        /// Identificador y busqueda para el tesauro.
        /// </summary>
        public const string BUSQUEDA_TESIS_THESAURO = "THE_TESIS";
        public const int BUSQUEDA_PALABRA_OP_Y = 1;
        public const int BUSQUEDA_PALABRA_OP_O = 2;
        public const int BUSQUEDA_PALABRA_OP_NO = 3;
        /// <summary>
        ///     Los separadores para juntar campos en busqueda por palabras
        /// </summary>
        /// <remarks>
        ///     
        /// </remarks>
        public const String SEPARADOR_FRASES = " &&& ";
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
        public const int BUSQUEDA_PALABRA_CAMPO_LOC = 1;
        public const int BUSQUEDA_PALABRA_CAMPO_TEXTO = 2;
        public const int BUSQUEDA_PALABRA_CAMPO_RUBRO = 3;
        public const int BUSQUEDA_PALABRA_CAMPO_PRECE = 4;
        public const int BUSQUEDA_PALABRA_CAMPO_ASUNTO = 5;
        public const int BUSQUEDA_PALABRA_CAMPO_TEMA = 6;
        public const int BUSQUEDA_PALABRA_CAMPO_EMISOR = 7;
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
        public const String CADENA_VACIA = "";
        /// <summary>
        /// Los separadores comunes de las palabras.
        /// </summary>
        public static String[] SEPARADORES = new String[] { " ", ",", ".", "\n", "\"", ";", ":", "'", "´", "‘", ")", "(" };
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
        public static String[] NO_PERMITIDOS_CORREO = new String[] { "+", "=", "'", "&", "^", "$", "#",
                                 "!","¡","¿","?","<",">","~","¬","|","°",",",";",";","%","\n",
                                 "(",")","[","]","{","}","´","¨","`","¥","€", "\""};
        /// <summary>
        /// Caracteres no permitidos en una búsqueda por palabra.
        /// </summary>
        public static String[] NO_PERMITIDOS = new String[] { "+", "=", "'", "&", "^", "$", "#", "@","-","\\",
                                 "!","¡","¿","?","<",">","~","¬","|","°",".",";",";","%","\n",
                                 "(",")","[","]","{","}","´","¨","_","`","¥","€"};

    }
}
