using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBG : MonoBehaviour
{
    [SerializeField] private float _speedX;
    [SerializeField] private float _speedY;
    RawImage _rawImage => GetComponentInChildren<RawImage>();

    private void Update()
    {
        float offsetX = Time.deltaTime * _speedX * 0.01f;
        float offsetY = Time.deltaTime * _speedY * 0.01f;
        _rawImage.uvRect = new Rect(_rawImage.uvRect.position + new Vector2(offsetX, offsetY), _rawImage.uvRect.size);
    }
}
