using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerGlass : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        string tag = collision.gameObject.tag ;
        if (tag == "Win")
        {
            Debug.Log("win..");
        }
        else if (tag == "Lose")

        {
            Debug.Log("lose..");

        }
    }
}
