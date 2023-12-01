namespace MetroBackup.Domain.Interfaces
{
    public interface IProgressReporter
    {
        delegate void ProgressHandler(double progress);
        event ProgressHandler ProgressChanged;
        void ReportProgress(double progress);
    }
}