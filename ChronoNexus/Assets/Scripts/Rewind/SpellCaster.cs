using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Zenject;

public class SpellCaster : MonoBehaviour
{
    private RecallSpell _recallSpell;
    private PlayerInputActions _input;
    [Inject]
    private void Construct(RecallSpell recallSpell, PlayerInputActions input)
    {
        _recallSpell = recallSpell;
        _input = input;

        _input.Player.TimeRewind.started += OnCastSpellKeyPressed;
    }

    private void OnCastSpellKeyPressed(InputAction.CallbackContext obj)
    {
      //  _input.Player.TimeRewind.started -= OnCastSpellKeyPressed;

        if (!_recallSpell.IsApplied)
        {
        //    _input.Player.RewindCancel.started += OnCastCanceledKeyPressed;
       //     _input.Player.RewindApply.started += OnCastAppliedKeyPressed;

            _recallSpell.StartCast();
            OnCastAppliedKeyPressed(obj);
        }
        else
        {
            OnCastCanceledKeyPressed(obj);
        }
    }

    private void OnCastAppliedKeyPressed(InputAction.CallbackContext obj)
    {
        if (_recallSpell.TryApplyCast() == false)
            return;

        Debug.Log("Applied");
        _input.Player.RewindApply.started -= OnCastAppliedKeyPressed;

        _recallSpell.CastPerformed += OnCastPerformed;
    }

    private void OnCastCanceledKeyPressed(InputAction.CallbackContext obj)
    {
      //  _input.Player.RewindCancel.started -= OnCastCanceledKeyPressed;
       // _input.Player.RewindApply.started -= OnCastAppliedKeyPressed;

        _recallSpell.CastCanceled += OnCastCanceled;
        _recallSpell.CancelCast();
        Debug.Log("OnCastAppliedKeyPressed");

    }

    private void OnCastCanceled()
    {
        _recallSpell.CastCanceled -= OnCastCanceled;
        _recallSpell.CastPerformed -= OnCastPerformed;
        _input.Player.TimeRewind.started += OnCastSpellKeyPressed;
    }

    private void OnCastPerformed(Transform rewindObjectTransform)
    {
        _input.Player.RewindCancel.started -= OnCastCanceledKeyPressed;
        _recallSpell.CastCanceled -= OnCastCanceled;
        _recallSpell.CastPerformed -= OnCastPerformed;
        _input.Player.TimeRewind.started += OnCastSpellKeyPressed;
    }
}
