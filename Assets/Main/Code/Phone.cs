using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField] private Transform target;
   /* [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;*/
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        targetPosition = target.position;
        originalRotation = transform.rotation;
        targetRotation = target.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GoToCamera()
    {
        StartCoroutine(GoToCameraIEnumerator());
    }

    [SerializeField]  private AnimationCurve curve;

    private IEnumerator GoToCameraIEnumerator()
    {

        float time = 0;
        float endTime = curve.keys[curve.length - 1].time;
        Transform myTransform = transform;
        while( time < endTime)
        {
            //float deltaTime = Time.deltaTime;
            time += Time.deltaTime;

            float t = curve.Evaluate(time);

            myTransform.position = Vector3.Lerp(originalPosition, targetPosition, t);
            //  Vector3.MoveTowards(transform.position, target.position, speed * deltaTime);
            myTransform.rotation = Quaternion.Lerp(originalRotation, targetRotation,t);
                //Quaternion.RotateTowards(transform.rotation, target.rotation, rotationSpeed * deltaTime);

            yield return null;

        }


        myTransform.position = targetPosition;
        myTransform.rotation = targetRotation;
    }


}
