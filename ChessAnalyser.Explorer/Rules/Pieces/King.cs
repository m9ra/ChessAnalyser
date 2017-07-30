using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules.Pieces
{
    class King : Piece
    {
        MoveDirection[] _kingDirections = Enum.GetValues(typeof(MoveDirection)).Cast<MoveDirection>().ToArray();

        /// <inheritdoc/>
        protected override string getPieceNotation()
        {
            return "K";
        }

        /// <inheritdoc/>
        protected override IEnumerable<Move> generateMoves(Square pieceSquare, BoardState board)
        {
            var moves = new List<Move>();
            foreach (var direction in _kingDirections)
            {
                var target = pieceSquare + direction;
                if (!board.IsChecked(target))
                    GenerateMove(pieceSquare, target, board, moves);
            }

            if (board.CanCastleShort && isCastleFree(pieceSquare, MoveDirection.Right, board))
            {
                //short castle
                GenerateMove(pieceSquare, pieceSquare.Shift(2, 0), board, moves);
            }

            if (board.CanCastleLong && isCastleFree(pieceSquare, MoveDirection.Left, board))
            {
                //long castle
                GenerateMove(pieceSquare, pieceSquare.Shift(-2, 0), board, moves);
            }

            return moves;
        }

        /// <summary>
        /// Determine whether castle can be made over squares in given direction.
        /// </summary>
        /// <param name="pieceSquare">Square of castling piece.</param>
        /// <param name="direction">Direction of castling.</param>
        /// <param name="board">Board where castling is done.</param>
        /// <returns><c>true</c> if no obstacle or checked square in castle area. <c>false</c> otherwise</returns>
        private bool isCastleFree(Square pieceSquare, MoveDirection direction, BoardState board)
        {
            var crossedSquare = pieceSquare + direction;
            var targetSquare = crossedSquare + direction;

            return board.IsEmpty(crossedSquare) && board.IsEmpty(targetSquare) &&
                !board.IsChecked(pieceSquare) && !board.IsChecked(crossedSquare);
        }
    }
}
