using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules.Pieces
{
    public abstract class PieceBase
    {
        /// <summary>
        /// Name of piece as in algebraic notation.
        /// </summary>
        /// <returns>The piece name.</returns>
        protected abstract string getPieceNotation();

        /// <summary>
        /// Generates moves within given board. Preliminary controls (pre/post checks, same color placement, move right, piece position) can be omitted.
        /// </summary>
        /// <param name="pieceSquare">Square where piece is standing.</param>
        /// <param name="board">Board where piece is standing.</param>
        /// <returns>The generated moves.</returns>
        protected abstract IEnumerable<Move> generateMoves(Square pieceSquare, BoardState board);

        /// <summary>
        /// Generates move from source to target.
        /// </summary>
        /// <param name="source">Source square.</param>
        /// <param name="target">Target square.</param>
        /// <param name="board">Board where move is made.</param>
        /// <param name="moves">Storage for generated moves.</param>
        protected void GenerateMove(Square source, Square target, BoardState board, List<Move> moves)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates move from source to squares specified by generators.
        /// </summary>
        /// <param name="pieceSquare">Square where the moved piece is standing.</param>
        /// <param name="generators">Generators that are used for move generation.</param>
        /// <param name="board">Board where move is made.</param>
        /// <param name="moves">Storage for generated moves.</param>
        /// <returns>Generated moves.</returns>
        protected IEnumerable<Move> GenerateMoves(Square pieceSquare, IEnumerable<RayGenerator> generators, BoardState board)
        {
            var moves = new List<Move>();
            foreach (var generator in generators)
            {
                generator.GenerateMoves(pieceSquare, board, moves);
            }

            return moves;
        }
        /// <summary>
        /// Generates move from source to target which has to result in take action.
        /// </summary>
        /// <param name="source">Source square.</param>
        /// <param name="target">Target square.</param>
        /// <param name="board">Board where move is made.</param>
        /// <param name="moves">Storage for generated moves.</param>
        protected void GenerateTakeOnlyMove(Square source, Square target, BoardState board, List<Move> moves)
        {
            throw new NotImplementedException();
        }
    }
}
