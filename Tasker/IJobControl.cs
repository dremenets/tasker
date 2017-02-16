using System.Collections.Generic;

namespace Tasker
{
    public interface IJobControl
    {
        void Init();
        List<int> GetScheduledTaskIds();
    }
}
