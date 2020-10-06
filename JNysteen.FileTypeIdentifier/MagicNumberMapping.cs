using System;
using System.Collections.Generic;
using System.Linq;
using JNysteen.FileTypeIdentifier.Exceptions;

namespace JNysteen.FileTypeIdentifier
{
    internal class MagicNumberMapping
    {
        internal readonly List<FileMagicNumberDefinition> FileMagicNumberMappingTable;

        /// <summary>
        ///     Creates a new, empty FileMagicNumberMapping
        /// </summary>
        public MagicNumberMapping()
        {
            FileMagicNumberMappingTable = new List<FileMagicNumberDefinition>();
        }

        public void AddMagicNumberDefinition(FileMagicNumberDefinition fileMagicNumberDefinition)
        {
            if (fileMagicNumberDefinition == null) 
                throw new ArgumentNullException(nameof(fileMagicNumberDefinition));
            
            FileMagicNumberMappingTable.Add(fileMagicNumberDefinition);
            FileMagicNumberMappingTable.Sort((oneMagicNumber, otherMagicNumber) => oneMagicNumber.LongestMagicNumber > otherMagicNumber.LongestMagicNumber ? -1 : 1);
        }

        /// <summary>
        ///     Fetches the longest magic number found in the configuration
        /// </summary>
        /// <returns>The longest magic number found in the configuration</returns>
        /// <exception cref="InvalidConfigurationException">Thrown if no magic numbers are contained in the mapping table</exception>
        public int GetLongestMagicNumber()
        {
            if (!FileMagicNumberMappingTable.Any())
                throw new InvalidConfigurationException("The mapping does not contain any magic numbers!");

            return FileMagicNumberMappingTable.Max(t => t.LongestMagicNumber);
        }
    }
}