using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ChessAnalyser.Satellite.PGN;
using ChessAnalyser.Satellite.PGN.Downloaders;

using ChessAnalyser.Satellite.Network;


namespace ChessAnalyser.Satellite
{
    class Program
    {
        static void Main(string[] args)
        {
            //WORKAROUND for mono SSL handling
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            var networkManager = new NetworkServiceManager(SatelliteConfiguration.ListenPort);

            //register manager for games
            var gamesManager = CreateGamesManager();
            networkManager.RegisterServices(gamesManager.GetServices());

            //run network manager routines in separate thread
            var th = new System.Threading.Thread(networkManager.Run);
            th.Start();
        }

        /// <summary>
        /// Creates manager for games.
        /// </summary>
        /// <returns>The created manager.</returns>
        private static GamesManager CreateGamesManager()
        {
            var manager = new GamesManager(
                new ChessComDownloader("m9ra"),
                new ChessComDownloader("sladot")
                );

            manager.OnSave += (pgn, target) => Console.WriteLine("[DOWNLOAD] " + pgn.Id + "\t" + target);
            manager.OnDownloadError += (pgn, source) => Console.WriteLine("[ERROR] " + pgn.Id + "\t" + source);
            manager.OnListingError += (pgn, source) => Console.WriteLine("[LISTING_ERROR] " + source);
            manager.Start();
            return manager;
        }
    }
}
