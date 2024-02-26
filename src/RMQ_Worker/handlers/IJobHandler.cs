using Common.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMQ_Worker.handlers
{
    public interface IJobHandler<T> where T : Job
    {
        Task DoWork(T job);
    }
}
