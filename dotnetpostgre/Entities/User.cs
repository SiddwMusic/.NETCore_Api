using System;

namespace dotnetpostgre.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }

    public class blog
    {
        public int id { get; set; }
        public string title { get; set; }
        public string blogdetails { get; set; }
        public string author { get; set; }
        public string description { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
    }
}