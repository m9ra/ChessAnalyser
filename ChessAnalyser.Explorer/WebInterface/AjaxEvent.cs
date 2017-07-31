using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.WebInterface
{
    class AjaxEvent : Dictionary<string, object>
    {
        internal AjaxEvent(Dictionary<string, object> data)
            : base(data)
        {
        }
    }
}
