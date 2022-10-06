using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.GenericInitializationParts
{
    public interface IGenericInitializationPart
    {
        /// <summary>
        /// 无问题时返回null,否则返回对问题的描述
        /// 对问题的描述为如下格式
        /// 错误类型：
        /// 错误1
        /// 错误2
        /// ...
        /// </summary>
        /// <returns></returns>
        string CheckIfReady();
    }
}