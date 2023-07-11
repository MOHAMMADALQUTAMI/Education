namespace ManyToMany.Model
{
    public class Student
    {
        public int Id {get;set;}
        public string Name {get;set;} 
        public int? AdressId {get;set;}
        public Address? Address {get;set;}
        public List<Course> Courses {get;set;} =new();
    }
}