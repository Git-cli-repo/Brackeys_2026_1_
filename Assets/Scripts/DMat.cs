using UnityEngine;

public class DMat : MonoBehaviour
{
    public int _iDialogueGeneratorID;
    public Dialogue dialogue;
    public bool alreadyTalked = false;
    public int choiceChosen;
    public readonly string type = "IDialogueGenerator";
}