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
            var events = parseEvents(commandJson);
            var responseCommands = new List<AjaxCommand>();

            foreach (var ajaxEvent in events)
            {
                var eventName = ajaxEvent["name"].ToString();
                handleCommand(eventName, ajaxEvent, responseCommands);
            }

            RenderJson(responseCommands);
        }


        private void handleCommand(string commandName, AjaxEvent ajaxEvent, List<AjaxCommand> responseCommands)
        {
            var user = getCurrentUser();

            switch (commandName)
            {
                case "on_board_updated":
                    command_onBoardUpdated(user, ajaxEvent, responseCommands);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        #region Command handlers

        private void command_onBoardUpdated(UserData data, AjaxEvent ajaxEvent, List<AjaxCommand> responseCommands)
        {
            var boardName = ajaxEvent["board_name"].ToString();
            var history = getMovesFromArray((ArrayList)ajaxEvent["history"]);

            data.OnBoardUpdated(boardName, history, responseCommands);
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

        private AjaxEvent[] parseEvents(string json)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var data = serializer.Deserialize<Dictionary<string, object>[]>(json);

            return data.Select(d => new AjaxEvent(d)).ToArray();
        }

        #endregion

        #region User handling

        private UserData getCurrentUser()
        {
            //TODO 
            return Session<UserData>(() => new UserData("m9ra"));
        }

        #endregion
    }
}
