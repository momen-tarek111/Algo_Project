using System;
using System.Collections.Generic;
using System.IO;

public class DictionaryWriter
{
    public static void WriteValuesToFile<TKey, TValue>(Dictionary<TKey, TValue> dict, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(dict.Count);
            foreach (var value in dict.Values)
            {
                writer.WriteLine(value);
            }
        }
    }
}
