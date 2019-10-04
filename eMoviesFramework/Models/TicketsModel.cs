using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using eMoviesFramework.Repositories;

namespace eMoviesFramework.Models
{
    public class TicketsModel
    {
        //public TicketsModel()
        //{
        //    Movies = new Movie[0];
        //}

        public double NewTotal { get; set; }


        //public double NewTotal => Movies.Sum(a => a.Price * a.Quantity);

        public string CurrencyTotal => NewTotal.ToString("c", CultureInfo.GetCultureInfo("en-gb"));

        public Movie[] Movies { get; set; }

        //[Range(1, 1000, ErrorMessage = "Please enter a number between 1 and 1000")]
        public int TotalQuantity { get; set; }
    }
}
