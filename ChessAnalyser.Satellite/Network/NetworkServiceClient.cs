using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using System.Net;
using System.Net.Sockets;

using System.Runtime.Serialization.Formatters.Binary;

using ChessAnalyser.Satellite.Services;

namespace ChessAnalyser.Satellite.Network
{
    public class NetworkServiceClient<DataEntry>
    {
        /// <summary>
        /// Event fired whenever new entry is added.
        /// </summary>
        public event DataEntryEvent<DataEntry> EntryAdded;

        /// <summary>
        /// Service which is current client attached to.
        /// </summary>
        internal readonly string ServiceName;

        /// <summary>
        /// Client which provides communication with sattelite.
        /// </summary>
        private readonly TcpClient _client;

        /// <summary>
        /// Entries that are provided by the service.
        /// </summary>
        private readonly List<DataEntry> _entries = new List<DataEntry>();

        /// <summary>
        /// Thread listening to new entries.
        /// </summary>
        private readonly Thread _thread;

        /// <summary>
        /// Lock for entries.
        /// </summary>
        private readonly object _L_entries = new object();

        /// <summary>
        /// Determine whether client connection to satellite will be ended.
        /// </summary>
        private volatile bool _isEnd = false;

        public NetworkServiceClient(string serviceName, string satelliteHost)
        {
            ServiceName = serviceName;

            _client = new TcpClient(satelliteHost, SatelliteConfiguration.ListenPort);

            _thread = new Thread(_listenToEntries);
            _thread.Start();
        }

        /// <summary>
        /// Thread safe copy of entries that are provided by the service.
        /// </summary>
        public IEnumerable<DataEntry> GetEntries()
        {
            lock (_L_entries)
            {
                return _entries.ToArray();
            }
        }

        /// <summary>
        /// Listens to entries incoming from the service.
        /// </summary>
        private void _listenToEntries()
        {
            sendConnectCommand(ServiceName);

            var formatter = new BinaryFormatter();
            while (!_isEnd)
            {
                //recieve commands from service
                var command = formatter.Deserialize(_client.GetStream()) as ServiceCommand<DataEntry>;
                if (command == null)
                    break;

                //execute commands
                switch (command.Command)
                {
                    case NetworkService.InitCommand:
                        if (_entries.Count > 0)
                            throw new NotSupportedException("Cannot initialize non-empty service now");

                        registerNewEntries(command.Entries);
                        break;

                    case NetworkService.NewEntryCommand:
                        registerNewEntries(command.Entries);
                        break;

                    default:
                        throw new NotSupportedException(command.ToString());
                }
            }

            try
            {
                _client.Close();
            }
            catch
            { 
                //we don't need to handle exceptions here
            }
        }

        /// <summary>
        /// Registers new entry from service.
        /// </summary>
        /// <param name="entry">The registered entry.</param>
        private void registerNewEntries(IEnumerable<DataEntry> entries)
        {
            lock (_L_entries)
            {
                foreach (var entry in entries)
                {
                    _entries.Add(entry);

                    if (EntryAdded != null)
                        EntryAdded(entry);
                }
            }
        }

        /// <summary>
        /// Sends connect command to given service.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        private void sendConnectCommand(string serviceName)
        {
            var command = new ServiceCommand<DataEntry>(NetworkService.InitCommand, serviceName, null);
            sendCommand(command);
        }

        /// <summary>
        /// Sends command to satelitte.
        /// </summary>
        /// <param name="command">Command to send.</param>
        private void sendCommand(ServiceCommand<DataEntry> command)
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(_client.GetStream(), command);
        }
    }
}
