using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public List<Item> inventory;
    public List<ChestContainer> chestContainers;
    public List<DMat> npcContainers;
    public List<RoomContainer> roomContainers;
    public List<BarrierContainer> barrierContainers;
    // public List<EnemyContainer> enemies;
    public Transform playerTransform;
}