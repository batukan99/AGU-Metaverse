using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EmoteWheel : MonoBehaviour //, IPointerEnterHandler, IPointerExitHandler
 {
    [SerializeField] private EmoteSlice[] slices; // 0 1
                                                  // 3 2
    public string currentEmote = "";

    // Update is called once per frame
    void Update()
    {
        currentEmote = "";
        foreach (EmoteSlice slice in slices)
        {
            if (slice.hoveredOn) {
                currentEmote = slice.emote;
                return;
            }
        }
    }
}
