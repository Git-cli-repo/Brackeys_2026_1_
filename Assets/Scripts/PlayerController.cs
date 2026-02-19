using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float baseOffense;
    public float baseDefense;
    public float maxHp;
    public float attackFrequency;

    public float offense;
    public float defense;
    public float hp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offense = baseOffense;
        defense = baseDefense;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
