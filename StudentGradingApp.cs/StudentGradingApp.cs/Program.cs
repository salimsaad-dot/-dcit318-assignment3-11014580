using System;
using System.Collections.Generic;
using System.IO;

namespace DCIT318_Assignment3
{
    // Custom Exceptions
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // Student Class
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Score { get; set; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            return Score switch
            {
                >= 80 => "A",
                >= 70 => "B",
                >= 60 => "C",
                >= 50 => "D",
                _ => "F"
            };
        }
    }

    // Processor Class
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts.Length != 3)
                        throw new MissingFieldException("Missing fields in line: " + line);

                    if (!int.TryParse(parts[0], out int id))
                        throw new FormatException("Invalid ID format in line: " + line);

                    string name = parts[1].Trim();
                    if (!int.TryParse(parts[2], out int score))
                        throw new InvalidScoreFormatException("Invalid score format in line: " + line);

                    students.Add(new Student(id, name, score));
                }
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (var student in students)
                {
                    writer.WriteLine($"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}");
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "students.txt";      // Input: Salim Saad, Abanga Kwame, Esi Quaye
            string outputFile = "report.txt";

            try
            {
                var processor = new StudentResultProcessor();
                var students = processor.ReadStudentsFromFile(inputFile);
                processor.WriteReportToFile(students, outputFile);
                Console.WriteLine("Report successfully written to " + outputFile);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: Input file not found.");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
