using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ImpersonationTest.Classes {
    internal class Extensions {


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);


        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool OpenProcessToken([In] IntPtr ProcessToken, [In] TokenAccessLevels DesiredAccess, [In, Out] ref IntPtr TokenHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);



        [Flags]
        enum ProcessAccessFlags : uint {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }
 

        /// <summary>
        /// Impersonates login and returns a list of system folders
        /// </summary>
        /// <param name="hprocessid"></param>
        public static List<String> ImpersonateLoginByProcess(uint hprocessid) {
            IntPtr handle = IntPtr.Zero;
            IntPtr tokenref = IntPtr.Zero;
            List<string> list = new List<string>();
            //
            try {
                handle = OpenProcess((uint)(ProcessAccessFlags.All), false, hprocessid);

                if (handle != IntPtr.Zero) {
                    bool refbool = OpenProcessToken(handle,TokenAccessLevels.MaximumAllowed, 
                        ref tokenref);

                    string s = WindowsIdentity.GetCurrent().Name;

                    using (WindowsImpersonationContext impersonatedUser = WindowsIdentity.Impersonate(tokenref)) {
                        s = WindowsIdentity.GetCurrent().Name;
                        list.Add("Impersonating user: " + s);

                        Type enumType = typeof(Environment.SpecialFolder);
                        foreach (string names in Enum.GetNames(enumType)) {
                            Environment.SpecialFolder esf = (Environment.SpecialFolder)Enum.Parse(enumType, names);
                            string pstring = String.Format("{0} - {1}", names, Environment.GetFolderPath(esf));
                            list.Add(pstring);
                        }

                    } 

                }
            }
            catch (Exception ex) {
                Trace.WriteLine(ex.ToString());
            }
            finally {
                if (handle != IntPtr.Zero) {
                    CloseHandle(handle);
                }
            }
            return list;
        }
    }
}
