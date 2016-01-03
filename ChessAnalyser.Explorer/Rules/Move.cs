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

    class Move
    {
        /// <summary>
        /// The piece which is used as a result of promotion.
        /// </summary>
        public readonly PieceBase PromotionPiece;

        /// <summary>
        /// Source square of the move.
        /// </summary>
        public readonly Square Source;

        /// <summary>
        /// Target square of the move.
        /// </summary>
        public readonly Square Target;
    }
}
