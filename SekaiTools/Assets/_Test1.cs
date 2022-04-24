using SekaiTools.UI.Transition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _Test1 : MonoBehaviour
{
    public Transition01 transition;
    public float waitTime;

    private void Start()
    {
        StartCoroutine(MainProcess());
    }
    IEnumerator MainProcess()
    {
        while (true)
        {
            transition.StartTransition(() => { });
            yield return new WaitForSeconds(waitTime);
        }
    }
}
