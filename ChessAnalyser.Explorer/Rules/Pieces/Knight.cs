using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules.Pieces
{
    class Knight : PieceBase
    {
        /// <inheritdoc/>
        protected override string getPieceNotation()
        {
            return "N";
        }

        /// <inheritdoc/>
        protected override IEnumerable<Move> generateMoves(Square pieceSquare, BoardState board)
        {
            throw new NotImplementedException();
        }
    }
}
