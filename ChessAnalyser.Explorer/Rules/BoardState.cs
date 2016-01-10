using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules
{
    public class BoardState
    {
        /// <summary>
        /// Move that has been done lastly.
        /// </summary>
        public readonly Move LastMove;

        /// <summary>
        /// Determine whether short castle is possible. (No Rook, King movement)
        /// </summary>
        public readonly bool ShortCastleVanished;

        /// <summary>
        /// Determine whether long castle is possible. (No Rook, King movement)
        /// </summary>
        public bool LongCastleVanished;

        /// <summary>
        /// Gets moves that are available from current state.
        /// </summary>
        /// <returns>The generated moves.</returns>
        public IEnumerable<Move> GetAvailableMoves()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates board state which we will get after making given move.
        /// </summary>
        /// <param name="move">The move to make.</param>
        /// <returns>The resulting position.</returns>
        public BoardState MakeMove(Move move)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds move according to is algebraic notation.
        /// </summary>
        /// <param name="algebraicNotation">Notation of the move.</param>
        /// <returns>The move if available <c>null</c> otherwise.</returns>
        public Move FindMove(string algebraicNotation)
        {
            var availableMoves = GetAvailableMoves();
            var matchedMoves = new List<Move>();
            foreach (var availableMove in availableMoves)
            {
                if (isMatch(availableMove, algebraicNotation))
                    matchedMoves.Add(availableMove);
            }

            if (matchedMoves.Count == 1)
                //we have found single matching move
                return matchedMoves[0];

            //There is no or many matching moves - we cannot distinguish them
            //with given notation
            return null;
        }

        /// <summary>
        /// Determine whether piece on given square is white or not.
        /// </summary>
        /// <param name="pieceSquare">The tested square.</param>
        /// <returns><c>true</c> for white piece, <c>false</c> for black or no piece.</returns>
        internal bool IsWhitePiece(Square pieceSquare)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determine whether there is no piece standing no given square.
        /// </summary>
        /// <param name="square">The tested square.</param>
        /// <returns><c>true</c> if square is empty, <c>false</c> otherwise.</returns>
        internal bool IsEmpty(Square square)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determine whether given move matches to given algebraic notation.
        /// </summary>
        /// <param name="move">The tested move.</param>
        /// <param name="algebraicNotation">The notation.</param>
        /// <returns><c>true</c> when move matches the notation, <c>false</c> otherwise.</returns>
        private bool isMatch(Move move, string algebraicNotation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determine whether piece on given square is a pawn.
        /// </summary>
        /// <param name="square">The tested square.</param>
        /// <returns><c>true</c> when the piece is pawn, <c>false</c> otherwise.</returns>
        internal bool IsPawn(Square square)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determine whether king standing on given square would be in check.
        /// </summary>
        /// <param name="square">The tested square.</param>
        /// <returns><c>true</c> if the square is attacked by enemy piece, <c>false</c> otherwise.</returns>
        internal bool IsChecked(Square square)
        {
            throw new NotImplementedException();
        }
    }
}
