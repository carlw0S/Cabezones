using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemNumber = 2;
    public float disappearTime = 10f;

    private int itemID;

    void Awake()
    {
        itemID = Random.Range(1, itemNumber + 1);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        switch (itemID)
        {
            case 1:
            case 3:
                sr.color = Color.green;
                break;
            case 2:
            case 4:
                sr.color = Color.red;
                break;
            default:
                sr.color = Color.yellow;
                break;
        }
    }

    void Start()
    {
        Destroy(gameObject, disappearTime);
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject GO = col.gameObject;
        if (GO.CompareTag("Ball"))
        {
            Ball ballScript = GO.GetComponent<Ball>();

            GameController.ActivateItem(itemID, ballScript.GetLastPlayerTouch());

            Destroy(gameObject);
        }
    }
}
