using DBLibrary.Entity;
using System.Collections.Generic;

namespace Tasker
{
    public interface IJobControl
    {
        void Init(Task task);
        List<int> GetScheduledTaskIds();
    }
}
