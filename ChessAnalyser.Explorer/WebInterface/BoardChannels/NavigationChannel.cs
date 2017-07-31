using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessAnalyser.Explorer.Rules;

namespace ChessAnalyser.Explorer.WebInterface.BoardChannels
{
    class NavigationChannel : BoardChannelBase
    {
        internal override void OnBoardUpdated(string boardName, Board board, List<AjaxCommand> responseCommands)
        {
            var history = board.History;


            var auxBoard = new Board();
            var shortMovesHistory = new List<ShortMove>();
            foreach (var move in history)
            {
                shortMovesHistory.Add(new ShortMove(move, auxBoard.CurrentState));
                auxBoard.MakeMove(move);
            }

            var info = string.Join("</br>\n", shortMovesHistory.ToList());

            var responseCommand = new AjaxCommand("set_board_navigation")
            {
                { "info", info},
                { "board_name", boardName},
            };

            responseCommands.Add(responseCommand);
        }
    }
}
