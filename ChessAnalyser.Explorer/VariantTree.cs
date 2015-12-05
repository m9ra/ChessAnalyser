using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Explorer
{
    class VariantTree
    {
        internal VariantBuilder AddNewVariant()
        {
            return new VariantBuilder(this);
        }
    }
}
