﻿namespace SV20T1080031.Web.Models
{
    public class Student
    {
        public string StudentId { get; set; }
        public string StudentName { get; set;}
    }

    public class StudentDAL
    {
        public List<Student> List()
        {
            List<Student> students = new List<Student>();
            students.Add(new Student()
            {
                StudentId = "20T1080031",
                StudentName = "Nahoshi",
            });
            students.Add(new Student()
            {
                StudentId = "20T1080032",
                StudentName = "Ateo",
            });
            return students;
        }
    }
}
