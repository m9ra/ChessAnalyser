using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.Rules
{
    public class Board
    {
        private readonly List<Move> _moveHistory = new List<Move>();

        private BoardState _currentState = new BoardState();

        public BoardState CurrentState
        {
            get
            {
                return _currentState;
            }
        }

        public void MakeMove(Move move)
        {
            _moveHistory.Add(move);
            _currentState = _currentState.MakeMove(move);
        }

        public void MakeMoves(params Move[] moves)
        {
            foreach (var move in moves)
                MakeMove(move);
        }
    }
}
