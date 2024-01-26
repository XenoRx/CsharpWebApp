using Market.Abstraction;

namespace Market.Models
{
    public class MyLogger : IMyLogger
    {
        private readonly IWriter _writer;

        public MyLogger(IWriter writer) => _writer = writer;

        public void Log(string message)
        {
            _writer.Write(message);
        }
    }
}
