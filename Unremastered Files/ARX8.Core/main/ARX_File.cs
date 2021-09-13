using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using FEN;
using System.Runtime.Serialization.Formatters.Binary;

using ARX;

// Simple synchronous file copy operations with no user interface.
// To run this sample, first create the following directories and files:
// C:\Users\Public\TestFolder
// C:\Users\Public\TestFolder\test.txt
// C:\Users\Public\TestFolder\SubDir\test.txt


/// <summary>
/// Static class containing multiple simplified system file functions
/// </summary>
public static class ARX_File
{
    public static string GetFullFilePath(string strFilename)
    {
        return Application.persistentDataPath + "/" + strFilename + ".save";

    }

    public static void CreateBlankCards()
    {
        //Commented out in preparation for ARXV9
        //int i = 0;
        //CardIDs.CARDID eID = (CardIDs.CARDID)i;

        //while (System.Enum.IsDefined(typeof( CardIDs.CARDID), eID))
        //{
        //    CreateBlankCardCopy(eID.ToString().ToLower() + ".png");
        //    i++;
        //    eID = (CardIDs.CARDID)i;
        //}
    }

    static void CreateBlankCardCopy(string strNewFileName)
    {
        string fileName = "blankcard.png";
        string sourcePath = @"C:\Users\Public\TestFolder";
        string targetPath = @"C:\Users\Public\TestFolder\SubDir";

        // Use Path class to manipulate file and directory paths.
        string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
        string destFile = System.IO.Path.Combine(targetPath, strNewFileName);

        // To copy a folder's contents to a new location:
        // Create a new target folder. 
        // If the directory already exists, this method does not create a new directory.
        System.IO.Directory.CreateDirectory(targetPath);

        // To copy a file to another location and 
        // overwrite the destination file if it already exists.
        System.IO.File.Copy(sourceFile, destFile, true);

        // To copy all the files in one directory to another directory.
        // Get the files in the source folder. (To recursively iterate through
        // all subfolders under the current directory, see
        // "How to: Iterate Through a Directory Tree.")
        // Note: Check for target path was performed previously
        //       in this code example.
        if (System.IO.Directory.Exists(sourcePath))
        {
            string[] files = System.IO.Directory.GetFiles(sourcePath);

            // Copy the files and overwrite destination files if they already exist.
            foreach (string s in files)
            {
                // Use static Path methods to extract only the file name from the path.
                fileName = System.IO.Path.GetFileName(s);
                destFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Copy(s, destFile, true);
            }
        }
        else
        {
            Console.WriteLine("Source path does not exist!");
        }

        // Keep console window open in debug mode.
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();
    }

    public static string SerializeObject(System.Object obj)
    {
        try
        {
            string _XmlizedString = null;
            MemoryStream _memoryStream = new MemoryStream();
            XmlSerializer _xs = new XmlSerializer(obj.GetType());
            XmlTextWriter _xmlTextWriter = new XmlTextWriter(_memoryStream, Encoding.GetEncoding("ISO-8859-1"));

            _xs.Serialize(_xmlTextWriter, obj);
            _memoryStream = (MemoryStream)_xmlTextWriter.BaseStream;
            _XmlizedString = ByteArrayToString(_memoryStream.ToArray());

            return _XmlizedString;
        }
        catch (Exception e)
        {
            Debug.LogError("IGD2: PlayerPrefsSerializer failed to SerializeObject throwing the exception \n" + e);
            return null;
        }
    }

    public static T DeserializeObject<T>(string xml)
    {
        try
        {
            XmlSerializer _xs = new XmlSerializer(typeof(T));
            MemoryStream _memoryStream = new MemoryStream(StringToByteArray(xml));
            return (T)_xs.Deserialize(_memoryStream);
        }
        catch (Exception e)
        {
            Debug.LogError("IGD2: PlayerPrefsSerializer failed to DeserializeObject throwing the exception \n" + e);
            return default(T);
        }
    }

    public static byte[] StringToByteArray(string s)
    {
        byte[] b = new byte[s.Length];

        for (int i = 0; i < s.Length; i++)
            b[i] = (byte)s[i];

        return b;
    }

    public static string ByteArrayToString(byte[] b)
    {
        string s = "";

        for (int i = 0; i < b.Length; i++)
            s += (char)b[i];

        return s;
    }

    public static void SaveFile(object obj, string strFilename)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GetFullFilePath(strFilename));
        bf.Serialize(file, obj);
        file.Close();

        Debug.Log("Saved to " + GetFullFilePath(strFilename));
    }

    public static void SaveFile(string strFileContents, string strFilename)
    {
        // 2
        BinaryFormatter bf = new BinaryFormatter();
        File.WriteAllText(GetFullFilePath(strFilename), strFileContents);
        Debug.Log("Saved to " + GetFullFilePath(strFilename));
    }

    public static string LoadGame(string strFilename)
    {

        // 1
        if (File.Exists(GetFullFilePath(strFilename)))
        {
            Debug.Log("Loaded Game File found at " + GetFullFilePath(strFilename));
            return GetFullFilePath(strFilename);
        }

        Debug.LogError("No Game File found at " + GetFullFilePath(strFilename));
        return default;
    }

    public static T LoadFile<T>(string strFilename)
    {
        string strFullPath = GetFullFilePath(strFilename);
        // 1
        if (File.Exists(strFullPath))
        {
            //Debug.Log("Loading file found at " + strFullPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(strFullPath, FileMode.Open);
            //Debug.Log("Opened file found at " + strFullPath);
            T save = (T)bf.Deserialize(file);
            //Debug.Log("Deserialized file found at " + strFullPath);
            file.Close();
            Debug.Log("Loaded file found at " + strFullPath);
            return save;
        }

        Debug.LogError("No file found at " + strFullPath);
        return default;
    }

}