namespace eMoviesFramework.Services
{
    public interface ISessionService
    {
        T GetObject<T>(string key);
        string GetString(string key);
        void SetObject(string key, object value);
        void SetString(string key, string value);
    }
}