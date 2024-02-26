namespace Common.Jobs
{
    public record ExportJob : Job
    {
        public string FileName { get; init; }
        public int FileId { get; init; }

        public ExportJob(string fileName, int fileId) : base(JobType.Export)
        {
            FileName = fileName;
            FileId = fileId;
        }
    }
}
