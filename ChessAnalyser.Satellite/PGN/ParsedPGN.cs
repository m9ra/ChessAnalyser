using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Satellite.PGN
{
    public class ParsedPGN
    {
        /// <summary>
        /// Moves of white.
        /// </summary>
        private readonly List<string> _whiteMoves = new List<string>();

        /// <summary>
        /// Moves of black.
        /// </summary>
        private readonly List<string> _blackMoves = new List<string>();

        /// <summary>
        /// Count of stored moves.
        /// </summary>
        public int Count { get { return _whiteMoves.Count; } }

        internal ParsedPGN(IEnumerable<string> whiteMoves, IEnumerable<string> blackMoves)
        {
            _whiteMoves.AddRange(whiteMoves);
            _blackMoves.AddRange(blackMoves);
        }

        /// <summary>
        /// Gets white move at given index.
        /// </summary>
        /// <param name="moveIndex">The index of the move.</param>
        /// <returns>The requested move.</returns>
        public string GetWhiteMove(int moveIndex)
        {
            if (moveIndex >= _whiteMoves.Count)
                //we are out of moves
                return null;

            return _whiteMoves[moveIndex];
        }

        /// <summary>
        /// Gets black move at given index.
        /// </summary>
        /// <param name="moveIndex">The index of the move.</param>
        /// <returns>The requested move.</returns>
        public string GetBlackMove(int moveIndex)
        {
            if (moveIndex >= _blackMoves.Count)
                //we are out of moves
                return null;

            return _blackMoves[moveIndex];
        }
    }
}
