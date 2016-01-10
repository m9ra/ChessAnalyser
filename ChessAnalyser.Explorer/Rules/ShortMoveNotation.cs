using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChessAnalyser.Explorer.Rules.Pieces;

namespace ChessAnalyser.Explorer.Rules
{
    /// <summary>
    /// Representation of move in algebraic notation.
    /// </summary>
    public class ShortMoveNotation
    {
        /// <summary>
        /// <c>true</c> if it is white move, <c>false</c> otherwise.
        /// </summary>
        public readonly bool IsWhitesMove;

        /// <summary>
        /// Determine if the move is causing check.
        /// </summary>
        public readonly bool IsCheck;

        /// <summary>
        /// Determine whether the move is take.
        /// </summary>
        public readonly bool IsTake;

        /// <summary>
        /// Target square of the move.
        /// </summary>
        public readonly Square TargetSquare;

        /// <summary>
        /// If available gives rank of the moved piece.
        /// </summary>
        public readonly int? SourceRankHint;

        /// <summary>
        /// If available gives file of the moved piece.
        /// </summary>
        public readonly BoardFile? SourceFileHint;

        /// <summary>
        /// The piece moved.
        /// </summary>
        public readonly PieceBase Piece;

        public ShortMoveNotation(string notation, bool isWhitesMove)
        {
            //parse castling
            var upperNotation = notation.ToUpperInvariant();
            if (upperNotation == "O-O")
            {
                throw new NotImplementedException("Short castle");
            }

            if (upperNotation == "O-O-O")
            {
                throw new NotImplementedException("Long castle");
            }

            //'Rbxc8+'
            var promotionSplit = notation.Split('=');
            var takeSplit = promotionSplit[0].Split('x');
            IsTake = takeSplit.Length > 1;

            if (char.IsLower(takeSplit[0][0]))
            {
                //'a4' or 'bxc3'
                throw new NotImplementedException("pawn move");
            }

            Piece = parsePiece(takeSplit[0][0]);
            string hint, targetSquareRepresentation;
            if (IsTake)
            {
                //taking has 'x' between target and source
                hint = takeSplit[0].Substring(1);
                targetSquareRepresentation = takeSplit[1];
            }
            else
            {
                //source follows target square if taking is not present
                var pieceMove = takeSplit[0];
                hint = takeSplit[0].Substring(1, pieceMove.Length - 2);
                targetSquareRepresentation = pieceMove.Substring(pieceMove.Length - 2);
            }

            parse(hint, out SourceFileHint, out SourceRankHint);
            TargetSquare = parseSquare(targetSquareRepresentation);

            throw new NotImplementedException("Promotion parsing.");
        }

        /// <summary>
        /// Parses hint information.
        /// </summary>
        /// <param name="hint">The parsed hint.</param>
        /// <param name="fileHint">Hint for file if available.</param>
        /// <param name="rankHint">Hint for rank if available.</param>
        private void parse(string hint, out BoardFile? fileHint, out int? rankHint)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses piece from given char.
        /// </summary>
        /// <param name="pieceRepresentation">Char representation of a piece.</param>
        /// <returns>The parsed piece.</returns>
        private PieceBase parsePiece(char pieceRepresentation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Parses square from string.
        /// </summary>
        /// <param name="squareRepresentation">Representation of the square.</param>
        /// <returns>The parsed square.</returns>
        private Square parseSquare(string squareRepresentation)
        {
            throw new NotImplementedException();
        }
    }
}
