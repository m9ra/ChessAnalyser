﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChessAnalyser.Satellite.Network;
using ChessAnalyser.Satellite.PGN;

using ChessAnalyser.Explorer.Rules;

namespace ChessAnalyser.Explorer
{
    class Program
    {
        static int move_count = 0;

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
                checkMove(parsed.GetWhiteMove(i), true);
                checkMove(parsed.GetBlackMove(i), false);
                ++move_count;
            }

            Console.WriteLine(move_count);
        }

        private static void checkMove(string move, bool isWhite)
        {
            if (move == null)
                return;

            var parsedMove = new ShortMoveNotation(move, isWhite);
            var parsedMoveString = parsedMove.ToString();

            if (parsedMoveString != move)
                Console.WriteLine("diff: {0} || {1}", move, parsedMoveString);
        }
    }
}
