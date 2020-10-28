using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private HoldableItem myHeldItem = null;
    [SerializeField] private float itemHeightWhileBeingHeld = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(myHeldItem == null)
            {
                Vector2 mousePosition = Input.mousePosition;
                Ray ray = camera.ScreenPointToRay(mousePosition);
                RaycastHit raycastHit;
                Physics.Raycast(ray, out raycastHit);
                if (raycastHit.collider != null)
                {
                    Debug.Log(raycastHit.collider.gameObject.name);
                    HoldableItem item = (raycastHit.collider.gameObject.GetComponent<HoldableItem>());
                    if (item == null)
                    {
                        item = (raycastHit.collider.gameObject.GetComponentInParent<HoldableItem>());
                    }
                    if (item != null)
                    {
                        GrabItem(item);
                    }
                }
            }
    
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseItem();
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        // mousePosition.z = 1;// camera.transform.position.y;
        mousePosition = MouseToGroundPlane(mousePosition);
       // mousePosition = camera.ScreenToWorldPoint(mousePosition);
        mousePosition.y = itemHeightWhileBeingHeld;
        return mousePosition;
    }


    [SerializeField] private float groundY = 1;
    [SerializeField] private Transform mouseRayMarker;
    private Vector3 MouseToGroundPlane(Vector3 mousePosition)
    {
        Ray ray = camera.ScreenPointToRay(mousePosition);
        float rayLength = (ray.origin.y - groundY) / ray.direction.y;

        Debug.DrawLine(ray.origin, ray.origin-( ray.direction* rayLength), Color.red);

        Vector3 results = ray.origin - (ray.direction * rayLength);
        mouseRayMarker.position = results;
        return ray.origin - (ray.direction * rayLength);
    }

    //[SerializeField] float MouseZ;
    private void FixedUpdate()
    {
        if (myHeldItem != null)
        {        
            myHeldItem.rigidbody.position = GetMouseWorldPosition();
        }

    }

    private void GrabItem(HoldableItem item)
    {
        StartCoroutine(GrabItemCoroutine(item));
    }

    private IEnumerator GrabItemCoroutine(HoldableItem item)
    {

        item.rigidbody.isKinematic = true;
        float addition = 2.2f;
        while (true)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector3 newItemPosition =
                Vector3.MoveTowards(item.rigidbody.position, GetMouseWorldPosition(), addition * Time.deltaTime);
            item.rigidbody.position = newItemPosition;
            if(Vector3.Distance (mousePosition, newItemPosition) < 0.01f)
            {
                break;
            }
            yield return null;
        }

        myHeldItem = item;
    }
    private void ReleaseItem()
    {
        myHeldItem.rigidbody.isKinematic = false;
        myHeldItem = null;
    }
}
