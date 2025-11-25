using CourseManagement.Core;
using System.Text.Json;

namespace Main
{
    internal static class Program
    {
        private static void Main()
        {
            string systemName = "ITMO Course Management System";
            
            var teachers = new List<Teacher>();
            try
            {
                string teachersText = File.ReadAllText("data/teachers.json");
                var teachersData = JsonSerializer.Deserialize<TeachersData>(teachersText);
                if (teachersData != null && teachersData.Teachers != null)
                {
                    foreach (var teacherData in teachersData.Teachers)
                    {
                        Teacher teacher = new Teacher(
                            teacherData.Id,
                            teacherData.Name,
                            teacherData.Email,
                            teacherData.Department,
                            teacherData.Specialization
                        );
                        teachers.Add(teacher);
                    }
                }
            }
            catch
            {
            }

            var students = new List<Student>();
            try
            {
                string studentsText = File.ReadAllText("data/students.json");
                var studentsData = JsonSerializer.Deserialize<StudentsData>(studentsText);
                if (studentsData != null && studentsData.Students != null)
                {
                    foreach (var studentData in studentsData.Students)
                    {
                        Student student = new Student(
                            studentData.Id,
                            studentData.Name,
                            studentData.Email,
                            studentData.Year,
                            studentData.Major
                        );
                        students.Add(student);
                    }
                }
            }
            catch
            {
            }

            var courses = new List<Course>();
            try
            {
                string coursesText = File.ReadAllText("data/courses.json");
                var coursesData = JsonSerializer.Deserialize<CoursesData>(coursesText);
                if (coursesData != null && coursesData.Courses != null)
                {
                    foreach (var courseData in coursesData.Courses)
                    {
                        Course course;
                        if (courseData.Type.ToLower() == "online")
                        {
                            course = new OnlineCourse(
                                courseData.Id,
                                courseData.Name,
                                courseData.Description,
                                courseData.TeacherId,
                                courseData.MaxStudents,
                                courseData.Platform ?? ""
                            );
                        }
                        else if (courseData.Type.ToLower() == "offline")
                        {
                            course = new OfflineCourse(
                                courseData.Id,
                                courseData.Name,
                                courseData.Description,
                                courseData.TeacherId,
                                courseData.MaxStudents,
                                courseData.Room ?? "",
                                courseData.Schedule ?? ""
                            );
                        }
                        else
                        {
                            continue;
                        }

                        if (courseData.Students != null)
                        {
                            foreach (var studentId in courseData.Students)
                            {
                                course.EnrollStudent(studentId);
                            }
                        }

                        courses.Add(course);
                    }
                }
            }
            catch
            {
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
            if (courses.Count == 0)
            {
                Console.WriteLine("No courses available.");
                return;
            }

            foreach (var course in courses)
            {
                var teacher = courseManager.GetTeacher(course.TeacherId);
                string teacherName = teacher != null ? teacher.Name : "Unknown";
                Console.WriteLine($"{course.Id}. {course.Name} | Teacher: {teacherName} | Students: {course.Students.Count}/{course.MaxStudents}");
            }
        }

        private static void ShowAllTeachers(CourseManager courseManager)
        {
            Console.WriteLine("-- All Teachers --");
            var teachers = courseManager.GetAllTeachers();
            if (teachers.Count == 0)
            {
                Console.WriteLine("No teachers available.");
                return;
            }

            foreach (var teacher in teachers)
            {
                var courses = courseManager.GetCoursesByTeacher(teacher.Id);
                int courseCount = courses.Count;
                Console.WriteLine($"{teacher.Id}. {teacher} | Courses: {courseCount}");
            }
        }

        private static void ShowAllStudents(CourseManager courseManager)
        {
            Console.WriteLine("-- All Students --");
            var students = courseManager.GetAllStudents();
            if (students.Count == 0)
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
            if (courses.Count == 0)
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
            string teacherName = teacher != null ? teacher.Name : "Unknown";
            Console.WriteLine($"Teacher: {teacherName}");
            Console.WriteLine($"Location: {course.GetLocationInfo()}");
            Console.WriteLine($"Enrolled Students: {string.Join(", ", course.Students)}");
        }
    }

    internal class TeachersData
    {
        public List<TeacherData>? Teachers { get; set; }
    }

    internal class TeacherData
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Department { get; set; } = "";
        public string Specialization { get; set; } = "";
    }

    internal class StudentsData
    {
        public List<StudentData>? Students { get; set; }
    }

    internal class StudentData
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public int Year { get; set; }
        public string Major { get; set; } = "";
    }

    internal class CoursesData
    {
        public List<CourseData>? Courses { get; set; }
    }

    internal class CourseData
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Type { get; set; } = "";
        public int TeacherId { get; set; }
        public int MaxStudents { get; set; }
        public string? Platform { get; set; }
        public string? Room { get; set; }
        public string? Schedule { get; set; }
        public List<int>? Students { get; set; }
    }
}
