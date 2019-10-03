using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eMoviesFramework.Models
{
    public class SummaryModel
    {
        public CustomerDetails Customer { get; set; }
        public TicketsModel Tickets { get; set; }
    }
}
