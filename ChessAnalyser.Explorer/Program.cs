using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChessAnalyser.Satellite.Network;
using ChessAnalyser.Satellite.PGN;

namespace ChessAnalyser.Explorer
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new NetworkServiceClient<DataPGN>("games.m9ra", "www.packa2.cz");
            client.EntryAdded += client_EntryAdded;
        }

        static void client_EntryAdded(DataPGN entry)
        {
            var parsed = Parser.Parse(entry);
            for (var i = 0; i < parsed.Count; ++i)
            {
                var whiteMove = parsed.GetWhiteMove(i);
                var blackMove = parsed.GetBlackMove(i);

                Console.WriteLine("{0}. {1} {2}", i, whiteMove, blackMove);
            }
        }
    }
}
