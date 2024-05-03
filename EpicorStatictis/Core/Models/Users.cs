


namespace Core.Models
{
    public  class Users
    {
        public string Name { get; set; }
        public int Total { get; set; }

        public string Status { get; set; }

        public Users(UsersBuilder builder)
        {
            Name = builder.Name;
            Total = builder.Total;
            Status = builder.Status;
        }


        public class UsersBuilder 
        {
            public string Name { get; set; }
            public int Total { get; set; }

            public string Status { get; set; }

            public UsersBuilder WithName(string name)
            {
                Name = name;
                return this;
            }

            public UsersBuilder WithTotal(int total)
            {
                Total = total;
                return this;
            }

            public UsersBuilder WithStatus(string status)
            {
                Status= status;
                return this;
            }

            public Users Build()
            {
                return new Users(this);
            }
        }
    }
}
