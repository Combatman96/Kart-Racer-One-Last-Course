using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    ScreenStartGame screenStartGame => transform.GetComponentInParent<ScreenStartGame>();

    public void OnAnimationEventRaise(string text)
    {
        screenStartGame.UpdateCountDownText(text);
    }
}
