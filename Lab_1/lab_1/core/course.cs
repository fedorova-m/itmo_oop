namespace CourseManagement.Core
{
    public abstract class Course
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int TeacherId { get; }
        public int MaxStudents { get; }
        public List<int> Students { get; }

        protected Course(int id, string name, string description, int teacherId, int maxStudents)
        {
            Id = id;
            Name = name;
            Description = description;
            TeacherId = teacherId;
            MaxStudents = maxStudents;
            Students = new List<int>();
        }
        public bool CanEnrollStudent(int studentId)
        {
            return !Students.Contains(studentId) && Students.Count < MaxStudents;
        }
        public bool EnrollStudent(int studentId)
        {
            if (!CanEnrollStudent(studentId))
                return false;
            
            Students.Add(studentId);
            return true;
        }
        public bool UnenrollStudent(int studentId)
        {
            return Students.Remove(studentId);
        }
        public abstract string GetCourseDetails();
        public abstract string GetLocationInfo();
    }
}
