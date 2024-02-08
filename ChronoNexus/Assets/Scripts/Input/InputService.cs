using System;
using UnityEngine;

[Fix]
public class InputService : MonoBehaviour, IInputService
{
    public event Action Attacked;

    public event Action Shot;

   // [SerializeField] private LongClickButton longClickButton;

    private void OnEnable()
    {
      //  longClickButton.OnClicked += Fire;
    }

    private void OnDisable()
    {
     //   longClickButton.OnClicked -= Fire;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Fire();
        }
    }

    public void Attack() => Attacked?.Invoke();

    public void Fire() => Shot?.Invoke();
}