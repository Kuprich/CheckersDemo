namespace CheckersDemo.Shared;

public static class HubMethodNames
{
    private const string _invokedPrefix = "_On";

    public const string JoinTable = "JoinTable";
    public const string CreateTable = "CreateTable";
    public const string Move = "Move";

    public static string On(this string value)
    {
        return value + _invokedPrefix;
    }
}


