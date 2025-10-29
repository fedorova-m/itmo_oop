using CourseManagement.Core;
using Microsoft.Extensions.Configuration;

namespace Main
{
    internal static class Program
    {
        private static void Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("data/appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("data/courses.json", optional: false, reloadOnChange: true)
                .AddJsonFile("data/teachers.json", optional: false, reloadOnChange: true)
                .AddJsonFile("data/students.json", optional: false, reloadOnChange: true)
                .Build();

            var systemName = configuration["SystemName"] ?? "Course Management System";

            // Загружаем преподавателей
            var teachers = new List<Teacher>();
            var teachersSection = configuration.GetSection("Teachers");
            foreach (var teacherSection in teachersSection.GetChildren())
            {
                var id = int.Parse(teacherSection["Id"] ?? "0");
                var name = teacherSection["Name"] ?? "";
                var email = teacherSection["Email"] ?? "";
                var department = teacherSection["Department"] ?? "";
                var specialization = teacherSection["Specialization"] ?? "";
                teachers.Add(new Teacher(id, name, email, department, specialization));
            }

            // Загружаем студентов
            var students = new List<Student>();
            var studentsSection = configuration.GetSection("Students");
            foreach (var studentSection in studentsSection.GetChildren())
            {
                var id = int.Parse(studentSection["Id"] ?? "0");
                var name = studentSection["Name"] ?? "";
                var email = studentSection["Email"] ?? "";
                var year = int.Parse(studentSection["Year"] ?? "0");
                var major = studentSection["Major"] ?? "";
                students.Add(new Student(id, name, email, year, major));
            }

            // Загружаем курсы
            var courses = new List<Course>();
            var coursesSection = configuration.GetSection("Courses");
            foreach (var courseSection in coursesSection.GetChildren())
            {
                var id = int.Parse(courseSection["Id"] ?? "0");
                var name = courseSection["Name"] ?? "";
                var description = courseSection["Description"] ?? "";
                var type = courseSection["Type"] ?? "";
                var teacherId = int.Parse(courseSection["TeacherId"] ?? "0");
                var maxStudents = int.Parse(courseSection["MaxStudents"] ?? "0");

                Course course = type.ToLower() switch
                {
                    "online" => new OnlineCourse(id, name, description, teacherId, maxStudents, courseSection["Platform"] ?? ""),
                    "offline" => new OfflineCourse(id, name, description, teacherId, maxStudents, courseSection["Room"] ?? "", courseSection["Schedule"] ?? ""),
                    _ => throw new ArgumentException($"Unknown course type: {type}")
                };

                // Загружаем студентов курса
                var courseStudentsSection = courseSection.GetSection("Students");
                foreach (var studentId in courseStudentsSection.GetChildren())
                {
                    if (int.TryParse(studentId.Value, out var studentIdInt))
                    {
                        course.EnrollStudent(studentIdInt);
                    }
                }

                courses.Add(course);
            }

            var courseManager = new CourseManager(courses, teachers, students);

            Console.WriteLine($"== {systemName} ==");
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1) Show all courses");
                Console.WriteLine("2) Show all teachers");
                Console.WriteLine("3) Show all students");
                Console.WriteLine("4) Show courses by teacher");
                Console.WriteLine("5) Add new course");
                Console.WriteLine("6) Remove course");
                Console.WriteLine("7) Enroll student in course");
                Console.WriteLine("8) Unenroll student from course");
                Console.WriteLine("9) Show course details");
                Console.WriteLine("0) Exit");
                Console.Write("Choose: ");

                var choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        ShowAllCourses(courseManager);
                        break;

                    case "2":
                        ShowAllTeachers(courseManager);
                        break;

                    case "3":
                        ShowAllStudents(courseManager);
                        break;

                    case "4":
                        ShowCoursesByTeacher(courseManager);
                        break;

                    case "5":
                        AddNewCourse(courseManager);
                        break;

                    case "6":
                        RemoveCourse(courseManager);
                        break;

                    case "7":
                        EnrollStudent(courseManager);
                        break;

                    case "8":
                        UnenrollStudent(courseManager);
                        break;

