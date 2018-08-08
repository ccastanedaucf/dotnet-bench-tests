using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;

namespace GetDirBench
{
    public class WindowsCheck
    {
        static string dir = @"C:\Users\chcasta\Documents\test";
        static string[] paths = { "test1", "test2", "test3", "test4", "test5", "test6", "test7", "test8", "test9", "test10" };
[Benchmark]
        public void Single()
        {
            GetLowestSingle(dir, paths);
        }
        [Benchmark]
        public void Multiple()
        {
            GetLowestMultiple(dir);
        }

        internal static void GetLowestSingle(string directory, string[] paths)
        {
            foreach (string path in paths)
            {
                string fullPath = Path.Combine(directory, path);
                File.GetLastWriteTimeUtc(fullPath);
            }
        }

        internal static void GetLowestMultiple(string directory)
        {
            var dir = new DirectoryInfo(directory);
            foreach (FileInfo file in dir.EnumerateFiles())
            {
                var c = file.LastWriteTimeUtc;
            }
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<WindowsCheck>();
        }
    }
}
