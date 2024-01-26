
using Market.Abstraction;

namespace Market.Models
{
    public class FileWriter : IWriter
    {
        private readonly string _filename;

        public FileWriter(string filename)
        {
            _filename = filename;
        }

        public void Write(string value)
        {
            File.AppendAllText(_filename, value);
        }
    }
}
