using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.SpineAniGIFGenerator
{
    public class SpineAniGIFGenerator_CaptureArea : MonoBehaviour
    {
        public SpineAniGIFGenerator spineAniGIFGenerator;
        public RectTransform captureBox => spineAniGIFGenerator.captureBox;

        [Header("position")]
        public InputField positionXInputField;
        public InputField positionYInputField;
        public Vector2Int defaultPosition = Vector2Int.zero;
        public Vector2Int Position
        {
            get
            {
                int x, y;
                if(!int.TryParse(positionXInputField.text, out x)) x = defaultPosition.x;
                if(!int.TryParse(positionYInputField.text, out y)) y = defaultPosition.y;
                return new Vector2Int(x,y);
            }
        }

        public void ResetPosition()
        {
            positionXInputField.text = defaultPosition.x.ToString();
            positionYInputField.text = defaultPosition.y.ToString();
        }

        void InitializePosition()
        {
            ResetPosition();
            positionXInputField.onValueChanged.AddListener((string str) => { ApplyPosition(); });
            positionYInputField.onValueChanged.AddListener((string str) => { ApplyPosition(); });
            ApplyPosition();
        }

        void ApplyPosition()
        {
            captureBox.anchoredPosition = Position;
        }

        [Header("size")]
        public InputField sizeXInputField;
        public InputField sizeYInputField;
        public Vector2Int defaultSize = new Vector2Int(176,254);
        public Vector2Int Size
        {
            get
            {
                int x, y;
                if(!int.TryParse(sizeXInputField.text,out x)) x = defaultSize.x;
                if(!int.TryParse(sizeYInputField.text,out y)) y = defaultSize.y;
                return new Vector2Int(
                    string.IsNullOrEmpty(sizeXInputField.text) ? defaultSize.x : int.Parse(sizeXInputField.text),
                    string.IsNullOrEmpty(sizeYInputField.text) ? defaultSize.y : int.Parse(sizeYInputField.text)
                    );
            }
        }

        public void ResetSize()
        {
            sizeXInputField.text = defaultSize.x.ToString();
            sizeYInputField.text = defaultSize.y.ToString();
        }

        void InitializeSize()
        {
            ResetSize();
            sizeXInputField.onValueChanged.AddListener((string str) => { ApplySize(); });
            sizeYInputField.onValueChanged.AddListener((string str) => { ApplySize(); });
            ApplySize();
        }

        void ApplySize()
        {
            captureBox.sizeDelta = Size;
        }

        [Header("frame")]
        public InputField frameInputField;
        public float defaultFrame = 60f;
        public float Frame => string.IsNullOrEmpty(frameInputField.text) ? defaultFrame : float.Parse(frameInputField.text);

        public void ResetFrame()
        {
            frameInputField.text = defaultFrame.ToString();
        }

        void InitializeFrame()
        {
            ResetFrame();
        }

        private void Awake()
        {
            InitializePosition();
            InitializeSize();
            InitializeFrame();
        }
    }
}