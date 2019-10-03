using System.Web;

namespace eMoviesFramework.Services
{
    public class SessionService : ISessionService
    {
   
        public virtual void SetString(string key, string value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public virtual string GetString(string key)
        {
            return HttpContext.Current.Session[key]?.ToString();
        }

        public virtual void SetObject(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public virtual T GetObject<T>(string key)
        {
            return (T)(object)HttpContext.Current.Session[key];
        }
    }
}
