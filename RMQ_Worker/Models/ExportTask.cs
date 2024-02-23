namespace RMQ_App.Models
{
    public record ExportTask : Task
    {
        public string FileName { get; init; }
        public int FileId { get; init; }

        public ExportTask(string fileName, int fileId)
        {
            this.Type = TaskType.Export;
            FileName = fileName;
            FileId = fileId;
        }
    }
}
