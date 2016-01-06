using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules.Pieces
{
    class Knight : PieceBase
    {
        private readonly int[][] _knightMoveOffsets = new[]{
            new[]{1,2},
            new[]{1,-2},
            new[]{-1,2},
            new[]{-1,-2},
            new[]{2,1},
            new[]{2,-1},
            new[]{-2,1},
            new[]{-2,-1}
        };

        /// <inheritdoc/>
        protected override string getPieceNotation()
        {
            return "N";
        }

        /// <inheritdoc/>
        protected override IEnumerable<Move> generateMoves(Square pieceSquare, BoardState board)
        {
            var moves = new List<Move>();
            foreach (var offset in _knightMoveOffsets)
            {
                generateKnightMove(offset[0], offset[1], pieceSquare, board, moves);
            }

            return moves;
        }

        private void generateKnightMove(int fileOffset, int rowOffset, Square startSquare, BoardState board, List<Move> moves)
        {
            var targetSquare = startSquare.Shift(fileOffset, rowOffset);
            GenerateMove(startSquare, targetSquare, board, moves);
        }
    }
}
