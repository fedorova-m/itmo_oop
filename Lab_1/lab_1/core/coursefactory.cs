namespace CourseManagement.Core
{
    public sealed class CourseFactory : ICourseFactory
    {
        public Course CreateOnlineCourse(int id, string name, string description, int teacherId, int maxStudents, string platform)
        {
            return new OnlineCourse(id, name, description, teacherId, maxStudents, platform);
        }

        public Course CreateOfflineCourse(int id, string name, string description, int teacherId, int maxStudents, string room, string schedule)
        {
            return new OfflineCourse(id, name, description, teacherId, maxStudents, room, schedule);
        }
    }

    // Builder Pattern для создания курсов
    public sealed class CourseBuilder
    {
        private int _id;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private int _teacherId;
        private int _maxStudents;
        private string _platform = string.Empty;
        private string _room = string.Empty;
        private string _schedule = string.Empty;
        private CourseType _type;

        public CourseBuilder SetId(int id)
        {
            _id = id;
            return this;
        }

        public CourseBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public CourseBuilder SetDescription(string description)
        {
            _description = description;
            return this;
        }

        public CourseBuilder SetTeacherId(int teacherId)
        {
            _teacherId = teacherId;
            return this;
        }

        public CourseBuilder SetMaxStudents(int maxStudents)
        {
            _maxStudents = maxStudents;
            return this;
        }

        public CourseBuilder SetType(CourseType type)
        {
            _type = type;
            return this;
        }

        public CourseBuilder SetPlatform(string platform)
        {
            _platform = platform;
            return this;
        }

        public CourseBuilder SetRoom(string room)
        {
            _room = room;
            return this;
        }

        public CourseBuilder SetSchedule(string schedule)
        {
            _schedule = schedule;
            return this;
        }

        public Course Build()
        {
            return _type switch
            {
                CourseType.Online => new OnlineCourse(_id, _name, _description, _teacherId, _maxStudents, _platform),
                CourseType.Offline => new OfflineCourse(_id, _name, _description, _teacherId, _maxStudents, _room, _schedule),
                _ => throw new ArgumentException("Invalid course type")
            };
        }
    }

    public enum CourseType
    {
        Online,
        Offline
    }
}
