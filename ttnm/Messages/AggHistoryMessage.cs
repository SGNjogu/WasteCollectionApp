using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ttnm.Messages
{
    public class AggHistoryMessage
    {
        public bool UpdateHistory { get; set; } = false;

        public bool Fetch { get; set; } = false;
    }
}
