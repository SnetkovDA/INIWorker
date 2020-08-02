using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INIWorker
{
    public class INIFile
    {
        readonly char commentChar;
        readonly string pathToFile;
        readonly string[] plainText;

        public INIFile(string pathToINIFile, char commentChar = '#')
        {
            this.commentChar = commentChar;
            this.pathToFile = pathToINIFile;
            if (File.Exists(pathToFile))
                plainText = File.ReadAllLines(pathToFile);
            else
                throw new FileNotFoundException("File at this path not found", pathToFile);
        }

        public string GetValue(string section, string name)
        {
            string currentSection = "";
            string searchSection = section.ToLower();
            string searchName = name.ToLower();
            foreach (string line in plainText)
            {
                string currLine = line.Trim();
                if (currLine.Length == 0 || currLine[0] == commentChar)
                    continue;
                if(IsSection(currLine))
                {
                    currentSection = currLine.TrimStart('[').TrimEnd(']').ToLower();
                    continue;
                }
                string[] keyValuePair = currLine.Split('=');
                if (keyValuePair.Length < 2)
                    continue;
                if(keyValuePair[0].Trim().ToLower().Equals(searchName)
                    && searchSection.Equals(currentSection))
                {
                    StringBuilder builder = new StringBuilder(keyValuePair.Length - 1);
                    for (int i = 1; i < keyValuePair.Length; i++)
                        builder.Append(keyValuePair[i]);
                    return builder.ToString();
                }
            }
            return null;
        }

        bool IsSection(string line)
        {
            int eqInd = line.IndexOf('=');
            eqInd = eqInd == -1 ? int.MaxValue : eqInd;
            int braInd = line.IndexOf('[');
            return (braInd != -1) && (braInd < eqInd);
        }
    }
}
