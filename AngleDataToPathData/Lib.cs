using System;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace AngleDataToPathData
{
    public class Lib
    {
        // 定义hashmap
        public Hashtable Map = new Hashtable();
        /// <summary>
        /// 初始化哈希表
        /// </summary>
        public void MapSet()
        {
            Map.Add(0, "R");
            Map.Add(15, "p");
            Map.Add(30, "j");
            Map.Add(45, "E");
            Map.Add(60, "T");
            Map.Add(75, "o");
            Map.Add(90, "U");
            Map.Add(105, "q");
            Map.Add(120, "G");
            Map.Add(135, "Q");
            Map.Add(150, "H");
            Map.Add(165, "W");
            Map.Add(180, "L");
            Map.Add(195, "x");
            Map.Add(210, "N");
            Map.Add(225, "Z");
            Map.Add(240, "F");
            Map.Add(255, "V");
            Map.Add(270, "D");
            Map.Add(285, "Y");
            Map.Add(300, "B");
            Map.Add(315, "C");
            Map.Add(330, "M");
            Map.Add(345, "A");
            Map.Add(999, "!");
        }
        /// <summary>
        /// 转换String为Json
        /// </summary>
        /// <param name="jsonText">待转换string</param>
        /// <returns>JsonObject(NewtonSoft.json)</returns>
        public JObject AdofaiParse(string jsonText)
        {
            try
            {
                JObject jo = JObject.Parse(jsonText);
                return jo;
            }
            catch
            {
                jsonText.Trim();
                jsonText = jsonText.Replace(", ,", ",");
                jsonText = jsonText.Replace("},\n\t]", "}\n\t]");
                jsonText = jsonText.Replace(", },", " },");
                jsonText = jsonText.Replace(", }", " }");
                jsonText = jsonText.Replace(",\n,", "");
                jsonText = jsonText.Replace("]\n\t", "],\n\t");
                JObject jo = JObject.Parse(jsonText);
                return jo;
            }

        }
        /// <summary>
        /// 转换path
        /// </summary>
        /// <param name="path">带转换的path</param>
        /// <returns>转换好的path</returns>
        public string FlipPath(string path)
        {
            string r = "1";
            switch (path)
            {
                case "5":
                    r = "6";
                    break;
                case "6":
                    r = "5";
                    break;
                case "7":
                    r = "8";
                    break;
                case "8":
                    r = "7";
                    break;
                case "A":
                    r = "x";
                    break;
                case "B":
                    r = "F";
                    break;
                case "C":
                    r = "Z";
                    break;
                case "D":
                    r = "D";
                    break;
                case "E":
                    r = "Q";
                    break;
                case "F":
                    r = "B";
                    break;
                case "G":
                    r = "T";
                    break;
                case "H":
                    r = "J";
                    break;
                case "J":
                    r = "H";
                    break;
                case "L":
                    r = "R";
                    break;
                case "M":
                    r = "N";
                    break;
                case "N":
                    r = "M";
                    break;
                case "Q":
                    r = "E";
                    break;
                case "R":
                    r = "L";
                    break;
                case "T":
                    r = "G";
                    break;
                case "U":
                    r = "U";
                    break;
                case "V":
                    r = "Y";
                    break;
                case "W":
                    r = "p";
                    break;
                case "Y":
                    r = "V";
                    break;
                case "Z":
                    r = "C";
                    break;
                case "o":
                    r = "q";
                    break;
                case "p":
                    r = "W";
                    break;
                case "q":
                    r = "o";
                    break;
                case "x":
                    r = "A";
                    break;
            }
            return r;
        }
    }
}
