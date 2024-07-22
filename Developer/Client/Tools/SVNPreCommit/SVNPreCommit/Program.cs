using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace SVNPreCommit
{
    class Program
    {
        static string[] LOG_PREFIX =
        {
            "[add]", "[添加]", "[新增]","【add】",
            "[del]", "[delete]", "[移除]","[删除]","【del】",
            "[modify]", "[change]", "[修改]","【modify】",
            "[optimize]", "[优化]","【optimize】",
            "[debug]","【debug】",
            "[merge]","[合并]","【merge】",
            "[rollback]","[回滚]","【rollback】",
            "[branch]","[分支]","【branch】",
        };
        static string[] LOG_DESC =
        {
            "说明:", "说明：", "desc:",
            "描述:", "描述：",
        };
        //使用说明, 在pre_commit hook 中添加
        //"E:\VisualSVNServe\pre_commit.exe" %1 %2
        private static int Main(string[] args)
        {
            string strLogPS = "范例:\r\n";
            strLogPS += "[add]\r\n新增\"等级\"多语言\r\n";
            strLogPS += "[del]\r\n去除\"等级\"多语言\r\n";
            strLogPS += "[modify]\r\n修改\"玩家\"多语言为\"英雄\"\r\n";
            strLogPS += "[optimize]\r\n背包列表显示item项,弹出间隔改为0.1秒\r\n";
            strLogPS += "[debug]\r\n对背包中，砖石图标显示错误进行修复\r\n";
            strLogPS += "[marge]\r\n将主线的2001-3001内容合并到11月版本分支\r\n";
            strLogPS += "[rollback]\r\n将主线2001 版本回滚到3001，背包列表刷新问题\r\n";

            string svnhook = @"C:\Program Files\VisualSVN Server\bin\svnlook.exe";
            string Svnpath = Environment.GetEnvironmentVariable("VISUALSVN_SERVER");
            if (!string.IsNullOrEmpty(Svnpath))
                svnhook = Svnpath + @"bin\svnlook.exe";
            string repository_path = args[0];
            string transactionId = args[1];
            bool validateMeta = true;
            //验证日志
            using (var process = new Process())
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = svnhook;// @"C:\Program Files\VisualSVN Server\bin\svnlook.exe";
                process.StartInfo.Arguments = string.Format("log -t {0} \"{1}\"", transactionId, repository_path);
                process.Start();
                string content = process.StandardOutput.ReadToEnd();
                //强制写标记可以放你一马
                if (content.Contains("[meta]")) validateMeta = false;
                if (content.Length < 10)
                {
                    Console.Error.WriteLine("提示:输入日志事务描述不够明确，至少需要10个字");
                    Console.Error.WriteLine(strLogPS);
                    return 1;
                }
                content = content.Trim();
                bool hasPrefix = false;
                string tempContent = content.ToLower();
                for (int i = 0; i < LOG_PREFIX.Length; ++i)
                {
                    if (tempContent.StartsWith(LOG_PREFIX[i]))
                    {
                        hasPrefix = true;
                        break;
                    }

                }
                if (!hasPrefix)
                {
                    Console.Error.WriteLine("提示:没有按指定格式说明");
                    Console.Error.WriteLine(strLogPS);
                    return 1;
                }
                bool bAllDigtal = true;
                int bCommon = 0;
                for (int i = 0; i < content.Length; ++i)
                {
                    if (content[i] == ' ') continue;
                    if (content[i] == '\t') continue;
                    if (content[i] == '\n') continue;
                    if (bCommon <= 0)
                    {
                        bCommon = i;
                    }
                    else if (bCommon < content.Length)
                    {
                        if (content[i] != content[bCommon])
                        {
                            bCommon = 10000;
                        }
                    }

                    if (content[i] < 0 || content[i] > 9)
                    {
                        bAllDigtal = false;
                    }
                }
                if (bAllDigtal || bCommon != 10000)
                {
                    Console.Error.WriteLine("提示:请不要乱输入，务必正确描述日志事务!");
                    return 1;
                }
                process.WaitForExit();
            }
            //验证meta
            if (validateMeta)
            {
                using (var process = new Process())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.FileName = svnhook;// @"C:\Program Files\VisualSVN Server\bin\svnlook.exe";
                    process.StartInfo.Arguments = string.Format("changed -t {0} \"{1}\"", transactionId, repository_path);
                    process.Start();
                    string content = process.StandardOutput.ReadToEnd();
                    string[] lines = content.Split('\n');
                    HashSet<string> addSource = new HashSet<string>();
                    HashSet<string> addMeta = new HashSet<string>();
                    foreach (var v in lines)
                    {
                        if (v.Length <= 0) continue;
                        //只看新增和删除的
                        if (v.StartsWith("D") || v.StartsWith("A"))
                        {
                            string str = Regex.Replace(v, @"\s", "");
                            str = str.Replace("\\", "/");
                            if (str.Contains("/Client/") && str.Contains("/Assets/"))
                            {
                                int lenth = str.IndexOf("/Assets/");
                                if (lenth < str.Length)
                                {
                                    str = str.Substring(lenth, str.Length - lenth);
                                    if (str.Contains(".meta"))
                                    {
                                        addMeta.Add(str.Replace(".meta", ""));
                                    }
                                    else
                                    {
                                        if (str.EndsWith("/"))
                                            str = str.Substring(0, str.Length - 1);
                                        string externs = System.IO.Path.GetExtension(str);
                                        if (string.IsNullOrEmpty(externs))
                                            addSource.Add(str);
                                    }
                                }
                            }
                        }
                    }
                    string strError = "";
                    foreach (var db in addSource)
                    {
                        if (!addMeta.Contains(db))
                        {
                            strError += db + "\r\n";
                        }
                    }
                    if (!string.IsNullOrEmpty(strError))
                    {
                        Console.Error.WriteLine("丢失meta:" + strError);
                        return 1;
                    }
                    process.WaitForExit();
                }
            }
            return 0;
        }
    }
}
