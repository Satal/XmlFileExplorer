using System;
using System.Runtime.InteropServices;

namespace XmlFileExplorer
{
    class NativeMethods
    {
        private const int SwShow = 5;
        private const uint SeeMaskInvokeidlist = 12;

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern bool ShellExecuteEx(ref ShellExecuteInfo lpExecInfo);


        public static bool ShowProperties(string filename)
        {
            var info = new ShellExecuteInfo();
            info.cbSize = Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = filename;
            info.nShow = SwShow;
            info.fMask = SeeMaskInvokeidlist;
            return ShellExecuteEx(ref info);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct ShellExecuteInfo
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        #region File type

        // Based on the code from http://objectlistview.sourceforge.net/cs/index.html
        private const int MAX_PATH = 260; 
        private const int SHGFI_TYPENAME = 0x00400;     // get type name

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SHGetFileInfo(string pszPath, int dwFileAttributes,
            out SHFILEINFO psfi, int cbFileInfo, int uFlags);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public int dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        public static string GetFileType(string filename)
        {
            SHFILEINFO shfi = new SHFILEINFO();
            int flags = SHGFI_TYPENAME;
            IntPtr result = SHGetFileInfo(filename, 0, out shfi, Marshal.SizeOf(shfi), flags);
            return result.ToInt32() == 0 ? String.Empty : shfi.szTypeName;
        }
        #endregion
    }
}
