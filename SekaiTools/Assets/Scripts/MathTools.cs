using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SekaiTools
{
    public class MathTools : MonoBehaviour
    {
        public static int[] GetRandomArray(int size)
        {
            //使用洗牌算法返回一个乱序数组
            int[] arr = new int[size];
            int rdn, temp;
            for (int i = 0; i < size; i++)
                arr[i] = i;
            for (int i = 0; i < size; i++)
            {
                rdn = Random.Range(0, size - 1 - i);
                temp = arr[rdn];
                arr[rdn] = arr[size - 1 - i];
                arr[size - 1 - i] = temp;
            }
            return arr;
        }

        public static float RadianToAngle(float pi) { return 360 * pi / 2; }
        public static float AngleToRadian(float angle) { return angle / 360 * 2; }
        public static Vector2 RadianToRadiusOne(float pi)
        {
            float sin = (float)Math.Sin(Math.PI * pi);
            float cos = (float)Math.Cos(Math.PI * pi);
            return new Vector2(cos, sin);
        }
        public static Vector2 AngleToRadiusOne(float angle)
        {
            return RadianToRadiusOne(AngleToRadian(angle));
        }
        public static float RadiusOneToRadian(Vector2 RadiusOne)
        {
            if (RadiusOne.y >= 0)
                return (float)(Math.Acos(RadiusOne.x) / Math.PI);
            else
                return 2 - (float)(Math.Acos(RadiusOne.x) / Math.PI);
        }
        public static float RadiusOneToAngle(Vector2 RadiusOne)
        {
            return RadianToAngle(RadiusOneToRadian(RadiusOne));
        }

        /// <summary>
        ///获取方向随机，长度为radius的二维向量
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Vector2 GetRandomVector2_Circle(float radius)
        {
            return RadianToRadiusOne(UnityEngine.Random.Range(0f, 2f)) * radius;
        }
    }
}