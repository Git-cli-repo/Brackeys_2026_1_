using UnityEngine;
using System.Collections.Generic;
using UnityEngine.U2D.Animation;

[System.Serializable]
public class DialogueRow
{
    public string talkLine;
    public Sprite talkSprite;
    public ProfileDirection profileDirection;

    public enum ProfileDirection
    {
        Left,
        Right
    }
    public DialogueRow(string str, Sprite spr, ProfileDirection profDir)
    {
        talkLine = str;
        talkSprite = spr;
        profileDirection = profDir;
    }
}

[CreateAssetMenu(fileName = "New Dialogue Script", menuName = "DialogueData")]
public class Dialogue : ScriptableObject
{
    public List<DialogueRow> dialogueRows;
}
