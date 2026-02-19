using UnityEngine;
using System.Collections.Generic;
using UnityEngine.U2D.Animation;

[System.Serializable]
public class DialogueRow
{
    public string talkLine;
    public Sprite talkSprite;
    public ProfileDirection profileDirection;
    public AudioClip talkSound;

    public enum ProfileDirection
    {
        Left,
        Right
    }
    public DialogueRow(string str, Sprite spr, ProfileDirection profDir, AudioClip acer)
    {
        talkLine = str;
        talkSprite = spr;
        profileDirection = profDir;
        talkSound = acer;
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
