using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules.Pieces
{
    class Queen: PieceBase
    {
        /// <inheritdoc/>
        protected override string getPieceNotation()
        {
            return "Q";
        }

        /// <inheritdoc/>
        protected override IEnumerable<Move> generateMoves(Square pieceSquare, BoardState board)
        {
            throw new NotImplementedException();
        }
    }
}
