using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HealthPresentor : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private float _duration;

    private Slider _hpBar;

    private void Start()
    {
        _hpBar = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _health.Changed += UpdateValue;
    }

    private void OnDisable()
    {
        _health.Changed -= UpdateValue;
    }

    private void UpdateValue(float value)
    {
        AnimateSlider(value).Forget();
    }

    private async UniTaskVoid AnimateSlider(float targetValue)
    {
        float startValue = _hpBar.value;
        float time = 0f;

        while (time < _duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / _duration);
            _hpBar.value = Mathf.Lerp(startValue, targetValue, t);
            await UniTask.Yield();
        }
    }
}