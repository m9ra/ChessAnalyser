using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Net.Sockets;

using System.Runtime.Serialization.Formatters.Binary;

using ChessAnalyser.Satellite.Services;

namespace ChessAnalyser.Satellite.Network
{
    abstract class NetworkService
    {
        /// <summary>
        /// Command for initialization of service communication.
        /// </summary>
        public const string InitCommand = "INIT";

        /// <summary>
        /// Command for reporting of new entry.
        /// </summary>
        public const string NewEntryCommand = "NEW_ENTRY";

        /// <summary>
        /// Lock for clients
        /// </summary>
        protected readonly object _L_Clients = new object();

        /// <summary>
        /// Clients handled by current manager.
        /// </summary>
        private readonly List<TcpClient> _clients = new List<TcpClient>();

        /// <summary>
        /// Threads of registered clients.
        /// </summary>
        private readonly List<Thread> _clientThreads = new List<Thread>();

        /// <summary>
        /// Service which is provided through the network
        /// </summary>
        private readonly ServiceBase _providedService;

        /// <summary>
        /// Name of provided service.
        /// </summary>
        internal string Name { get { return _providedService.Name; } }


        /// <summary>
        /// Blocking call which will execute commands from given client.
        /// </summary>
        /// <param name="client">Client which commands will be read.</param>
        protected abstract void ReadCommands(TcpClient client);

        internal NetworkService(ServiceBase service)
        {
            _providedService = service;
        }

        internal void AcceptClient(TcpClient client)
        {
            lock (_L_Clients)
            {
                _clients.Add(client);
                var clientThread = new Thread(() => ReadCommands(client));
                _clientThreads.Add(clientThread);
                clientThread.Start();
            }
        }

        /// <summary>
        /// Gets thread safe copy of clients registered to service.
        /// </summary>
        /// <returns>The clients copy.</returns>
        protected IEnumerable<TcpClient> GetClientsCopy()
        {
            lock (_L_Clients)
            {
                return _clients.ToArray();
            }
        }


        /// <summary>
        /// Removes given client from service.
        /// </summary>
        /// <param name="client"></param>
        protected void RemoveClient(TcpClient client)
        {
            lock (_L_Clients)
            {
                var clientIndex = _clients.IndexOf(client);
                _clients.RemoveAt(clientIndex);
                _clientThreads.RemoveAt(clientIndex);
            }
        }
    }

    class NetworkService<DataEntry> : NetworkService
    {
        /// <summary>
        /// Service which is provided by the manager.
        /// </summary>
        private readonly ServiceBase<DataEntry> _service;

        internal NetworkService(ServiceBase<DataEntry> service)
            : base(service)
        {
            _service = service;

            _service.OnAdd += broadcastNewEntry;
        }

        #region Single client handling

        /// <inheritdoc/>
        protected override void ReadCommands(TcpClient client)
        {
            //we initialize communication by sending all data
            sendAllEntries(client);

            var isEnd = false;
            while (!isEnd)
            {
                var formatter = new BinaryFormatter();
                ServiceCommand<DataEntry> command;
                try
                {
                    command = formatter.Deserialize(client.GetStream()) as ServiceCommand<DataEntry>;
                }
                catch
                {
                    break;
                }

                switch (command.Command)
                {
                    default:
                        throw new NotImplementedException(command.ToString());
                }
            }

            RemoveClient(client);
        }

        /// <inheritdoc/>
        private void sendAllEntries(TcpClient client)
        {
            var entries = _service.GetAllEntries();
            var command = new ServiceCommand<DataEntry>(NetworkService.InitCommand, null, entries);
            send(client,command);
        }

        /// <summary>
        /// Sends command to given client.
        /// </summary>
        /// <param name="client">Client where entries will be sent.</param>
        /// <param name="command">Command to send.</param>
        private void send(TcpClient client, ServiceCommand<DataEntry> command)
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(client.GetStream(), command);
        }

        #endregion


        #region Broadcasting utilities

        /// <summary>
        /// Sends entry to all registered clients.
        /// </summary>
        /// <param name="entry">Entry to send.</param>
        private void broadcastNewEntry(DataEntry entry)
        {
            var clients = GetClientsCopy();

            var entryWrap = new[] { entry };
            var newEntryCommand = new ServiceCommand<DataEntry>(NetworkService.NewEntryCommand, null, entryWrap);

            foreach (var client in clients)
            {
                send(client, newEntryCommand);
            }
        }

        #endregion

    }
}
