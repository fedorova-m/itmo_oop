namespace CourseManagement.Core
{
    public sealed class Teacher
    {
        public int Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Department { get; }
        public string Specialization { get; }

        public Teacher(int id, string name, string email, string department, string specialization)
        {
            Id = id;
            Name = name;
            Email = email;
            Department = department;
            Specialization = specialization;
        }

        public override string ToString()
        {
            return $"{Name} ({Department}) - {Specialization}";
        }
    }
}
