using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DictionaryWriter
{
    public static void WriteValuesToFile<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection,string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(collection.Count());
            foreach (var value in collection)
            {
                writer.WriteLine(value.Value);
            }
        }
    }

}
