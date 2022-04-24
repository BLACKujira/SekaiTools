using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.BackGround
{
    /// <summary>
    /// 改造器基类
    /// </summary>
    public abstract class BGModifierBase : MonoBehaviour
    {
        public string itemName;
        public GameObject uIPrefab;

        /// <summary>
        /// 连接改造器UI和被改造的对象
        /// </summary>
        /// <param name="modifierUI"></param>
        public virtual void Initialize(GameObject modifierUI)
        {

        }

        public abstract string Serialize();

        public abstract void Deserialize(string serializedData);
    }
}