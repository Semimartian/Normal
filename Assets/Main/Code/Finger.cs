using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finger : MonoBehaviour
{
    private List<Vector3> positions = new List<Vector3>();
    private Vector3 previousPosition;
    [SerializeField] private Camera camera;

    [SerializeField] private Transform fingerTransform;
    [SerializeField] private GameObject fingerWhileTouching;
    [SerializeField] private GameObject fingerWhileHovering;

    public static Finger instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Tried to instantiate more than one fINGER!");
            return;
        }
    }

    private Vector3 MouseToGroundPlane(Vector3 mousePosition)
    {
        //TODO: learn from this
        Ray ray = camera.ScreenPointToRay(mousePosition);
        float rayLength = ray.origin.y / ray.direction.y;
        Vector3 result = ray.origin - (ray.direction * rayLength);
        // Debug.Log("MouseToGroundPlane: " + result);
        return result;
    }

    private void Start()
    {

        fingerWhileHovering.SetActive(true);
        fingerWhileTouching.SetActive(false);

        Cursor.visible = false;
    }

    private void Update()
    {

        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
        {
   
            fingerWhileHovering.SetActive(true);
            fingerWhileTouching.SetActive(false);
        }
        else if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0))
        {
            fingerWhileHovering.SetActive(false);
            fingerWhileTouching.SetActive(true);
        }

      //  Vector3 currentMouseGroundPosition = MouseToGroundPlane(Input.mousePosition);
            Vector3 fingerPosition = /*camera.WorldToScreenPoint*/(Input.mousePosition);
            fingerTransform.position = fingerPosition;
    }


   /* private void FixedUpdate()
    {
        Vector3 currentMouseGroundPosition = MouseToGroundPlane(Input.mousePosition);

        /*Vector3 fingerPosition = camera.WorldToScreenPoint(currentMouseGroundPosition);
        fingerTransform.position = fingerPosition;*
    }   */
}
