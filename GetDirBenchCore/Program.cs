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
        static string fullPath = @"C:\src\msbuild-working\msbuild\src\Build\BackEnd\Components\RequestBuilder\TargetUpToDateChecker.cs";

        [Benchmark]
        public void GetLastWriteTimeManaged()
        {
            File.GetLastWriteTimeUtc(fullPath);
        }

        [Benchmark]
        public void GetLastWriteTimeNative()
        {
            WIN32_FILE_ATTRIBUTE_DATA data = new WIN32_FILE_ATTRIBUTE_DATA();
            bool success = false;

            success = GetFileAttributesEx(fullPath, 0, ref data);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WIN32_FILE_ATTRIBUTE_DATA
        {
            internal int fileAttributes;
            internal uint ftCreationTimeLow;
            internal uint ftCreationTimeHigh;
            internal uint ftLastAccessTimeLow;
            internal uint ftLastAccessTimeHigh;
            internal uint ftLastWriteTimeLow;
            internal uint ftLastWriteTimeHigh;
            internal uint fileSizeHigh;
            internal uint fileSizeLow;
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetFileTime(
            SafeFileHandle hFile,
            out FILETIME lpCreationTime,
            out FILETIME lpLastAccessTime,
            out FILETIME lpLastWriteTime
            );


        [SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetFileAttributesEx(String name, int fileInfoLevel, ref WIN32_FILE_ATTRIBUTE_DATA lpFileInformation);
    }
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<WindowsCheck>();
        }
    }
}
