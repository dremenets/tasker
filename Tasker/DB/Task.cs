using System;

namespace Tasker.DB
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ExpectedStart { get; set; }
        public string Params { get; set; }
        public Status Status { get; set; }
        public string Type { get; set; }
    }
}
