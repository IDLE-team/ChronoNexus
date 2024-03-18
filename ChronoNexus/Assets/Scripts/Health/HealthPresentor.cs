using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HealthPresentor : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private float _duration;

    private Slider _hpBar;

    private void FindHealth()
    {
        _health = InventoryItemManager.manager.GetPlayer().gameObject.GetComponent<Health>();
        if (_health)
        {
            _health.Changed += UpdateValue;
            _hpBar.maxValue = _health.MaxHealth;
            UpdateValue(_health.Value);
        }
    }
    private void Start()
    {
        _hpBar = GetComponent<Slider>();
        InventoryItemManager.manager.OnCharacterLinked += FindHealth;
    }

    private void OnEnable()
    {
        FindHealth();
    }

    private void OnDisable()
    {
        _health.Changed -= UpdateValue;
        InventoryItemManager.manager.OnCharacterLinked -= FindHealth;
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