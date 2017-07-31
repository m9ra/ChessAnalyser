using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChessAnalyser.Explorer.Rules.Pieces;

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
        public readonly bool CanCastleShort;

        /// <summary>
        /// Determine whether long castle is possible. (No Rook, King movement)
        /// </summary>
        public readonly bool CanCastleLong;

        public readonly bool CanOtherCastleLong;

        public readonly bool CanOtherCastleShort;

        /// <summary>
        /// Determine whether white is playing next move.
        /// </summary>
        public bool IsWhiteMove
        {
            get
            {
                return LastMove == null || _whitePiecePlacement[LastMove.Target.SquareIndex] == null;
            }
        }

        /// <summary>
        /// Placement of all white pieces.
        /// </summary>
        private readonly Piece[] _whitePiecePlacement = new Piece[64];

        /// <summary>
        /// Placement of all black pieces.
        /// </summary>
        private readonly Piece[] _blackPiecePlacement = new Piece[64];

        /// <summary>
        /// Creates initial board state.
        /// </summary>
        public BoardState()
        {
            //setup pawns
            for (var i = 0; i < 8; ++i)
                putSymmetric(Piece.Pawn, (BoardFile)i, BoardRank._2);

            putSymmetric(Piece.Rook, BoardFile.a);
            putSymmetric(Piece.Rook, BoardFile.h);

            putSymmetric(Piece.Knight, BoardFile.b);
            putSymmetric(Piece.Knight, BoardFile.g);

            putSymmetric(Piece.Bishop, BoardFile.c);
            putSymmetric(Piece.Bishop, BoardFile.f);

            putSymmetric(Piece.Queen, BoardFile.d);
            putSymmetric(Piece.King, BoardFile.e);

            CanCastleLong = true;
            CanCastleShort = true;
            CanOtherCastleLong = true;
            CanOtherCastleShort = true;
        }

        private BoardState(BoardState previousState, Move move)
        {
            CanCastleLong = previousState.CanOtherCastleLong;
            CanCastleShort = previousState.CanOtherCastleShort;
            CanOtherCastleLong = previousState.CanCastleLong;
            CanOtherCastleShort = previousState.CanCastleShort;

            Array.Copy(previousState._whitePiecePlacement, _whitePiecePlacement, _whitePiecePlacement.Length);
            Array.Copy(previousState._blackPiecePlacement, _blackPiecePlacement, _blackPiecePlacement.Length);

            LastMove = move;

            var sourceSquare = move.Source;
            var targetSquare = move.Target;
            var sourceIndex = move.Source.SquareIndex;
            var targetIndex = move.Target.SquareIndex;

            var wasWhiteMove = _whitePiecePlacement[sourceIndex] != null;
            var relevantPiecePlacement = wasWhiteMove ? _whitePiecePlacement : _blackPiecePlacement;
            var oppositePiecePlacement = wasWhiteMove ? _blackPiecePlacement : _whitePiecePlacement;

            var movedPiece = relevantPiecePlacement[sourceIndex];
            var takenPiece = oppositePiecePlacement[targetIndex];

            setMove(sourceSquare, targetSquare);

            var wasKingMove = movedPiece is King;
            var wasRookMove = movedPiece is Rook;
            var wasPawnMove = movedPiece is Pawn;

            //handles castling, promotion and en passan side effects
            if (wasPawnMove)
            {
                var isEnPassan = sourceSquare.File != targetSquare.File && takenPiece == null;
                var isPromotion = targetSquare.Rank == BoardRank._1 || targetSquare.Rank == BoardRank._8;
                if (isEnPassan)
                    oppositePiecePlacement[Square.From(targetSquare.File, sourceSquare.Rank).SquareIndex] = null;
                else if (isPromotion)
                    relevantPiecePlacement[targetIndex] = move.PromotionPiece;
            }
            else if (wasKingMove)
            {
                //handle castling side effects
                if (CanOtherCastleLong && targetSquare.File == BoardFile.c)
                {
                    //move a rook
                    setMove(Square.From(BoardFile.a, targetSquare.Rank), Square.From(BoardFile.d, targetSquare.Rank));
                }
                else if (CanOtherCastleShort && targetSquare.File == BoardFile.g)
                {
                    //move h rook
                    setMove(Square.From(BoardFile.h, targetSquare.Rank), Square.From(BoardFile.f, targetSquare.Rank));
                }

                CanOtherCastleLong = false;
                CanOtherCastleShort = false;
            }
            else if (wasRookMove)
            {
                //rook move vanishes castling
                if (sourceSquare.File == BoardFile.a)
                    CanOtherCastleLong = false;
                else
                    CanOtherCastleShort = false;
            }
        }

        /// <summary>
        /// Generats FEN representation of the current state.
        /// </summary>
        /// <returns>The FEN string.</returns>
        public string GetFEN()
        {
            var positionColumn = new StringBuilder();
            for (var rank = 0; rank < 8; ++rank)
            {
                var emptySquareCounter = 0;
                for (var file = 0; file < 8; ++file)
                {
                    var square = Square.FromIndexes(file, 7 - rank);
                    var whitePiece = _whitePiecePlacement[square.SquareIndex];
                    var blackPiece = _blackPiecePlacement[square.SquareIndex];

                    var isEmpty = whitePiece == null && blackPiece == null;

                    if (isEmpty)
                    {
                        ++emptySquareCounter;
                        continue;
                    }

                    if (emptySquareCounter > 0)
                        positionColumn.Append(emptySquareCounter);

                    emptySquareCounter = 0;

                    var isWhite = whitePiece != null;
                    var pieceLetter = isWhite ? whitePiece.Notation : blackPiece.Notation;
                    if (pieceLetter == "")
                        pieceLetter = "p";

                    pieceLetter = isWhite ? pieceLetter.ToUpper() : pieceLetter.ToLower();

                    positionColumn.Append(pieceLetter);
                }

                if (emptySquareCounter > 0)
                    positionColumn.Append(emptySquareCounter);

                if (rank < 7)
                    //delimiter for all except last rank
                    positionColumn.Append("/"); //file delimiter
            }

            var colorColumn = IsWhiteMove ? "w" : "b";
            //TODO implement properly
            var notImplementedColumns = "KQkq - 0 1";

            return positionColumn.ToString() + " " + colorColumn + " " + notImplementedColumns;
        }

        /// <summary>
        /// Gets moves that are available from current state.
        /// </summary>
        /// <returns>The generated moves.</returns>
        public IEnumerable<Move> GetAvailableMoves()
        {
            var relevantPieces = IsWhiteMove ? _whitePiecePlacement : _blackPiecePlacement;
            var result = new List<Move>();
            for (var i = 0; i < relevantPieces.Length; ++i)
            {
                var piece = relevantPieces[i];
                if (piece == null)
                    continue;

                var pieceSquare = Square.FromIndex(i);
                result.AddRange(piece.GenerateMoves(pieceSquare, this));
            }

            return result;
        }

        /// <summary>
        /// Creates board state which we will get after making given move.
        /// </summary>
        /// <param name="move">The move to make.</param>
        /// <returns>The resulting position.</returns>
        public BoardState MakeMove(Move move)
        {
            return new BoardState(this, move);
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
            return _whitePiecePlacement[pieceSquare.SquareIndex] != null;
        }

        /// <summary>
        /// Determine whether there is no piece standing no given square.
        /// </summary>
        /// <param name="square">The tested square.</param>
        /// <returns><c>true</c> if square is empty, <c>false</c> otherwise.</returns>
        internal bool IsEmpty(Square square)
        {
            if (square == null)
                return false;

            return _whitePiecePlacement[square.SquareIndex] == null && _blackPiecePlacement[square.SquareIndex] == null;
        }

        /// <summary>
        /// Determine whether moving to square is taking.
        /// </summary>
        /// <param name="square">The tested square.</param>
        /// <returns><c>true</c> if piece on square can be taken, <c>false</c> otherwise.</returns>
        internal bool IsTake(Square square)
        {
            if (square == null)
                return false;

            var relevantPieces = IsWhiteMove ? _blackPiecePlacement : _whitePiecePlacement;
            return relevantPieces[square.SquareIndex] != null;
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
            var piece = GetPiece(square);
            return piece is Pawn;
        }

        /// <summary>
        /// Determine whether king standing on given square would be in check.
        /// </summary>
        /// <param name="square">The tested square.</param>
        /// <returns><c>true</c> if the square is attacked by enemy piece, <c>false</c> otherwise.</returns>
        internal bool IsChecked(Square square)
        {
            //TODO check control
            return false;
        }

        internal Piece GetPiece(Square square)
        {
            if (square == null)
                return null;

            var piece = _whitePiecePlacement[square.SquareIndex];
            if (piece == null)
                piece = _blackPiecePlacement[square.SquareIndex];

            return piece;
        }

        #region Position setup utilities

        private void putSymmetric(Piece piece, BoardFile file, BoardRank whiteRank = BoardRank._1)
        {
            var whiteSquare = Square.FromIndexes((int)file, (int)whiteRank);
            var blackSquare = Square.FromIndexes((int)file, (int)(7 - whiteRank));

            _whitePiecePlacement[whiteSquare.SquareIndex] = piece;
            _blackPiecePlacement[blackSquare.SquareIndex] = piece;
        }

        private void setMove(Square source, Square target)
        {
            var sourceIndex = source.SquareIndex;
            var targetIndex = target.SquareIndex;

            _whitePiecePlacement[targetIndex] = _whitePiecePlacement[sourceIndex];
            _blackPiecePlacement[targetIndex] = _blackPiecePlacement[sourceIndex];
            _whitePiecePlacement[sourceIndex] = null;
            _blackPiecePlacement[sourceIndex] = null;
        }

        #endregion
    }
}
