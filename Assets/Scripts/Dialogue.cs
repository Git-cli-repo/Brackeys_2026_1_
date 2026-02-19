using UnityEngine;
using System.Collections.Generic;
using UnityEngine.U2D.Animation;

[System.Serializable]
public class DialogueRow
{
    [Header("Standard Dialogue Options")]
    public string talkLine;
    public Sprite talkSprite;
    public ProfileDirection profileDirection;
    public AudioClip talkSound;

    [Header("Choice Options")]
    public bool isChoice = false;
    public ChoiceResult choiceResult;

    public enum ProfileDirection
    {
        Left,
        Right
    }
    public DialogueRow(string str, Sprite spr, ProfileDirection profDir, AudioClip acer, ChoiceResult cr, bool hcr)
    {
        talkLine = str;
        talkSprite = spr;
        profileDirection = profDir;
        talkSound = acer;
        isChoice = hcr;
        choiceResult = cr;
    }
}

[CreateAssetMenu(fileName = "New Dialogue Script", menuName = "DialogueData")]
public class Dialogue : ScriptableObject
{
    public List<DialogueRow> dialogueRows;
}

public class DialoguePlay : MonoBehaviour
{
    // useless!!     or....
}
