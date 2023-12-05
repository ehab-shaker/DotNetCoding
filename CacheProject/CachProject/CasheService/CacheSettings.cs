namespace CachProject.CashService
{
    public class CacheSettings
    {
        public bool Enabled { get; set; } = true;
        public double AbsoluteExpiration { get; set; } = 600;
        public double SlidingExpiration { get; set; } = 300;
    }
}
