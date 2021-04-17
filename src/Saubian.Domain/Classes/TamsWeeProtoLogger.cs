using System;
using System.Text;
using MailKit;

namespace Saubian.Domain.Classes
{
    public class TamsWeeProtoLogger : IProtocolLogger
    {
        private StringBuilder _sb;
        private bool _disposed = false;
        private bool _wheeshd = false;

        public TamsWeeProtoLogger()
        {
                
        }

        public bool StartSpoutin(bool orFuckinShutIt) => _wheeshd = !orFuckinShutIt;

        public TamsWeeProtoLogger(bool? fuckinShutIt)
        {
            _wheeshd = fuckinShutIt ?? false;
        }

        public void LogClient(byte[] buffer, int offset, int count)
        {
            ChantIt(buffer, offset, count, "Client");
        }

        public void LogConnect(Uri uri)
        {
            Console.WriteLine($"Av just tickled : {uri.AbsoluteUri}");
        }

        public void LogServer(byte[] buffer, int offset, int count)
        {
            ChantIt(buffer, offset, count, "Server");
        }

        public void ChantIt(byte[] buffer, int offset, int count, string fae)
        {
            var theChant = GetTaeFuckWithYerByteArrays(buffer, offset, count);

            if (!_wheeshd)
                Console.WriteLine($"{fae} is chantin : {theChant}");
        }

        public string GetTaeFuckWithYerByteArrays(byte[] buffer, int offset, int count)
        {
            _sb = new StringBuilder();
            for (int i = offset; i < count; i++)
            {
                _sb.Append(buffer[i].ToString("x2"));
            }

            return _sb.ToString();
        }

        ~TamsWeeProtoLogger()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _sb = null;
                }

                _disposed = true;
            }
        }
    }
}
