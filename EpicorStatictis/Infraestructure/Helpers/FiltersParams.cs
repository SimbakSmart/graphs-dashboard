

namespace Infraestructure.Helpers
{
    public class FiltersParams
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Department { get; set; }

        public FiltersParams(FiltersParamsBuilder builder)
        {
            StartDate = builder.StartDate;
            EndDate = builder.EndDate;
            Department = builder.Department;
        }


        public class FiltersParamsBuilder
        {
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public string Department { get; set; }

            public FiltersParamsBuilder WithStartDate(DateTime starDate)
            {
                StartDate = starDate;
                return this;
            }

            public FiltersParamsBuilder WithEndDate(DateTime endDate)
            {
                EndDate = endDate;
                return this;
            }

            public FiltersParamsBuilder WithDeparment(string department)
            {
                Department = department;
                return this;
            }

            public FiltersParams Build()
            {
                return new FiltersParams(this);
            }
        }
    }
}
