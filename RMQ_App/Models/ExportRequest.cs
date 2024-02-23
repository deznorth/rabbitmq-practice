namespace RMQ_App.Models
{
    public record ExportRequest
    {
        public string FileName { get; init; } = "unknown";
        public int FileId { get; init; } = 0;
    }
}
