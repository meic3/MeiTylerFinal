using UnityEngine;
using System.Collections.Generic;

public class EnemyInRangeHelper : MonoBehaviour
{
    CircleCollider2D col;
    public List<GameObject> bugsInRange = new List<GameObject>();

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
    }

    public void SetRadius(float r)
    { col.radius = r; }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("bug"))
        {
            bugsInRange.Add(col.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("bug"))
        {
            bugsInRange.Remove(col.gameObject);
        }
    }
}
