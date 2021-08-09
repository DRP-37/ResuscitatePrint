namespace Resuscitate.DataClasses
{
    public class StaffMemberData
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Grade { get; set; }
        public string TimeOfArrival { get; set; }

        public StaffMemberData(string Name, string Position, string Grade, string TimeOfArrival)
        {
            this.Name = Name;
            this.Position = Position;
            this.Grade = Grade;
            this.TimeOfArrival = TimeOfArrival;
        }

        override public string ToString()
        {
            return $"Arrival Time: {TimeOfArrival}\tName: {Name}, Position: {Position}, Grade: {Grade}";
        }
    }
}
