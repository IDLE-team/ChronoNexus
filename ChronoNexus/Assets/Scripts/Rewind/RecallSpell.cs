using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RecallSpell 
{
    private ITransformable _caster;

    public event Action CastStarted;
    public event Action<Transform> CastApplied;
    public event Action CastCanceled;
    public event Action<Transform> CastPerformed;

    private Timer _timer;
    //private Aim _aimTarget;

    private List<RecallInteractor> _findedInteractors = new List<RecallInteractor>();
    private RecallInteractor _selectedInteractor;

    private PauseHandler _pauseHandler;
    private RecallSpellConfig _config;

    private bool _isApplied;
    
    public bool IsApplied { get { return _isApplied; } }
    
    public RecallSpell(RecallSpellConfig recallSpellConfig, /*PauseHandler pauseHandler,*/ Timer timer /*CharacterMovement caster*/)
    {
        _config = recallSpellConfig;
       // _pauseHandler = pauseHandler;

        _timer = timer;
        _timer.Set(recallSpellConfig.SecondsToRecord, recallSpellConfig.SecondsToRecord);

       // _aimTarget = aimTarget;
       _caster = GameObject.Find("Character").GetComponent<CharacterMovement>();
       Debug.Log("InitCaset: " + _caster);

    }

    public void StartCast()
    {
        

        Collider[] colliders = Physics.OverlapSphere(_caster.Transform.position, _config.Radius, _config.SpellInteractableLayerMask);

        foreach (Collider collider in colliders)
        {
            if(collider.TryGetComponent(out RecallInteractor recallInteractor))
            {
                if (_findedInteractors.Contains(recallInteractor))
                    continue;

                recallInteractor.Prepare(_config.SpellInteractableLayer);
                _findedInteractors.Add(recallInteractor);
            }
        }

      //  _pauseHandler.SetPause(true);

       // _aimTarget.Enable(_findedInteractors);
     //   _aimTarget.TargetDetected += OnTargetDetected;

        CastStarted?.Invoke();
    }

    private void OnTargetDetected(bool hasTarget)
    {
        if (hasTarget)
        {
          //  _selectedInteractor = _aimTarget.CurrentTarget;
            _selectedInteractor.Activate(_config.TimeRewindLayer);
        }
        else
        {
            _selectedInteractor?.Prepare(_config.SpellInteractableLayer);
            _selectedInteractor = null;
        }
    }

    public bool TryApplyCast()
    {
    //    _selectedInteractor = _aimTarget.CurrentTarget;

      //  if (_selectedInteractor == null)
          //  return false;

        foreach (RecallInteractor interactor in _findedInteractors)
            interactor.Activate(_config.TimeRewindLayer);


        //interactor.Deactivate(_config.SpellInteractableLayer);

        //_findedInteractors.Clear();

       // _pauseHandler.SetPause(false);

      //  _selectedInteractor.Activate(_config.TimeRewindLayer);

        _timer.StartCountingTime();
        _timer.TimeIsOver += CancelCast;

       // _aimTarget.Disable();

        _isApplied = true;

        foreach (RecallInteractor interactor in _findedInteractors)
            CastApplied?.Invoke(interactor.transform);

        return true;
    }

    public void CancelCast()
    {
        if (_isApplied)
        {
            foreach (RecallInteractor interactor in _findedInteractors)
                interactor.Deactivate(_config.SpellInteractableLayer);

            _timer.TimeIsOver -= CancelCast;
            _timer.StopCountingTime();

            foreach (RecallInteractor interactor in _findedInteractors)
                CastPerformed?.Invoke(interactor.transform);
        }
        else
        {
            foreach (RecallInteractor interactor in _findedInteractors)
                interactor.Deactivate(_config.SpellInteractableLayer);

            _findedInteractors.Clear();

         //   _pauseHandler.SetPause(false);

         //   _aimTarget.Disable();

            CastCanceled?.Invoke();
        }

        _isApplied = false;
    }
}
