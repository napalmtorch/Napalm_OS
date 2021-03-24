using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using NapalmOS.Core;
using NapalmOS.Graphics;
using Cosmos.System.FileSystem;

namespace NapalmOS.Hardware
{
    public static class FileSystem
    {
        // driver
        public static CosmosVFS Driver { get; private set; }
        public static List<Sys.FileSystem.Listing.DirectoryEntry> Partitions { get; private set; }

        // task
        public static Task Task = new Task("File Manager", "fat.sys");

        // initialize
        public static void Initialize()
        {
            try
            {
                // initialize driver
                Driver = new CosmosVFS();
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(Driver);

                // register task
                TaskManager.RegisterTask(Task);
            }
            catch (Exception ex) { ExceptionHandler.ThrowFatal("INIT_FAT", ex.Message); }
        }


        // convert argument to path
        public static string ConvertArgumentToPath(string[] args)
        {
            // get file path
            string file = "";
            for (int i = 1; i < args.Length; i++) { file += args[i] + " "; }
            file = file.Replace('/', '\\');
            if (!file.StartsWith("0:\\")) { file = Programs.WinTerm.CurrentPath + file; }
            if (file.EndsWith(" ")) { file = file.Remove(file.Length - 1, 1); }
            if (!file.EndsWith("\\")) { file += "\\"; }
            return file;
        }

        // convert string to proper path
        public static string ConvertStringToPath(string text)
        {
            string file = text.Replace('/', '\\');
            if (!file.StartsWith("0:\\")) { file = "0:\\" + text; }
            return file;
        }

        // check if file exists
        public static bool FileExists(string file)
        {
            try
            {
                string path = Path.GetDirectoryName(file);
                if (Directory.Exists(path))
                {
                    // file found
                    if (File.Exists(file)) { return true; }
                    // file not found
                    else { return false; }
                }
                else { return false; }
            }
            // error fetching file
            catch (Exception) { return false; }
        }

        // check if directory exists
        public static bool DirectoryExists(string path)
        {
            try
            {
                // directory found
                if (Directory.Exists(path)) { return true; }
                // directory not found
                else { return false; }
            }
            // error fetching directory
            catch (Exception ex) { Terminal.WriteLine(ex.Message); return false; }
        }

        // read all bytes from a file
        public static byte[] ReadAllBytes(string file)
        {
            // file not found
            if (!FileExists(file)) { return new byte[0]; }
            // attempt to load data from file
            try
            {
                // file found
                byte[] output = File.ReadAllBytes(file);
                return output;
            }
            // error fetching file
            catch (Exception) { return new byte[0]; }
        }

        // read all text from a file
        public static string ReadAllText(string file)
        {
            // file not found
            if (!FileExists(file)) { return ""; }
            // attempt to load data from file
            try
            {
                // file found
                return File.ReadAllText(file);
            }
            // error fetching file
            catch (Exception) { return ""; }
        }

        // read all lines from a file
        public static string[] ReadAllLines(string file)
        {
            // file not found
            if (!FileExists(file)) { return new string[0]; }
            // attempt to load data from file
            try
            {
                // file found
                return File.ReadAllLines(file);
            }
            // error fetching file
            catch (Exception) { return new string[0]; }
        }

        // write all bytes to a file
        public static bool WriteAllBytes(string file, byte[] data)
        {
            try
            {
                // file found
                File.WriteAllBytes(file, data);
                return true;
            }
            // error fetching file
            catch (Exception) { return false; }
        }

        // write all text to a file
        public static bool WriteAllText(string file, string data)
        {
            try
            {
                // file found
                File.WriteAllText(file, data);
                return true;
            }
            // error fetching file
            catch (Exception) { return false; }
        }

        // write all lines to a file
        public static bool WriteAllLines(string file, string[] data)
        {
            try
            {
                // file found
                File.WriteAllLines(file, data);
                return true;
            }
            // error fetching file
            catch (Exception) { return false; }
        }

