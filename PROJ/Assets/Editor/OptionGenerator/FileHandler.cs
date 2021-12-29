using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class FileHandler {

    public static void Save<T>(T[] items, string fileName) {
        Write(fileName, JsonHelper.ToJson<T>(items, true));
    }

    public static T[] Read<T>(string fileName) {
        string contents = Read(fileName);

        if (string.IsNullOrEmpty(contents) || contents.Equals("{}"))
            return new T[0];

        return JsonHelper.FromJson<T> (contents);
    }
    
    public static void Write(string path, string contents) {
        
        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        
        StreamWriter streamWriter = new StreamWriter(fileStream);
        
        streamWriter.Write(contents);
        
        streamWriter.Close();
        fileStream.Close();
    }

    public static string Read(string path) {
        if (File.Exists(path) == false)
            return "";
        
        StreamReader streamReader = new StreamReader(path);

        string contents = streamReader.ReadToEnd();
    
        streamReader.Close();

        return contents;
    }

    public static void CreateFile(string fileName, string targetPath, string fileSuffix, string contents) {

        Write($"{targetPath}/{fileName}.{fileSuffix}", contents);

    }
    

}
