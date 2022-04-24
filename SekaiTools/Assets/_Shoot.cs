using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class _Shoot : MonoBehaviour
{
    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(IShoot());
        }
    }
    IEnumerator IShoot()
    {
        yield return new WaitForEndOfFrame();
        Texture2D frame = new Texture2D(Screen.width, Screen.height);
        frame.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        frame.Apply();
        byte[] png = frame.EncodeToPNG();
        string savePath = @"C:\Users\KUROKAWA_KUJIRA\Desktop\0\" + "0.png";
        File.WriteAllBytes(savePath, png);
        Debug.Log(savePath);
    }
}
