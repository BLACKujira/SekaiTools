using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineSceneEditor
{
    public class SpineSceneEditor_Main_EditArea_PageTransform : SpineSceneEditor_Main_EditArea_Page
    {
        public InputField inputFieldPositionX;
        public InputField inputFieldPositionY;
        public InputField inputFieldRotation;
        public InputField inputFieldScaleX;
        public InputField inputFieldScaleY;
        public Button buttonFlip;

        public void RefreshValues()
        {
            Vector3 position = modelPair.Model.transform.position;
            inputFieldPositionX.text = position.x.ToString();
            inputFieldPositionY.text = position.y.ToString();
            inputFieldRotation.text = modelPair.Model.transform.rotation.eulerAngles.z.ToString();
            Vector3 localScale = modelPair.Model.transform.localScale;
            inputFieldScaleX.text = localScale.x.ToString();
            inputFieldScaleY.text = localScale.y.ToString();
        }

        private void Awake()
        {
            inputFieldPositionX.onEndEdit.AddListener((value) =>
            {
                Vector3 position = modelPair.Model.transform.position;
                position.x = TryGetValue(value, 0);
                modelPair.Model.transform.position = position;
                spineSceneEditor.SaveChanges();
            });

            inputFieldPositionY.onEndEdit.AddListener((value) =>
            {
                Vector3 position = modelPair.Model.transform.position;
                position.y = TryGetValue(value, 0);
                modelPair.Model.transform.position = position;
                spineSceneEditor.SaveChanges();
            });

            inputFieldRotation.onEndEdit.AddListener((value) =>
            {
                modelPair.Model.transform.rotation = Quaternion.Euler(0,0, TryGetValue(value, 0));
                spineSceneEditor.SaveChanges();
            });

            inputFieldScaleX.onEndEdit.AddListener((value) =>
            {
                Vector3 scale = modelPair.Model.transform.localScale;
                scale.x = TryGetValue(value, 0);
                modelPair.Model.transform.localScale = scale;
                spineSceneEditor.SaveChanges();
            });

            inputFieldScaleY.onEndEdit.AddListener((value) =>
            {
                Vector3 scale = modelPair.Model.transform.localScale;
                scale.y = TryGetValue(value, 0);
                modelPair.Model.transform.localScale = scale;
                spineSceneEditor.SaveChanges();
            });

            buttonFlip.onClick.AddListener(() =>
            {
                modelPair.SetFlip(!modelPair.IfFlip);
                spineSceneEditor.SaveChanges();
            });
        }

        float TryGetValue(string input,float defaultValue)
        {
            float value;
            if (!float.TryParse(input, out value))
                return defaultValue;
            return value;
        }

        public override void Refresh()
        {
            RefreshValues();
        }

        protected override void Interactive()
        {
            inputFieldPositionX.interactable = true;
            inputFieldPositionY.interactable = true;
            inputFieldRotation.interactable = true;
            inputFieldScaleX.interactable = true;
            inputFieldScaleY.interactable = true;
            buttonFlip.interactable = true;
        }

        protected override void NonInteractive()
        {
            inputFieldPositionX.interactable = false;
            inputFieldPositionY.interactable = false;
            inputFieldRotation.interactable = false;
            inputFieldScaleX.interactable = false;
            inputFieldScaleY.interactable = false;
            buttonFlip.interactable = false;

            string zero = "0";
            inputFieldPositionX.text = zero;
            inputFieldPositionY.text = zero;
            inputFieldRotation.text = zero;
            inputFieldScaleX.text = zero;
            inputFieldScaleY.text = zero;
        }
    }
}