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

    private float projectorTimeInterval = 0.25f;
    private float projectorSoundDelay = 0.1f;
    [SerializeField] private AudioSource projectorSound;

    [SerializeField] private TextEffect text;
    private IEnumerator StartScene()
    {
        text.Print();
        HoldableItem[] sportsItems = FindObjectsOfType<HoldableItem>();
        HoldableItem[] sortedSportsItems = new HoldableItem[sportsItems.Length];

        for (int i = 0; i < sportsItems.Length; i++)
        {
            HoldableItem item = sportsItems[i];
            item.rigidbody.isKinematic = true;
            sortedSportsItems[item.priority] = item;
        }

        for (int i = 0; i < sortedSportsItems.Length; i++)
        {
            yield return new WaitForSeconds(projectorTimeInterval);
            Instantiate(projectorSound);
           // projectorSound.Play();
            yield return new WaitForSeconds(projectorSoundDelay);

            SportsItemSpotLight spotLight = Instantiate(sportsItemSpotLightPreFab);
            spotLight.target = sortedSportsItems[i].transform;
            sortedSportsItems[i].rigidbody.isKinematic = false;
        }
    }
}
