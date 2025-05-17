using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DictionaryWriter
{
    public static void WriteValuesToFile<TKey, TValue>(Dictionary<TKey, TValue> dict, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {

            writer.WriteLine(data.time);
            writer.WriteLine(dict.Count);
            foreach (var value in dict.OrderByDescending(p => p.Value))
            {
                writer.WriteLine(value.Value);
            }
        }
    }

}
