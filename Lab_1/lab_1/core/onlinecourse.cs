namespace CourseManagement.Core
{
    public sealed class OnlineCourse : Course
    {
        public string Platform { get; }

        public OnlineCourse(int id, string name, string description, int teacherId, int maxStudents, string platform)
            : base(id, name, description, teacherId, maxStudents)
        {
            Platform = platform;
        }

        public override string GetCourseDetails()
        {
            return $"Online Course: {Name}\nDescription: {Description}\nPlatform: {Platform}\nMax Students: {MaxStudents}\nCurrent Students: {Students.Count}";
        }

        public override string GetLocationInfo()
        {
            return $"Online platform: {Platform}";
        }
    }
}
