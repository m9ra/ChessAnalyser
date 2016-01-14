using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules
{
    public enum BoardFile { a = 0, b, c, d, e, f, g, h };

    public enum BoardRank { _1 = 0, _2, r3, _4, _5, _6, _7, _8 };

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

        /// <summary>
        /// Index of represented square.
        /// </summary>
        private readonly int _squareIndex;

        #region Operation indexing

        /// <summary>
        /// Squares indexed directly according to their indexes.
        /// </summary>
        static private readonly Square[] _indexedSquares = new Square[64];

        /// <summary>
        /// Squares indexed by board coordinates.
        /// </summary>
        static private readonly Square[,] _boardSquares = new Square[8, 8];

        /// <summary>
        /// Target squares indexed by direction and starting square.
        /// </summary>
        static private readonly Square[, ,] _squareMoves = new Square[8, 8, 8];

        #endregion

        static Square()
        {
            //create squares
            var squareIndex = 0;
            for (var fileIndex = 0; fileIndex < 8; ++fileIndex)
            {
                for (var rankIndex = 0; rankIndex < 8; ++rankIndex)
                {
                    var square = new Square(squareIndex, (BoardFile)fileIndex, (BoardRank)rankIndex);
                    _indexedSquares[square._squareIndex] = square;
                    _boardSquares[fileIndex, rankIndex] = square;
                }
            }
        }

        private Square(int squareIndex, BoardFile file, BoardRank rank)
        {
            File = file;
            Rank = rank;
            _squareIndex = squareIndex;
        }

        /// <summary>
        /// Gives square shifted allong given offsets.
        /// </summary>
        /// <param name="fileOffset">File offset.</param>
        /// <param name="rankOffset">Row offset.</param>
        /// <returns>The shifted square.</returns>
        internal Square Shift(int fileOffset, int rankOffset)
        {
            var targetFile = (int)File + fileOffset;
            var targetRank = (int)Rank + rankOffset;

            //test whether coordinates lies within the board
            if (targetFile >= 8 || targetRank >= 8)
                return null;

            if (targetFile < 0 || targetRank < 0)
                return null;

            //return requested square
            return _boardSquares[targetFile, targetRank];
        }

        /// <summary>
        /// Parses square from string.
        /// </summary>
        /// <param name="squareRepresentation">Representation of the square.</param>
        /// <returns>The parsed square.</returns>
        internal static Square FromString(string squareRepresentation)
        {
            var file = FileFrom(squareRepresentation[0]);
            var rank = RankFrom(squareRepresentation[1]);

            return _boardSquares[(int)file, (int)rank];
        }

        /// <summary>
        /// Parses rank from char.
        /// </summary>
        /// <param name="charRepresentation">Representation of the rank.</param>
        /// <returns>The parsed rank.</returns>
        internal static BoardRank RankFrom(char charRepresentation)
        {
            return (BoardRank)(int)(charRepresentation - '1');
        }

        /// <summary>
        /// Parses file from char.
        /// </summary>
        /// <param name="charRepresentation">Representation of the file.</param>
        /// <returns>The parsed file.</returns>
        internal static BoardFile FileFrom(char charRepresentation)
        {
            return (BoardFile)(int)(charRepresentation - 'a');
        }

        /// <summary>
        /// Gets name of the rank.
        /// </summary>
        /// <param name="rank">The rank.</param>
        /// <returns>The name.</returns>
        internal static char GetName(BoardRank rank)
        {
            return (char)('1' + ((int)rank));
        }

        /// <summary>
        /// Gets name of the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The name.</returns>
        internal static char GetName(BoardFile file)
        {
            return (char)('a' + ((int)file));
        }

        /// <summary>
        /// Computes target square when moving one square in given direction.
        /// </summary>
        /// <param name="square">The source square.</param>
        /// <param name="direction">The direction of move.</param>
        /// <returns>The target square.</returns>
        public static Square operator +(Square square, MoveDirection direction)
        {
            var rankOffset = 0;
            var fileOffset = 0;

            switch (direction)
            {
                case MoveDirection.Bottom:
                    rankOffset = -1;
                    break;
                case MoveDirection.BottomLeft:
                    rankOffset = -1;
                    fileOffset = -1;
                    break;
                case MoveDirection.BottomRight:
                    rankOffset = -1;
                    fileOffset = 1;
                    break;
                case MoveDirection.Left:
                    fileOffset = -1;
                    break;
                case MoveDirection.Right:
                    fileOffset = 1;
                    break;
                case MoveDirection.Up:
                    rankOffset = 1;
                    break;
                case MoveDirection.UpLeft:
                    rankOffset = 1;
                    fileOffset = -1;
                    break;
                case MoveDirection.UpRight:
                    rankOffset = 1;
                    fileOffset = 1;
                    break;
            }

            //compute coordinates
            var targetFile = (int)square.File + fileOffset;
            var targetRank = (int)square.Rank + rankOffset;

            if (targetFile > 7 || targetRank > 7 || targetFile < 0 || targetRank < 0)
                //coordinates out of range
                return null;

            return _boardSquares[targetFile, targetRank];
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "" + GetName(File) + GetName(Rank);
        }
    }
}
