using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockBase : MonoBehaviour
{
    [SerializeField] private  int numberOfActualNumbers = 60;
    [SerializeField] private int numberOfWheelNumbers = 20;

    private float angleIncrement;
    private const string MOUSE_Y_AXIS = "Mouse Y";

    [Header("Text Properties:")]
    [SerializeField] private RotatableNumber rotatableNumberPreFab;
    [SerializeField] private float numberDistanceFromBase = 1;
    [SerializeField] private float textSize = 1f;
    private RotatableNumber[] rotatableNumbers;
    private Transform myTransform;

    private float currentWheelVelocity;
    [Header("Wheel Velocity:")]
    [SerializeField] private float mouseVelocityMultiplier = 10;
    [SerializeField] private float wheelVelocityLossPerPosition;
    [SerializeField] private float minWheelVelocity;
    private int currentWheelPosition;
    [SerializeField] private Bounds bounds;
    private bool mouseIsLockedOnMe;//TODO: Mouse should be a seperate class that we clocks check 
    private void Start()
    {
        //bounds = boundsCollider.bounds;
        angleIncrement = 360 / numberOfWheelNumbers;
        //ACTUAL_ANGLE_INCREMENT = 360 / numberOfActualNumbers;
        myTransform = this.transform;
        SpawnNumbers();
        UpdateTexts();
    }

    private string[] actualNumbersText;
    [SerializeField] private AudioSource audioSource;
    private void SpawnNumbers()
    {
        actualNumbersText = new string[numberOfActualNumbers];

        for (int i = 0; i < numberOfActualNumbers; i++)
        {
            int numberNum = Mathf.Abs(numberOfActualNumbers - i) - 1;

            string s =
/* number.textMesh.text = */((numberNum > 9) ? numberNum.ToString() : "0" + numberNum.ToString());
            //Debug.Log("i = " + i + "number = " + s);

            actualNumbersText[i] = s;
        }

        rotatableNumbers = new  RotatableNumber[numberOfWheelNumbers];

        for (int i = 0; i < numberOfWheelNumbers; i++)
        {
            RotatableNumber number = Instantiate(rotatableNumberPreFab);
            Transform numberTransform = number.transform;
            numberTransform.position = this.transform.position;

            numberTransform.rotation = Quaternion.Euler((i+1) * angleIncrement, 0, 0);//TODO:cheap fix might damage stuff
            numberTransform.position -= (numberTransform.forward)* numberDistanceFromBase;
            numberTransform.SetParent( myTransform);

           // (numberNum > 9) ? numberNum.ToString() : "0" + numberNum.ToString();
            number.textMesh.fontSize = textSize;
            rotatableNumbers[i] = number;
        }
      
    }

    private int actualIndex = 0;
    private int wheelIndex = 0;

    private void UpdateTexts()
    {
        bool testColours = false;
        if (testColours)
        {
            for (int i = 0; i < numberOfWheelNumbers; i++)
            {
                RotatableNumber rotatableNumber = rotatableNumbers[i];
                rotatableNumber.textMesh.color = Color.white;

            }
        }


        for (int i = -5; i < 6 ; i++)//TODO: beutify
        {
            int myActualNumberIndex = actualIndex + i;
            if (myActualNumberIndex >= numberOfActualNumbers)
            {
                myActualNumberIndex -= numberOfActualNumbers; //-1+i;
            }
            else if (myActualNumberIndex < 0)
            {
                myActualNumberIndex += numberOfActualNumbers;
            }
            int myWheelNumberIndex = wheelIndex + i;
            if (myWheelNumberIndex >= numberOfWheelNumbers)
            {
                // myWheelNumberIndex = 0;
                myWheelNumberIndex -= numberOfWheelNumbers;// -1 + i;

            }
            else if (myWheelNumberIndex < 0)
            {
                myWheelNumberIndex += (numberOfWheelNumbers) ;
            }
           // int absI = Math.Abs(i);

            RotatableNumber rotatableNumber = rotatableNumbers[myWheelNumberIndex];
            rotatableNumber.textMesh.text = actualNumbersText[myActualNumberIndex];

            if (testColours)
            {
                Color colour = Color.white;
                switch (i)
                {

                    case -2:
                        colour = Color.blue; break;
                    case -1:
                        colour = Color.magenta; break;
                    case 0:
                        colour = Color.red; break;
                    case 1:
                        colour = Color.yellow; break;
                    case 2:
                        colour = Color.green; break;
                }
                rotatableNumber.textMesh.color = colour;
            }
        }

    }

    //private float[] mouseVelocities = new float[4];
    float mouseVelocity;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateTexts();
        }

        float deltaTime = Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.C))
        {
            CorrectAngle();
        }


        if (!mouseIsLockedOnMe)
        {
            if (Input.GetMouseButtonDown(0))
            {
               
                Vector3 mousePosition = /*Camera.main.ScreenToWorldPoint*/(Input.mousePosition);
                mousePosition.x /= (float)Screen.width;
                mousePosition.y /= (float)Screen.height;
                if (bounds.Contains(mousePosition))
                {
                    mouseIsLockedOnMe = true;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouseIsLockedOnMe = false ;
            mouseVelocity = 0;
        }


       // Debug.Log("mousePosition"+ mousePosition);
        if (mouseIsLockedOnMe)
        {
            float mouseYMovement =
                (Input.GetAxis(MOUSE_Y_AXIS) * mouseVelocityMultiplier ) ;//TODOD: DeltaTime?

            bool isNegative = mouseYMovement < 0;
            mouseYMovement *= mouseYMovement;
            if (isNegative)
            {
                mouseYMovement = -mouseYMovement;
            }
            /*if (mouseVelocity != 0)
            {
                mouseVelocity = (mouseVelocity + mouseYMovement) / 2;
            }
            else
            {
                mouseVelocity =  mouseYMovement;
            }*/
            mouseVelocity = mouseYMovement;
            if (Math.Abs( mouseYMovement) > 0)
            {
                currentWheelVelocity = mouseVelocity * deltaTime;
            }
           
        }
        else
        {

            if (currentWheelVelocity > 0 && currentWheelVelocity< minWheelVelocity)
            {
                currentWheelVelocity = minWheelVelocity;
            }
            else if (currentWheelVelocity < 0 && currentWheelVelocity > -minWheelVelocity)
            {
                currentWheelVelocity = -minWheelVelocity;
            }
            else
            {
                currentWheelVelocity = Mathf.RoundToInt(currentWheelVelocity / minWheelVelocity) * minWheelVelocity;
            }
        }

        if (currentWheelVelocity != 0)
        {
           // Debug.Log("currentWheelVelocity:" + currentWheelVelocity);

            Vector3 rotation = new Vector3(currentWheelVelocity * deltaTime, 0, 0);
            myTransform.Rotate(rotation);
            CheckNewPosition();
        }

        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Vector3 rotation = new Vector3(angleIncrement * 2, 0, 0);
                myTransform.Rotate(rotation);
                CheckNewPosition();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Vector3 rotation = new Vector3(-angleIncrement * 2, 0, 0);
                myTransform.Rotate(rotation);
                CheckNewPosition();
            }
        }
       
        /* else if(currentWheelVelocity !=0)
         {
             float absoluteWheelVelocity = Math.Abs(currentWheelVelocity);
             if (absoluteWheelVelocity < 1)
             {
                 CorrectAngle();
                 currentWheelVelocity = 0;

             }
             else
             {
                 float velocityLoss = velocityLossPerSecond * deltaTime;
                 if (absoluteWheelVelocity - velocityLoss < 0)
                 {
                     CorrectAngle();
                     currentWheelVelocity = 0;
                 }
                 else
                 {
                     float multiplier = (currentWheelVelocity > 0 ? 1 : -1);
                     currentWheelVelocity -= (velocityLoss * multiplier);
                 }
             }

         }*/
    }

    private void CorrectAngle()
    {

        float myAngle = getCurrentAngle();

        Debug.Log("myAngle:" + myAngle);
        float normalisedAngle = (Mathf.Round ( myAngle / angleIncrement)) * angleIncrement;
        Debug.Log("normalisedAngle:" + normalisedAngle);

        myTransform.rotation = Quaternion.Euler(normalisedAngle, 0,0);

    }

    private float getCurrentAngle()
    {
        //float myAngle = Vector3.ang(myTransform.forward, Vector3.forward);
        // float myAngle = myTransform.eulerAngles.x;
        float myAngle = Vector3.SignedAngle(myTransform.forward, Vector3.forward, Vector3.left);
        if (myAngle < 0)
        {
            myAngle += 360;
        }
        return myAngle;
    }

    private void CheckNewPosition()
    {
        float myAngle = getCurrentAngle();

        float normalisedCurrentAngle = myAngle / angleIncrement;
        int newPosition = (int)(Mathf.Round(normalisedCurrentAngle));

        if(newPosition >= numberOfWheelNumbers)
        {
            Debug.LogWarning("New Position = " + newPosition);
            newPosition -= numberOfWheelNumbers * (newPosition/numberOfWheelNumbers );
            Debug.LogWarning("Corrected Position = " + newPosition);
        }

        if (newPosition != currentWheelPosition)
        {

            int difference = newPosition - currentWheelPosition;

            int AbsDifference = Math.Abs(difference);
            float physicalDifference = Math.Abs(normalisedCurrentAngle - currentWheelPosition);
            if(physicalDifference > numberOfWheelNumbers / 2)
            {
                physicalDifference = (Math.Abs(physicalDifference - numberOfWheelNumbers));
            }
           if ((AbsDifference > numberOfWheelNumbers / 2) && (AbsDifference - numberOfWheelNumbers < physicalDifference))
            {
                Debug.Log("AbsDifference: " + AbsDifference);
                Debug.Log("physicalDifference: " + physicalDifference);

            }
            bool passedNewNumber =
                (AbsDifference < physicalDifference) || 
                ((AbsDifference > numberOfWheelNumbers / 2) && (Math.Abs(AbsDifference - numberOfWheelNumbers) < physicalDifference));
            if (passedNewNumber)
            {
                audioSource.Play();
                if (AbsDifference > numberOfWheelNumbers / 2)
                {
                    Debug.LogWarning("AbsDifference > numberOfWheelNumbers / 2");
                    if (difference < 0)
                    {
                        difference = numberOfWheelNumbers + difference;
                        //Debug.Log("difference < 0");

                    }
                    else if (difference > 0)
                    {
                        difference = difference - numberOfWheelNumbers;//TODO: aint there a simpler rule for this?
                      //  Debug.Log("difference > 0");

                    }
                }
               // Debug.Log("corrected difference: " + difference);


                wheelIndex += -difference;
                if (wheelIndex >= numberOfWheelNumbers)
                {
                    wheelIndex -= numberOfWheelNumbers;
                }
                else if (wheelIndex < 0)
                {
                    wheelIndex += numberOfWheelNumbers;
                }
                actualIndex += -difference;
                if (actualIndex >= numberOfActualNumbers)
                {
                    actualIndex -= numberOfActualNumbers;
                }
                else if (actualIndex < 0)
                {
                    actualIndex += numberOfActualNumbers;
                }

                UpdateTexts();

                currentWheelPosition = newPosition;
                Debug.Log(
               "actualIndex: " + actualIndex +
               " wheelIndex: " + wheelIndex +
               " currentWheelPosition: " + currentWheelPosition);

                 float absWheelVelocity = Math.Abs(currentWheelVelocity);
                 float velocityLoss = wheelVelocityLossPerPosition * AbsDifference;
                 if (absWheelVelocity - velocityLoss <= 0)
                 {
                    Debug.Log("currentWheelVelocity = 0");
                    currentWheelVelocity = 0;
                    //SNAP
                    float snap = newPosition * angleIncrement;
                    myTransform.rotation = Quaternion.Euler(snap, 0, 0);
                 }
                 else
                 {
                     currentWheelVelocity += velocityLoss * (currentWheelVelocity < 0 ? 1 : -1);
                 }

            }

            /* {
                 currentWheelVelocity = 0;
                 currentWheelVelocity -= (wheelVelocityLossPerPosition * difference);
             }*/

            return;

            if (AbsDifference > 1 && AbsDifference != numberOfWheelNumbers-1)
            {
                Debug.LogError(newPosition +"-"+ currentWheelPosition);

                Debug.LogError("difference = " + AbsDifference);
            }
            //float physicalDifference = Math.Abs(normalisedCurrentAngle - currentWheelPosition);

            float destinationAngle = newPosition * angleIncrement;
            if (AbsDifference < physicalDifference || (AbsDifference == numberOfWheelNumbers - 1 && AbsDifference > physicalDifference))
            {
                int change = 0;

                if (currentWheelPosition == 0 && newPosition == numberOfWheelNumbers - 1)
                {
                    change = 1;
                }
                else if(currentWheelPosition == numberOfWheelNumbers - 1 && newPosition == 0)
                {
                    change = -1;                  
                }
                else  if (currentWheelPosition > newPosition )
                {
                    change = 1;
                }
                else if (currentWheelPosition < newPosition )
                {
                    change = -1;
                }

                switch (change)
                {
                    case 1:
                    {
                        wheelIndex++;
                        if (wheelIndex >= numberOfWheelNumbers)
                        {
                                wheelIndex = 0;
                        }
                        actualIndex++;
                        if (actualIndex >= numberOfActualNumbers)
                        {
                             actualIndex = 0;
                        }
                    } break;
                    case -1:
                    {
                        wheelIndex--;
                        if (wheelIndex < 0)
                        {
                             wheelIndex = numberOfWheelNumbers - 1;
                        }
                        actualIndex--;
                        if (actualIndex < 0)
                        {
                                actualIndex = numberOfActualNumbers - 1;
                        }
                    }break;
                    default:
                    {
                        Debug.LogError("change has an illegal value");
                    }break;
                }

                UpdateTexts();

                currentWheelPosition = newPosition;
                Debug.Log(
               "actualIndex: " + actualIndex + 
               " wheelIndex: " + actualIndex + 
               " currentWheelPosition: " + currentWheelPosition);

                float absoluteWheelVelocity = Math.Abs(currentWheelVelocity);
                if (absoluteWheelVelocity - wheelVelocityLossPerPosition < 0)
                {
                    currentWheelVelocity = 0;
                }
                else
                {
                    currentWheelVelocity -= (wheelVelocityLossPerPosition * (currentWheelVelocity > 0 ? 1 : -1));
                }
                myTransform.rotation = Quaternion.Euler(destinationAngle, 0, 0);

            }
        }      
    }
}
