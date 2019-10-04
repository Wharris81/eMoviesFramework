using eMoviesFramework.Models;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Configuration;

namespace eMoviesFramework.Repositories
{
    public class DatabaseMovieRepository : IMovieRepository
    {
        public static string GetConnectionString()
        {
            return ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
        }

        private static string ConnectionString = GetConnectionString();

        public Movie[] LoadMovies()
        {
            List<Movie> movies = new List<Movie>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand GetMovies = new SqlCommand("GetMovies", connection))
                {
                    using (SqlDataReader reader = GetMovies.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var movie = new Movie
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDouble(2),
                                Description = reader.GetString(3),
                                ImagePath = reader.GetString(4),
                                Quantity = 0
                            };

                            movies.Add(movie);
                        }
                    }
                }
            }

            return movies.ToArray();
        }



        public void SaveCustomerOrder(TicketsModel ticketsModel, int customerId)
        {
            foreach (var movie in ticketsModel.Movies)
            {
                if (movie.Quantity > 0)
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        connection.Open();

                        using (SqlCommand saveCustomerOrder = new SqlCommand("SaveCustomerOrder", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        })
                        {
                            saveCustomerOrder.Parameters.AddWithValue("@CustomerID", customerId);
                            saveCustomerOrder.Parameters.AddWithValue("@MovieID", movie.Id);
                            saveCustomerOrder.Parameters.AddWithValue("@Quantity", movie.Quantity);
                            saveCustomerOrder.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
        public void SaveCustomerDetails(CustomerDetails customerDetails)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand saveCustomerDetails = new SqlCommand("SaveCustomerDetails", connection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    saveCustomerDetails.Parameters.AddWithValue("@CustomerID", customerDetails.CustomerID);
                    saveCustomerDetails.Parameters.AddWithValue("@Name", customerDetails.Name);
                    saveCustomerDetails.Parameters.AddWithValue("@Email", customerDetails.Email);
                    saveCustomerDetails.Parameters.AddWithValue("@CardNumber", customerDetails.CardNumberObfuscated);
                    saveCustomerDetails.Parameters.AddWithValue("@CardType", customerDetails.CardType);
                    saveCustomerDetails.Parameters.AddWithValue("@EmailPreference", customerDetails.EmailPreference);
                    saveCustomerDetails.ExecuteNonQuery();
                }
            }
        }

        public CustomerDetails GetCustomerInfo(int customerId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand getCustomerInfo = new SqlCommand("getCustomerInfo", connection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    getCustomerInfo.Parameters.AddWithValue("@CustomerID", customerId);
                    using (SqlDataReader reader = getCustomerInfo.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerDetails customerDetails = new CustomerDetails
                            {
                                Name = reader.GetString(0),
                                Email = reader.GetString(1),
                                CardNumber = reader.GetString(2),
                                CardType = reader.GetString(3),
                                EmailPreference = reader.GetBoolean(4)
                            };
                            return (customerDetails);
                        }
                    }
                }

            }
            return null;
        }

        public TicketsModel GetOrderInfo(int customerId, TicketsModel ticketsModel)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand getOrderInfo = new SqlCommand("getOrderInfo", connection)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    getOrderInfo.Parameters.AddWithValue("@CustomerID", customerId);
                    using (SqlDataReader reader = getOrderInfo.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int movieId = reader.GetInt32(0);

                            if (ticketsModel.Movies.Any(a => a.Id == movieId))
                            {
                                ticketsModel.Movies.FirstOrDefault(a => a.Id == movieId).Quantity = reader.GetInt32(1);
                            }
                        }
                    };
                    return (ticketsModel);
                }
            }
        }

        public void LookupMovieDetails(TicketsModel ticketsModel)
        {
            var allMovies = new DatabaseMovieRepository().LoadMovies();

            foreach (var movie in ticketsModel.Movies)
            {
                var matchingMovie = allMovies.FirstOrDefault(i => i.Id == movie.Id);

                if (matchingMovie != null)
                {
                    movie.Name = matchingMovie.Name;

                    movie.Price = matchingMovie.Price;
                }
            }
        }

        public int NewCustomer(TicketsModel ticketsmodel)
        {
            int customerId = -1;


            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (SqlCommand NewCustomer = new SqlCommand("NewCustomer", connection))
                {
                    customerId = Convert.ToInt32(NewCustomer.ExecuteScalar());
                }
            }
            SaveCustomerOrder(ticketsmodel, customerId);

            return customerId;
        }

        public void CalculateTotalQuantity(TicketsModel ticketsModel)
        {
            foreach (var movie in ticketsModel.Movies)
            {
                ticketsModel.TotalQuantity = ticketsModel.TotalQuantity + movie.Quantity;
            }
        }

        public double CalculateNewTotal(TicketsModel ticketsModel)
        {
            foreach (Movie movie in ticketsModel.Movies)
            {
                ticketsModel.NewTotal = ticketsModel.NewTotal + (movie.Quantity * movie.Price);
            }
            return ticketsModel.NewTotal;
        }


    }
}






