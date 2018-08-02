using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using BenchmarkDotNet.Running;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GetDirBench {
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

    [ClrJob, CoreJob]
    public class ManagedVsNative
    {
        private static string path = @"C:\Users\chcasta";

        [Benchmark]
        public bool IsFileOrDirectoryManaged() {
            try {
                File.GetAttributes(path);
                return true;
            }
            catch {
                return false;
            }
        }

        [Benchmark]
        public bool IsFileOrDirectoryNative() {
            WIN32_FILE_ATTRIBUTE_DATA data = new WIN32_FILE_ATTRIBUTE_DATA();
            return GetFileAttributesEx(path, 0, data);
        }

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Class name is NativeMethodsShared for increased clarity")]
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetFileAttributesEx(String name, int fileInfoLevel, ref WIN32_FILE_ATTRIBUTE_DATA lpFileInformation);
    }
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ManagedVsNative>();
        }
    }
}
