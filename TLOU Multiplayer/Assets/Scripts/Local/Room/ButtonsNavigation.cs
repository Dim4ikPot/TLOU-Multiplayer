using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ButtonsNavigation : MonoBehaviour
{
    public GameObject main;
    public GameObject characterCustomization;
    public GameObject gearAndPerks;

    void Start()
    {
        main.SetActive(true);
        characterCustomization.SetActive(false);
        gearAndPerks.SetActive(false);
    }

    public void CharacterCustomize()
    {
        SkinSelector.gameModel.transform.DOLocalMoveX(-0.7f, 1f);
        main.SetActive(false);
        characterCustomization.SetActive(true);
    }

    public void AcceptCharacter()
    {
        SkinSelector.gameModel.transform.DOLocalMoveX(-2f, 1f);
        main.SetActive(true);
        characterCustomization.SetActive(false);
    }

    public void GearSetUp()
    {
        main.SetActive(false);
        gearAndPerks.SetActive(true);
    }

    public void GearSetDone()
    {
        main.SetActive(true);
        gearAndPerks.SetActive(false);
    }
}
