using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileWrite
{

    // Start is called before the first frame update

    public void PrintResults(string fileName, string fileContent)
    {

        string filePath = @"Assets\" + fileName;
        File.AppendAllText(filePath, fileContent);

    }
}
