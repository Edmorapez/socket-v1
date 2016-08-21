using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Microsoft.Win32;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace socket_v1
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] data = new byte[3];

            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
            Socket newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            newsock.Bind(ipep);
            newsock.Listen(10);
            ASCIIEncoding asen = new ASCIIEncoding();
            Socket client = newsock.Accept();
            client.Send(asen.GetBytes("if you want data type letter (g)..  "));//envindo al client

            byte[] b = new byte[100];
            int k = client.Receive(b);
            string szReceived = Encoding.ASCII.GetString(b, 0, k);//send to server

            if (szReceived.Equals("g"))
            {
                Console.WriteLine("The byte data is : " + szReceived);

                client.Send(asen.GetBytes("  Nombre de equipo : " + nombreEquipo() + "\r" + "\n" +
                    " Nombre Usuario: " + nombreUsr() + "\r" + "\n" + " Direccion Maq: " + GetMacAddress() + "\r" + "\n" +
                    " Fecha y Hora: " + getDate() + "\r" + "\n" + " Procesador: " + procesador() + "\r" + "\n" + " Unidades: "
                    + unidades() + " \r" + "\n" + " Sistema Operativo: " + OsName() +
                    " \r" + "\n" + " Procesos en Ejecución :" + procesosEjecu()));
            }
            else
            {
                client.Send(asen.GetBytes("The Value is not defined "));
            }

            Console.WriteLine("\nSent Data to client");


            newsock.Close();
            Console.ReadKey();
        }


        ////////////////////////////////METHODS//////////////////////

        static string GetMacAddress()
        {
            string macAddresses = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Wireless80211) continue;
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }


        static string getDate()
        {
            DateTime x = DateTime.Now;
            return x.ToString();
        }

        static string unidades()
        {
            StringBuilder strinbuilderDisk = new StringBuilder();
            string[] drives = System.IO.Directory.GetLogicalDrives();
            foreach (string str in drives)
            {
                strinbuilderDisk.Append(str);
            }
            return strinbuilderDisk.ToString();
        }

        static string nombreEquipo()
        {
            string nombre;
            return nombre = Environment.MachineName;
        }

        static string nombreUsr()
        {
            string nombre;
            return nombre = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        }

        static string procesador()
        {
            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", RegistryKeyPermissionCheck.ReadSubTree);
            if ((processor_name != null) && (processor_name.GetValue("ProcessorNameString") != null)) { }
            string proce = (string)(processor_name.GetValue("ProcessorNameString"));
            return proce;
        }
        static string procesosEjecu()
        {
            StringBuilder strinbuilder = new StringBuilder();
            Process[] myProcesses = Process.GetProcesses();
            foreach (Process myProcess in myProcesses)
            {
                strinbuilder.Append(myProcess.ProcessName);
                strinbuilder.Append('\r');
                strinbuilder.Append('\n');
            }
            return strinbuilder.ToString();
        }
        public static string HKLM_GetString(string path, string key)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return "";
                return (string)rk.GetValue(key);
            }
            catch { return ""; }
        }

        public static string OsName()
        {
            string ProductName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
            string CSDVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
            if (ProductName != "")
            {
                return (ProductName.StartsWith("Microsoft") ? "" : "Microsoft ") + ProductName +
                            (CSDVersion != "" ? " " + CSDVersion : "");
            }
            return "";
        }
        /*
        SO
        NOMBRE EQUIPO y
        MAQ            y
        UNIDADES*      y
        FECHA/HORA*     y
        PROCESADOR*     y
        USUARIO*       y
        PROCESO EJEC*

         */
    }
}




