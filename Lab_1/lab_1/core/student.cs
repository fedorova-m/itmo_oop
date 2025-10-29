namespace CourseManagement.Core
{
    public sealed class Student
    {
        public int Id { get; }
        public string Name { get; }
        public string Email { get; }
        public int Year { get; }
        public string Major { get; }

        public Student(int id, string name, string email, int year, string major)
        {
            Id = id;
            Name = name;
            Email = email;
            Year = year;
            Major = major;
        }

        public override string ToString()
        {
            return $"{Name} (Year {Year}, {Major})";
        }
    }
}
