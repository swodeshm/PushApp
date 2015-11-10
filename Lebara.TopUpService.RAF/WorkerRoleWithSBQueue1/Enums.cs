using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRoleWithSBQueue1
{
    public class Enums
    {
        public enum HttpVerb
        {
            GET,
            POST,
            PUT,
            DELETE
        }
        public enum CountryCode
        {
            [System.ComponentModel.Description("Unknown")]
            unknown = 0,
            [System.ComponentModel.Description("Germany")]
            DE = 49,
            [System.ComponentModel.Description("U.K.")]
            GB = 44,
            [System.ComponentModel.Description("France")]
            FR = 33,
            [System.ComponentModel.Description("Spain")]
            ES = 34,
            [System.ComponentModel.Description("Australia")]
            AU = 61,
            [System.ComponentModel.Description("Switzerland")]
            CH = 41,
            [System.ComponentModel.Description("Denmark")]
            DK = 45,
            [System.ComponentModel.Description("Netherlands")]
            NL = 31,
        };
        public enum Locale
        {
            [System.ComponentModel.Description("Unknown")]//---------- English
            Unknown = 0,
            [System.ComponentModel.Description("en-GB")]//---------- English
            en_GB = 1,
            [System.ComponentModel.Description("pl-PL")]//--------------Polish
            pl_PL = 2,
            [System.ComponentModel.Description("fr-FR")]//-----------French
            fr_FR = 3,
            [System.ComponentModel.Description("de-DE")]//----------German
            de_DE = 4,
            [System.ComponentModel.Description("nl-NL")]//------------Dutch
            nl_NL = 5,
            [System.ComponentModel.Description("da-DK")]//----------Danish
            da_DK = 6,
            [System.ComponentModel.Description("es-ES")]//---------Spanish
            es_ES = 7,
            [System.ComponentModel.Description("tr-TR")]//---------Turkish
            tr_TR = 8,
            [System.ComponentModel.Description("ru-RU")]//---------Russian
            ru_RU = 9,
            [System.ComponentModel.Description("en-AU")]//---------English-Australia
            en_AU = 10,
            [System.ComponentModel.Description("de-CH")]//---------English-Australia
            de_CH = 11,
            [System.ComponentModel.Description("it-CH")]//---------Italian-Swiss
            it_CH = 12,
            [System.ComponentModel.Description("de-RO")]//---------Italian-Swiss
            de_RO = 13

        }
    }
}
