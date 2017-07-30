using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ServeRick;
using ServeRick.Database;
using ServeRick.Defaults;

namespace ChessAnalyser.Explorer.WebInterface
{
    class ExplorerWebApplication : WebApplication
    {
        private readonly string _wwwPath;

        internal ExplorerWebApplication(string wwwPath)
        {
            _wwwPath = wwwPath;
        }

        protected override ResponseManagerBase createResponseManager()
        {
            var manager = new ResponseManager(this, _wwwPath, typeof(RootController));
            manager.AddDirectoryContent("");
            manager.AddDirectoryTree("images");
            manager.AddDirectoryContent("js");
            manager.AddDirectoryContent("css");

            return manager;
        }

        protected override InputManagerBase createInputManager()
        {
            return new InputManager();
        }

        protected override IEnumerable<DataTable> createTables()
        {
            return new DataTable[0];
        }

        protected override Type[] getHelpers()
        {
            return new Type[0];
        }

        #region Server utilities

        /// <summary>
        /// Run server providing web on given path.
        /// </summary>
        /// <param name="wwwPath">Root path of provided web.</param>
        internal static void RunWebServer(int port, string wwwPath)
        {
            var webApp = new ExplorerWebApplication(wwwPath);

            ServerEnvironment.AddApplication(webApp);
            var server = ServerEnvironment.Start(port);
        }

        /// <summary>
        /// Run console that allows to control the server.
        /// </summary>
        internal static void RunConsole()
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) => { OnExit(); };
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey();

                switch (keyInfo.KeyChar)
                {
                    case 't':
                        Log.TraceDisabled = !Log.TraceDisabled;
                        break;
                    case 'n':
                        Log.NoticeDisabled = !Log.NoticeDisabled;
                        break;
                }

            } while (keyInfo.Key != ConsoleKey.Escape);

            Environment.Exit(0);
        }

        /// <summary>
        /// Handler that is called on server exit.
        /// </summary>
        private static void OnExit()
        {
        }

        #endregion
    }
}
