using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private Image _img;
    [SerializeField] private Sprite[] _frames;
    private int _currentFrame;
    private float _timer;
    [SerializeField] private float _framerate = 0.1f;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _framerate)
        {
            _timer -= _framerate;
            _currentFrame = (_currentFrame + 1) % _frames.Length;
            _img.sprite = _frames[_currentFrame];
            // _img.SetNativeSize();
        }

    }
}
