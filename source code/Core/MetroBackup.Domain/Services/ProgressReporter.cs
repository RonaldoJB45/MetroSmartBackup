using MetroBackup.Domain.Interfaces;

namespace MetroBackup.Domain.Services
{
    public class ProgressReporter : IProgressReporter
    {
        public delegate void ProgressHandler(double progress);
        public event IProgressReporter.ProgressHandler ProgressChanged;

        public void ReportProgress(double progress)
        {
            ProgressChanged?.Invoke(progress);
        }
    }
}
