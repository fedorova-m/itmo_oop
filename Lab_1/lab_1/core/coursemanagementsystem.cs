namespace CourseManagement.Core
{
    // Singleton Pattern для системы управления курсами
    public sealed class CourseManagementSystem
    {
        private static CourseManagementSystem? _instance;
        private static readonly object _lock = new object();

        private CourseManagementSystem()
        {
            CourseManager = new CourseManager();
            CourseFactory = new CourseFactory();
        }

        public static CourseManagementSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new CourseManagementSystem();
                    }
                }
                return _instance;
            }
        }

        public ICourseManager CourseManager { get; }
        public ICourseFactory CourseFactory { get; }

        public void Reset()
        {
            lock (_lock)
            {
                _instance = null;
            }
        }
    }
}
