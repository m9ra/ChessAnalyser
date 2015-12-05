using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Satellite.PGN
{
    [Serializable]
    public class DataPGN
    {
        /// <summary>
        /// Id of pgn.
        /// </summary>
        public readonly string Id;

        /// <summary>
        /// Raw data of PGN.
        /// </summary>
        public readonly string Data;

        internal DataPGN(string id, string data)
        {
            Id = id;
            Data = data;
        }

        public override string ToString()
        {
            return Id + "\n\n" + Data;
        }
    }
}
