using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI
{
    /// <summary>
    /// 生成一列按钮，选择后关闭窗口，null按钮的值为-1
    /// </summary>
    public class UniversalSelector : MonoBehaviour
    {
        public Window window;
        [SerializeField] Text title;
        public ButtonGeneratorBase buttonGenerator;
        public bool generateNullButton = false;

        public string Title { get => title.text; set => title.text = value; }

        public void Generate(int count, Action<Button, int> initialize, Action<int> onClick)
        {
            if(generateNullButton)
                buttonGenerator.Generate(count+1,
                    (btn, id) =>
                    {
                        if(id>0)
                            initialize(btn, id - 1);
                    }, 
                    (int id)=>
                    { 
                        onClick(id-1);
                        window.Close();
                    });
            else
                buttonGenerator.Generate(count, initialize, (int id) => { onClick(id); window.Close(); });
        }
    }
}