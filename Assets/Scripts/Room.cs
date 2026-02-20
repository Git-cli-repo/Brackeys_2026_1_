using UnityEngine;


[CreateAssetMenu(fileName = "New Room", menuName = "Room")]
public class Room : ScriptableObject
{
    public int roomId;
    // barriers preventing entry to that room to toggle off on load
    // now in Barrier.cs
}
