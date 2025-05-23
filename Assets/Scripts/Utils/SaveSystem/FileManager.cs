using System;
using System.IO;
using UnityEngine;

namespace Unity.MegacityMetro.Utils
{
    public static class FileManager
    {
        public static bool WriteToFile(string fileName, string fileContents)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);

            try
            {
                File.WriteAllText(fullPath, fileContents);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to write to {fullPath} with exception {e}");
                return false;
            }
        }

        public static bool LoadFromFile(string fileName, out string result)
        {
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);

            if (!File.Exists(fullPath))
            {
                result = "File not found.";
                return false;
            }
            
            try
            {
                result = File.ReadAllText(fullPath);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to read from {fullPath} with exception {e}");
                result = "";
                return false;
            }
        }
    }
}