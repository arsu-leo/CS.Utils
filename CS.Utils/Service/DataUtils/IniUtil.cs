using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ArsuLeo.CS.Utils.Service.DataUtils
{
    public static class IniUtil
    {
        private const string INI_LINE_ENDING = "\r\n";

        public static string AsIni(Dictionary<string, Dictionary<string, string>> data)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("#");
            sb.Append(DateTime.Now.ToString("yyyyMMddTHHmmsszzz"));

            if (data.TryGetValue(string.Empty, out Dictionary<string, string>? noSectionData))
            {
                sb.Append(INI_LINE_ENDING);
                AppendSectionContent(sb, noSectionData);
            }
            foreach (KeyValuePair<string, Dictionary<string, string>> kp in data)
            {
                if (!string.IsNullOrEmpty(kp.Key))
                {
                    string sectionName = kp.Key.Trim();

                    sb.Append(INI_LINE_ENDING);
                    sb.Append(INI_LINE_ENDING);
                    sb.Append('[');
                    sb.Append(sectionName);
                    sb.Append(']');
                    AppendSectionContent(sb, kp.Value);
                }
            }
            return sb.ToString();
        }

        public static void WriteIniFile(string filePath, Dictionary<string, Dictionary<string, string>> data, bool ensureDirectoryExists = false)
        {
            string content = AsIni(data);
            FileUtil.WriteWholeFile(filePath, content, ensureDirectoryExists);
        }

        private static void AppendSectionContent(StringBuilder sb, Dictionary<string, string> sectionData)
        {
            foreach (KeyValuePair<string, string> kp in sectionData)
            {
                string key = kp.Key.Trim();
                if (key.Contains('=') || key[0] == '[' && key[^1] == ']')
                {
                    throw new Exception($"Invalid key characters for ini key: \"{key}\"");
                }
                sb.Append(INI_LINE_ENDING);
                sb.Append(key);
                sb.Append('=');
                string content = EscapeValue(kp.Value);
                sb.Append(content);
            }
        }

        public static string EscapeValue(string str)
        {
            return string.Join(@"\n", 
                str.Trim().Replace(@"\", @"\\").SplitStringByAnyNewLines()
            );
        }

        public static string UnEscapeValue(string str)
        {
            string[] lines = str.Split(@"\\");
            for(int i = 0; i<lines.Length; i++)
            {
                lines[i] = lines[i].Replace(@"\n", "\n");
            }
            string result = string.Join(@"\", lines);
            return result;
        }

        public static readonly char[] CommentStarters = new char[] { '#', ';' };
        
        public static Dictionary<string, Dictionary<string, string>> ReadIniFile(string path, char[]? commentStarters = null)
        {
            commentStarters ??= CommentStarters;

            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();

            using FileStream fileStream = File.OpenRead(path);
            using StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8, true, 4096);
            string? line;
            int i = 0;

            Dictionary<string, string> currentSection = new Dictionary<string, string>();
            result.Add(string.Empty, currentSection);
            string currentSectionName = string.Empty; ;
            while ((line = streamReader.ReadLine()) != null)
            {
                line = line.Trim();
                //Equal not found
                if (line.Length == 0 || commentStarters.Contains(line[0]))
                {
                    continue;
                }
                if (line[0] == '[' && line[^1] == ']')
                {
                    currentSection = new Dictionary<string, string>();
                    currentSectionName = line.Substring(1, line.Length - 2);
                    result.Add(currentSectionName, currentSection);
                    continue;
                }
                int index = line.IndexOf('=');
                //No equal
                if (index < 0)
                {
                    currentSection[line] = string.Empty;
                    continue;
                }
                string key = line.Substring(0, index);
                string content = string.Empty;
                if (line.Length > index + 1)
                {
                    string rawContent = line.Substring(index + 1);
                    content = UnEscapeValue(rawContent);
                }
                currentSection[key] = content;


                i++;
            }
            return result;



            //string text = File.ReadAllText(path);
            //string[] lines = StringUtil.SplitStringByAnyNewLines(text, StringSplitOptions.RemoveEmptyEntries);

            ////Declared here for debug purposes

            //for (int i = 0; i < lines.Length; i++)
            //{
            //    string line = lines[i].Trim();
            //    //Equal not found
            //    if (line.Length == 0 || commentStarters.Contains(line[0]))
            //    {
            //        continue;
            //    }
            //    if (line[0] == '[' && line[^1] == ']')
            //    {
            //        currentSection = new Dictionary<string, string>();
            //        currentSectionName = line.Substring(1, line.Length - 2);
            //        result.Add(currentSectionName, currentSection);
            //        continue;
            //    }
            //    int index = line.IndexOf('=');
            //    //No equal
            //    if (index < 0)
            //    {
            //        currentSection[line] = string.Empty;
            //        continue;
            //    }
            //    string key = line.Substring(0, index);
            //    string content = string.Empty;
            //    if (line.Length > index + 1)
            //    {
            //        string rawContent = line.Substring(index + 1);
            //        content = rawContent.Replace("\\n", "\\n", StringComparison.InvariantCultureIgnoreCase);
            //    }
            //    currentSection[key] = content;
            //}



        }
    }
}
