using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportsGameManager : MonoBehaviour
{

    void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        StartCoroutine(StartScene());
    }

    [SerializeField] private float sportsItemsInterval = 0.25f;

    private IEnumerator StartScene()
    {
        HoldableItem[] sportsItems = FindObjectsOfType<HoldableItem>();
        HoldableItem[] sortedSportsItems = new HoldableItem[sportsItems.Length];

        for (int i = 0; i < sportsItems.Length; i++)
        {
            HoldableItem item = sportsItems[i];
            item.rigidbody.isKinematic = true;
            sortedSportsItems[item.priority] = item;
        }

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < sortedSportsItems.Length; i++)
        {
            sortedSportsItems[i].rigidbody.isKinematic = false;
            yield return new WaitForSeconds(sportsItemsInterval);

        }
    }
}
