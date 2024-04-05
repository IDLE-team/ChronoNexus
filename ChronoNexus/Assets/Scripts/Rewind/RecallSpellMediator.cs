using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RecallSpellMediator: IDisposable
{
    private const float StandardVignetteSmoothness = 0.4f;
    private const float ActivatedVignetteSmoothness = 1f;

    private RecallSpell _recallSpell;
   // private CharacterView _characterView;
    private PlayerInputActions _input;
   // private ScannerEffect _scannerEffect;
   // private GrayscaleScreenEffect _grayscaleScreenEffect;
   // private CameraModeSwitcher _cameraModeSwitcher;
    private TimerView _timerView;
    private Vignette _vignette;

    private ParticleSystem _activateSparksPrefab;

    public RecallSpellMediator(RecallSpell recallSpell, PlayerInputActions input,
        TimerView timerView)
    {
        _recallSpell = recallSpell;

        _input = input;

        _timerView = timerView;

        _recallSpell.CastStarted += OnCastStarted;
        _recallSpell.CastCanceled += OnCastCanceled;
        _recallSpell.CastApplied += OnCastApplied;
        _recallSpell.CastPerformed += OnCastPerformed;
    }

    private void OnCastStarted()
    {
    }

    public void Dispose()
    {
        _recallSpell.CastStarted -= OnCastStarted;
        _recallSpell.CastCanceled -= OnCastCanceled;
        _recallSpell.CastApplied -= OnCastApplied;
        _recallSpell.CastPerformed -= OnCastPerformed;
    }

    private void OnCastApplied(Transform rewindObjectTransform)
    {

      //  _timerView.Show(rewindObjectTransform);

    }

    private void OnCastCanceled()
    {

    }

    private void OnCastPerformed(Transform rewindObjectTransform)
    {
      //  _timerView.Hide();
    }
}
