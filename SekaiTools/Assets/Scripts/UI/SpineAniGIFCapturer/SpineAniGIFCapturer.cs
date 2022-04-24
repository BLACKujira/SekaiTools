using Moments;
using Moments.Encoder;
using SekaiTools.Spine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SekaiTools.UI.SpineAniGIFCapturer
{
    public class SpineAniGIFCapturer : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public PerecntBar perecntBar;
        public RectTransform captureBox;
        public Canvas targetCanvas;
        [Header("Settings")]
        public RenderTexture spineRenderTexture;

        public enum CaptureMode { Screen,RenderTexture }
        public enum EncodeMode { png,gif }

        [System.NonSerialized] public SpineControllerTypeA spineController;
        [System.NonSerialized] public CaptureMode captureMode;
        [System.NonSerialized] public EncodeMode encodeMode;
        [System.NonSerialized] public Vector2Int position;
        [System.NonSerialized] public Vector2Int size;
        [System.NonSerialized] public string saveFilePath;
        [System.NonSerialized] public float frameRate;

        RenderTexture lastActive;

        public void Initialize(SpineAniGIFCapturerSettings settings)
        {
            lastActive = RenderTexture.active;

            spineController = settings.spineController;
            captureMode = settings.captureMode;
            encodeMode = settings.encodeMode;
            position = settings.position;
            size = settings.size;
            saveFilePath = settings.saveFilePath;
            frameRate = settings.frameRate;

            captureBox.anchoredPosition = position;
            captureBox.sizeDelta = size;
        }

        private void OnDestroy()
        {
            RenderTexture.active = lastActive;
        }

        public class SpineAniGIFCapturerSettings
        {
            public SpineControllerTypeA spineController;
            public CaptureMode captureMode;
            public EncodeMode encodeMode;
            public Vector2Int position;
            public Vector2Int size;
            public string saveFilePath;
            public float frameRate;
        }

        public void StartCapture()
        {
            StartCoroutine(IStartCapture());
        }
        IEnumerator IStartCapture()
        {

            List<global::Spine.TrackEntry> tracks = new List<global::Spine.TrackEntry>();
            foreach (var modelPair in spineController.models)
            {
                global::Spine.TrackEntry trackEntry = modelPair.Model.AnimationState.SetAnimation(0, modelPair.Model.AnimationName, true);
                tracks.Add(trackEntry);
            }
            double length = tracks[0].AnimationEnd;
            double delta = 1f / frameRate;
            List<Texture2D> frames = new List<Texture2D>();

            foreach (var trackEntry in tracks)
            {
                trackEntry.TrackTime = 0;
            }
            yield return new WaitForSeconds((float)length);

            for (double i = 0; i < length; i+=delta)
            {
                foreach (var trackEntry in tracks)
                {
                    trackEntry.TrackTime = (float)i;
                }
                yield return new WaitForEndOfFrame();
                frames.Add(captureMode == CaptureMode.Screen ? CaptureScreen() : CaptureRenderTexture());
                perecntBar.priority = (float)(i / length);
            }

            if (encodeMode == EncodeMode.png)
            {
                SaveToPNG(frames.ToArray());
            }
            else
            {
                SaveToGIF(frames.ToArray());
            }

            window.Close();

        }

        public Texture2D CaptureScreen()
        {
            return ExtensionTools.Capture(captureBox, targetCanvas);
        }

        public Texture2D CaptureRenderTexture()
        {
            float startX = spineRenderTexture.width/2;
            float startY = spineRenderTexture.height/2;
            startX += position.x;
            startY += position.y;
            startX -= size.x / 2;
            startY -= size.y / 2;

            RenderTexture.active = spineRenderTexture;
            Texture2D texture2D = new Texture2D(size.x, size.y, TextureFormat.RGBA32,false);
            texture2D.ReadPixels(new Rect(startX, startY, size.x, size.y), 0, 0);

            return texture2D;
        }
     
        public void SaveToPNG(Texture2D[] frames)
        {
            for (int i = 0; i < frames.Length; i++)
            {
                Texture2D texture2D = frames[i];
                string file = Path.GetDirectoryName(saveFilePath);
                file = Path.Combine(file, $"{Path.GetFileNameWithoutExtension(saveFilePath)}_{i}{Path.GetExtension(saveFilePath)}");
                File.WriteAllBytes(file, texture2D.EncodeToPNG());
            }
        }
        
        public void SaveToGIF(Texture2D[] frames)
        {
            List<GifFrame> gifFrames = new List<GifFrame>();
            foreach (var texture2D in frames)
            {
                GifFrame frame = new GifFrame() { Width = size.x, Height = size.y, Data = texture2D.GetPixels32() }; ;
                gifFrames.Add(frame);
            }
            GifEncoder encoder = new GifEncoder(0, 100);
            encoder.SetDelay(Mathf.RoundToInt(1000f/frameRate));
            Worker worker = new Worker(System.Threading.ThreadPriority.Normal)
            {
                m_Encoder = encoder,
                m_Frames = gifFrames,
                m_FilePath = saveFilePath,
                m_OnFileSaved = null,
                m_OnFileSaveProgress = null
            };
            worker.Start();
        }

    }
}