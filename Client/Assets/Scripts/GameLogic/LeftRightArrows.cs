using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftRightArrows : MonoBehaviour
{
    ScrollRect scrollRect;

    float Target = 0;

    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    public void LeftArrowPressed()
    {
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }
    public void RightArrowPressed()
    {
        scrollRect.normalizedPosition = new Vector2(1, 0);
    }
}
