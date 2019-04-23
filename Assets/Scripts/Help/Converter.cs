using System;

public class Converter  {

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
