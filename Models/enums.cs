namespace AdTechAPI.Enums
{


    public enum ClientType
    {
        Publisher = 1,
        Advertiser = 2,
        Owner = 3
    }

    public enum UserRole
    {
        Admin = 1,
        Moderator = 2,
        Regular = 3
    }

    public enum Platform
    {
        Mobile = 1,
        Tablet = 2,
        Desktop = 3
    }

    public enum CampaignStatus
    {
        Active = 1,
        Paused = 2,
        Capped = 3,
        Inactive = 4,
        Draft = 5,
    }
}