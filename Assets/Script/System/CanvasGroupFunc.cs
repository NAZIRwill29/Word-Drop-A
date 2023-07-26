using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroupFunc : MonoBehaviour
{
    public void ModifyCG(CanvasGroup objectCG, int alpha, bool isInteractable, bool isBlockRayCast)
    {
        objectCG.alpha = alpha;
        objectCG.interactable = isInteractable;
        objectCG.blocksRaycasts = isBlockRayCast;
    }
}
