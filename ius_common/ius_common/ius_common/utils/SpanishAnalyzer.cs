using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

namespace mx.gob.scjn.ius_common.utils
{
    public sealed class SpanishAnalyzer : Analyzer
    {

        private System.Collections.Hashtable stopWords;

        public static readonly System.String[] SPANISH_STOP_WORDS = new System.String[] {
            //"el","la","las", "le","lo", "los", 
            //"no", ".",
            //"pero", "puede",
            //"se", "sus"
            "."
            /*
            "a", "al", "aquel", "aun", "cada", "como", "con", "cual", 
            "de", "debe", "deben", "del", "el",
            "en", "este", "esta",
            "la", "las", "le", "lo", "los", 
            "no", "o", 
            "para", "pero", "por", "puede", 
            "que",
            "se", "sin", "sus",
            "un", "una", "y" */};

        public SpanishAnalyzer()
        {
            stopWords = StopFilter.MakeStopSet(SPANISH_STOP_WORDS);
        }



        public override TokenStream TokenStream(System.String fieldName, System.IO.TextReader reader)
        {

            

            return new LowerCaseFilter(new ISOLatin1AccentFilter(new StopFilter(new StandardTokenizer(reader), stopWords)));


                   
        }
    }
}
