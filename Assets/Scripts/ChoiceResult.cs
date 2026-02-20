
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Choice Result", menuName = "ChoiceResult")]
public class ChoiceResult : ScriptableObject
{
    public enum ChoiceType
    {
        Message, 
        Item,
        SetValue,
        TakeItem
    }

    public ChoiceType choiceType1;
    public ChoiceType choiceType2;
    public string option1;
    public string option2;
    public List<DialogueRow> message1;
    public List<DialogueRow> message2;
    public Item item1;
    public Item item2;
    public List<DialogueRow> messageIfAlreadyGotten1;
    public List<DialogueRow> messageIfAlreadyGotten2;
}