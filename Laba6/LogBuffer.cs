using System;
using System.Collections.Concurrent;
using System.IO;
using System.Timers;

using Timer = System.Timers.Timer;

namespace Laba6
{
    public class LogBuffer
    {
        private readonly ConcurrentQueue<string> _bufferQueue = new ConcurrentQueue<string>();
        private readonly StreamWriter _streamWriter;

        private readonly int _bufLimit;

        public LogBuffer(string filePath, int bufLimit, int timerLimit)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception($"File {filePath} doesn't exists");
            }

            _streamWriter = new StreamWriter(filePath, true);

            _bufLimit = bufLimit;
            
            var timer = new Timer {Interval = timerLimit};
            timer.Elapsed += CheckTimeInterval;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public void Add(string msg)
        {
            _bufferQueue.Enqueue(msg);
            CheckBufferLimit();
        }

        private async void CheckBufferLimit()
        {
            if (_bufferQueue.Count < _bufLimit) return;
            while (!_bufferQueue.IsEmpty)
            {
                _bufferQueue.TryDequeue(out var message);
                if (message != null)
                {
                    await _streamWriter.WriteLineAsync(message);
                }
            }
        }

        private async void CheckTimeInterval(object sender, ElapsedEventArgs e)
        {
            while (!_bufferQueue.IsEmpty)
            {
                _bufferQueue.TryDequeue(out var message);
                if (message != null)
                {
                    await _streamWriter.WriteLineAsync(message);
                }
            }
        }

        public void Close()
        {
            _streamWriter.Close();
        }
    }
}