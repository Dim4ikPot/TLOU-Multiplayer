using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinSelector : MonoBehaviour
{
    public GameObject skinSelectors;
    public static GameObject gameModel;
    //int[] skinsIndexes = new int[skinsCount];

    public static int skinsCount;
    public static bool isMale;

    void Start()
    {
        gameModel = GameObject.Find("SkinPreview");
        /*for (int i = 0; i < skinsCount; i++)
        {
            skinsIndexes[i] = i;
        }*/
    }


    public void ChangeSkin(int index)
    {
        foreach(Transform skin in gameModel.transform)
        {
            if (skin.name != "Root")
                skin.gameObject.SetActive(false);
        }

        gameModel.transform.GetChild(index).gameObject.SetActive(true);
    }

    public void ChangeSex(int index)
    {
        isMale = index == 0;
    }
}
