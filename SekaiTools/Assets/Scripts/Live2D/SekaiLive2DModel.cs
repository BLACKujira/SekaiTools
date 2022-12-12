using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Framework.Motion;
using Live2D.Cubism.Framework;
using Live2D.Cubism.Framework.HarmonicMotion;
using Live2D.Cubism.Framework.MouthMovement;
using Live2D.Cubism.Framework.Expression;
using Live2D.Cubism.Framework.Pose;
using Live2D.Cubism.Framework.MotionFade;
using Live2D.Cubism.Core;

namespace SekaiTools.Live2D
{
    /// <summary>
    /// live2d模型控制器
    /// </summary>
    public class SekaiLive2DModel : MonoBehaviour
    {
        [SerializeField] L2DAnimationSet _animationSet;
        CubismMotionController _motionController;
        AudioSource _audioSource;
        CubismModel _cubismModel;
        CubismFadeController _fadeController;
        //CubismExpressionController _expressionController;

        //同时设置CubismFadeMotionList
        public L2DAnimationSet AnimationSet
        {
            get
            {
                //if (!_animationSet) throw new Exception.AnimationSetNotSetException(name);
                return _animationSet;
            }

            set
            {
                _animationSet = value;
                if (value != null)
                {
                    FadeController.CubismFadeMotionList = _animationSet.fadeMotionList;
                }
                else
                {
                    FadeController.enabled = false;
                }

                FadeController.Refresh();
            }
        }
        public CubismFadeController FadeController
        {
            get
            {
                if (_fadeController == null) _fadeController = GetComponent<CubismFadeController>();
                return _fadeController;
            }
        }
        public CubismModel CubismModel
        {
            get
            {
                if (!_cubismModel) _cubismModel = GetComponent<CubismModel>();
                return _cubismModel;
            }
        }

        public CubismParameter ParameterEyeLOpen
        {
            get
            {
                if (!_parameterEyeLOpen) _parameterEyeLOpen = GetParameter("ParamEyeLOpen");
                return _parameterEyeLOpen;
            }
        }
        public CubismParameter ParameterEyeROpen
        {
            get
            {
                if (!_parameterEyeROpen) _parameterEyeROpen = GetParameter("ParamEyeROpen");
                return _parameterEyeROpen;
            }
        }
        public CubismParameter ParameterMouthOpenY
        {
            get
            {
                if (!_parameterMouthOpenY) _parameterMouthOpenY = GetParameter("ParamMouthOpenY");
                return _parameterMouthOpenY;
            }
        }

        private void Awake()
        {
            //_expressionController = GetComponent<CubismExpressionController>();
            _motionController = GetComponent<CubismMotionController>();
            _audioSource = GetComponent<AudioSource>();
        }

