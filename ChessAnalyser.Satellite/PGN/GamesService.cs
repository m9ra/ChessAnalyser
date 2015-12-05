using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChessAnalyser.Satellite.Services;

namespace ChessAnalyser.Satellite.PGN
{
    class GamesService : InMemoryService<DataPGN>
    {
        internal GamesService(IEnumerable<DataPGN> data, string serviceName) :
            base(data, serviceName)
        {
        }
    }
}
