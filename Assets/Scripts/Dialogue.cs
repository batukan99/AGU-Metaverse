using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    [System.Serializable]
    public class DialogueOption
    {
        public string dialogue;
        public string optionText; // The text to display on the button
        public bool teleportable;
        public Transform teleport;
        public DialogueOption[] nextOptions; // The next set of dialogue options
    }
    public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public InteractableNPC npc;
    public Button optionButtonPrefab; 
    public Transform optionsParent; 
    
    private GameObject player;
    private void Start()
    {
        //DisplayOptions(options);
    }

    public void CloseDialogueScreen()
    {
        gameObject.SetActive(false);
    }
    public void StartDialogue(DialogueOption[] options, string firstDialogue, InteractableNPC npc)
    {
        dialogueText.text = firstDialogue;
        this.npc = npc;
        DisplayOptions(options);
    }
    private void DisplayOptions(DialogueOption[] options)
    {

        foreach (Transform child in optionsParent)
        {
            Destroy(child.gameObject);
        }

        var yPos = 0;
        foreach (DialogueOption option in options)
        {
            Button button = Instantiate(optionButtonPrefab, optionsParent);
            button.GetComponentInChildren<TextMeshProUGUI>().text = option.optionText;
            
            if(option.teleportable)
                button.onClick.AddListener(() => SelectTeleportOption(option.teleport));
            else
                button.onClick.AddListener(() => SelectOption(option));
            button.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, yPos);
            yPos -= 125;
        }
    }
    void SelectOption(DialogueOption option)
    {
        dialogueText.text = option.dialogue;
        DisplayOptions(option.nextOptions);
    }

    void SelectTeleportOption(Transform to)
    {
        npc.StartCoroutine("Teleport",to);
    }
}
