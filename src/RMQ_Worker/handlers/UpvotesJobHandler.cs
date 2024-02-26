using Common.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ_Worker.handlers
{
    public class UpvotesJobHandler : IJobHandler<UpvotesJob>
    {
        public UpvotesJobHandler() { }

        public Task DoWork(UpvotesJob job)
        {
            Console.WriteLine($" [x] Received upvotes request. Giving {job?.NumOfVotes} votes to postId: {job?.PostId}.");


            for (int i = 0; i < job?.NumOfVotes; i++)
            {
                Thread.Sleep(500);
                Console.WriteLine($"[postId: {job?.PostId}] votes completed: {i}");
            }

            return Task.CompletedTask;
        }
    }
}
