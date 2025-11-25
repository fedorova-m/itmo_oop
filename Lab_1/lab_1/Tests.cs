using Xunit;
using CourseManagement.Core;

namespace CourseManagement.Tests
{
    public class StudentTests
    {
        [Fact]
        public void Student_ShouldCreateCorrectly()
        {
            // Arrange 
            var student = new Student(101, "Anna Smirnova", "asmirnova@student.itmo.ru", 2, "Computer Science");

            // Assert
            Assert.Equal(101, student.Id);
            Assert.Equal("Anna Smirnova", student.Name);
            Assert.Equal("asmirnova@student.itmo.ru", student.Email);
            Assert.Equal(2, student.Year);
            Assert.Equal("Computer Science", student.Major);
        }

        [Fact]
        public void Student_ToString_ShouldReturnCorrectFormat()
        {
            // Arrange
            var student = new Student(101, "Anna Smirnova", "asmirnova@student.itmo.ru", 2, "Computer Science");

            // Act
            var result = student.ToString();

            // Assert
            Assert.Equal("Anna Smirnova (Year 2, Computer Science)", result);
        }
    }

    public class TeacherTests
    {
        [Fact]
        public void Teacher_ShouldCreateCorrectly()
        {
            // Arrange & Act
            var teacher = new Teacher(1, "Ivan Petrov", "ipetrov@itmo.ru", "Computer Science", "Software Engineering");

            // Assert
            Assert.Equal(1, teacher.Id);
            Assert.Equal("Ivan Petrov", teacher.Name);
            Assert.Equal("ipetrov@itmo.ru", teacher.Email);
            Assert.Equal("Computer Science", teacher.Department);
            Assert.Equal("Software Engineering", teacher.Specialization);
        }

        [Fact]
        public void Teacher_ToString_ShouldReturnCorrectFormat()
        {
            // Arrange
            var teacher = new Teacher(1, "Ivan Petrov", "ipetrov@itmo.ru", "Computer Science", "Software Engineering");

            // Act
            var result = teacher.ToString();

            // Assert
            Assert.Equal("Ivan Petrov (Computer Science) - Software Engineering", result);
        }
    }

    public class CourseTests
    {
        [Fact]
        public void OnlineCourse_ShouldCreateCorrectly()
        {
            // Arrange & Act
            var course = new OnlineCourse(1, "Programming Fundamentals", "Introduction to programming", 1, 30, "Zoom");

            // Assert
            Assert.Equal(1, course.Id);
            Assert.Equal("Programming Fundamentals", course.Name);
            Assert.Equal("Introduction to programming", course.Description);
            Assert.Equal(1, course.TeacherId);
            Assert.Equal(30, course.MaxStudents);
            Assert.Equal("Zoom", course.Platform);
            Assert.Empty(course.Students);
        }

        [Fact]
        public void OfflineCourse_ShouldCreateCorrectly()
        {
            // Arrange & Act
            var course = new OfflineCourse(1, "Data Structures", "Advanced algorithms", 2, 25, "1101", "Mon, Wed, Fri 10:00-11:30");

            // Assert
            Assert.Equal(1, course.Id);
            Assert.Equal("Data Structures", course.Name);
            Assert.Equal("Advanced algorithms", course.Description);
            Assert.Equal(2, course.TeacherId);
            Assert.Equal(25, course.MaxStudents);
            Assert.Equal("1101", course.Room);
            Assert.Equal("Mon, Wed, Fri 10:00-11:30", course.Schedule);
            Assert.Empty(course.Students);
        }

        [Fact]
        public void EnrollStudent_ShouldAddStudentWhenCapacityAvailable()
        {
            // Arrange
            var course = new OnlineCourse(1, "Test Course", "Test Description", 1, 2, "Zoom");

            // Act
            var result = course.EnrollStudent(101);

            // Assert
            Assert.True(result);
            Assert.Contains(101, course.Students);
            Assert.Single(course.Students);
        }

        [Fact]
        public void EnrollStudent_ShouldNotAddStudentWhenCapacityFull()
        {
            // Arrange
            var course = new OnlineCourse(1, "Test Course", "Test Description", 1, 1, "Zoom");
            course.EnrollStudent(101);

            // Act
            var result = course.EnrollStudent(102);

            // Assert
            Assert.False(result);
            Assert.Single(course.Students);
            Assert.Contains(101, course.Students);
            Assert.DoesNotContain(102, course.Students);
        }

