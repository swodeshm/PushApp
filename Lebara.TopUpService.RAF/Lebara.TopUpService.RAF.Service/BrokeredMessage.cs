using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lebara.TopUpService.RAF
{
    public class BrokeredMessageTemplate
    {
        public string ParaValue
        {
            get;
            set;
        }

        public string RefereeMSISDN
        {
            get;
            set;
        }

        public double TopUpAmount
        {
            get;
            set;
        }
    }
}
