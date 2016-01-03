using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules
{
    public enum BoardFile { a = 1, b, c, d, e, f, g, h };

    /// <summary>
    /// Represents square of the chessboard. Squares are treated as singletons.
    /// </summary>
    class Square
    {
        /// <summary>
        /// Rank of the square.
        /// </summary>
        public readonly int Rank;

        /// <summary>
        /// File on the board.
        /// </summary>
        public readonly BoardFile File;

        public static Square operator +(Square square, MoveDirection direction)
        {
            throw new NotImplementedException();
        }
    }
}