        [Fact]
        public void EnrollStudent_ShouldNotAddDuplicateStudent()
        {
            // Arrange
            var course = new OnlineCourse(1, "Test Course", "Test Description", 1, 30, "Zoom");
            course.EnrollStudent(101);

            // Act
            var result = course.EnrollStudent(101);

            // Assert
            Assert.False(result);
            Assert.Single(course.Students);
        }

        [Fact]
        public void UnenrollStudent_ShouldRemoveStudent()
        {
            // Arrange
            var course = new OnlineCourse(1, "Test Course", "Test Description", 1, 30, "Zoom");
            course.EnrollStudent(101);

            // Act
            var result = course.UnenrollStudent(101);

            // Assert
            Assert.True(result);
            Assert.Empty(course.Students);
        }

        [Fact]
        public void UnenrollStudent_ShouldReturnFalseForNonExistentStudent()
        {
            // Arrange
            var course = new OnlineCourse(1, "Test Course", "Test Description", 1, 30, "Zoom");

            // Act
            var result = course.UnenrollStudent(101);

            // Assert
            Assert.False(result);
            Assert.Empty(course.Students);
        }

        [Fact]
        public void OnlineCourse_GetCourseDetails_ShouldReturnCorrectFormat()
        {
            // Arrange
            var course = new OnlineCourse(1, "Programming", "Learn programming", 1, 30, "Zoom");

            // Act
            var result = course.GetCourseDetails();

            // Assert
            Assert.Contains("Online Course: Programming", result);
            Assert.Contains("Description: Learn programming", result);
            Assert.Contains("Platform: Zoom", result);
            Assert.Contains("Max Students: 30", result);
        }

        [Fact]
        public void OfflineCourse_GetCourseDetails_ShouldReturnCorrectFormat()
        {
            // Arrange
            var course = new OfflineCourse(1, "Algorithms", "Learn algorithms", 1, 25, "1101", "Mon 10:00-11:30");

            // Act
            var result = course.GetCourseDetails();

            // Assert
            Assert.Contains("Offline Course: Algorithms", result);
            Assert.Contains("Description: Learn algorithms", result);
            Assert.Contains("Room: 1101", result);
            Assert.Contains("Schedule: Mon 10:00-11:30", result);
            Assert.Contains("Max Students: 25", result);
        }

        [Fact]
        public void OnlineCourse_GetLocationInfo_ShouldReturnPlatform()
        {
            // Arrange
            var course = new OnlineCourse(1, "Test", "Test", 1, 30, "Teams");

            // Act
            var result = course.GetLocationInfo();

            // Assert
            Assert.Equal("Online platform: Teams", result);
        }

        [Fact]
        public void OfflineCourse_GetLocationInfo_ShouldReturnRoomAndSchedule()
        {
            // Arrange
            var course = new OfflineCourse(1, "Test", "Test", 1, 25, "1205", "Tue, Thu 14:00-15:30");

            // Act
            var result = course.GetLocationInfo();

            // Assert
            Assert.Equal("Room: 1205, Schedule: Tue, Thu 14:00-15:30", result);
        }
    }

    public class CourseManagerTests
    {
        [Fact]
        public void AddCourse_ShouldAddCourseSuccessfully()
        {
            // Arrange
            var manager = new CourseManager();
            var course = new OnlineCourse(1, "Test Course", "Test Description", 1, 30, "Zoom");

            // Act
            var result = manager.AddCourse(course);

            // Assert
            Assert.True(result);
            Assert.Single(manager.GetAllCourses());
            Assert.Equal(course, manager.GetCourse(1));
        }

        [Fact]
        public void AddCourse_ShouldNotAddDuplicateCourse()
        {
            // Arrange
            var manager = new CourseManager();
            var course1 = new OnlineCourse(1, "Test Course 1", "Test Description", 1, 30, "Zoom");
            var course2 = new OnlineCourse(1, "Test Course 2", "Test Description", 2, 25, "Teams");

            // Act
            manager.AddCourse(course1);
            var result = manager.AddCourse(course2);

            // Assert
            Assert.False(result);
            Assert.Single(manager.GetAllCourses());
            Assert.Equal(course1, manager.GetCourse(1));
        }

