using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private int sceneIndex;
    private Vector3 movementPerFrame = new Vector3(1, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        textBubble.SetActive(false);
       for (int i = 0; i < objectsToAppearAfterTextMessage.Length; i++)
       {
           objectsToAppearAfterTextMessage[i].SetActive(false);
       }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            AdvanceScene();
        }

    }
    [SerializeField] private Phone iPhone;

    [SerializeField] private TextEffect textBubbleText;
    [SerializeField] private GameObject textBubble;

    [SerializeField] private GameObject[] objectsToAppearAfterTextMessage;

    [SerializeField] private Animator fadeMaskAnimator;
    [SerializeField] private Animator bubbleAnimator;

    private void AdvanceScene()
    {
        sceneIndex++;

        if (sceneIndex == 1)
        {
            iPhone.GoToCamera();
        }

        else if (sceneIndex == 2)
        {
            textBubble.SetActive(true);
            bubbleAnimator.SetTrigger("In");
        }
        else if (sceneIndex == 3)
        {
            if (textBubbleText.enabled)
            {
                textBubbleText.Print();
            }
            else
            {
                AdvanceScene();
            }
        }
        else  if (sceneIndex == 4)
        {
            for (int i = 0; i < objectsToAppearAfterTextMessage.Length; i++)
            {
                objectsToAppearAfterTextMessage[i].SetActive(true);
            }
            fadeMaskAnimator.SetTrigger("Fade");
        }
    }




}
