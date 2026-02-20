using UnityEngine;


[CreateAssetMenu(fileName = "New Room", menuName = "Barrier")]
public class Barrier : ScriptableObject
{
    public int roomId;
    // barriers preventing entry to that room to toggle off on load
}
