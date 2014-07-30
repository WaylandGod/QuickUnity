using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// The Utilitys namespace.
/// </summary>
namespace QuickUnity.Utilitys
{
    /// <summary>
    /// A class to process object things. This class cannot be inherited.
    /// </summary>
    public sealed class ObjectUtility
    {
        /// <summary>
        /// Deeps clone.
        /// </summary>
        /// <param name="source">The object of source.</param>
        /// <returns>System.Object.</returns>
        public object DeepClone(object source)
        {
            BinaryFormatter fomatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            MemoryStream stream = new MemoryStream();
            fomatter.Serialize(stream, source);
            stream.Position = 0;
            object clone = fomatter.Deserialize(stream);
            stream.Close();
            return clone;
        }
    }
}