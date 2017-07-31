using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.WebInterface
{
    class AjaxCommand : Dictionary<string, object>
    {
        internal AjaxCommand(string commandName)
        {
            this["name"] = commandName;
        }
    }
}
