using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacketStringManager.Model
{
    public class Job
    {
        public Guid JobId { get; set; }

        public string Name { get; set; }

        public string Racket { get; set; }

        public double Tension { get; set; }

        public string StringName { get; set; }

        public DateOnly StartDate { get; set; }

        public bool IsCompleted { get; set; }

        public DateOnly EndDate { get; set; }

        public bool IsPaid { get; set; }

        public string Comment { get; set; }
    }
}
