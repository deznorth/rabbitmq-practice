namespace Common.Jobs
{
    public record UpvotesJob : Job
    {
        public int PostId { get; init; }
        public int NumOfVotes { get; init; }
        public UpvotesJob(int postId, int numOfVotes) : base(JobType.Upvotes)
        {
            PostId = postId;
            NumOfVotes = numOfVotes;
        }
    }
}
