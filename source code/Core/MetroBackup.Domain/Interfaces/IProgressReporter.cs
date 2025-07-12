namespace MetroBackup.Domain.Interfaces
{
    public interface IProgressReporter
    {
        delegate void ProgressHandler(double progress, string message);
        event ProgressHandler ProgressChanged;
        void ReportProgress(double progress, string message);
    }
}