        public void Initialize()
        {
            gameObject.SetActive(false);

            if(gameObject.GetComponent<CubismUpdateController>()==null)
                gameObject.AddComponent<CubismUpdateController>();
            if (gameObject.GetComponent<CubismParameterStore>() == null)
                gameObject.AddComponent<CubismParameterStore>();
            if (gameObject.GetComponent<CubismPoseController> () == null)
                gameObject.AddComponent<CubismPoseController>();
            if (gameObject.GetComponent<CubismExpressionController>() == null)
                gameObject.AddComponent<CubismExpressionController>();

            gameObject.AddComponent<CubismEyeBlinkController>().BlendMode = CubismParameterBlendMode.Multiply;
            CubismAutoEyeBlinkInput cubismAutoEyeBlinkInput = gameObject.AddComponent<CubismAutoEyeBlinkInput>();
            cubismAutoEyeBlinkInput.MaximumDeviation = 5;
            cubismAutoEyeBlinkInput.Mean = 4;
            cubismAutoEyeBlinkInput.Timescale = 16;


            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;

            gameObject.AddComponent<CubismMouthController>().BlendMode = CubismParameterBlendMode.Override;
            CubismAudioMouthInput cubismAudioMouthInput = gameObject.AddComponent<CubismAudioMouthInput>();
            cubismAudioMouthInput.AudioInput = _audioSource;
            cubismAudioMouthInput.Smoothing = 1;
            cubismAudioMouthInput.Gain = 2.5f;

            CubismHarmonicMotionController cubismHarmonicMotionController = gameObject.AddComponent<CubismHarmonicMotionController>();

            Transform parameters = gameObject.transform.GetChild(0);
            parameters.Find("ParamEyeROpen").gameObject.AddComponent<CubismEyeBlinkParameter>();
            parameters.Find("ParamEyeLOpen").gameObject.AddComponent<CubismEyeBlinkParameter>();
            parameters.Find("ParamMouthOpenY").gameObject.AddComponent<CubismMouthParameter>();
            CubismHarmonicMotionParameter cubismHarmonicMotionParameter = parameters.Find("ParamBreath").gameObject.AddComponent<CubismHarmonicMotionParameter>();
            cubismHarmonicMotionParameter.Duration = 7;

            cubismHarmonicMotionController.Refresh();
            cubismHarmonicMotionController.ResetChannels();

            _motionController = gameObject.AddComponent<CubismMotionController>();
            _motionController.LayerCount = 2;

            if (FadeController.CubismFadeMotionList == null)
                FadeController.CubismFadeMotionList = new CubismFadeMotionList();

            gameObject.GetComponent<CubismUpdateController>().Refresh();

        }

        public void PlayVoice(AudioClip voice)
        {
            _audioSource.clip = voice;
            _audioSource.Play();
        }
        public void PlayAnimation(AnimationClip motion, AnimationClip facial, float speed = 1)
        {
            if (motion != null)
            {
                _motionController.PlayAnimation(motion, isLoop: false, layerIndex: 0,priority: 3, speed: speed);
            }

            if (facial != null)
            {
                _motionController.PlayAnimation(facial, isLoop: false, layerIndex: 1, priority: 3, speed: speed);
            }
        }
        public void PlayAnimation(string motion, string facial, float speed = 1)
        {
            AnimationClip motionAnimation = AnimationSet.GetAnimation(motion);
            AnimationClip facialAnimation = AnimationSet.GetAnimation(facial);
            PlayAnimation(motionAnimation, facialAnimation, speed);

            if (!string.IsNullOrEmpty(motion) && motionAnimation == null)
            {
                string str = motion;
                if (!string.IsNullOrEmpty(facial) && facialAnimation == null)
                    str += ' ' + facial;
                throw new Exception.AnimationNotFoundException(str);
            }
            if (!string.IsNullOrEmpty(facial) && facialAnimation == null)
                throw new Exception.AnimationNotFoundException(facial);
        }
        public void StopAllAnimation()
        {
            if(_motionController)
                _motionController.StopAllAnimation();
        }
        public void StopAutoBreathAndBlink()
        {
            GetComponent<CubismEyeBlinkController>().enabled = false;
            GetComponent<CubismHarmonicMotionController>().enabled = false;
        }

        CubismParameter _parameterEyeLOpen;
        CubismParameter _parameterEyeROpen;
        CubismParameter _parameterMouthOpenY;
        private void OnEnable()
        {
            ResetFacialParameter();
        }

        private void OnDisable()
        {
            ResetFacialParameter();
        }

        public void ResetFacialParameter()
        {
            ParameterEyeLOpen.Value = 1;
            ParameterEyeROpen.Value = 1;
            ParameterMouthOpenY.Value = 0;
        }

        public CubismParameter GetParameter(string name)
        {
            if (!CubismModel) return null;
            CubismParameter[] parameters = CubismModel.Parameters;
            foreach (var parameter in parameters)
            {
                if (parameter.name == name) return parameter;
            }
            return null;
        }

    }
}