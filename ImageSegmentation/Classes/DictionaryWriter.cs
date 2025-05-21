using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DictionaryWriter
{
    public static void WriteValuesToFile<TKey, TValue>(Dictionary<TKey, TValue> dict, string filePath,string timeFilePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(dict.Count);
            foreach (var value in dict.OrderByDescending(p => p.Value))
            {
                writer.WriteLine(value.Value);
            }
        }
        using (StreamWriter writer = new StreamWriter(timeFilePath))
        {
            writer.WriteLine(" Construnct Graph time -> " + data.time3);
            writer.WriteLine(" sort time -> " + data.time2);
            writer.WriteLine(" Total Time -> "+data.time);

        }
    }

}
