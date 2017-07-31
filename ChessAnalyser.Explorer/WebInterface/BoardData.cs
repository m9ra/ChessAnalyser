using ChessAnalyser.Explorer.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.WebInterface
{
    class BoardData
    {
        /// <summary>
        /// Board represented by the data.
        /// </summary>
        internal readonly string BoardName;

        /// <summary>
        /// Current state of the board.
        /// </summary>
        private Board _currentBoardState = new Board();

        /// <summary>
        /// Channels attached to the board.
        /// </summary>
        private readonly IEnumerable<BoardChannelBase> _channels;

        internal BoardData(string boardName, IEnumerable<BoardChannelBase> boardChannels)
        {
            BoardName = boardName;
            _channels = boardChannels.ToArray();
        }

        /// <summary>
        /// Updates board state while handling response commands.
        /// </summary>
        internal void UpdateBoard(Move[] moves, List<AjaxCommand> responseCommands)
        {
            if (moves.Length > 0)
            {
                //otherwise we will keep last board state
                _currentBoardState = new Board();
                _currentBoardState.MakeMoves(moves);
            }

            foreach (var channel in _channels)
            {
                channel.OnBoardUpdated(BoardName, _currentBoardState, responseCommands);
            }
        }
    }
}
