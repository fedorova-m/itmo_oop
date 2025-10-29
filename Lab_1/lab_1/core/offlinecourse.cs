namespace CourseManagement.Core
{
    public sealed class OfflineCourse : Course
    {
        public string Room { get; }
        public string Schedule { get; }

        public OfflineCourse(int id, string name, string description, int teacherId, int maxStudents, string room, string schedule)
            : base(id, name, description, teacherId, maxStudents)
        {
            Room = room;
            Schedule = schedule;
        }

        public override string GetCourseDetails()
        {
            return $"Offline Course: {Name}\nDescription: {Description}\nRoom: {Room}\nSchedule: {Schedule}\nMax Students: {MaxStudents}\nCurrent Students: {Students.Count}";
        }

        public override string GetLocationInfo()
        {
            return $"Room: {Room}, Schedule: {Schedule}";
        }
    }
}
