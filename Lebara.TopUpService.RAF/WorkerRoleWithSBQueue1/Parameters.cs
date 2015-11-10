using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRoleWithSBQueue1
{
    public class Parameters
    {
        public Parameters()
        {
            to = new List<string>();
        }
        public List<string> to { get; set; }
        public string from { get; set; }
        public string message { get; set; }
    }
}
