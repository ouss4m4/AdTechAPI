using System.Text.Json.Serialization;

namespace AdTechAPI.Models
{
    public class Vertical
    {
        public int Id
        {
            get; set;
        }
        public required string Name
        {
            get; set;
        }
        public string? Description
        {
            get; set;
        }
        public DateTime CreatedAt
        {
            get; set;
        }
        public DateTime UpdatedAt
        {
            get; set;
        }
        [JsonIgnore]
        public ICollection<Campaign> Campaigns { get; set; } = [];
    }
}

/*
INSERT INTO public."Verticals" ("Name", "Description", "CreatedAt", "UpdatedAt")  
VALUES  
    ('Finance', 'Financial services and investments', NOW(), NOW()),  
    ('Health', 'Healthcare and medical services', NOW(), NOW()),  
    ('Technology', 'Software, hardware, and IT solutions', NOW(), NOW()),  
    ('E-commerce', 'Online stores and marketplaces', NOW(), NOW()),  
    ('Education', 'E-learning and educational resources', NOW(), NOW()),  
    ('Travel', 'Tourism, flights, and accommodations', NOW(), NOW());  
*/