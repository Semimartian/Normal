using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffect : MonoBehaviour
{

    [SerializeField]  private TMPro.TextMeshProUGUI text;
    private char[] originalChars;// = new char[charCount];
    private int charCount;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float minTypeSpeed;
    [SerializeField] private float maxTypeSpeed;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Print();
        }
    }

    private void Start()
    {
        charCount = text.text.Length;
        originalChars = new char[charCount];
        text.text.CopyTo(0, originalChars, 0, charCount);
        text.text = null;

    }

    public void Print()
    {
        StartCoroutine(PrintCoroutine());
    }

    private IEnumerator PrintCoroutine()
    {
        char[] newChars = new char[charCount];
        for (int i = 0; i < charCount; i++)
        {
            float waitTime = Random.Range(minTypeSpeed, maxTypeSpeed);
            yield return new WaitForSeconds(waitTime);
            newChars[i] = originalChars[i];
            text.text = new string( newChars);
            audioSource.Play();
        }
    }
}
