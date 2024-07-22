using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace DataConverter
{
    class IniParser
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string default_var, StringBuilder retVal, int buff_size, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string section, string key, int default_var, string filePath);

        private string _file_path;

        public IniParser()
        {
            _file_path = Directory.GetCurrentDirectory() + "\\" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".ini";
        }
        public IniParser(string t_filename)
        {
            if (string.IsNullOrEmpty(t_filename))
            {
                _file_path = Directory.GetCurrentDirectory() + "\\"  + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".ini";
                return;
            }
            else
            {
                _file_path = Directory.GetCurrentDirectory() + "\\" + t_filename;
            }
        }

        public bool writeData(string t_section, string t_key, string t_data)
        {
            long i_result = WritePrivateProfileString(t_section, t_key, t_data, _file_path);
            return (i_result != 0 ? true : false);
        }

        public string readStringData(string t_section, string t_key, string t_default = "")
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(t_section, t_key, t_default, temp, 255, this._file_path);
            return temp.ToString();
        }

        public int readIntData(string t_section, string t_key, int t_default = 0)
        {
            int i_result = GetPrivateProfileInt(t_section, t_key, t_default, this._file_path);
            return i_result;
        }
    }
}
