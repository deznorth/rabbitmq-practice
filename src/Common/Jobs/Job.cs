namespace Common.Jobs
{
    public record Job
    {
        public Guid Id { get; init; }
        public JobType JobType { get; init; }

        public Job(JobType type, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
            JobType = type;
        }
    }
}
