namespace checkers_api.Services;

public static class IdGenerator
{
    private static readonly string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
    private static readonly Random random = new();
    public static string GetId()
    {
        return new string(Enumerable.Repeat(characters, 12).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}