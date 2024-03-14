using MetroBackup.Domain.Interfaces;

namespace MetroBackup.Domain.Services
{
    public class ProgressReporter : IProgressReporter
    {
        private readonly object lockObject = new object();
        public delegate void ProgressHandler(double progress);
        public event IProgressReporter.ProgressHandler ProgressChanged;

        public void ReportProgress(double progress)
        {
            lock (lockObject)
            {
                ProgressChanged?.Invoke(progress);
            }
        }
    }
}
