using System;
using UnityEngine;

internal interface ISeeker
{
    public event Action OnSeekStart;

    public event Action OnSeekEnd;

    public bool IsTargetFound { get; set; }

    public ITargetable Target { get; set; }

    public void StartSeek();

    public void StopSeek();
}