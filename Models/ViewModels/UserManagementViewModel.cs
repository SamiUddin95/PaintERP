namespace PaintERP.Models.ViewModels;

public class UserManagementViewModel
{
    public List<UserStatCard> Stats { get; set; } = new();
    public List<UserDirectoryRow> Users { get; set; } = new();
    public List<string> Plants { get; set; } = new();
    public List<string> Regions { get; set; } = new();
    public List<string> AccessPolicies { get; set; } = new();
    public DateTime LastSyncUtc { get; set; }
    public string PrimaryPlant { get; set; } = string.Empty;

    public static UserManagementViewModel BuildDemo()
    {
        var demoUsers = new List<UserDirectoryRow>
        {
            new("System Administrator", "ops@usapainterp.com", "Admin", "Executive", "Unicorp Bissonet", "USA", "Swing", "Tier 4", "Active", true, new DateTime(2026, 7, 27, 18, 51, 22, DateTimeKind.Utc))
        };

        return new UserManagementViewModel
        {
            PrimaryPlant = "Atlanta Distribution",
            LastSyncUtc = DateTime.UtcNow,
            Stats =
            [
                new("Active Directory Users", "1", "0 suspended", "#2563eb"),
                new("Tier 4 / Admin", "1", "1 total users", "#16a34a"),
                new("Logins (24h)", "1", "0 pending review", "#f97316"),
                new("MFA Verified", "1", "0 pending", "#ec4899"),
            ],
            Users = demoUsers,
            Plants = ["Atlanta Distribution", "Dallas Main"],
            Regions = ["USA"],
            AccessPolicies =
            [
                "Tier 4 - Executive & Compliance",
                "Tier 3 - Production + R&D",
                "Tier 2 - Regional & QA",
                "Tier 1 - Contractors / Maintenance"
            ]
        };
    }
}

public record UserStatCard(string Title, string Value, string Subtitle, string AccentHex);

public record UserDirectoryRow(
    string Name,
    string Email,
    string Role,
    string Department,
    string Plant,
    string Region,
    string Shift,
    string AccessTier,
    string Status,
    bool MfaEnabled,
    DateTime LastActiveUtc);
