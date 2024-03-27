using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEventsHolder : MonoBehaviour
{
    public event Action OnFinisherInteract;
    public event Action OnShootInteract;
    public event Action OnInteractionInteract;
    public event Action OnHideInteract;
    
    public void CallOnFinisherInteractEvent()
    {
        OnFinisherInteract?.Invoke();
    }
    public void CallOnShootInteractEvent()
    {
        OnShootInteract?.Invoke();
    }

    public void CallOnInteractionInteractEvent()
    {
        OnInteractionInteract?.Invoke();
    }
    
    public void CallOnHideInteractEvent()
    {
        OnHideInteract?.Invoke();
    }
}
