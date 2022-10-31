using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ATPCliMode
{
    class Program
    {
        static Lib lib = new Lib();
        static bool isFirst = true;
        static void Main(string[] args)
        {
            try
            {
                if (args[0] == "-file")
                {
                    string AdofaiSpectralFile = args[1];
                    Console.WriteLine("已获取谱面文件" + Path.GetFileName(AdofaiSpectralFile) + ".");
                    StreamReader file = File.OpenText(AdofaiSpectralFile);
                    if (isFirst)
                    {
                        // 初始化map
                        lib.MapSet();
                        isFirst = false;
                    }
                    // String2Json
                    try
                    {
                        JObject Adofai = lib.AdofaiParse(file.ReadToEnd());
                        file.Close();
                        if (args[2] == "-Backup")
                        {
                            if (!File.Exists(@".\backup_" + Path.GetFileName(AdofaiSpectralFile)))
                            {
                                File.Copy(AdofaiSpectralFile, @".\backup_" + Path.GetFileName(AdofaiSpectralFile));
                            }
                        }
                        if (!Adofai.ContainsKey("angleData"))
                        {
                            Console.WriteLine("此铺面没有angleData类");
                            return;
                        }
                        string pathData = "";
                        string lastPath = "";
                        foreach (int a in Adofai["angleData"])
                        {
                            string path = "";
                            if (a == 360)
                            {
                                path = lib.FlipPath(lastPath);
                            }
                            else if (a < 0)
                            {
                                path = lib.Map[360 + a].ToString();
                            }
                            else
                            {
                                path = lib.Map[a].ToString();
                            }
                            lastPath = path;
                            pathData += path;
                        }
                        Adofai.Remove("angleData");
                        Adofai.Add("pathData", pathData);
                        File.WriteAllText(AdofaiSpectralFile, JsonConvert.SerializeObject(Adofai, Formatting.Indented));
                    }
                    catch
                    {
                        file.Close();
                        Console.WriteLine("发生了意料之外的错误，可能是您的谱面文件格式有误\n" +
                            "请打开json.cn网站并且将谱面文件内容复制进去进行检验\n");
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                File.WriteAllText(@".\fs.txt", e.ToString());
            }
        }
    }
}
