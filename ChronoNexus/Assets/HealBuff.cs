using UnityEngine;
using Random = UnityEngine.Random;

public class HealBuff : DropObject
{
    [SerializeField] private int _baseHealValue;
    protected override void IncreaseValue()
    {
        int healValue = Mathf.CeilToInt(_baseHealValue + _baseHealValue * UpgradeData.Instance.HpDropPercentMultiplierUpgradeValue / 100);
        player.GetComponent<Health>().Increase(healValue);
        base.IncreaseValue();
    }
}
