using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlash : MonoBehaviour
{
    private SpriteRenderer sprite;
    public Color[] flashColors = { Color.white };
    public Color usualColor = Color.white;
    public float waitTimeLower; //Minimum time for each flash of color
    public float waitTimeUpper; //Maximum time for each falsh of color
    public float flashTime; //How long a color flash lasts
    public float offset = 0; //Offset on intial flash, should be > 0 but < WaitTimeLower

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine("Flash");
    }

    IEnumerator Flash()
    {
        if (offset != 0)
        {
            sprite.color = usualColor;
            yield return new WaitForSeconds(waitTimeLower - offset);
            sprite.color = flashColors[(int)Random.Range(0, flashColors.Length + 0.01f)];
            yield return new WaitForSeconds(flashTime);
        }
        while (true)
        {
            sprite.color = usualColor;
            yield return new WaitForSeconds(Random.Range(waitTimeLower, waitTimeUpper));
            sprite.color = flashColors[(int)Random.Range(0, flashColors.Length - 0.01f)];
            yield return new WaitForSeconds(flashTime);
        }
    }
}
