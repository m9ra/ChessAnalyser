using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Threading;

using ChessAnalyser.Satellite.Services;

using ChessAnalyser.Satellite.PGN.Downloaders;

namespace ChessAnalyser.Satellite.PGN
{
    /// <summary>
    /// Action registered on given pgn with given path.
    /// </summary>
    /// <param name="pgn">PGN which action has been registered.</param>
    /// <param name="path">Path of action.</param>
    delegate void ActionPGN(DataPGN pgn, string path);

    class GamesManager
    {
        /// <summary>
        /// Wait time for new games occuring.
        /// </summary>
        internal readonly int MillisecondsCheckTime = 5 * 60 * 1000;

        /// <summary>
        /// Directory where games are stored.
        /// </summary>
        internal readonly string GamesDirectory = Path.Combine("data", "games");

        /// <summary>
        /// Event fired when pgn is saved.
        /// </summary>
        internal event ActionPGN OnSave;

        /// <summary>
        /// Event fired when error occured during download.
        /// </summary>
        internal event ActionPGN OnDownloadError;

        /// <summary>
        /// Event fired when error occured during listing.
        /// </summary>
        internal event ActionPGN OnListingError;

        /// <summary>
        /// Downloaders that are managed by this manager.
        /// </summary>
        private readonly DownloaderPGN[] _downloaders;

        /// <summary>
        /// Services indexed by their names
        /// </summary>
        private readonly Dictionary<string, GamesService> _services = new Dictionary<string, GamesService>();

        /// <summary>
        /// Determine whether manager running ends.
        /// </summary>
        private volatile bool _isEnd = false;

        /// <summary>
        /// thread where manager routines runs.
        /// </summary>
        private Thread _thread;

        internal GamesManager(params DownloaderPGN[] downloaders)
        {
            _downloaders = downloaders.ToArray();

            //prepare file structure
            foreach (var downloader in _downloaders)
            {
                Directory.CreateDirectory(getStoragePath(downloader));
            }
        }

        /// <summary>
        /// Runs management of PGN games.
        /// </summary>
        internal void Start()
        {
            if (_thread != null)
                throw new NotSupportedException("Manager cannot be start multiple times");

            //initialize downloaders
            foreach (var downloader in _downloaders)
            {
                var sourceDirectory = getStoragePath(downloader);
                var registeredIds = getSavedIds(sourceDirectory);
                downloader.InitializeRegisteredIds(registeredIds);

                var pgns = loadPGNs(sourceDirectory, registeredIds);
                var gamesServiceName = getServiceName(downloader);
                var gamesService = new GamesService(pgns, gamesServiceName);
                _services.Add(gamesServiceName, gamesService);
            }

            _thread = new Thread(_run);
            _thread.Start();
        }

        /// <summary>
        /// Runs manager routine in _thread.
        /// </summary>
        internal void _run()
        {
            while (!_isEnd)
            {
                foreach (var downloader in _downloaders)
                {
                    var service = getService(downloader);

                    foreach (var pgn in downloader.DownloadNew())
                    {
                        if (pgn.Id == null)
                        {
                            //error while listing ids has occured
                            if (OnListingError != null)
                                OnListingError(pgn, service.Name);

                        }
                        else
                            if (pgn.Data == null)
                            {
                                //error has occured
                                if (OnDownloadError != null)
                                    OnDownloadError(pgn, getPGNPath(pgn, downloader));
                            }
                            else
                            {
                                save(pgn, downloader);
                                service.AddNewEntry(pgn);
                            }
                    }
                }

                //wait to be not very aggresive
                Thread.Sleep(MillisecondsCheckTime);
            }
        }

        /// <summary>
        /// Saves given pgn according to downloader's hierarchy.
        /// </summary>
        /// <param name="pgn">The saved pagn.</param>
        /// <param name="downloader">The source downloader.</param>
        private void save(DataPGN pgn, DownloaderPGN downloader)
        {
            var targetPath = getPGNPath(pgn, downloader);
            File.WriteAllText(targetPath, pgn.Data);

            if (OnSave != null)
                OnSave(pgn, targetPath);
        }

        /// <summary>
        /// Gets path of pgn in context of given downloader.
        /// </summary>
        /// <param name="pgn">Pgn which path is requested.</param>
        /// <param name="downloader">The source downloader.</param>
        /// <returns>The path.</returns>
        private string getPGNPath(DataPGN pgn, DownloaderPGN downloader)
        {
            var targetDirectory = getStoragePath(downloader);
            var targetPath = Path.Combine(targetDirectory, pgn.Id + ".pgn");
            return targetPath;
        }

        /// <summary>
        /// Gets path where files of downloader are stored.
        /// </summary>
        /// <param name="downloader">Downloader which files are stored.</param>
        /// <returns>The path.</returns>
        private string getStoragePath(DownloaderPGN downloader)
        {
            var hierarchy = downloader.GetHierarchy();
            var targetDirectory = Path.Combine(GamesDirectory, Path.Combine(downloader.GetHierarchy()));

            return targetDirectory;
        }

        /// <summary>
        /// Gets service name corresponding to given downloader.
        /// </summary>
        /// <param name="downloader">Downloader which service is loaded.</param>
        /// <returns>The service name.</returns>
        private string getServiceName(DownloaderPGN downloader)
        {
            var hierarchy = downloader.GetHierarchy();
            var gamesServiceName = "games." + string.Join(".", hierarchy);
            return gamesServiceName;
        }

        /// <summary>
        /// Gets service indexed by name of given downloader.
        /// </summary>
        /// <param name="downloader">Downloader which service is requested.</param>
        /// <returns>The service.</returns>
        private GamesService getService(DownloaderPGN downloader)
        {
            var name = getServiceName(downloader);
            return _services[name];
        }

        /// <summary>
        /// Gets ids that are saved under given path.
        /// </summary>
        /// <param name="path">Path where saved ids will be searched.</param>
        /// <returns>The saved ids.</returns>
        private IEnumerable<string> getSavedIds(string path)
        {
            var ids = new List<string>();
            foreach (var file in Directory.EnumerateFiles(path, "*.pgn"))
            {
                ids.Add(Path.GetFileNameWithoutExtension(file));
            }

            return ids;
        }

        /// <summary>
        /// Loads pgn from given directory into memory.
        /// </summary>
        /// <param name="directory">Directory where pgn is stored.</param>
        /// <param name="id">Id of the pgn.</param>
        /// <returns>The pgn.</returns>
        private DataPGN loadPGN(string directory, string id)
        {
            return new DataPGN(id, File.ReadAllText(Path.Combine(directory, id + ".pgn")));
        }

        /// <summary>
        /// Loads all specified pgns.
        /// </summary>
        /// <param name="directory">Directory where pgns are stored.</param>
        /// <param name="ids">Ids of pgns to load.</param>
        /// <returns>The pgns.</returns>
        private IEnumerable<DataPGN> loadPGNs(string directory, IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                yield return loadPGN(directory, id);
            }
        }

        /// <summary>
        /// Gets services attached to managed downloaders.
        /// </summary>
        /// <returns>The services.</returns>
        internal IEnumerable<GamesService> GetServices()
        {
            return _services.Values;
        }
    }
}
