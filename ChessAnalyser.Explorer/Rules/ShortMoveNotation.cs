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
        /// Determine if the move is causing mate.
        /// </summary>
        public readonly bool IsMate;

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
        public readonly BoardRank? SourceRankHint;

        /// <summary>
        /// If available gives file of the moved piece.
        /// </summary>
        public readonly BoardFile? SourceFileHint;

        /// <summary>
        /// The piece moved.
        /// </summary>
        public readonly Piece Piece;

        /// <summary>
        /// The piece which is result of promotion if available, <c>null</c> otherwise.
        /// </summary>
        public readonly Piece PromotedPiece;

        public ShortMoveNotation(string notation, bool isWhitesMove)
        {
            IsWhitesMove = isWhitesMove;
            IsMate = notation.EndsWith("#");
            IsCheck = notation.EndsWith("+") || IsMate;

            if (IsCheck || IsMate)
                notation = notation.Substring(0, notation.Length - 1);

            //parse castling
            var upperNotation = notation.ToUpperInvariant();
            if (upperNotation == "O-O")
            {
                Piece = Piece.King;
                if (isWhitesMove)
                {
                    TargetSquare = Square.FromString("g1");
                    parse("e1", out SourceFileHint, out SourceRankHint);
                }
                else
                {
                    TargetSquare = Square.FromString("g8");
                    parse("e8", out SourceFileHint, out SourceRankHint);
                }
                return;
            }

            if (upperNotation == "O-O-O")
            {
                Piece = Piece.King;
                if (isWhitesMove)
                {
                    TargetSquare = Square.FromString("c1");
                    parse("e1", out SourceFileHint, out SourceRankHint);
                }
                else
                {
                    TargetSquare = Square.FromString("c8");
                    parse("e8", out SourceFileHint, out SourceRankHint);
                }
                return;
            }

            //'Rbxc8+'
            var promotionSplit = notation.Split('=');
            var takeSplit = promotionSplit[0].Split('x');
            IsTake = takeSplit.Length > 1;

            if (char.IsLower(takeSplit[0][0]))
            {
                //'a4' or 'bxc3'
                Piece = Piece.Pawn;
                if (IsTake)
                {
                    parse(takeSplit[0], out SourceFileHint, out SourceRankHint);
                    TargetSquare = parseSquare(takeSplit[1]);
                }
                else
                {
                    //there are no hints
                    TargetSquare = parseSquare(takeSplit[0]);
                }

                //only pawn moves can cause promotion
                if (promotionSplit.Length > 1)
                    PromotedPiece = parsePromotion(promotionSplit[1]);

                return;
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
                hint = takeSplit[0].Substring(1, pieceMove.Length - 3);
                targetSquareRepresentation = pieceMove.Substring(pieceMove.Length - 2);
            }

            parse(hint, out SourceFileHint, out SourceRankHint);
            TargetSquare = parseSquare(targetSquareRepresentation);
        }

        /// <summary>
        /// Parses hint information.
        /// </summary>
        /// <param name="hint">The parsed hint.</param>
        /// <param name="fileHint">Hint for file if available.</param>
        /// <param name="rankHint">Hint for rank if available.</param>
        private void parse(string hint, out BoardFile? fileHint, out BoardRank? rankHint)
        {
            fileHint = null;
            rankHint = null;

            if (hint.Length == 0)
                //nothing to parse
                return;

            if (hint.Length == 2)
            {
                //we have both hints
                var hintSquare = Square.FromString(hint);
                fileHint = hintSquare.File;
                rankHint = hintSquare.Rank;
                return;
            }

            var hintChar = hint[0];
            if (char.IsDigit(hint[0]))
                rankHint = Square.RankFrom(hintChar);
            else
                fileHint = Square.FileFrom(hintChar);
        }

        /// <summary>
        /// Parses piece from given char.
        /// </summary>
        /// <param name="pieceRepresentation">Char representation of a piece.</param>
        /// <returns>The parsed piece.</returns>
        private Piece parsePiece(char pieceRepresentation)
        {
            return Piece.From(pieceRepresentation);
        }

        /// <summary>
        /// Parses square from string.
        /// </summary>
        /// <param name="squareRepresentation">Representation of the square.</param>
        /// <returns>The parsed square.</returns>
        private Square parseSquare(string squareRepresentation)
        {
            return Square.FromString(squareRepresentation);
        }

        /// <summary>
        /// Parses promotion string.
        /// </summary>
        /// <param name="promotionString">The promotion string.</param>
        /// <returns>Piece which was promoted.</returns>
        private Piece parsePromotion(string promotionString)
        {
            return Piece.From(promotionString[0]);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var result = getMoveRepresentation();

            if (IsMate)
                result = result + "#";
            else if (IsCheck)
                result = result + "+";

            return result;
        }

        /// <summary>
        /// Gets move representation (without check/mate).
        /// </summary>
        /// <returns>The representation.</returns>
        private string getMoveRepresentation()
        {
            var hasFullHint = SourceFileHint.HasValue && SourceRankHint.HasValue;

            var isShortCastle = hasFullHint && SourceFileHint.Value == BoardFile.e && TargetSquare.File == BoardFile.g;
            var isLongCastle = hasFullHint && SourceFileHint.Value == BoardFile.e && TargetSquare.File == BoardFile.c;

            if (isLongCastle)
                return "O-O-O";

            if (isShortCastle)
                return "O-O";

            var hint = "";
            if (SourceFileHint.HasValue)
                hint = hint + Square.GetName(SourceFileHint.Value);

            if (SourceRankHint.HasValue)
                hint = hint + Square.GetName(SourceRankHint.Value);

            var representation = TargetSquare.ToString();
            if (IsTake)
                representation = "x" + representation;

            representation = hint + representation;
            representation = Piece.Notation + representation;

            if (PromotedPiece != null)
                representation = representation + "=" + PromotedPiece.Notation;

            return representation;
        }
    }
}
