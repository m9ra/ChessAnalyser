using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules
{
    public enum BoardFile { a = 1, b, c, d, e, f, g, h };

    public enum BoardRank { _1 = 1, _2, r3, _4, _5, _6, _7, _8 };

    /// <summary>
    /// Represents square of the chessboard. Squares are treated as singletons.
    /// </summary>
    public class Square
    {
        /// <summary>
        /// Rank of the square.
        /// </summary>
        public readonly BoardRank Rank;

        /// <summary>
        /// File on the board.
        /// </summary>
        public readonly BoardFile File;

        /// <summary>
        /// Determine whether the square lies out of the board.
        /// </summary>
        public readonly bool IsOutOfBoard;

        public static Square operator +(Square square, MoveDirection direction)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gives square shifted allong given offsets.
        /// </summary>
        /// <param name="fileOffset">File offset.</param>
        /// <param name="rowOffset">Row offset.</param>
        /// <returns>The shifted square.</returns>
        internal Square Shift(int fileOffset, int rowOffset)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses square from string.
        /// </summary>
        /// <param name="squareRepresentation">Representation of the square.</param>
        /// <returns>The parsed square.</returns>
        internal static Square FromString(string squareRepresentation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses rank from char.
        /// </summary>
        /// <param name="charRepresentation">Representation of the rank.</param>
        /// <returns>The parsed rank.</returns>
        internal static BoardRank RankFrom(char charRepresentation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses file from char.
        /// </summary>
        /// <param name="charRepresentation">Representation of the file.</param>
        /// <returns>The parsed file.</returns>
        internal static BoardFile FileFrom(char charRepresentation)
        {
            throw new NotImplementedException();
        }
    }
}
