using ChessAnalyser.Explorer.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.WebInterface
{
    abstract class BoardChannelBase
    {
        /// <summary>
        /// Reports channel about board change. Response commands can be added.
        /// </summary>
        internal abstract void OnBoardUpdated(string boardName, Board board, List<AjaxCommand> responseCommands);
    }
}
