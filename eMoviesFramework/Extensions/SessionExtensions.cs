using Newtonsoft.Json;
using System.Web;


namespace eMovies.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject(this HttpContext session, string key, object value)
        {
            session.SetObject(key, value);
        }

        public static T GetObject<T>(this HttpContext session, string key)
        {
            var value = session.GetObject<T>(key);
            return value;
        }
    }
}
