using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemNumber = 2;
    public float disappearTime = 10;
    public GameController gameController;

    private int itemID;

    void Awake()
    {
        itemID = Random.Range(1, itemNumber + 1);
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

            gameController.ActivateItem(itemID, ballScript.GetLastPlayerTouch());

            Destroy(gameObject);
        }
    }
}