                    case "9":
                        ShowCourseDetails(courseManager);
                        break;


                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Unknown option");
                        break;
                }
            }
        }

        private static void ShowAllCourses(CourseManager courseManager)
        {
            Console.WriteLine("-- All Courses --");
            var courses = courseManager.GetAllCourses();
            if (!courses.Any())
            {
                Console.WriteLine("No courses available.");
                return;
            }

            foreach (var course in courses)
            {
                var teacher = courseManager.GetTeacher(course.TeacherId);
                Console.WriteLine($"{course.Id}. {course.Name} | Teacher: {teacher?.Name ?? "Unknown"} | Students: {course.Students.Count}/{course.MaxStudents}");
            }
        }

        private static void ShowAllTeachers(CourseManager courseManager)
        {
            Console.WriteLine("-- All Teachers --");
            var teachers = courseManager.GetAllTeachers();
            if (!teachers.Any())
            {
                Console.WriteLine("No teachers available.");
                return;
            }

            foreach (var teacher in teachers)
            {
                var courseCount = courseManager.GetCoursesByTeacher(teacher.Id).Count();
                Console.WriteLine($"{teacher.Id}. {teacher} | Courses: {courseCount}");
            }
        }

        private static void ShowAllStudents(CourseManager courseManager)
        {
            Console.WriteLine("-- All Students --");
            var students = courseManager.GetAllStudents();
            if (!students.Any())
            {
                Console.WriteLine("No students available.");
                return;
            }

            foreach (var student in students)
            {
                Console.WriteLine($"{student.Id}. {student}");
            }
        }

        private static void ShowCoursesByTeacher(CourseManager courseManager)
        {
            Console.Write("Enter teacher ID: ");
            if (!int.TryParse(Console.ReadLine(), out var teacherId))
            {
                Console.WriteLine("Invalid teacher ID.");
                return;
            }

            var teacher = courseManager.GetTeacher(teacherId);
            if (teacher == null)
            {
                Console.WriteLine("Teacher not found.");
                return;
            }

            Console.WriteLine($"-- Courses taught by {teacher.Name} --");
            var courses = courseManager.GetCoursesByTeacher(teacherId);
            if (!courses.Any())
            {
                Console.WriteLine("No courses found for this teacher.");
                return;
            }

            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Id}. {course.Name} | Students: {course.Students.Count}/{course.MaxStudents}");
            }
        }

        private static void AddNewCourse(CourseManager courseManager)
        {
            Console.WriteLine("-- Add New Course --");
            Console.Write("Course ID: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid ID.");
                return;
            }

            Console.Write("Course Name: ");
            var name = Console.ReadLine() ?? "";

            Console.Write("Description: ");
            var description = Console.ReadLine() ?? "";

            Console.Write("Teacher ID: ");
            if (!int.TryParse(Console.ReadLine(), out var teacherId))
            {
                Console.WriteLine("Invalid teacher ID.");
                return;
            }

            Console.Write("Max Students: ");
            if (!int.TryParse(Console.ReadLine(), out var maxStudents))
            {
                Console.WriteLine("Invalid max students.");
                return;
            }

            Console.WriteLine("Course Type (1 - Online, 2 - Offline): ");
            var typeChoice = Console.ReadLine();

            Course? course = typeChoice switch
            {
                "1" => CreateOnlineCourse(id, name, description, teacherId, maxStudents),
                "2" => CreateOfflineCourse(id, name, description, teacherId, maxStudents),
                _ => null
            };

            if (course == null)
            {
                Console.WriteLine("Invalid course type.");
                return;
            }

            if (courseManager.AddCourse(course))
            {
                Console.WriteLine($"Course '{name}' added successfully!");
            }
            else
            {
                Console.WriteLine("Failed to add course. Course ID might already exist.");
            }
        }

        private static Course CreateOnlineCourse(int id, string name, string description, int teacherId, int maxStudents)
        {
            Console.Write("Platform (e.g., Zoom, Teams): ");
            var platform = Console.ReadLine() ?? "";
            return new OnlineCourse(id, name, description, teacherId, maxStudents, platform);
        }

        private static Course CreateOfflineCourse(int id, string name, string description, int teacherId, int maxStudents)
        {
            Console.Write("Room: ");
            var room = Console.ReadLine() ?? "";
            Console.Write("Schedule (e.g., Mon, Wed, Fri 10:00-11:30): ");
            var schedule = Console.ReadLine() ?? "";
            return new OfflineCourse(id, name, description, teacherId, maxStudents, room, schedule);
        }

        private static void RemoveCourse(CourseManager courseManager)
        {
            Console.Write("Enter course ID to remove: ");
            if (!int.TryParse(Console.ReadLine(), out var courseId))
            {
                Console.WriteLine("Invalid course ID.");
                return;
            }

            var course = courseManager.GetCourse(courseId);
            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            Console.WriteLine($"Are you sure you want to remove course '{course.Name}'? (y/n)");
            var confirmation = Console.ReadLine()?.ToLower();
            if (confirmation != "y")
            {
                Console.WriteLine("Operation cancelled.");
                return;
            }

            if (courseManager.RemoveCourse(courseId))
            {
                Console.WriteLine($"Course '{course.Name}' removed successfully!");
            }
            else
            {
                Console.WriteLine("Failed to remove course.");
            }
        }

        private static void EnrollStudent(CourseManager courseManager)
        {
            Console.Write("Enter course ID: ");
            if (!int.TryParse(Console.ReadLine(), out var courseId))
            {
                Console.WriteLine("Invalid course ID.");
                return;
            }

            Console.Write("Enter student ID: ");
            if (!int.TryParse(Console.ReadLine(), out var studentId))
            {
                Console.WriteLine("Invalid student ID.");
                return;
            }

            var course = courseManager.GetCourse(courseId);
            var student = courseManager.GetStudent(studentId);

            if (course == null)
            {
                Console.WriteLine($"Course with ID {courseId} not found.");
                return;
            }

            if (student == null)
            {
                Console.WriteLine($"Student with ID {studentId} not found.");
                return;
            }

            if (courseManager.EnrollStudentInCourse(courseId, studentId))
            {
                Console.WriteLine($"Student {student.Name} (ID: {studentId}) enrolled successfully in course '{course.Name}'!");
            }
            else
            {
                Console.WriteLine($"Failed to enroll student. Course might be full or student already enrolled.");
            }
        }

        private static void UnenrollStudent(CourseManager courseManager)
        {
            Console.Write("Enter course ID: ");
            if (!int.TryParse(Console.ReadLine(), out var courseId))
            {
                Console.WriteLine("Invalid course ID.");
                return;
            }

            Console.Write("Enter student ID: ");
            if (!int.TryParse(Console.ReadLine(), out var studentId))
            {
                Console.WriteLine("Invalid student ID.");
                return;
            }

            var course = courseManager.GetCourse(courseId);
            var student = courseManager.GetStudent(studentId);

            if (course == null)
            {
                Console.WriteLine($"Course with ID {courseId} not found.");
                return;
            }

            if (student == null)
            {
                Console.WriteLine($"Student with ID {studentId} not found.");
                return;
            }

            if (courseManager.UnenrollStudentFromCourse(courseId, studentId))
            {
                Console.WriteLine($"Student {student.Name} (ID: {studentId}) unenrolled successfully from course '{course.Name}'!");
            }
            else
            {
                Console.WriteLine($"Failed to unenroll student. Student might not be enrolled in this course.");
            }
        }

        private static void ShowCourseDetails(CourseManager courseManager)
        {
            Console.Write("Enter course ID: ");
            if (!int.TryParse(Console.ReadLine(), out var courseId))
            {
                Console.WriteLine("Invalid course ID.");
                return;
            }

            var course = courseManager.GetCourse(courseId);
            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            var teacher = courseManager.GetTeacher(course.TeacherId);
            Console.WriteLine($"-- Course Details --");
            Console.WriteLine(course.GetCourseDetails());
            Console.WriteLine($"Teacher: {teacher?.Name ?? "Unknown"}");
            Console.WriteLine($"Location: {course.GetLocationInfo()}");
            Console.WriteLine($"Enrolled Students: {string.Join(", ", course.Students)}");
        }

    }
}
