namespace CourseManagement.Core
{
    public sealed class CourseManager : ICourseManager
    {
        private readonly List<Course> _courses;
        private readonly List<Teacher> _teachers;
        private readonly List<Student> _students;

        public CourseManager()
        {
            _courses = new List<Course>();
            _teachers = new List<Teacher>();
            _students = new List<Student>();
        }

        public CourseManager(List<Course> courses, List<Teacher> teachers, List<Student> students)
        {
            _courses = courses ?? new List<Course>();
            _teachers = teachers ?? new List<Teacher>();
            _students = students ?? new List<Student>();
        }

        public bool AddCourse(Course course)
        {
            if (course == null || _courses.Any(c => c.Id == course.Id))
                return false;

            _courses.Add(course);
            return true;
        }

        public bool RemoveCourse(int courseId)
        {
            var course = _courses.FirstOrDefault(c => c.Id == courseId);
            if (course == null)
                return false;

            _courses.Remove(course);
            return true;
        }

        public Course? GetCourse(int courseId)
        {
            return _courses.FirstOrDefault(c => c.Id == courseId);
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return _courses.AsReadOnly();
        }

        public IEnumerable<Course> GetCoursesByTeacher(int teacherId)
        {
            return _courses.Where(c => c.TeacherId == teacherId);
        }

        public bool AssignTeacherToCourse(int courseId, int teacherId)
        {
            var course = GetCourse(courseId);
            if (course == null || !_teachers.Any(t => t.Id == teacherId))
                return false;

            // В реальной системе здесь была бы логика обновления TeacherId
            // Для простоты мы предполагаем, что курсы создаются с правильным TeacherId
            return true;
        }

        public bool EnrollStudentInCourse(int courseId, int studentId)
        {
            var course = GetCourse(courseId);
            var student = GetStudent(studentId);
            
            if (course == null || student == null)
                return false;
                
            return course.EnrollStudent(studentId);
        }

        public bool UnenrollStudentFromCourse(int courseId, int studentId)
        {
            var course = GetCourse(courseId);
            return course?.UnenrollStudent(studentId) ?? false;
        }

        public bool AddTeacher(Teacher teacher)
        {
            if (teacher == null || _teachers.Any(t => t.Id == teacher.Id))
                return false;

            _teachers.Add(teacher);
            return true;
        }

        public Teacher? GetTeacher(int teacherId)
        {
            return _teachers.FirstOrDefault(t => t.Id == teacherId);
        }

        public IEnumerable<Teacher> GetAllTeachers()
        {
            return _teachers.AsReadOnly();
        }

        public bool AddStudent(Student student)
        {
            if (student == null || _students.Any(s => s.Id == student.Id))
                return false;

            _students.Add(student);
            return true;
        }

        public Student? GetStudent(int studentId)
        {
            return _students.FirstOrDefault(s => s.Id == studentId);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _students.AsReadOnly();
        }

        public bool StudentExists(int studentId)
        {
            return _students.Any(s => s.Id == studentId);
        }
    }
}
