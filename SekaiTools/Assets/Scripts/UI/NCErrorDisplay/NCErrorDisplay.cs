using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.UI.NCErrorDisplay
{
    public class NCErrorDisplay : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public UniversalGenerator2D universalGenerator;

        public void Initialize(params NCError[] nCErrors)
        {
            universalGenerator.Generate(nCErrors.Length,
                (gobj, id) =>
                {
                    NCErrorDisplay_Item nCErrorDisplay_Item = gobj.GetComponent<NCErrorDisplay_Item>();
                    nCErrorDisplay_Item.Initialize(nCErrors[id]);
                });
        }
    }
}