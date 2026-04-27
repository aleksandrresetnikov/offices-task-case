namespace Offices.Models.Settings;

public class DbConnectionSettings
{
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string Database { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }

    // Сразу под npgsq
    public string ConnectionString => 
        $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};";
}