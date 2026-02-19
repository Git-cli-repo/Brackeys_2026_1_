using UnityEngine;


[CreateAssetMenu(fileName = "New Room", menuName = "Room File")]
public class Room : ScriptableObject
{
    public GameObject room;
    public int roomId;
    //barriers preventing entry to that room to toggle off on load
}
