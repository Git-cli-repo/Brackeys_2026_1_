using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TMP_Text diaLine;
    public CanvasGroup dialogueCanvas;
    public Image imgLeft;
    public Image imgRight;
    public TMP_Text choiceRight;
    public TMP_Text choiceLeft;
    public bool inDialogueMode = false;
    public Dialogue dialogue;
    public InputActionReference dialogueKeyReference;
    public InputActionReference moveKeyReference;
    public AudioSource diaSoundPlayer;
    public AudioClip diaSoundClip;
    public int currentLineIndex = 0;
    public int lineCount = 0;
    public List<DialogueRow> lines;
    public float iTimePassed = 0.1f;
    public float tSpeed = 0.01f;
    public bool inputPressed; 
    public bool isPlaying;
    public bool dialogueStarted = false;
    public bool choiceMode = false;
    public string choice1 = "";
    public string choice2 = "";
    public int currentChoice;
    public Dictionary<Chest, bool> chestIndex = new Dictionary<Chest, bool>(); // TODO: Add chest indexing via FindObjectsOfType -> ChestContainer
    public List<Item> inventoryCopy;
    public GameObject abbey;
    public Dictionary<int, int> choiceValueStorage = new Dictionary<int, int>();
    public List<GameObject> roomsList = new List<GameObject>();
    public List<GameObject> barrierList = new List<GameObject>();
    public Dictionary<ChoiceResult, bool> hasAlreadyUsed = new Dictionary<ChoiceResult, bool>();
    public DMat dMat;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        dialogueCanvas.alpha = 0;
        abbey = GameObject.FindFirstObjectByType<PlayerMovement>().gameObject;
        inventoryCopy = GameObject.FindFirstObjectByType<InventoryManager>().inventory;

        // this logic lives in start temporarily - need a more reliable hook 
        // for example, OnActiveSceneChanged()

        // Initialize lists + search
        roomsList = GameObject.FindObjectsByType<RoomContainer>(FindObjectsSortMode.None).Select(p => p.gameObject).ToList();
        barrierList = GameObject.FindObjectsByType<BarrierContainer>(FindObjectsSortMode.None).Select(p => p.gameObject).ToList();

        // Disable 
        foreach(RoomContainer rm in roomsList.Select(p => p.GetComponent<RoomContainer>()))
        {
            rm.isEnabled = false;
            rm.gameObject.SetActive(false);
        }

        // Enable
        foreach(BarrierContainer bc in barrierList.Select(p => p.GetComponent<BarrierContainer>()))
        {
            bc.enabled = true;
            bc.gameObject.SetActive(true);
        }
    }

    public IEnumerator FadeIn()
    {
        float timePassed = 0;
        while(dialogueCanvas.alpha < 1){
            timePassed += Time.deltaTime;
            dialogueCanvas.alpha = Mathf.Lerp(0, 1, timePassed / iTimePassed);
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        float timePassed = 0;
        while(dialogueCanvas.alpha > 0){
            timePassed += Time.deltaTime;
            dialogueCanvas.alpha = Mathf.Lerp(1, 0, timePassed / iTimePassed);
            yield return null;
        }      
    }

    public IEnumerator TypewriterEffect()
    {
        isPlaying = true;
        while(diaLine.maxVisibleCharacters < diaLine.text.Length)
        {
            diaLine.maxVisibleCharacters++;
            List<string> forbiddenChars = new List<string>{" ", ",", ".", "!", "?", "\"", "'", "$", "-", "_", ":"};
            if(!forbiddenChars.Contains(diaLine.text[diaLine.maxVisibleCharacters - 1].ToString())) diaSoundPlayer.PlayOneShot(diaSoundClip);
            yield return new WaitForSeconds(tSpeed);
        } 
        
        if(diaLine.maxVisibleCharacters >= diaLine.text.Length){
            isPlaying = false; 
            yield return null;
        }

    }

    public IEnumerator DisplayDialogue()
    {
        dialogueCanvas.alpha = 0;
        diaLine.maxVisibleCharacters = 0;
        currentLineIndex = 0;
        yield return StartCoroutine(FadeIn());
        for (currentLineIndex = 0; currentLineIndex < lines.Count;)
        {
            DialogueRow dia = lines[currentLineIndex];
            inputPressed = false;
            currentLineIndex++;
            diaLine.text = dia.talkLine;
            diaLine.maxVisibleCharacters = 0;

            choiceMode = dia.isChoice;
            choiceLeft.text = "";
            choiceRight.text = "";

            if(dia.profileDirection == DialogueRow.ProfileDirection.Left && !choiceMode)
            {
                imgLeft.gameObject.SetActive(true);
                imgLeft.sprite = dia.talkSprite;
                imgRight.gameObject.SetActive(false);
            } else if (!choiceMode)
            {
                imgRight.gameObject.SetActive(true);
                imgRight.sprite = dia.talkSprite;
                imgLeft.gameObject.SetActive(false);
            } else
            {
                imgLeft.gameObject.SetActive(false);
                imgRight.gameObject.SetActive(false);
                diaLine.text = "";
                choiceLeft.text = dia.choiceResult.option1;
                choiceRight.text = dia.choiceResult.option2;
                currentChoice = 1;
                choiceLeft.color = Color.yellow;
                choiceRight.color = Color.white;
            }

            diaSoundClip = dia.talkSound;

            if(!choiceMode) yield return StartCoroutine(TypewriterEffect());
            yield return new WaitUntil(() => inputPressed);
        }

        if (currentLineIndex == lines.Count)
        {
            yield return StartCoroutine(FadeOut());
            inDialogueMode = false;
            dialogueStarted = false;
        }
    }

    public IEnumerator ButtonPressLogic()
    {
        inputPressed = false;

        if (choiceMode)
        {
            if(!dMat.alreadyTalked){   
                ChoiceResult choice = lines[currentLineIndex - 1].choiceResult;
                switch (choice.choiceType1)
                {
                    case ChoiceResult.ChoiceType.Message:
                        if(currentChoice == 1)
                        {
                            lines.AddRange(choice.message1);

                        } 
                        hasAlreadyUsed.Add(choice, true);
                        break;
                    case ChoiceResult.ChoiceType.Item:
                        if(currentChoice == 1)
                        {
                            inventoryCopy.Add(choice.item1);

                        }
                        hasAlreadyUsed.Add(choice, true);
                        break;
                    case ChoiceResult.ChoiceType.TakeItem:
                        if(currentChoice == 1)
                        {
                            inventoryCopy.Remove(choice.item1);
                            // other logic goes here
                        }
                        break;
                }

                switch (choice.choiceType2)
                {
                    case ChoiceResult.ChoiceType.Message:
                        if(currentChoice == 2)
                        {
                            lines.AddRange(choice.message2);
                        } 
                        hasAlreadyUsed.Add(choice, true);
                        break;
                    case ChoiceResult.ChoiceType.Item:
                        if(currentChoice == 2)
                        {
                            inventoryCopy.Add(choice.item2);

                        }
                        hasAlreadyUsed.Add(choice, true);
                        break;
                    case ChoiceResult.ChoiceType.TakeItem:
                        if(currentChoice == 2)
                        {
                            inventoryCopy.Remove(choice.item2);
                            // other logic here
                        }
                        break;
                }

                dMat.alreadyTalked = true;
                dMat.choiceChosen = currentChoice;
            }
            if (dMat.alreadyTalked)
            {
                ChoiceResult choice = lines[currentLineIndex - 1].choiceResult;
                if(dMat.choiceChosen == 1) lines.AddRange(choice.messageIfAlreadyGotten1);
                if(dMat.choiceChosen == 2) lines.AddRange(choice.messageIfAlreadyGotten2);
            }
        }

        if (isPlaying)
        {
            diaLine.maxVisibleCharacters = diaLine.text.Length;
        } else if (diaLine.maxVisibleCharacters == diaLine.text.Length)
        {
            inputPressed = true;
        }
        yield return null;
    }

    void Update()
    {
        if(!dialogueStarted){
            lineCount = dialogue.dialogueRows.Count;
            lines = dialogue.dialogueRows;
            dialogueStarted = true;
            StartCoroutine(DisplayDialogue());
        }

        if(!inDialogueMode && abbey.GetComponent<PlayerMovement>())
        {
            // chest logic
        }
        
        if (dialogueKeyReference.action.WasPressedThisFrame())
        {
            StartCoroutine(ButtonPressLogic());
        }

        if(moveKeyReference.action.ReadValue<Vector2>()[0] != 0 && choiceMode)
        {
            float directionalPress = moveKeyReference.action.ReadValue<Vector2>()[0];
            if(directionalPress > 0)
            {
                choiceRight.color = Color.yellow;
                choiceLeft.color = Color.white;
                currentChoice = 2;
            } else if(directionalPress < 0)
            {
                choiceLeft.color = Color.yellow;
                choiceRight.color = Color.white;
                currentChoice = 1;
            }
        }
    }

    public void UseRoom(Item item)
    {
        if(item.objectType != ItemType.Room) throw new ArgumentException($"UseRoom() cannot accept Items of type {item.objectType}");
        RoomContainer rc = roomsList.Where(p => p.GetComponent<RoomContainer>().room.roomId == item.roomID).Select(p => p.GetComponent<RoomContainer>()).ToList()[0];
        rc.isEnabled = true;
        rc.gameObject.SetActive(true);
        BarrierContainer bc = barrierList.Where(p => p.GetComponent<BarrierContainer>().barrier.roomId == item.roomID).Select(p => p.GetComponent<BarrierContainer>()).ToList()[0];
        bc.enabled = false;
        bc.gameObject.SetActive(false);
    }
}