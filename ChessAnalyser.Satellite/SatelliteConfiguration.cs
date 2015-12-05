using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Satellite
{
    public static class SatelliteConfiguration
    {
        #region Network configuration

        /// <summary>
        /// Port where satelites manager is listening.
        /// </summary>
        public static readonly int ListenPort = 46841;

        /// <summary>
        /// Size of connecting header which has to be sent to network service manager.
        /// </summary>
        public static readonly int ConnectHeaderSize = 100;

        #endregion
    }
}
