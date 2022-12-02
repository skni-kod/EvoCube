using System;

public static class Guard
{
    public static void AgainstNull(object argument, string argumentName)
    {
        if (argument == null)
        {
            throw new ArgumentNullException(argumentName);
        }
    }
    public static void AgainstFalse(bool argument, string argumentName)
    {
        if (argument == false)
        {
            throw new ArgumentNullException(argumentName);
        }
    }

}