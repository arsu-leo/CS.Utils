using ArsuLeo.CS.Utils.Model.Exceptions;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsuLeo.CS.Utils.Service.SystemUtils
{
    public static class FileUtil
    {
        public const int BYTES_IN_MEGABYTE = 1024 * 1024;



        /// <summary>
        /// Overwrites file if existing
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <param name="ensureDirectoryExists"></param>
        public static void WriteWholeFile(string path, string content, bool ensureDirectoryExists = true)
        {
            FileInfo f = new FileInfo(path);
            if (ensureDirectoryExists)
            {
                EnsureDirectoryExists(f.Directory);
            }
            using StreamWriter file = File.CreateText(f.FullName);
            file.Write(content);
        }



        public static string ReadWholeFile(string path)
        {
            using StreamReader file = File.OpenText(path);
            return file.ReadToEnd();
        }

        public static async Task<string> ReadWholeFileAsync(string path)
        {
            using StreamReader file = File.OpenText(path);
            return await file.ReadToEndAsync();
        }

        public static string ReadWholeFile(string path, string dflt = "")
        {
            try
            {
                return ReadWholeFile(path);
            }
            catch (FileNotFoundException)
            {
                return dflt;
            }
        }

        public static FileInfo? GetMostRecentModifiedFile(string dir)
        {
            if (!Directory.Exists(dir))
            {
                return null;
            }
            string[] files = Directory.GetFiles(dir);
            return files.Aggregate<string, FileInfo?>(null, (lastModified, file) =>
            {
                FileInfo fInfo = new FileInfo(file);
                if (lastModified == null)
                {
                    return fInfo;
                }
                if (fInfo.LastWriteTimeUtc > lastModified.LastWriteTimeUtc)
                {
                    return fInfo;
                }
                return lastModified;
            });
        }


        public static void EnsureDirectoryExists(string path)
        {
            EnsureDirectoryExists(new DirectoryInfo(path));
        }

        public static void EnsureDirectoryExists(DirectoryInfo di)
        {
            if (!di.Exists)
            {
                di.Create();
            }
            di.Refresh();
            if (!di.Exists)
            {
                throw new CouldNotCreateDirectoryException(di.FullName, $"Could not create directory with path \"{di.FullName}\"");
            }
        }

        public static void EnsureTextFileExists(string path)
        {
            EnsureTextFileExists(new FileInfo(path));
        }

        public static void EnsureTextFileExists(FileInfo fi)
        {
            fi.Refresh();
            if (!fi.Exists)
            {
                EnsureDirectoryExists(fi.Directory);
                fi.Refresh();
                fi.CreateText().Close();
            }
            fi.Refresh();

            if (!fi.Exists)
            {
                throw new CouldNotCreateFileException(fi.FullName, $"Could not create file with path \"{fi.FullName}\"");
            }
        }

        public static void CreateTextFileInDesktop(string name, string content, out string path)
        {
            string dsk = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // Logical Desktop
            path = Path.Combine(dsk, name);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            WriteWholeFile(path, content);
        }

        public static void DeleteFileIfExists(string filePath)
        {
            FileInfo f = new FileInfo(filePath);
            DeleteFileIfExists(f);
        }

        public static void DeleteFileIfExists(FileInfo file)
        {
            file.Refresh();
            if (file.Exists)
            {
                file.Delete();
            }
        }

        public static bool IsDirectoryWritable(string dirPath, out Exception? ex)
        {
            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        dirPath,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                ) { }
                ex = null;
                return true;
            }
            catch (Exception th)
            {
                ex = th;
                return false;
            }
        }
        public static bool IsDirectoryWritable(string dirPath)
        {
            return IsDirectoryWritable(dirPath, out _);
        }

        /// <summary>
        /// Validates the name of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns><c>true</c> if the file name is valid, <c>false</c> otherwise.</returns>
        public static bool IsValidFileName(string? fileName)
        {
            return !string.IsNullOrWhiteSpace(fileName) && fileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }



        /// <summary>
        /// Validates the file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns><c>true</c> if the path path is valid, <c>false</c> otherwise.</returns>
        public static bool IsValidFilePath(string? filePath)
        {
            string? fileName = Path.GetFileName(filePath);
            if (!IsValidFileName(fileName))
            {
                return false;
            }
            string? fileDir = Path.GetDirectoryName(filePath);
            return IsValidPath(fileDir);

        }

        public static bool IsValidPath(string? filePath)
        {
            return !string.IsNullOrWhiteSpace(filePath) && filePath.IndexOfAny(Path.GetInvalidPathChars()) < 0;
        }

        public static string ReplaceInvalidFileNameCharsBy(string fileName, string replaceBy = "_")
        {
            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < fileName.Length; i++)
            {
                char ch = fileName[i];
                if (invalidFileNameChars.Contains(ch))
                {
                    result.Append(replaceBy);
                }
                else
                {
                    result.Append(ch);
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Returns true if <paramref name="path"/> starts with the path <paramref name="baseDirPath"/>.
        /// The comparison is case-insensitive, handles / and \ slashes as folder separators and
        /// only matches if the base dir folder name is matched exactly ("c:\foobar\file.txt" is not a sub path of "c:\foo").
        /// </summary>
        public static bool IsChidlOf(string parentDirPath, string childPath)
        {
            try
            {
                DirectoryInfo parentDir = new DirectoryInfo(parentDirPath);
                FileInfo fi = new FileInfo(childPath);
                var compareDir = fi.Directory;
                while (parentDir.FullName.Length < compareDir.FullName.Length && compareDir.FullName != compareDir.Root.FullName)
                {
                    if (parentDir.FullName == compareDir.FullName)
                    {
                        return true;
                    }
                    compareDir = compareDir.Parent;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsFileInDirectory(string file, string dir)
        {
            return IsFileInDirectory(new FileInfo(file), new DirectoryInfo(dir));
        }
        public static bool IsFileInDirectory(FileInfo fi, DirectoryInfo di)
        {
            return IsDirecotryInDirectory(fi.Directory, di, true);
        }
        public static bool IsDirecotryInDirectory(DirectoryInfo checkDir, DirectoryInfo di, bool trueIfSame = false)
        {
            if (checkDir.FullName == di.FullName)
            {
                return trueIfSame;
            }
            do
            {
                if (checkDir.FullName == di.FullName)
                {
                    return true;
                }
                checkDir = checkDir.Parent;
            }
            while (checkDir != null && checkDir.FullName.Length >= di.FullName.Length);
            return false;
        }

        public static string GetNameWithoutExtension(this FileInfo f)
            => f.Name.Substring(0, f.Name.Length - f.Extension.Length);

        public static string GetNameWithoutExtension(string name)
        {
            int index = name.LastIndexOf('.');
            if (index < 0)
            {
                return name;
            }
            return name.Substring(0, index);
        }
    }
}
