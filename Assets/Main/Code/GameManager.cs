using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SportsItemSpotLight sportsItemSpotLightPreFab;

    void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        StartCoroutine(StartScene());
    }

    [SerializeField] private float projectorTimeInterval = 0.25f;
    [SerializeField] private float projectorSoundDelay = 0.2f;
    [SerializeField] private AudioSource projectorSound;

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
            StartCoroutine(InitialiseSportsItem(sortedSportsItems[i]));
            yield return new WaitForSeconds(projectorTimeInterval);

        }
    }

    private IEnumerator InitialiseSportsItem(HoldableItem item)
    {
        Instantiate(projectorSound);
        yield return new WaitForSeconds(projectorSoundDelay);
        SportsItemSpotLight spotLight = Instantiate(sportsItemSpotLightPreFab);
        spotLight.target = item.transform;
        spotLight.GoHome();
        item.rigidbody.isKinematic = false;
    }
}
