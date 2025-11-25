namespace CourseManagement.Core
{
    public interface ICourseManager
    {
        bool AddCourse(Course course);
        bool RemoveCourse(int courseId);
        Course? GetCourse(int courseId);
        List<Course> GetAllCourses();
        List<Course> GetCoursesByTeacher(int teacherId);
        bool EnrollStudentInCourse(int courseId, int studentId);
        bool UnenrollStudentFromCourse(int courseId, int studentId);
    }
}
