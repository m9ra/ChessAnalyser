using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessAnalyser.Explorer.Rules;

namespace ChessAnalyser.Explorer.WebInterface.BoardChannels
{
    class BoardChannel : BoardChannelBase
    {
        /// <inheritdoc/>
        internal override void OnBoardUpdated(string boardName, Board board, List<AjaxCommand> responseCommands)
        {
            var fen = board.CurrentState.GetFEN();
            var availableMoves = getAvailableMovesIndex(board);
            var formattedHistory = board.History.Select(m => new[] { m.Source.ToString(), m.Target.ToString() });

            var responseCommand = new AjaxCommand("set_board_position")
            {
                { "board_name", boardName},
                { "current_fen", fen},
                { "available_moves", availableMoves},
                { "history", formattedHistory}
            };

            responseCommands.Add(responseCommand);
        }

        private Dictionary<string, object> getAvailableMovesIndex(Board board)
        {
            var moves = board.CurrentState.GetAvailableMoves();

            var result = moves.GroupBy(m => m.Source).ToDictionary(g => g.Key.ToString(), g => (object)g.Select(m => m.Target.ToString()).ToList());
            return result;
        }
    }
}
