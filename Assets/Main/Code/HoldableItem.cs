using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SurfaceImpactSound
{
    public string physicMatName;
    public AudioClip[] clips;
}

public class HoldableItem : MonoBehaviour
{
    public Rigidbody rigidbody;
    [SerializeField] private Transform centreOfMass;
    public int priority;

    [SerializeField] private SurfaceImpactSound[] surfaceImpactSounds;
    [SerializeField] private AudioSource audioSource;

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

    [SerializeField] private Vector3 releaseTorque;
    public void Torque()
    {
        rigidbody.AddRelativeTorque(releaseTorque, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {

        //Debug.Log("impact" + impact);

        string matName = collision.collider.material.name;

        //Debug.Log(matName);

        AudioClip clip = null;
        for (int i = 0; i < surfaceImpactSounds.Length; i++)
        {
            if (matName==(surfaceImpactSounds[i].physicMatName))
            {
                //Debug.Log(surfaceImpactSounds[i].physicMat.ToString());
                int clipIndex = Random.Range(0, surfaceImpactSounds[i].clips.Length);
                clip =  surfaceImpactSounds[i].clips[clipIndex];
                break;
            }
        }

        if(clip != null)
        {
            float impact = collision.relativeVelocity.magnitude;
            float volume = Mathf.Lerp(0, 1, impact * 0.25f);
            //Debug.Log("volume: " +volume);
            audioSource.clip = clip;

            audioSource.pitch = Random.Range(0.96f,1.04f);


            audioSource.volume = volume;

            audioSource.Play();
        }
    }

}
