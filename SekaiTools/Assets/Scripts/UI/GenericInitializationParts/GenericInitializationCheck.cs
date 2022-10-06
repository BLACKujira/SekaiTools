using SekaiTools.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.GenericInitializationParts
{
    public static class GenericInitializationCheck
    {
        public const string STR_ERROR = Error.STR_ERROR;

        public static string CheckIfReady(params IGenericInitializationPart[] genericInitializationParts)
        {
            List<string> errors = new List<string>();
            foreach (var genericInitializationPart in genericInitializationParts)
            {
                string error = genericInitializationPart.CheckIfReady();
                if (!string.IsNullOrEmpty(error))
                    errors.Add(error);
            }

            if (errors.Count > 0)
            {
                return string.Join("\n\n", errors);
            }
            else
                return null;
        }

        public static string GetErrorString(string errorType,params string[] errors)
        {
            return GetErrorString(errorType, new List<string>(errors));
        }

        public static string GetErrorString(string errorType,List<string> errors)
        {
            if (errors.Count > 0)
            {
                errors.Insert(0, $"{errorType}:");
                return string.Join("\n", errors);
            }
            else
                return null;
        }
    }
}