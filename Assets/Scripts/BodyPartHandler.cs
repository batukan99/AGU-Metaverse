using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartHandler : MonoBehaviour
{
    [SerializeField] public SkinnedMeshRenderer originalHead;
    [SerializeField] public SkinnedMeshRenderer originalBody;
    [SerializeField] public SkinnedMeshRenderer originalLeg;
    [SerializeField] public SkinnedMeshRenderer originalFeet;
    [SerializeField] public SkinnedMeshRenderer[] heads;
    [SerializeField] public SkinnedMeshRenderer[] bodies;
    [SerializeField] public SkinnedMeshRenderer[] legs;
    [SerializeField] public SkinnedMeshRenderer[] feet;
    [SerializeField] public Transform rootBone;

    public void ChangeHead(int index)
    {
        SkinnedMeshRenderer newHead = heads[index];
        if(newHead == originalHead)
            return;
        newHead.bones = originalHead.bones;
        newHead.rootBone = rootBone;
        newHead.gameObject.SetActive(true);
        originalHead.gameObject.SetActive(false);
        originalHead = newHead;
    }

    public void ChangeBody(int index)
    {
        SkinnedMeshRenderer newBody = bodies[index];
        if(newBody == originalBody)
            return;
        newBody.bones = originalBody.bones;
        newBody.rootBone = rootBone;
        newBody.gameObject.SetActive(true);
        originalBody.gameObject.SetActive(false);
        originalBody = newBody;
    }
    public void ChangeLeg(int index)
    {
        SkinnedMeshRenderer newLeg = legs[index];
        if(newLeg == originalLeg)
            return;
        newLeg.bones = originalLeg.bones;
        newLeg.rootBone = rootBone;
        newLeg.gameObject.SetActive(true);
        originalLeg.gameObject.SetActive(false);
        originalLeg = newLeg;
    }
    public void ChangeFoot(int index)
    {
        SkinnedMeshRenderer newFoot = feet[index];
        if(newFoot == originalFeet)
            return;
        newFoot.bones = originalFeet.bones;
        newFoot.rootBone = rootBone;
        newFoot.gameObject.SetActive(true);
        originalFeet.gameObject.SetActive(false);
        originalFeet = newFoot;
    }

}
