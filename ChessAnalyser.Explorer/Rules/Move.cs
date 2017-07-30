using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChessAnalyser.Explorer.Rules.Pieces;

namespace ChessAnalyser.Explorer.Rules
{
    /// <summary>
    /// Direction on chessboard - relative to white's board view.
    /// </summary>
    public enum MoveDirection
    {
        Up, UpRight, Right, BottomRight, Bottom, BottomLeft, Left, UpLeft
    }

    public class Move
    {
        /// <summary>
        /// The piece which is used as a result of promotion.
        /// </summary>
        public readonly Piece PromotionPiece;

        /// <summary>
        /// Source square of the move.
        /// </summary>
        public readonly Square Source;

        /// <summary>
        /// Target square of the move.
        /// </summary>
        public readonly Square Target;

        private Move(Piece promotionPiece, Square source, Square target)
        {
            PromotionPiece = promotionPiece;
            Source = source;
            Target = target;
        }

        /// <summary>
        /// Creates move between given string represented squares.
        /// </summary>
        public static Move FromString(string source, string target)
        {
            return new Move(null, Square.FromString(source), Square.FromString(target));
        }

        /// <summary>
        /// Creates move between given squares.
        /// </summary>
        public static Move Between(Square source, Square target)
        {
            return new Move(null, source, target);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("{0}-{1}", Source, Target);
        }
    }
}
