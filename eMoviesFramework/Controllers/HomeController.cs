using System.Linq;
using eMoviesFramework.Models;
using System.Web.Mvc;
using eMoviesFramework.Services;
using eMoviesFramework.Repositories;

namespace eMoviesFramework.Controllers
{
    public class HomeController : Controller
    {
        private const string customerSessionKey = "customerdetails";
        private const string TicketsSessionKey = "ticketsModel";
        private const string CustomerIDSessionKey = "customerId";
        //private readonly SessionService _sessionService;
        //private readonly DatabaseMovieRepository _movieRepository;

        private readonly DatabaseMovieRepository _movieRepository = new DatabaseMovieRepository();
        private readonly SessionService _sessionService = new SessionService();

        //public HomeController(ISessionService sessionService, IMovieRepository movieRepository)
        //{
        //    _sessionService = sessionService;
        //    _movieRepository = movieRepository;
        //}

        [HttpGet]
        public ActionResult Index()
        {
            var ticketsModel = new TicketsModel
            {
                Movies = _movieRepository.LoadMovies()
            };
            return View("Index", ticketsModel);
        }

        //[HttpGet]
        //public ActionResult SessionTest()
        //{
        //    _sessionService.SetString("key", "value");

        //    _sessionService.SetObject("key", new[] { "a", "b" });

        //    return View();
        //}

        [HttpPost]
        public  ActionResult Index(TicketsModel ticketsModel, string submitbutton)
        {
            _movieRepository.LookupMovieDetails(ticketsModel);
            _sessionService.SetObject(TicketsSessionKey, ticketsModel);

            ticketsModel.TotalQuantity = _movieRepository.CalculateTotalQuantity(ticketsModel);

            if (ticketsModel.TotalQuantity > 0)
            {
                if (ModelState.IsValid)
                {
                    if (submitbutton.Equals("Update"))
                    {
                        _movieRepository.CalculateNewTotal(ticketsModel);
                        return View("Index", ticketsModel);
                    }
                    else if (submitbutton.Equals("Order"))
                    {
                        int customerId = _movieRepository.NewCustomer(ticketsModel);

                        _sessionService.SetObject(CustomerIDSessionKey, customerId);

                        return Redirect("/home/order");
                    }
                }
            }
            return View("Index", ticketsModel);
        }

        [HttpGet]
        public ActionResult Order()
        {
            var ticketsmodel = _sessionService.GetObject<TicketsModel>(TicketsSessionKey);
            if (ticketsmodel == null)
            {
                return Redirect("/home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Order(CustomerDetails customerDetails)
        {
            if (ModelState.IsValid)
            {
                customerDetails.CustomerID = _sessionService.GetObject<int>(CustomerIDSessionKey);

                _movieRepository.SaveCustomerDetails(customerDetails);

                _sessionService.SetObject(customerSessionKey, customerDetails);

                return Redirect("/home/orderconf");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult OrderConf()
        {
            var customerdetailscheck = _sessionService.GetObject<CustomerDetails>(customerSessionKey);

            if ((customerdetailscheck) == null)
            {
                return Redirect("/home");
            };

            var customerdetails = _movieRepository.GetCustomerInfo(_sessionService.GetObject<int>(CustomerIDSessionKey));
            var ticketsmodel = _movieRepository.GetOrderInfo(_sessionService.GetObject<int>(CustomerIDSessionKey), _sessionService.GetObject<TicketsModel>(TicketsSessionKey));

            SummaryModel summarymodel = new SummaryModel
            {
                Customer = customerdetails,
                Tickets = ticketsmodel
            };

            return View(summarymodel);
        }

        [HttpGet]
        public ActionResult MovieDescription(int id)
        {
            if (id == 0)
            {
                return Redirect("/home");
            }

            var allMovies = _movieRepository.LoadMovies();

            var matchingMovie = allMovies.FirstOrDefault(a => a.Id == id);

            return View(matchingMovie);
        }
    }
}