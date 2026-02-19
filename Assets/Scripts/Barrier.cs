using UnityEngine;


[CreateAssetMenu(fileName = "New Room", menuName = "Room File")]
public class Barrier : ScriptableObject
{
    public int roomId;
    // barriers preventing entry to that room to toggle off on load
}
