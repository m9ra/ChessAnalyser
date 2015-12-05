using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Satellite.PGN
{
    abstract class DownloaderPGN
    {
        /// <summary>
        /// Initializes ids that has been already registered with current downloader.
        /// </summary>
        /// <param name="registeredIds">Ids that has been registered.</param>
        internal abstract void InitializeRegisteredIds(IEnumerable<string> registeredIds);

        /// <summary>
        /// Downloads pgns that has been added newly.
        /// </summary>
        /// <returns>The downloaded pgns.</returns>
        internal abstract IEnumerable<DataPGN> DownloadNew();

        /// <summary>
        /// Gets hierarchy of downloaded pgns.
        /// </summary>
        /// <returns>The parts of hierarchy.</returns>
        internal abstract string[] GetHierarchy();
    }
}
