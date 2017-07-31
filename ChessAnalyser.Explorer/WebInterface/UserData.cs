using ChessAnalyser.Explorer.Rules;
using ChessAnalyser.Explorer.WebInterface.BoardChannels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.WebInterface
{
    class UserData
    {
        /// <summary>
        /// Name of the logged user.
        /// </summary>
        internal readonly string UserName;

        /// <summary>
        /// Boards that are registered for the user.
        /// </summary>
        private readonly Dictionary<string, BoardData> _boards = new Dictionary<string, BoardData>();

        internal UserData(string userName)
        {
            UserName = userName;

            registerBoard("opening_board",
                new BoardChannel(),
                new NavigationChannel()
                );
        }

        /// <summary>
        /// Reports users board update.
        /// </summary>
        internal void OnBoardUpdated(string boardName, Move[] history, List<AjaxCommand> responseCommands)
        {
            _boards[boardName].UpdateBoard(history, responseCommands);
        }

        /// <summary>
        /// Registers board with given name and channels.
        /// </summary>
        private void registerBoard(string name, params BoardChannelBase[] boardChannels)
        {
            var board = new BoardData(name, boardChannels);
            _boards.Add(name, board);
        }
    }
}
