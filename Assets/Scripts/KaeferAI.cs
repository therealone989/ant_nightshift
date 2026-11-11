using System;
using UnityEngine;

public class KaeferAI : MonoBehaviour
{

    public LayerMask playerMask;
    public float range =5f;
    void Start()
    {
        
    }

    void Update()
    {

        // Verfolgen
        // Aufladen
        // Sturmangriff - collider? - Schnell?
        // Erschöpft - keine bewegung - zeit zum zubeißen


        Collider2D playerCol = Physics2D.OverlapCircle(transform.position, range, playerMask);

        if (playerCol != null)
        {
            Debug.Log("LELEL " + playerCol.gameObject.name);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("GATCHAAAAA");
        }
    }


}
