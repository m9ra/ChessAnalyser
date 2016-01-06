using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules
{
    class RayGenerator
    {
        /// <summary>
        /// Ray generator for up direction.
        /// </summary>
        internal static readonly RayGenerator UpGenerator = new RayGenerator(MoveDirection.Up);

        /// <summary>
        /// Ray generator for up left direction.
        /// </summary>
        internal static readonly RayGenerator UpLeftGenerator = new RayGenerator(MoveDirection.UpLeft);

        /// <summary>
        /// Ray generator for up right direction.
        /// </summary>
        internal static readonly RayGenerator UpRightGenerator = new RayGenerator(MoveDirection.UpRight);

        /// <summary>
        /// Ray generator for bottom direction.
        /// </summary>
        internal static readonly RayGenerator BottomGenerator = new RayGenerator(MoveDirection.Bottom);

        /// <summary>
        /// Ray generator for bottom left direction.
        /// </summary>
        internal static readonly RayGenerator BottomLeftGenerator = new RayGenerator(MoveDirection.BottomLeft);

        /// <summary>
        /// Ray generator for bottom right direction.
        /// </summary>
        internal static readonly RayGenerator BottomRightGenerator = new RayGenerator(MoveDirection.BottomRight);

        /// <summary>
        /// Ray generator for left direction.
        /// </summary>
        internal static readonly RayGenerator LeftGenerator = new RayGenerator(MoveDirection.Left);

        /// <summary>
        /// Ray generator for right direction.
        /// </summary>
        internal static readonly RayGenerator RightGenerator = new RayGenerator(MoveDirection.Right);

        /// <summary>
        /// Direction of the generated ray.
        /// </summary>
        private readonly MoveDirection _direction;

        internal RayGenerator(MoveDirection direction)
        {
            _direction = direction;
        }

        /// <summary>
        /// Generates ray of moves from given starting square (without
        /// </summary>
        /// <param name="source">Source square of the moved piece.</param>
        /// <param name="board">Board where moves are made.</param>
        /// <param name="moves">Storage for generated moves.</param>
        internal void GenerateMoves(Square source, BoardState board, List<Move> moves)
        {
            var currentTarget = source;
            var isWhiteMove = board.IsWhitePiece(source);
            while (!(currentTarget = currentTarget + _direction).IsOutOfBoard)
            {
                if (board.IsEmpty(currentTarget) || board.IsWhitePiece(currentTarget) != isWhiteMove)
                {
                    //we can step there freely or take oposite color piece
                    moves.Add(generateMove(source, currentTarget, board));
                }

                //notice that this condition has to be repeated!
                if (!board.IsEmpty(currentTarget))
                {
                    //we cannot stop on piece of same color
                    break;
                }
            }
        }

        private Move generateMove(Square startingSquare, Square currentTarget, BoardState board)
        {
            throw new NotImplementedException();
        }
    }
}