        [Fact]
        public void RemoveCourse_ShouldRemoveCourseSuccessfully()
        {
            // Arrange
            var manager = new CourseManager();
            var course = new OnlineCourse(1, "Test Course", "Test Description", 1, 30, "Zoom");
            manager.AddCourse(course);

            // Act
            var result = manager.RemoveCourse(1);

            // Assert
            Assert.True(result);
            Assert.Empty(manager.GetAllCourses());
            Assert.Null(manager.GetCourse(1));
        }

        [Fact]
        public void RemoveCourse_ShouldReturnFalseForNonExistentCourse()
        {
            // Arrange
            var manager = new CourseManager();

            // Act
            var result = manager.RemoveCourse(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetCoursesByTeacher_ShouldReturnCorrectCourses()
        {
            // Arrange
            var manager = new CourseManager();
            var course1 = new OnlineCourse(1, "Course 1", "Description", 1, 30, "Zoom");
            var course2 = new OnlineCourse(2, "Course 2", "Description", 1, 25, "Teams");
            var course3 = new OnlineCourse(3, "Course 3", "Description", 2, 20, "Zoom");

            manager.AddCourse(course1);
            manager.AddCourse(course2);
            manager.AddCourse(course3);

            // Act
            var teacherCourses = manager.GetCoursesByTeacher(1);

            // Assert
            Assert.Equal(2, teacherCourses.Count());
            Assert.Contains(course1, teacherCourses);
            Assert.Contains(course2, teacherCourses);
            Assert.DoesNotContain(course3, teacherCourses);
        }

        [Fact]
        public void AddStudent_ShouldAddStudentSuccessfully()
        {
            // Arrange
            var manager = new CourseManager();
            var student = new Student(101, "Anna Smirnova", "asmirnova@student.itmo.ru", 2, "Computer Science");

            // Act
            var result = manager.AddStudent(student);

            // Assert
            Assert.True(result);
            Assert.Single(manager.GetAllStudents());
            Assert.Equal(student, manager.GetStudent(101));
        }

        [Fact]
        public void AddStudent_ShouldNotAddDuplicateStudent()
        {
            // Arrange
            var manager = new CourseManager();
            var student1 = new Student(101, "Anna Smirnova", "asmirnova@student.itmo.ru", 2, "Computer Science");
            var student2 = new Student(101, "Anna Petrov", "apetrov@student.itmo.ru", 3, "Software Engineering");

            // Act
            manager.AddStudent(student1);
            var result = manager.AddStudent(student2);

            // Assert
            Assert.False(result);
            Assert.Single(manager.GetAllStudents());
            Assert.Equal(student1, manager.GetStudent(101));
        }

        [Fact]
        public void EnrollStudentInCourse_ShouldEnrollStudentSuccessfully()
        {
            // Arrange
            var manager = new CourseManager();
            var course = new OnlineCourse(1, "Test Course", "Test Description", 1, 30, "Zoom");
            var student = new Student(101, "Anna Smirnova", "asmirnova@student.itmo.ru", 2, "Computer Science");
            
            manager.AddCourse(course);
            manager.AddStudent(student);

            // Act
            var result = manager.EnrollStudentInCourse(1, 101);

            // Assert
            Assert.True(result);
            Assert.Contains(101, course.Students);
        }

        [Fact]
        public void EnrollStudentInCourse_ShouldReturnFalseForNonExistentCourse()
        {
            // Arrange
            var manager = new CourseManager();
            var student = new Student(101, "Anna Smirnova", "asmirnova@student.itmo.ru", 2, "Computer Science");
            manager.AddStudent(student);

            // Act
            var result = manager.EnrollStudentInCourse(999, 101);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EnrollStudentInCourse_ShouldReturnFalseForNonExistentStudent()
        {
            // Arrange
            var manager = new CourseManager();
            var course = new OnlineCourse(1, "Test Course", "Test Description", 1, 30, "Zoom");
            manager.AddCourse(course);

            // Act
            var result = manager.EnrollStudentInCourse(1, 999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void StudentExists_ShouldReturnTrueForExistingStudent()
        {
            // Arrange
            var manager = new CourseManager();
            var student = new Student(101, "Anna Smirnova", "asmirnova@student.itmo.ru", 2, "Computer Science");
            manager.AddStudent(student);

            // Act
            var result = manager.StudentExists(101);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void StudentExists_ShouldReturnFalseForNonExistentStudent()
        {
            // Arrange
            var manager = new CourseManager();

            // Act
            var result = manager.StudentExists(999);

            // Assert
            Assert.False(result);
        }
    }
}
