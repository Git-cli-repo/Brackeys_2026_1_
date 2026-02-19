using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TMP_Text diaLine;
    public CanvasGroup dialogueCanvas;
    public Image imgLeft;
    public Image imgRight;
    public bool inDialogueMode = false;
    public Dialogue dialogue;
    public InputActionReference dialogueKeyReference;
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
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        dialogueCanvas.alpha = 0;
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
        foreach (DialogueRow dia in lines)
        {
            inputPressed = false;
            currentLineIndex++;
            diaLine.text = dia.talkLine;
            diaLine.maxVisibleCharacters = 0;

            if(dia.profileDirection == DialogueRow.ProfileDirection.Left)
            {
                imgLeft.gameObject.SetActive(true);
                imgLeft.sprite = dia.talkSprite;
                imgRight.gameObject.SetActive(false);
            } else
            {
                imgRight.gameObject.SetActive(true);
                imgRight.sprite = dia.talkSprite;
                imgLeft.gameObject.SetActive(false);
            }

            diaSoundClip = dia.talkSound;

            yield return StartCoroutine(TypewriterEffect());
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
        if (inDialogueMode)
        {
            if(!dialogueStarted){
                lineCount = dialogue.dialogueRows.Count;
                lines = dialogue.dialogueRows;
                dialogueStarted = true;
                StartCoroutine(DisplayDialogue());
            }
        }

        if (dialogueKeyReference.action.WasPressedThisFrame())
        {
            StartCoroutine(ButtonPressLogic());
        }
    }
}