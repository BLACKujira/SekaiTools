using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace SekaiTools.UI.NicknameCountShowcase
{
    public abstract class NCSScene_CountMT_Item : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform targetRectTransform;
        public Image ImgCharIcon;
        public Graphic[] graCharColor;
        [Header("Settings")]
        public IconSet charIconSet;
        public float distance = 20;
        public float distanceFromZeroScale = 1f;
        public float circleMoveRadius = 5f;
        public float circleMoveSpeed = 5f;

        Graphic[] graphics = new Graphic[0];
        float[] graphicsAlpha = new float[0];

        float alpha = 1;
        public float Alpha
        {
            get => alpha;
            set
            {
                for (int i = 0; i < graphics.Length; i++)
                {
                    Color color = graphics[i].color;
                    color.a = alpha * graphicsAlpha[i];
                    graphics[i].color = color;
                }
            }
        }

        public float Radius => targetRectTransform.sizeDelta.x / 2;
        public Vector2 Position { get; set; }

        public float circleMoveAngle;

        private void Awake()
        {
            graphics = GetComponentsInChildren<Graphic>();
            graphicsAlpha = new float[graphics.Length];
            for (int i = 0; i < graphics.Length; i++)
            {
                graphicsAlpha[i] = graphics[i].color.a;
            }
            circleMoveAngle = Random.Range(0, 360f);
        }

        private void Update()
        {
            Vector2 circleMoveVector = MathTools.AngleToRadiusOne(circleMoveAngle);
            circleMoveVector *= circleMoveRadius;
            targetRectTransform.anchoredPosition = Position + circleMoveVector;

            circleMoveAngle += circleMoveSpeed * Time.deltaTime;
        }

        public virtual void SetData(int talkerId, int count, int characterId)
        {
            foreach (var graphic in graCharColor)
            {
                Color oldColor = graphic.color;
                Color newColor = ConstData.characters[talkerId].imageColor;
                newColor.a = oldColor.a;
                graphic.color = newColor;
            }
            ImgCharIcon.sprite = charIconSet.icons[talkerId];
        }

        //物理模拟，废弃
        //public Vector2 GetForce(NCSScene_CountMT_Item other)
        //{
        //    float zeroDistance = Radius + distance;
        //    float itemDistance = Vector2.Distance(targetRectTransform.anchoredPosition, other.targetRectTransform.anchoredPosition);

        //    float distanceFromZero = itemDistance - zeroDistance;

        //    float forceMul;
        //    if (distanceFromZero > 0)
        //    {
        //        distanceFromZero *= distanceFromZeroScale;
        //        distanceFromZero = Mathf.Clamp(distanceFromZero, 0, Mathf.PI);
        //        forceMul = Mathf.Sin(distanceFromZero);
        //    }
        //    else
        //    {
        //        forceMul = -repulsiveForce;
        //    }

        //    Vector2 direction = other.targetRectTransform.anchoredPosition - targetRectTransform.anchoredPosition;
        //    direction.Normalize();

        //    return direction * forceMul;
        //}
    }
}