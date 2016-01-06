using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules.Pieces
{
    class Queen : PieceBase
    {
        /// <summary>
        /// Queen can move in any direction.
        /// </summary>
        private readonly RayGenerator[] _queenGenerators = new[]{
            RayGenerator.BottomGenerator,
            RayGenerator.BottomLeftGenerator,
            RayGenerator.BottomRightGenerator,
            RayGenerator.UpGenerator,
            RayGenerator.UpLeftGenerator,
            RayGenerator.UpRightGenerator,
            RayGenerator.LeftGenerator,
            RayGenerator.RightGenerator
        };

        /// <inheritdoc/>
        protected override string getPieceNotation()
        {
            return "Q";
        }

        /// <inheritdoc/>
        protected override IEnumerable<Move> generateMoves(Square pieceSquare, BoardState board)
        {
            return GenerateMoves(pieceSquare, _queenGenerators, board);
        }
    }
}
