using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ArsuLeo.CS.Utils.Service.SystemUtils
{
    public static class FileNextUtil
    {
        public const int BYTES_IN_MEGABYTE = 1024 * 1024;

        public delegate string SecuencedFileNameBuilder(string fileNameWithoutExtension, int fileNum, string fileExtensionWithDot);
        public static string UnderscoreNextFileNameCreator(string fileNameWithoutExtension, int fileNum, string fileExtensionWithDot)
            => $"{fileNameWithoutExtension}_{fileNum}{fileExtensionWithDot}";
        public static string ParenthesisNextFileNameCreator(string fileNameWithoutExtension, int fileNum, string fileExtensionWithDot) 
            => $"{fileNameWithoutExtension}({fileNum}){fileExtensionWithDot}";
        
        public static readonly SecuencedFileNameBuilder DefaultNextFileNameCreator = UnderscoreNextFileNameCreator;

        /// <summary>
        /// Creates a rotation for a given file path.
        /// If file path exists, it renames the file.ext to file_startNum.ext
        /// If file_n.ext exists, it renames it as file_n+1.ext while n+1 < maxNum
        /// </summary>
        /// <param name="srcFile"></param>
        /// <param name="maxNum"></param>
        /// <param name="startNum"></param>
        public static void ReplaceFileForNext(FileInfo srcFile, int maxNum = 10, int startNum = 1)
            => ReplaceFileForNext(srcFile, DefaultNextFileNameCreator, maxNum, startNum);

        
        public static void ReplaceFileForNext(FileInfo srcFile, SecuencedFileNameBuilder fnCreator, int maxNum = 10, int startNum = 1)
        {
            if (!srcFile.Exists)//we are done
            {
                return;
            }
            string fileNameWithoutExtension = srcFile.GetNameWithoutExtension();
            FileInfo firstFile = new FileInfo(Path.Combine(srcFile.DirectoryName, fnCreator(fileNameWithoutExtension, startNum, srcFile.Extension)));
            if (firstFile.Exists)
            {
                ReplaceFileForNextPriv(srcFile.Directory, fnCreator, fileNameWithoutExtension, srcFile.Extension, maxNum, startNum);
                firstFile.Refresh();
                //Firs file should not exists now
            }
            //We use a new FileInfo as issuing a move to on it changes the object path to the new one
            FileInfo fileToMove = new FileInfo(srcFile.FullName);
            fileToMove.MoveTo(firstFile.FullName);
        }

        private static void ReplaceFileForNextPriv(DirectoryInfo directory, SecuencedFileNameBuilder fnCreator, string fileNameWithoutExtension, string extensionWithDot, int max, int num)
        {
            if (num > max) //Should not happen but security for recursivity
            {
                return;
            }
            int next = num + 1;
            FileInfo srcFile = new FileInfo(Path.Combine(directory.FullName, fnCreator(fileNameWithoutExtension, num, extensionWithDot)));
            if (!srcFile.Exists)
            {
                return;
            }

            FileInfo dstFile = new FileInfo(Path.Combine(directory.FullName, fnCreator(fileNameWithoutExtension, next, extensionWithDot)));
            if (dstFile.Exists)
            {
                if (next == max) //Is last
                {
                    dstFile.Delete();
                }
                else //Replace next
                {
                    ReplaceFileForNextPriv(directory, fnCreator, fileNameWithoutExtension, extensionWithDot, max, next);
                }
            }
            srcFile.MoveTo(dstFile.FullName);
        }

        public static string WriteFileOrNextOrDie(string content, string dir, string fileNameWithoutExtension, string extensionWithDot,
           int maxNextFileNum, Encoding encoding) 
            => WriteFileOrNextOrDie(content, dir, fileNameWithoutExtension, extensionWithDot, maxNextFileNum, encoding, DefaultNextFileNameCreator);
        public static string WriteFileOrNextOrDie(string content, string dir, string fileNameWithoutExtension, string extensionWithDot,
           int maxNextFileNum, Encoding encoding, SecuencedFileNameBuilder fnCreator)
        {

            FileUtil.EnsureDirectoryExists(dir);
            string fullPath = Path.Combine(dir, fileNameWithoutExtension + extensionWithDot);
            try
            {
                File.WriteAllText(fullPath, content, encoding);
                return fullPath;
            }
            //Asume file is locked
            catch (IOException eBase)
            {
                List<Exception> exes = new List<Exception>()
                {
                    eBase
                };
                int retryCount = 0;
                string newName = string.Empty;
                while (retryCount < maxNextFileNum)
                {
                    try
                    {
                        newName = GetNextFileNameExt(dir, fileNameWithoutExtension, extensionWithDot, fnCreator);
                        File.WriteAllText(newName, content, encoding);
                        return newName;
                    }
                    catch (IOException eNum)
                    {
                        exes.Add(eNum);
                    }
                    retryCount++;
                }
                throw new AggregateException($"Was not possible to write file to \"{fullPath}\" or any of the retry paths, last tried path \"{newName}\"", exes);
            }
        }

        public static string GetNextFileNameExt(string basePath, string fileNameWithoutExtension, string extensionWithDot, SecuencedFileNameBuilder fnCreator)
        {
            //ToDo Test debug extension has no dot
            string testFileName = $"{fileNameWithoutExtension}{extensionWithDot}";
            string resultFilePath = Path.Combine(basePath, testFileName);
            int i = 0;
            while (File.Exists(resultFilePath))
            {
                testFileName = fnCreator(fileNameWithoutExtension, i, extensionWithDot);
                resultFilePath = Path.Combine(basePath, testFileName);
                i++;
            }
            return resultFilePath;
        }
    }
}
