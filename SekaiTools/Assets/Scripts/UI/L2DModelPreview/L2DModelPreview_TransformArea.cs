using SekaiTools.Live2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.L2DModelPreview
{
    public class L2DModelPreview_TransformArea : MonoBehaviour
    {
        public L2DModelPreview l2DModelPreview;
        [Header("Components")]
        public InputField infPosX;
        public InputField infPosY;
        public InputField infPosScale;
        [Header("Settings")]
        public Vector2 modelOffset = new Vector2(0, -3.56f);

        public L2DControllerTypeC l2DController => l2DModelPreview.l2DController;

        public void Initialize()
        {
            ResetTransform();

            infPosX.onEndEdit.AddListener((str) =>
                {
                    float value = TryGetValue(str, 0);
                    l2DController.SetModelPosition(
                        new Vector2(value+modelOffset.x, l2DController.ModelPosition.y));
                    infPosX.text = value.ToString();
                });

            infPosY.onEndEdit.AddListener((str) =>
            {
                float value = TryGetValue(str, 0);
                l2DController.SetModelPosition(
                    new Vector2(l2DController.ModelPosition.x,value + modelOffset.y));
                infPosY.text = value.ToString();
            });

            infPosScale.onEndEdit.AddListener((str) =>
                {
                    float value = TryGetValue(str, 1);
                    l2DController.SetModelScale(value);
                    infPosScale.text = value.ToString();
                });
        }

        float TryGetValue(string input, float defaultValue)
        {
            float value;
            if (!float.TryParse(input, out value))
                return defaultValue;
            return value;
        }

        public void ResetTransform()
        {
            infPosX.text = "0";
            infPosY.text = "0";
            infPosScale.text = "1";
            l2DController.SetModelPosition(Vector2.zero + modelOffset);
            l2DController.SetModelScale(1);
        }
    }
}