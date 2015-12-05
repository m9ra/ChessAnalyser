using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Satellite.Services
{

    /// <summary>
    /// Event used for data entries.
    /// </summary>
    /// <typeparam name="DataEntry">Type data entry.</typeparam>
    /// <param name="entry">Entry which fired the event.</param>
    public delegate void DataEntryEvent<DataEntry>(DataEntry entry);

    class ServiceBase
    {
        /// <summary>
        /// Name of service.
        /// </summary>
        internal readonly string Name;

        internal ServiceBase(string name)
        {
            Name = name;
        }
    }

    abstract class ServiceBase<DataEntry> : ServiceBase
    {

        /// <summary>
        /// Event fired whenever new data entry is added.
        /// </summary>
        internal event DataEntryEvent<DataEntry> OnAdd;

        /// <summary>
        /// Gets all entries that are provided by the service.
        /// </summary>
        /// <returns>The entries.</returns>
        internal abstract DataEntry[] GetAllEntries();

        internal ServiceBase(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Reports new data entry to service.
        /// </summary>
        /// <param name="entry">Reported entry.</param>
        protected void ReportNewEntry(DataEntry entry)
        {
            if (OnAdd != null)
                OnAdd(entry);
        }
    }
}
