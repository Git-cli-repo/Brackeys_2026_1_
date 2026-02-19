
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "New Choice Result", menuName = "ChoiceResult")]
public class ChoiceResult : ScriptableObject
{
    public enum ChoiceType
    {
        Message, 
        Item,
        SetValue
    }

    public ChoiceType choiceType;
    public string option1;
    public string option2;
    public DialogueRow message1;
    public DialogueRow message2;
    public Item item1;
    public Item item2;
    public DialogueRow messageIfAlreadyGotten;
}