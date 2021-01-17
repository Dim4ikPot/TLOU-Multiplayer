using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterObjects : MonoBehaviour
{
    //public GameObject myPlayer;
    public Rig weaponDefault;
    public Rig weaponAiming;
    public Rig bodyAiming;
    public Rig handsIK;
    public MultiAimConstraint multiAimConstraint;
    public TwoBoneIKConstraint rightHandIK;


    void Start()
    {
        WeightedTransformArray weightedTransforms = new WeightedTransformArray();
        weightedTransforms.Add(new WeightedTransform(Camera.main.transform.Find("AimingLookAt"), 1));
        multiAimConstraint.data.sourceObjects = weightedTransforms;
        rightHandIK = handsIK.transform.Find("RightHandIK").GetComponent<TwoBoneIKConstraint>();
        //logs.text = "Player ID : " + myPlayer.GetInstanceID();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
