using eMoviesFramework.Models;

namespace eMoviesFramework.Repositories
{
    public interface IMovieRepository
    {
        Movie[] LoadMovies();

        int NewCustomer(TicketsModel ticketsmodel);

        void LookupMovieDetails(TicketsModel ticketsModel);

        void SaveCustomerDetails(CustomerDetails customerDetails);

        CustomerDetails GetCustomerInfo(int customerId);

        TicketsModel GetOrderInfo(int customerId, TicketsModel ticketsModel);
    }
}