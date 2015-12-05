using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessAnalyser.Satellite.Network
{
    [Serializable]
    internal class ServiceCommand
    {
        /// <summary>
        /// Sent command.
        /// </summary>
        internal readonly string Command;

        /// <summary>
        /// Sent argument.
        /// </summary>
        internal readonly string Argument;

        internal ServiceCommand(string command, string argument)
        {
            Command = command;
            Argument = argument;
        }
    }

    [Serializable]
    internal class ServiceCommand<DataEntry> : ServiceCommand
    {
        /// <summary>
        /// Sent entries.
        /// </summary>
        internal IEnumerable<DataEntry> Entries { get { return _entries; } }

        /// <summary>
        /// Sent entries.
        /// </summary>
        private readonly DataEntry[] _entries;

        internal ServiceCommand(string command, string argument, DataEntry[] entries)
            : base(command, argument)
        {
            _entries = entries;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("{0}({1})", Command, Argument);
        }
    }
}
