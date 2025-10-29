namespace CourseManagement.Core
{
    public interface ICourseManager
    {
        bool AddCourse(Course course);
        bool RemoveCourse(int courseId);
        Course? GetCourse(int courseId);
        IEnumerable<Course> GetAllCourses();
        IEnumerable<Course> GetCoursesByTeacher(int teacherId);
        bool AssignTeacherToCourse(int courseId, int teacherId);
        bool EnrollStudentInCourse(int courseId, int studentId);
        bool UnenrollStudentFromCourse(int courseId, int studentId);
    }

    public interface ICourseFactory
    {
        Course CreateOnlineCourse(int id, string name, string description, int teacherId, int maxStudents, string platform);
        Course CreateOfflineCourse(int id, string name, string description, int teacherId, int maxStudents, string room, string schedule);
    }
}