        // get files in directory
        public static string[] GetFiles(string path)
        {
            // directory not found
            if (!DirectoryExists(path)) { return new string[0]; }
            // attempt to get files from directory
            try
            {
                // files found
                return Directory.GetFiles(path);
            }
            // error fetching files
            catch (Exception) { return new string[0]; }
        }

        // get file entries in directory
        public static List<Sys.FileSystem.Listing.DirectoryEntry> GetFileEntires(string path)
        {
            // directory not found
            if (!DirectoryExists(path)) { return new List<Sys.FileSystem.Listing.DirectoryEntry>(); }
            // attempt to get files from directory
            try
            {
                List<Sys.FileSystem.Listing.DirectoryEntry> entries = new List<Sys.FileSystem.Listing.DirectoryEntry>();
                // files found
                string[] files = GetFiles(path);
                for (int i = 0; i < files.Length; i++)
                {
                    entries.Add(Driver.GetFile(files[i]));
                }
                return entries;
            }
            // error fetching files
            catch (Exception) { return new List<Sys.FileSystem.Listing.DirectoryEntry>(); }
        }

        // get directories in directory
        public static string[] GetDirectories(string path)
        {
            // directory not found
            if (!DirectoryExists(path)) { return new string[0]; }
            // attempt to get files from directory
            try
            {
                // files found
                return Directory.GetDirectories(path);
            }
            // error fetching files
            catch (Exception) { return new string[0]; }
        }

        // get directory entries in directory
        public static List<Sys.FileSystem.Listing.DirectoryEntry> GetDirectoryEntires(string path)
        {
            // directory not found
            if (!DirectoryExists(path)) { return new List<Sys.FileSystem.Listing.DirectoryEntry>(); }
            // attempt to get files from directory
            try
            {
                List<Sys.FileSystem.Listing.DirectoryEntry> entries = new List<Sys.FileSystem.Listing.DirectoryEntry>();
                // files found
                string[] folders = GetDirectories(path);
                for (int i = 0; i < folders.Length; i++)
                {
                    entries.Add(Driver.GetDirectory(folders[i]));
                }
                return entries;
            }
            // error fetching files
            catch (Exception ex) { Terminal.WriteLine(ex.Message); return new List<Sys.FileSystem.Listing.DirectoryEntry>(); }
        }

        // get volumes
        public static List<Sys.FileSystem.Listing.DirectoryEntry> GetVolumes()
        {
            try { return Driver.GetVolumes(); }
            catch (Exception) { return null; }
        }

        // create new directory
        public static bool CreateDirectory(string name)
        {
            // directory already exists
            if (DirectoryExists(name)) { return false; }
            try
            {
                // file found
                Directory.CreateDirectory(name);
                return true;
            }
            catch (Exception) { return false; }
        }

        // rename directory
        public static bool RenameDirectory(string nameOld, string nameNew)
        {
            // file not found
            if (!DirectoryExists(nameOld)) { return false; }
            try
            {
                // file found
                Directory.Move(nameOld, nameNew);
                return true;
            }
            catch (Exception) { return false; }
        }

        // rename file
        public static bool RenameFile(string nameOld, string nameNew)
        {
            // file not found
            if (!DirectoryExists(nameOld)) { return false; }
            try
            {
                // file found
                File.Move(nameOld, nameNew);
                return true;
            }
            catch (Exception) { return false; }
        }

        // delete directory
        public static bool DeleteDirectory(string path)
        {
            // file not found
            if (!DirectoryExists(path)) { return false; }
            try
            {
                // file found
                Directory.Delete(path, true);
                return true;
            }
            catch (Exception) { return false; }
        }

        // delete file
        public static bool DeleteFile(string file)
        {
            // file not found
            if (!FileExists(file)) { return false; }
            try
            {
                // file found
                File.Delete(file);
                return true;
            }
            catch (Exception) { return false; }
        }

        // copy file
        public static bool CopyFile(string srcFile, string destFile)
        {
            if (!FileExists(srcFile)) { return false; }
            try
            {
                File.Copy(srcFile, destFile);
                return true;
            }
            catch (Exception) { return false; }
        }
    }
}
