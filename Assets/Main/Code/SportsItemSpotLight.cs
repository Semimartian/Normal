using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportsItemSpotLight : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float height;
    private Transform myTransform;

    void Update()
    {
        GoHome();
    }

    private void Awake()
    {
        myTransform = transform;
    }

    public void GoHome()
    {
        Vector3 newPosition = target.position;
        newPosition.y = height;
        myTransform.position = newPosition;
    }
}
