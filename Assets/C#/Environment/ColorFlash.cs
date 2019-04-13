using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFlash : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Color flashColor;
    public Color usualColor;
    public float waitTime; //Time each flash of color
    public float flashTime; //How long a color flash lasts
    public float offset = 0; //Offset on intial flash, should be > 0 but < WaitTime

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        flashColor = sprite.color;
        usualColor.a = 1;
        StartCoroutine("Flash");
    }

    IEnumerator Flash()
    {
        sprite.color = usualColor;
        yield return new WaitForSeconds(waitTime - offset);
        sprite.color = flashColor;
        yield return new WaitForSeconds(flashTime);
        while (true)
        {
            sprite.color = usualColor;
            yield return new WaitForSeconds(waitTime);
            sprite.color = flashColor;
            yield return new WaitForSeconds(flashTime);
        }
    }
}
