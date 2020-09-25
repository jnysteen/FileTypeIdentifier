using System;
using System.Runtime.Serialization;

namespace JNysteen.FileTypeIdentifier.Exceptions
{
    /// <summary>
    ///     Thrown when a component is misconfigured.
    /// </summary>
    public class InvalidConfigurationException : Exception
    {
        /// <inheritdoc />
        public InvalidConfigurationException()
        {
        }

        /// <inheritdoc />
        public InvalidConfigurationException(string message) : base(message)
        {
        }

        /// <inheritdoc />
        public InvalidConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <inheritdoc />
        protected InvalidConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}