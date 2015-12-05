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
    class NetworkServiceManager
    {
        /// <summary>
        /// Port where service manager is listening.
        /// </summary>
        internal readonly int ListenPort;

        /// <summary>
        /// Determine whether manager should end.
        /// </summary>
        private volatile bool _isEnd = false;

        /// <summary>
        /// Index of registered services.
        /// </summary>
        private readonly Dictionary<string, NetworkService> _serviceIndex = new Dictionary<string, NetworkService>();

        internal NetworkServiceManager(int listenPort)
        {
            ListenPort = listenPort;
        }

        /// <summary>
        /// Adds network services to the manager.
        /// </summary>
        /// <param name="services">Added services.</param>
        /// <typeparam name="DataEntry">Type of data entries provided by services</typeparam>
        internal void RegisterServices<DataEntry>(IEnumerable<ServiceBase<DataEntry>> services)
        {
            foreach (var service in services)
            {
                var serviceName = service.Name;
                var networkService = new NetworkService<DataEntry>(service);
                _serviceIndex.Add(serviceName, networkService);
            }
        }

        /// <summary>
        /// Runs service manager routines.
        /// </summary>
        internal void Run()
        {
            var listener = new TcpListener(IPAddress.Any, ListenPort);
            listener.Start();
            while (!_isEnd)
            {
                var client = listener.AcceptTcpClient();
                if (client != null)
                {
                    //simple server accepts clients in separate threads
                    var th = new Thread(() => _handleClient(client));
                    th.Start();
                }
            }
        }

        /// <summary>
        /// Handles accepted client.
        /// </summary>
        /// <param name="client">Client that has been accepted.</param>
        private void _handleClient(TcpClient client)
        {
            var formatter = new BinaryFormatter();
            var initCommand = formatter.Deserialize(client.GetStream()) as ServiceCommand;
            if (initCommand.Command != NetworkService.InitCommand)
            {
                Console.WriteLine("[ERROR] Requesting command {0}, before client is initialized", initCommand);
                disconnect(client);
            }

            var serviceName = initCommand.Argument;
            if (!_serviceIndex.ContainsKey(serviceName))
            {
                Console.WriteLine("[ERROR] Requesting service {0}, which is not known.", serviceName);
                disconnect(client);
            }

            _serviceIndex[serviceName].AcceptClient(client);
        }

        /// <summary>
        /// Disconnects given client without throwing any exceptions.
        /// </summary>
        /// <param name="client">Client to be disconnect.</param>
        private void disconnect(TcpClient client)
        {
            try
            {
                client.Close();
            }
            catch
            {
                //nothing to do
            }
        }
    }
}
