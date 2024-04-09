namespace checkers_api.Services;

public class CodeGenerator : ICodeGenerator
{
    private readonly string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public string GenerateCode()
    {
        var stringChars = new char[5];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = _chars[random.Next(_chars.Length)];
        }

        return new string(stringChars);
    }
}