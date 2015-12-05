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
            Parser.Parse(entry);
        }
    }
}
