using System;

public class Converter  {
    /// <summary>
    /// input an string and return an Enum value same as the string
    /// C#7.0 feature will caseu error in console, but actually it works
    /// </summary>
    /// <typeparam name="e">Enum</typeparam>
    /// <typeparam name="i">input type</typeparam>
    /// <param name="input">input string</param>
    /// <returns>return an Enum value of e</returns>
    public static e StringEnumConverter<e, i>(i input) where e : struct,IConvertible
    {
        foreach (var num in Enum.GetValues(typeof(e)))
        {
            if (input.ToString() == ((e)num).ToString())
            {
                return (e)num;
            }
        }
        return default(e);
    }
}
