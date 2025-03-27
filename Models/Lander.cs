namespace AdTechAPI.Models
{
    public class Lander
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }

        public required int ClientId { get; set; }

        public Client Client { get; set; }

    }
}