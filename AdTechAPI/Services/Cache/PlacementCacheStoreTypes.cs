namespace AdTechAPI.PlacementCache
{


    public static class PlacementCacheKeys
    {
        public const string Pool = "cache::placements_pool";
    }
    public class PlacementCacheData
    {
        public required string PlacementUuid
        {
            get; set;

        }

        public required int PlacementId
        {
            get; set;
        }

        public required int TrafficSourceId
        {
            get; set;
        }

        public required int PublisherId
        {
            get; set;
        }

        public required int[] Verticals
        {
            get; set;
        }
    }
}
