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
            
            long s = 0;
            writer.WriteLine("Total Regions: " + dict.Count);
            foreach (var value in dict.OrderByDescending(p => p.Value))
            {
                s += long.Parse(value.Value.ToString());
                writer.WriteLine($"Region {value.Key} → {value.Value} pixels");
            }
            writer.WriteLine(s);

        }
    }
}
