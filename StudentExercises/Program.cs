using System;
using System.Collections.Generic;

namespace StudentExercises
{
    class Program
    {
        static void Main(string[] args)
        {
            Repository repository = new Repository();

            List<Exercise> exercises = repository.GetAllExercises();
            string language = "Java";
            PrintAllExercises(exercises);
            
            Exercise insertExercise = new Exercise()
            {
                Name = "This one",
                Language = "Java"
            };
            Instructor insertInstructor = new Instructor()
            {
                FirstName = "instructorFirstName", LastName = "InstructorLastName", CohortId = 2, SlackHandle = "instructorSlack", Specialty = "throwing knives"
            };
            //repository.AddExercise(insertExercise);
            repository.AddInstructor(insertInstructor);
            PrintExercisesByLanguage(language, repository.GetExerciseByLanguage(language));
            PrintAllInstructors(repository.GetAllInstructors());

        }
        public static void PrintAllExercises(List<Exercise> exercises)
        {
            Console.WriteLine("Exercises:");
            foreach (Exercise exercise in exercises)
            {
                Console.WriteLine($"{exercise.Id}) {exercise.Name}: {exercise.Language}");
            }
        }

        public static void PrintAllInstructors(List<Instructor> instructors)
        {
            Console.WriteLine("Instructors:");
            foreach (Instructor instructor in instructors)
            {
                Console.WriteLine($"{instructor.Id}) {instructor.FirstName} {instructor.LastName}: {instructor.Cohort.Name}");
            }
        }
        public static void PrintExercisesByLanguage(string language, List<Exercise> exercises)
        {
            Console.WriteLine($"Exercises in {language}:");
            foreach (Exercise exercise in exercises)
            {
                Console.WriteLine($"{exercise.Name}: {exercise.Language}");
            }
        }
    }
}
