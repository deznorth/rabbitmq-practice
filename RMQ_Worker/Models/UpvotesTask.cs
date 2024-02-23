namespace RMQ_App.Models
{
    public record UpvotesTask : Task
    {
        public int PostId { get; init; }
        public int NumOfVotes { get; init; }
        public UpvotesTask(int postId, int numOfVotes)
        {
            this.Type = TaskType.Upvotes;
            PostId = postId;
            NumOfVotes = numOfVotes;
        }
    }
}
