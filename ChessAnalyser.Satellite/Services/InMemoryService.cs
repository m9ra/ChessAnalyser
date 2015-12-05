using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ChessAnalyser.Satellite.Services
{
    class InMemoryService<DataEntry> : ServiceBase<DataEntry>
    {
        /// <summary>
        /// Lock for data.
        /// </summary>
        private readonly object _L_data = new object();

        /// <summary>
        /// Data stored within service.
        /// </summary>
        private readonly List<DataEntry> _data = new List<DataEntry>();


        internal InMemoryService(IEnumerable<DataEntry> initialData, string serviceName)
            : base(serviceName)
        {
            lock (_L_data)
            {
                _data.AddRange(initialData);
            }
        }

        /// <summary>
        /// Adds new entry to memory service.
        /// </summary>
        /// <param name="entry">Added entry.</param>
        internal void AddNewEntry(DataEntry entry)
        {
            ReportNewEntry(entry);
        }

        /// <inheritdoc/>
        internal override DataEntry[] GetAllEntries()
        {
            lock (_L_data)
            {
                return _data.ToArray();
            }
        }
    }
}
