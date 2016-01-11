using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules.Pieces
{
    class Rook : Piece
    {
        /// <summary>
        /// Queen can move along files and rows.
        /// </summary>
        private readonly RayGenerator[] _rookGenerators = new[]{
            RayGenerator.BottomGenerator,
            RayGenerator.UpGenerator,
            RayGenerator.LeftGenerator,
            RayGenerator.RightGenerator
        };

        /// <inheritdoc/>
        protected override string getPieceNotation()
        {
            return "R";
        }

        /// <inheritdoc/>
        protected override IEnumerable<Move> generateMoves(Square pieceSquare, BoardState board)
        {
            return GenerateMoves(pieceSquare, _rookGenerators, board);
        }
    }
}
