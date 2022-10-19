using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace SekaiTools
{
    public static class MessagePackConverter
    {
        public static string ToJSONAssetList(byte[] msgPack)
        {
            MessagePackSerializerOptions options = MessagePackSerializerOptions.Standard;
            string json = MessagePackSerializer.ConvertToJson(msgPack, options);
            json = ModifyAssetListJSON(json);
            return json;
        }

        public static string ModifyAssetListJSON(string json)
        {
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

            json = json.Substring(0, startPos - 1) + '[' + string.Join(",", bundlesArray) + ']' + json.Substring(endPos + 2);
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
}