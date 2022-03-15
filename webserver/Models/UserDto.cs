namespace webserver.Models
{
    public class UserDto
    {
        public int Id { get; set; } = 0;
        public string? Email { get; set; } = string.Empty;
        public string? Username { get; set; } = string.Empty;
        public string? SteamID { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        public int Role { get; set; } = 0;
        public int MailConfirmed { get; set; } = 0;
    }
}
