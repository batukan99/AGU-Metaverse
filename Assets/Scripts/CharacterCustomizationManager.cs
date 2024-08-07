using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizationManager : MonoBehaviour
{
    [SerializeField] public Canvas loginCanvas;
    [SerializeField] public BodyPartHandler bodyPartHandler;
    public int headIndex = 0;
    public int bodyIndex = 0;
    public int legIndex = 0;
    public int footIndex = 0;

    public static string headData = "head";
    public static string bodyData = "body";
    public static string legData = "leg";
    public static string footData = "foot";



    public void LoadBodyParts(BodyPartHandler handler)
    {
        headIndex = PlayerPrefs.GetInt(headData, 0);
        bodyIndex = PlayerPrefs.GetInt(bodyData, 0);
        legIndex = PlayerPrefs.GetInt(legData, 0);
        footIndex = PlayerPrefs.GetInt(footData, 0);

        handler.ChangeHead(headIndex);
        handler.ChangeBody(bodyIndex);
        handler.ChangeLeg(legIndex);
        handler.ChangeFoot(footIndex);
    }
    public void SaveBodyParts()
    {
        PlayerPrefs.SetInt(headData, headIndex);
        PlayerPrefs.SetInt(bodyData, bodyIndex);
        PlayerPrefs.SetInt(legData, legIndex);
        PlayerPrefs.SetInt(footData, footIndex);
    }

    public virtual void OnApplyButtonClicked() 
    {
        SaveBodyParts();
        loginCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    private int IncreaseIndex(int index, int length) 
    {
        index += 1;
        if(index >= length)
            index = 0;
        return index;
    }
    private int DecreaseIndex(int index, int length) 
    {
        index -= 1;
        if(index < 0)
            index = length - 1;
        return index;
    }
    public virtual void OnHeadRButtonClicked() 
    {
        headIndex = IncreaseIndex(headIndex, bodyPartHandler.heads.Length);
        bodyPartHandler.ChangeHead(headIndex);
    }
    public virtual void OnBodyRButtonClicked() 
    {
        bodyIndex = IncreaseIndex(bodyIndex, bodyPartHandler.bodies.Length);
        bodyPartHandler.ChangeBody(bodyIndex);
    }
    public virtual void OnLegRButtonClicked() 
    {
        legIndex = IncreaseIndex(legIndex, bodyPartHandler.legs.Length);
        bodyPartHandler.ChangeLeg(legIndex);
    }
    public virtual void OnFootRButtonClicked() 
    {
        footIndex = IncreaseIndex(footIndex, bodyPartHandler.feet.Length);
        bodyPartHandler.ChangeFoot(footIndex);
    }

    public virtual void OnHeadLButtonClicked() 
    {
        headIndex = DecreaseIndex(headIndex, bodyPartHandler.heads.Length);
        bodyPartHandler.ChangeHead(headIndex);
    }
    public virtual void OnBodyLButtonClicked() 
    {
        bodyIndex = DecreaseIndex(bodyIndex, bodyPartHandler.bodies.Length);
        bodyPartHandler.ChangeBody(bodyIndex);
    }
    public virtual void OnLegLButtonClicked() 
    {
        legIndex = DecreaseIndex(legIndex, bodyPartHandler.legs.Length);
        bodyPartHandler.ChangeLeg(legIndex);
    }
    public virtual void OnFootLButtonClicked() 
    {
        footIndex = DecreaseIndex(footIndex, bodyPartHandler.feet.Length);
        bodyPartHandler.ChangeFoot(footIndex);
    }

}
