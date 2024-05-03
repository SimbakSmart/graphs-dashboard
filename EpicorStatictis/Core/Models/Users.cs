


namespace Core.Models
{
    public  class Users
    {
        public string Name { get; set; }
        public int Total { get; set; }


        public Users(UsersBuilder builder)
        {
            Name = builder.Name;
            Total = builder.Total;
        }


        public class UsersBuilder 
        {
            public string Name { get; set; }
            public int Total { get; set; }

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

            public Users Build()
            {
                return new Users(this);
            }
        }
    }
}
