using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Hitbox : MonoBehaviour
{
    public bool detectedObject;
    public string tagToDetect;
    public List<GameObject> detectedObjects;
    public Collider2D goCollider;

    void Start()
    {
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == tagToDetect)
        {
            detectedObject = true;
            detectedObjects.Add(collision.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == tagToDetect)
        {
            detectedObject = false;
            detectedObjects.Remove(collision.gameObject);
        }
    }
}
