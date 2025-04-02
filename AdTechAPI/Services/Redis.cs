using StackExchange.Redis;

namespace AdTechAPI.Services
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _redis;
        public IDatabase Db
        {
            get;
        }

        public RedisService(string connectionString = "localhost")
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            Db = _redis.GetDatabase();
        }
    }
}