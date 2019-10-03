using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace eMoviesFramework.Models
{
    public class Movie
    {
        public string Name { get; set; }

        public double Price { get; set; }

        [Range(0, 1000, ErrorMessage = "Please enter a number between 0 and 1000")]
        public int Quantity { get; set; }

        public int Id { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }
    }
}

