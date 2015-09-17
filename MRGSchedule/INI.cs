using System.Runtime.InteropServices;
using System.Text;

namespace MRGSchedule
{
    class INI
    {
        #region ini操作
        /// <summary>
        /// section：要读取的段落名
        /// key: 要读取的键
        /// defVal: 读取异常的情况下的缺省值
        /// retVal: key所对应的值，如果该key不存在则返回空值
        /// size: 值允许的大小
        /// filePath: INI文件的完整路径和文件名
        /// </summary>
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 自定义读取INI文件中的内容方法
        /// </summary>
        public static string GetIniContentValue(string section, string key, string path)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(section, key, "", temp, 1024, path);
            return temp.ToString();
        }

        /// <summary>
        /// section: 要写入的段落名
        /// key: 要写入的键，如果该key存在则覆盖写入
        /// val: key所对应的值
        /// filePath: INI文件的完整路径和文件名
        /// </summary>
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        #endregion
    }
}
