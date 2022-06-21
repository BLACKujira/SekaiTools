using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SekaiTools.Effect
{
    public class ObjectMoveRepeat : MonoBehaviour
    {
        public Transform[] moveTransforms;
        public float moveDistance = 5;
        public float moveAngle = 0;
        public float moveSpeed = 1;
        public float startOffset = 0;

        float offset = 0;
        private void Update()
        {
            Vector2 vector2 = MathTools.AngleToRadiusOne(moveAngle);
            foreach (var transform in moveTransforms)
            {
                transform.localPosition = (Vector3)vector2 * (startOffset + offset);
            }
            offset += Time.deltaTime * moveSpeed;
            if (offset >= moveDistance) offset -= moveDistance;
        }
    }
}