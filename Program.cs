using System;
using System.Management;
using System.Runtime.InteropServices;

namespace EL
{
    class Program
    {
        static void Main(string[] args)
        {
            int z = -132;
            Console.WriteLine("-132 = "+Convert.ToString(Math.Abs(z),2));

            float d = 122.5f;
            int[] n = Array.ConvertAll(d.ToString().Split(","), int.Parse);
            Console.WriteLine("122.5 = "+Convert.ToString(n[0], 2)+"."+ Convert.ToString(n[1], 2));

            Console.WriteLine(GetPhysicalMemory());
            Console.WriteLine(GetInstalledMemory());
            Console.WriteLine(GetProcessorName());
        }
        public static string GetPhysicalMemory()
        {
            ManagementScope oMs = new ManagementScope();
            ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
            ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
            ManagementObjectCollection oCollection = oSearcher.Get();

            long MemSize = 0;
            long mCap = 0;

            // In case more than one Memory sticks are installed
            foreach (ManagementObject obj in oCollection)
            {
                mCap = Convert.ToInt64(obj["Capacity"]);
                MemSize += mCap;
            }
            MemSize = (MemSize / 1024) / 1024;
            return MemSize.ToString() + "MB";
        }
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetPhysicallyInstalledSystemMemory(out long TotalMemoryInKilobytes);

        public static string GetInstalledMemory()
        {
            long memKb;
            GetPhysicallyInstalledSystemMemory(out memKb);
            return (memKb / 1024 / 1024) + " GB of RAM installed.";
        }
        public static string GetProcessorName()
        {

            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            String Id = String.Empty;
            foreach (ManagementObject mo in moc)
            {

                Id = mo.Properties["Name"].Value.ToString();
                break;
            }
            return Id;

        }
    }
}
