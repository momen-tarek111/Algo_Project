using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DictionaryWriter
{
    public static void WriteValuesToFile<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection,string filePath,string timeFilePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(collection.Count());
            foreach (var value in collection)
            {
                writer.WriteLine(value.Value);
            }
        }
        using (StreamWriter writer = new StreamWriter(timeFilePath))
        {
            writer.WriteLine(" Construnct Graph time -> " + data.time3);
            writer.WriteLine(" Segmentation time -> " + data.time4);
            writer.WriteLine(" Compain time -> " + data.time2);
            writer.WriteLine(" Total Time -> "+data.time);

        }
    }

}
