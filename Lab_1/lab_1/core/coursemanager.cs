namespace CourseManagement.Core
{
    public sealed class CourseManager : ICourseManager
    {
        private readonly List<Course> _courses;
        private readonly List<Teacher> _teachers;
        private readonly List<Student> _students;

        public CourseManager(List<Course>? courses = null, List<Teacher>? teachers = null, List<Student>? students = null)
        {
            _courses = courses ?? new List<Course>();
            _teachers = teachers ?? new List<Teacher>();
            _students = students ?? new List<Student>();
        }

        public bool AddCourse(Course course)
        {
            if (course == null)
            {
                return false;
            }

            if (GetCourse(course.Id) != null)
            {
                return false;
            }

            _courses.Add(course);
            return true;
        }

        public bool RemoveCourse(int courseId)
        {
            var course = GetCourse(courseId);
            if (course == null)
            {
                return false;
            }

            _courses.Remove(course);
            return true;
        }

        public Course? GetCourse(int courseId)
        {
            for (int i = 0; i < _courses.Count; i++)
            {
                if (_courses[i].Id == courseId)
                {
                    return _courses[i];
                }
            }
            return null;
        }

        public List<Course> GetAllCourses()
        {
            List<Course> result = new List<Course>();
            for (int i = 0; i < _courses.Count; i++)
            {
                result.Add(_courses[i]);
            }
            return result;
        }

        public List<Course> GetCoursesByTeacher(int teacherId)
        {
            List<Course> result = new List<Course>();
            for (int i = 0; i < _courses.Count; i++)
            {
                if (_courses[i].TeacherId == teacherId)
                {
                    result.Add(_courses[i]);
                }
            }
            return result;
        }

        public bool EnrollStudentInCourse(int courseId, int studentId)
        {
            var course = GetCourse(courseId);
            if (course == null)
            {
                return false;
            }

            var student = GetStudent(studentId);
            if (student == null)
            {
                return false;
            }
                
            return course.EnrollStudent(studentId);
        }

        public bool UnenrollStudentFromCourse(int courseId, int studentId)
        {
            var course = GetCourse(courseId);
            if (course == null)
            {
                return false;
            }
            return course.UnenrollStudent(studentId);
        }

        public bool AddTeacher(Teacher teacher)
        {
            if (teacher == null)
            {
                return false;
            }

            for (int i = 0; i < _teachers.Count; i++)
            {
                if (_teachers[i].Id == teacher.Id)
                {
                    return false;
                }
            }

            _teachers.Add(teacher);
            return true;
        }

        public Teacher? GetTeacher(int teacherId)
        {
            for (int i = 0; i < _teachers.Count; i++)
            {
                if (_teachers[i].Id == teacherId)
                {
                    return _teachers[i];
                }
            }
            return null;
        }

        public List<Teacher> GetAllTeachers()
        {
            List<Teacher> result = new List<Teacher>();
            for (int i = 0; i < _teachers.Count; i++)
            {
                result.Add(_teachers[i]);
            }
            return result;
        }

        public bool AddStudent(Student student)
        {
            if (student == null)
            {
                return false;
            }

            for (int i = 0; i < _students.Count; i++)
            {
                if (_students[i].Id == student.Id)
                {
                    return false;
                }
            }

            _students.Add(student);
            return true;
        }

        public Student? GetStudent(int studentId)
        {
            for (int i = 0; i < _students.Count; i++)
            {
                if (_students[i].Id == studentId)
                {
                    return _students[i];
                }
            }
            return null;
        }

        public List<Student> GetAllStudents()
        {
            List<Student> result = new List<Student>();
            for (int i = 0; i < _students.Count; i++)
            {
                result.Add(_students[i]);
            }
            return result;
        }

        public bool StudentExists(int studentId)
        {
            return GetStudent(studentId) != null;
        }
    }
}
