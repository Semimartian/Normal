using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldableItem : MonoBehaviour
{
    public Rigidbody rigidbody;
    [SerializeField] private Transform centreOfMass;
    public int priority;

    private void Start()
    {
        if(rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
            
        }
        if(centreOfMass != null)
        {
            rigidbody.centerOfMass = centreOfMass.localPosition;
        }

    }
}
