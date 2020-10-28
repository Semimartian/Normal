using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportsItemSpotLight : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float height;
    private Transform myTransform;

    private void Start()
    {
        myTransform = transform;
    }

    void Update()
    {
        Vector3 newPosition = target.position;
        newPosition.y = height;
        myTransform.position = newPosition;
    }
}
