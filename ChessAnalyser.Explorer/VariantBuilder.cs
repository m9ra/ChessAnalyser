using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer
{
    class VariantBuilder
    {
        /// <summary>
        /// Tree where variant originates.
        /// </summary>
        private readonly VariantTree _originTree;

        internal VariantBuilder(VariantTree originTree)
        {
            _originTree = originTree;
        }
    }
}
