using Random = UnityEngine.Random;

public class HealBuff : DropObject
{
    protected override void IncreaseValue()
    {
        player.GetComponent<Health>().Increase(Random.Range(5, _maxAmount));
        base.IncreaseValue();
    }
}
