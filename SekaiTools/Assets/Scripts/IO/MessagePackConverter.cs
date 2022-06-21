using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace SekaiTools.IO
{
    public static class MessagePackConverter
    {
        public static string ToJSON(byte[] msgPack)
        {
            MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard;
            string json = MessagePackSerializer.ConvertToJson(msgPack, options);
            int startPos = json.IndexOf("\"bundles\"");
            startPos = json.IndexOf('{', startPos);
            int endPos = FindNextCurlyBracket(json, startPos);
            startPos++;
            endPos--;
            string subStr = json.Substring(startPos, endPos - startPos);
            List<string> bundlesArray = new List<string>();
            for (int i = 0; i < subStr.Length;)
            {
                int start = subStr.IndexOf('{', i);
                int end = FindNextCurlyBracket(subStr, start);
                if (end == subStr.Length) break;
                end++;
                bundlesArray.Add(subStr.Substring(start, end - start));
                i = end;
            }

            json = json.Substring(0, startPos-1) + '[' + string.Join(",", bundlesArray) + ']' + json.Substring(endPos+2);
            return json;
        }

        static int FindNextCurlyBracket(string str,int bracketIndex)
        {
            int curlyCount = 0;
            bracketIndex++;
            while (bracketIndex<str.Length) 
            {
                char c = str[bracketIndex];
                if (c == '}')
                {
                    if (curlyCount == 0)
                        break;
                    else
                        curlyCount--;
                }
                else if (str[bracketIndex] == '{')
                    curlyCount++;

                bracketIndex++;
            }
            return bracketIndex;
        }
    }

    [System.Serializable]
    [MessagePackObject]
    public class BundlesItem
    {
        [Key(0)] public string bundleName;
        [Key(1)] public string cacheFileName;
        [Key(2)] public string cacheDirectoryName;
        [Key(3)] public string hash;
        [Key(4)] public string category;
        [Key(5)] public int crc;
        [Key(6)] public int fileSize;
        [Key(7)] public List<string> dependencies;
        [Key(8)] public List<string> paths;
        [Key(9)] public string isBuiltin;
    }
    [System.Serializable]
    [MessagePackObject]
    public class BundleRoot
    {

        [Key(0)] public string version;
        [Key(1)] public string os;
        [Key(2)] public List<BundlesItem> bundles;

    }
}