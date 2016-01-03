using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules.Pieces
{
    class Pawn : PieceBase
    {
        /// <inheritdoc/>
        protected override string getPieceNotation()
        {
            return "";
        }

        /// <inheritdoc/>
        protected override IEnumerable<Move> generateMoves(Square pieceSquare, BoardState board)
        {
            var moves = new List<Move>();

            var pawnDirection = getPawnDirection(pieceSquare, board);
            var pawn1Target = pieceSquare + pawnDirection;

            // usual pawn moves
            if (board.IsEmpty(pawn1Target))
            {
                GenerateMove(pieceSquare, pawn1Target, board, moves);

                var pawn2Target = pawn1Target + pawnDirection;
                if (board.IsEmpty(pawn2Target) && isInitialPawnPosition(pieceSquare, board))
                    //at first move pawn can go two squares ahead
                    GenerateMove(pieceSquare, pawn1Target, board, moves);
            }

            var leftTake = pawn1Target + MoveDirection.Left;
            var rightTake = pawn1Target + MoveDirection.Right;
            GenerateTakeOnlyMove(pieceSquare, leftTake, board, moves);
            GenerateTakeOnlyMove(pieceSquare, rightTake, board, moves);

            generateEnPassanOnly(pieceSquare, MoveDirection.Left, board, moves);
            generateEnPassanOnly(pieceSquare, MoveDirection.Right, board, moves);

            throw new NotImplementedException();

            return moves;
        }

        #region Move generation

        /// <summary>
        /// Determine whether pawn at given square is on initial position.
        /// </summary>
        /// <param name="pieceSquare">The square where piece is standing.</param>
        /// <param name="board">Board where piece is standing.</param>
        /// <returns><c>true</c> whether the pawn is on its initial position, <c>false</c> otherwise.</returns>
        private bool isInitialPawnPosition(Square pieceSquare, BoardState board)
        {
            return board.IsWhitePiece(pieceSquare) ? pieceSquare.Rank == 2 : pieceSquare.Rank == 7;
        }

        /// <summary>
        /// Gets pawn direction for pawn at given square.
        /// </summary>
        /// <param name="pawnSquare">Square where pawn has to be standing.</param>
        /// <param name="board">The tested board.</param>
        /// <returns>Direction for the pawn.</returns>
        private MoveDirection getPawnDirection(Square pawnSquare, BoardState board)
        {
            return board.IsWhitePiece(pawnSquare) ? MoveDirection.Up : MoveDirection.Bottom;
        }

        /// <summary>
        /// Generates en passan if possible for pawn at given square.
        /// </summary>
        /// <param name="pawnSquare">Pawn square.</param>
        /// <param name="takeDirection">Direction where en passan can be processed.</param>
        /// <param name="board">Board for which move is generated.</param>
        /// <param name="moves">Storage for generated move.</param>
        private void generateEnPassanOnly(Square pawnSquare, MoveDirection takeDirection, BoardState board, List<Move> moves)
        {
            var lastMove = board.LastMove;
            if (!board.IsPawn(lastMove.Target))
                //en passan can be done on pawns only.
                return;

            var enPassanTarget = pawnSquare + takeDirection;
            if (lastMove.Target != enPassanTarget)
                //the last move wasn't done with the target pawn.
                return;

            //target for en passan is not the square with target pawn!
            var targetSquare = pawnSquare + getPawnDirection(pawnSquare, board) + takeDirection;
            GenerateMove(pawnSquare, targetSquare, board, moves);
        }


        #endregion
    }
}
