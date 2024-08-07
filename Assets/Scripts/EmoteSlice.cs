using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EmoteSlice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
 {
    public string emote = "";
    public bool hoveredOn = false;

    Color32 neutral = new Color32(66, 66, 66, 200);
    Color32 selected =  new Color32(198, 40, 40, 255);
    Image img;

    // Start is called before the first frame update
    void Start()
    {
        img = gameObject.GetComponent<Image>();
        img.alphaHitTestMinimumThreshold = 0.00001f; 
    }

    /* Update is called once per frame
    void Update()  {}
    */
    
    public void OnEnable() {
        if (hoveredOn == true) {
            hoveredOn = false;
            img.color = neutral;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
     {  
        hoveredOn = true;
        img.color = selected;
     }
     public void OnPointerExit(PointerEventData eventData)
     {
         hoveredOn = false;
         img.color = neutral;
     }     
}
