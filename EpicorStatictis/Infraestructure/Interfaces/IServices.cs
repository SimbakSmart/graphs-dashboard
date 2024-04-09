
using Infraestructure.Helpers;

namespace Infraestructure.Interfaces
{
    public  interface IServices<T> where T : class
    {
        Task<List<T>> GetTotalsAsync(FiltersParams filters = null);
    }
}
