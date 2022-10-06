using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools
{
    public static class CSVTools
    {
        public static string[][] LoadCSV(string text, string separatorValue, string separatorRow)
        {
            string[] separatorValueArray = new string[] { separatorValue };
            string[] separatorRowArray = new string[] { separatorRow };

            string[] rowsRaw = text.Split(separatorRowArray, StringSplitOptions.None);
            string[][] rows = new string[rowsRaw.Length][];
            for (int i = 0; i < rowsRaw.Length; i++)
            {
                rows[i] = rowsRaw[i].Split(separatorValueArray,StringSplitOptions.None);
            }
            return rows;
        }

        public static string[][] LoadCSV(string text)
        {
            return LoadCSV(text, ",", Environment.NewLine);
        }

    }
}