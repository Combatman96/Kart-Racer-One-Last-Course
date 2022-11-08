using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowConfetti : MonoBehaviour
{
    [SerializeField] Transform _confettiPrefab;
    [SerializeField] Color[] _colors;
    private List<Confetti> _confettiList = new List<Confetti>();
    private RectTransform _rectTransform => GetComponent<RectTransform>();

    private float _spawnTimer;
    private const float SPAWN_TIMER_MAX = 0.033f;

    private void OnDisable()
    {
        _confettiList.Clear();
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        foreach (var confetti in new List<Confetti>(_confettiList))
        {
            if (confetti.Update())
            {
                _confettiList.Remove(confetti);
            }
        }
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer < 0)
        {
            _spawnTimer += SPAWN_TIMER_MAX;
            int spawnAmount = Random.Range(1, 4);
            for (int i = 0; i < spawnAmount; i++)
            {
                SpawnConfetti();
            }
        }
    }

    public void SpawnConfetti()
    {
        float width = _rectTransform.rect.width;
        float height = _rectTransform.rect.height;
        Vector2 anchor = new Vector2(Random.Range(-width / 2f, width / 2f), height * 0.5f);
        Color color = _colors[Random.Range(0, _colors.Length)];
        Confetti confetti = new Confetti(_confettiPrefab, transform, anchor, color, -height * 0.5f);
        _confettiList.Add(confetti);
    }

    private class Confetti
    {
        private Transform _transform;
        private RectTransform _rectTransform;
        private Vector2 _anchorPos;
        private Vector3 _euler;
        private float _eulerSpeed;
        private Vector2 _moveAmount;
        private float _minY;

        public Confetti(Transform prefab, Transform container, Vector2 anchor, Color color, float minY)
        {
            this._anchorPos = anchor;
            _transform = Instantiate(prefab, container);
            _rectTransform = _transform.GetComponent<RectTransform>();
            _rectTransform.anchoredPosition = anchor;

            _rectTransform.sizeDelta *= Random.Range(0.9f, 1.3f);

            _euler = new Vector3(0, 0, Random.Range(0, 360f));
            _eulerSpeed = Random.Range(100f, 200f);
            _eulerSpeed *= Random.Range(0, 2) == 0 ? 1f : -1f;
            _transform.localEulerAngles = _euler;

            _moveAmount = new Vector2(0, Random.Range(-50f, -200f));
            _transform.GetComponent<Image>().color = color;

            _minY = minY;
        }

        public bool Update()
        {
            _anchorPos += _moveAmount * Time.deltaTime;
            _rectTransform.anchoredPosition = _anchorPos;

            _euler.z += _eulerSpeed * Time.deltaTime;
            _transform.localEulerAngles = _euler;

            if (_anchorPos.y < _minY)
            {
                Destroy(_transform.gameObject);
                return true;
            }
            else return false;
        }
    }
}
