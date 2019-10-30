using System;
using System.Collections.Generic;
using System.Text;

namespace StudentExercises
{
    class Instructor
    {
        public string SlackHandle { get; set; }
        public string Specialty { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CohortId { get; set; }
        public int Id { get; set; }
        public Cohort Cohort { get; set; }


    }
}
