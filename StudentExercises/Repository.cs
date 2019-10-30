using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace StudentExercises
{
    class Repository
    {
            public SqlConnection Connection
            {
                get
                {
                    // This is "address" of the database
                    string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=StudentExercises;Integrated Security=True";
                    return new SqlConnection(_connectionString);
                }
            }


            /************************************************************************************
             * Departments
             ************************************************************************************/

            /// <summary>
            ///  Returns a list of all departments in the database
            /// </summary>
            public List<Exercise> GetAllExercises()
            {
                // We must "use" the database connection.
                //  Because a database is a shared resource (other applications may be using it too) we must
                //  be careful about how we interact with it. Specifically, we Open() connections when we need to
                //  interact with the database and we Close() them when we're finished.
                //  In C#, a "using" block ensures we correctly disconnect from a resource even if there is an error.
                //  For database connections, this means the connection will be properly closed.
                using (SqlConnection conn = Connection)
                {
                    // Note, we must Open() the connection, the "using" block doesn't do that for us.
                    conn.Open();

                    // We must "use" commands too.
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        // Here we setup the command with the SQL we want to execute before we execute it.
                        cmd.CommandText = "SELECT Id, name, language FROM Exercise";

                        // Execute the SQL in the database and get a "reader" that will give us access to the data.
                        SqlDataReader reader = cmd.ExecuteReader();

                        // A list to hold the departments we retrieve from the database.
                        List<Exercise> exercises = new List<Exercise>();

                        // Read() will return true if there's more data to read
                        while (reader.Read())
                        {
                            // The "ordinal" is the numeric position of the column in the query results.
                            //  For our query, "Id" has an ordinal value of 0 and "DeptName" is 1.
                            int idColumnPosition = reader.GetOrdinal("Id");

                            // We user the reader's GetXXX methods to get the value for a particular ordinal.
                            int idValue = reader.GetInt32(idColumnPosition);

                            int nameColumnPosition = reader.GetOrdinal("name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            int languageColumnPosition = reader.GetOrdinal("language");
                            string languageValue = reader.GetString(languageColumnPosition);

                        // Now let's create a new department object using the data from the database.
                        Exercise exercise = new Exercise
                            {
                                Id = idValue,
                                Name = nameValue,
                                Language = languageValue
                            };

                            // ...and add that department object to our list.
                            exercises.Add(exercise);
                        }

                        // We should Close() the reader. Unfortunately, a "using" block won't work here.
                        reader.Close();

                        // Return the list of departments who whomever called this method.
                        return exercises;
                    }
                }
            }
        public List<Exercise> GetExerciseByLanguage(string language)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, name, language FROM Exercise WHERE language = @language";
                    cmd.Parameters.Add(new SqlParameter("@language", language));
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();
                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");

                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        int languageColumnPosition = reader.GetOrdinal("language");
                        string languageValue = reader.GetString(languageColumnPosition);

                        Exercise exercise = new Exercise
                        {
                            Id = idValue,
                            Name = nameValue,
                            Language = languageValue
                        };
                        exercises.Add(exercise);
                    }

                    
                    reader.Close();

                    return exercises;
                }
            }
        }
        public void AddExercise(Exercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Exercise (name, language) Values (@name, @language)";
                    cmd.Parameters.Add(new SqlParameter("@name", exercise.Name));
                    cmd.Parameters.Add(new SqlParameter("@language", exercise.Language));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void AddInstructor(Instructor instructor)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Instructor (firstName, lastName, slackHandle, cohortId, specialty) Values (@firstName, @lastName, @slackHandle, @cohortId, @specialty)";
                    cmd.Parameters.Add(new SqlParameter("@firstName", instructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", instructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackHandle", instructor.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", instructor.CohortId));
                    cmd.Parameters.Add(new SqlParameter("@specialty", instructor.Specialty));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Instructor> GetAllInstructors()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select i.slackHandle, i.id, i.cohortId, i.specialty, i.firstName, i.lastName, c.Name from Instructor i left join Cohort c on c.id = i.cohortId";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Instructor> instructors = new List<Instructor>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");

                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstNameColumnPosition = reader.GetOrdinal("firstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);

                        int lastNameColumnPosition = reader.GetOrdinal("lastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);

                        Instructor instructor = new Instructor
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            SlackHandle = reader.GetString(reader.GetOrdinal("slackHandle")),
                            CohortId = reader.GetInt32(reader.GetOrdinal("cohortId")),
                            Cohort = new Cohort
                            {
                               Id = reader.GetInt32(reader.GetOrdinal("cohortId")),
                               Name = reader.GetString(reader.GetOrdinal("name"))
                            }
                        };

                        instructors.Add(instructor);
                    }

                    reader.Close();

                    return instructors;
                }
            }
        }


    }
}
