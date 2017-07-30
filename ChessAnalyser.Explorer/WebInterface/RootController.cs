using ChessAnalyser.Explorer.Rules;
using ServeRick;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer.WebInterface
{
    class RootController : ResponseController
    {
        public void index()
        {
            throw new NotImplementedException();
        }

        public void opening_manager()
        {
            Layout("board_layout.haml");
            Render("opening_manager.haml");
        }

        public void ajax_commands()
        {
            var commandJson = POST("commands_json");
            var commands = parseCommands(commandJson);
            var responseCommands = new List<Dictionary<string, object>>();

            foreach (var command in commands)
            {
                var commandName = command["name"].ToString();
                handleCommand(commandName, command, responseCommands);
            }

            RenderJson(responseCommands);
        }


        private void handleCommand(string commandName, Dictionary<string, object> command, List<Dictionary<string, object>> responseCommands)
        {
            switch (commandName)
            {
                case "on_board_updated":
                    command_onBoardUpdated(command, responseCommands);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        #region Response command handlers

        private void send_setBoardPosition(string boardName, Board board, List<Dictionary<string, object>> responseCommands)
        {
            var fen = board.CurrentState.GetFEN();
            var availableMoves = getAvailableMovesIndex(board);

            var responseCommand = new Dictionary<string, object>()
            {
                { "name", "set_board_position" },
                { "board_name", boardName},
                { "current_fen", fen},
                { "available_moves", availableMoves}
            };

            responseCommands.Add(responseCommand);
        }

        private void send_setBoardNavigation(string boardName, Move[] history, List<Dictionary<string, object>> responseCommands)
        {
            var info = string.Join("</br>\n", history.ToList());

            var responseCommand = new Dictionary<string, object>()
            {
                { "name", "set_board_navigation" },
                { "info", info},
                { "board_name", boardName},
            };

            responseCommands.Add(responseCommand);
        }

        #endregion

        #region Command handlers

        private void command_onBoardUpdated(Dictionary<string, object> command, List<Dictionary<string, object>> responseCommands)
        {
            var boardName = command["board_name"].ToString();
            var history = getMovesFromArray((ArrayList)command["history"]);

            var board = new Board();
            board.MakeMoves(history);

            send_setBoardPosition(boardName, board, responseCommands);
            send_setBoardNavigation(boardName, history, responseCommands);
        }

        #endregion

        private Dictionary<string, object> getAvailableMovesIndex(Board board)
        {
            var moves = board.CurrentState.GetAvailableMoves();

            var result = moves.GroupBy(m => m.Source).ToDictionary(g => g.Key.ToString(), g => (object)g.Select(m => m.Target.ToString()).ToList());
            return result;
        }

        private Move[] getMovesFromArray(ArrayList obj)
        {
            var moves = Array.ConvertAll(obj.ToArray(), (o) =>
            {
                var m = (ArrayList)o;

                var from = m[0].ToString();
                var to = m[1].ToString();

                return Move.FromString(from, to);
            });


            return moves;
        }

        private Dictionary<string, object>[] parseCommands(string json)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = serializer.Deserialize<Dictionary<string, object>[]>(json);

            return data;
        }
    }
}
