using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLoop : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Color startColor;
    public Color endColor;
    public float duration; //Time between the start and ending colors, not cycle time

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        startColor = sprite.color;
        endColor.a = 1;
        StartCoroutine("ColorShift");
    }
    
    IEnumerator ColorShift()
    {
        while (true)
        {
            for (float i = 0; i < duration; i += duration / 10)
            {
                Color newColor = ((startColor) * (duration - i)) + ((endColor) * (i));
                newColor /= duration;
                sprite.color = newColor;
                yield return new WaitForSeconds(duration / 10);
            }
            for(float i = duration; i > 0; i -= duration / 10)
            {
                Color newColor = ((startColor) * (duration - i)) + ((endColor) * (i));
                newColor /= duration;
                sprite.color = newColor;
                yield return new WaitForSeconds(duration / 10);
            }
        }
    }
}
