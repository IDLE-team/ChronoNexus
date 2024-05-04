using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VelocityRecorder : Recorder<VelocityValues>
{
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _storedVelocity;
    private Vector3 _storedAngularVelocity;

    private void OnValidate()
        => _rigidbody ??= GetComponent<Rigidbody>();

    public override void StartRecord()
    {
        base.StartRecord();

        _rigidbody.useGravity = true;
        _rigidbody.isKinematic = false;  
        RestoreStoredValues();
    }

    public override void StartRewind()
    {
        base.StartRewind();
        Debug.Log("VelocityRewind");

        _rigidbody.useGravity = false; 
        _rigidbody.isKinematic = true;
    }

    public override void SetPause(bool isPaused)
    {
        base.SetPause(isPaused);

        if (isPaused)
        {
            RememberCurrentValues();
            _rigidbody.isKinematic = true;
            return;
        }

        _rigidbody.isKinematic = false;
        RestoreStoredValues();
    }

    protected override VelocityValues GetRecordValue()
        =>  new VelocityValues(_rigidbody.linearVelocity, _rigidbody.angularVelocity);

    protected override void RestoreValue(VelocityValues currentValue, VelocityValues targetValue, float progress)
    {
        Debug.Log("VelocityRestore");
        _storedVelocity = Vector3.Lerp(targetValue.Velocity, currentValue.Velocity, progress);
        _storedAngularVelocity = Vector3.Lerp(targetValue.AngularVelocity, currentValue.AngularVelocity, progress);
    }

    protected override bool IsDataValuesChanged(VelocityValues lastValue)
        => lastValue.Velocity != _rigidbody.linearVelocity || lastValue.AngularVelocity != _rigidbody.angularVelocity;

    private void RememberCurrentValues()
    {
        _storedVelocity = _rigidbody.linearVelocity;
        _storedAngularVelocity = _rigidbody.angularVelocity;
    }

    private void RestoreStoredValues()
    {
        Debug.Log("VelocityRestoreStoredValues");
        _rigidbody.linearVelocity = _storedVelocity;
        _rigidbody.angularVelocity = _storedAngularVelocity;
    }
}
