namespace RMQ_App.Models
{
    public abstract record Task
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public TaskType Type { get; init; } = TaskType.None;
    }
}
