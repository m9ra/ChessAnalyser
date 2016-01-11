using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules.Pieces
{
    class Bishop : Piece
    {
        /// <summary>
        /// Bishop can move along diagonals.
        /// </summary>
        private readonly RayGenerator[] _bishopGenerators = new[]{
            RayGenerator.BottomLeftGenerator,
            RayGenerator.BottomRightGenerator,
            RayGenerator.UpLeftGenerator,
            RayGenerator.UpRightGenerator,
        };

        /// <inheritdoc/>
        protected override string getPieceNotation()
        {
            return "B";
        }

        /// <inheritdoc/>
        protected override IEnumerable<Move> generateMoves(Square pieceSquare, BoardState board)
        {
            return GenerateMoves(pieceSquare, _bishopGenerators, board);
        }
    }
}
