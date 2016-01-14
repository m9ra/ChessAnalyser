using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChessAnalyser.Explorer.Rules.Pieces;

namespace ChessAnalyser.Explorer.Rules
{
    public abstract class Piece
    {
        /// <summary>
        /// King representation.
        /// </summary>
        internal static readonly King King = new King();

        /// <summary>
        /// Queen representation.
        /// </summary>
        internal static readonly Queen Queen = new Queen();

        /// <summary>
        /// Rook representation.
        /// </summary>
        internal static readonly Rook Rook = new Rook();

        /// <summary>
        /// Biship representation.
        /// </summary>
        internal static readonly Bishop Bishop = new Bishop();

        /// <summary>
        /// Knight representation.
        /// </summary>
        internal static readonly Knight Knight = new Knight();

        /// <summary>
        /// Pawn representation.
        /// </summary>
        internal static readonly Pawn Pawn = new Pawn();

        /// <summary>
        /// Index of registered pieces.
        /// </summary>
        private static readonly Dictionary<char, Piece> _pieces = new Dictionary<char, Piece>();

        /// <summary>
        /// Notation of the piece.
        /// </summary>
        internal string Notation { get { return getPieceNotation(); } }

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

        static Piece()
        {
            register(King);
            register(Queen);
            register(Rook);
            register(Bishop);
            register(Knight);
            register(Pawn);
        }

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

        /// <summary>
        /// Parses piece from character representation.
        /// </summary>
        /// <param name="pieceRepresentation">The char representation.</param>
        /// <returns>The piece.</returns>
        internal static Piece From(char pieceRepresentation)
        {
            return _pieces[pieceRepresentation];
        }

        #region Registration utilities
        /// <summary>
        /// Registers given piece.
        /// </summary>
        /// <param name="piece">The registered piece.</param>
        private static void register(Piece piece)
        {
            var notation = piece.Notation;
            if (notation.Length > 0)
                _pieces[notation[0]] = piece;
        }

        #endregion
    }
}
