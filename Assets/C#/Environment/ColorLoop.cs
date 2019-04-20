using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLoop : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Color startColor;
    public Color endColor;
    public float duration; //Time between the start and ending colors, not cycle time
    public float offset; //Offset on intial loop, should be > 0 but < 2*duration
    public bool highFidelity; //Used to increase the number of steps between two colors, more CPU intensive but looks better.
    public int fidelity; //Custom Fidelity to use with HighFidelity

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        startColor = sprite.color;
        endColor.a = 1;
        if (highFidelity) StartCoroutine("HighColorShift");
        else StartCoroutine("ColorShift");
    }
    
    IEnumerator ColorShift()
    {
        if(offset != 0)
        {
            if(offset < duration)
            {
                for (float i = offset; i < duration; i += duration / 10)
                {
                    Color newColor = ((startColor) * (duration - i)) + ((endColor) * (i));
                    newColor /= duration;
                    sprite.color = newColor;
                    yield return new WaitForSeconds(duration / 10);
                }
                for (float i = duration; i > 0; i -= duration / 10)
                {
                    Color newColor = ((startColor) * (duration - i)) + ((endColor) * (i));
                    newColor /= duration;
                    sprite.color = newColor;
                    yield return new WaitForSeconds(duration / 10);
                }
            }
            if(offset > duration)
            {
                for (float i = offset - duration; i > 0; i -= duration / 10)
                {
                    Color newColor = ((startColor) * (duration - i)) + ((endColor) * (i));
                    newColor /= duration;
                    sprite.color = newColor;
                    yield return new WaitForSeconds(duration / 10);
                }
            }
            else
            {
                for (float i = duration; i > 0; i -= duration / 10)
                {
                    Color newColor = ((startColor) * (duration - i)) + ((endColor) * (i));
                    newColor /= duration;
                    sprite.color = newColor;
                    yield return new WaitForSeconds(duration / 10);
                }
            }
        }
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

    IEnumerator HighColorShift()
    {
        if (offset != 0)
        {
            if (offset < duration)
            {
                for (float i = offset; i < duration; i += duration / fidelity)
                {
                    Color newColor = ((startColor) * (duration - i)) + ((endColor) * (i));
                    newColor /= duration;
                    sprite.color = newColor;
                    yield return new WaitForSeconds(duration / fidelity);
                }
            }
            if (offset > duration)
            {
                for (float i = offset - duration; i > 0; i -= duration / fidelity)
                {
                    Color newColor = ((startColor) * (duration - i)) + ((endColor) * (i));
                    newColor /= duration;
                    sprite.color = newColor;
                    yield return new WaitForSeconds(duration / fidelity);
                }
            }
            else
            {
                for (float i = duration; i > 0; i -= duration / fidelity)
                {
                    Color newColor = ((startColor) * (duration - i)) + ((endColor) * (i));
                    newColor /= duration;
                    sprite.color = newColor;
                    yield return new WaitForSeconds(duration / fidelity);
                }
            }
        }
        while (true)
        {
            for (float i = 0; i < duration; i += duration / fidelity)
            {
                Color newColor = ((startColor) * (duration - i)) + ((endColor) * (i));
                newColor /= duration;
                sprite.color = newColor;
                yield return new WaitForSeconds(duration / fidelity);
            }
            for (float i = duration; i > 0; i -= duration / fidelity)
            {
                Color newColor = ((startColor) * (duration - i)) + ((endColor) * (i));
                newColor /= duration;
                sprite.color = newColor;
                yield return new WaitForSeconds(duration / fidelity);
            }
        }
    }
}
