
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;
public class DamagePopup : MonoBehaviour {

    // Create a Damage Popup
    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit)
    {
        position.y += 2f;
        var damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);

        return damagePopup;
    }

    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _critColor;

    private static int sortingOrder;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;
    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCriticalHit) {
        textMesh.SetText(damageAmount.ToString());
        if (!isCriticalHit) {
            // Normal hit
            textMesh.fontSize = 4;
            textColor = _normalColor;
        } else {
            // Critical hit
            textMesh.fontSize = 6;
            textColor = _critColor;
        }
        disappearTimer = DISAPPEAR_TIMER_MAX;
        textMesh.color = textColor;
        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        moveVector = new Vector3(.7f, 1) * 60f;
        Activate();
    }

    /*
    private void Update() {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f) {
            // First half of the popup lifetime
            float increaseScaleAmount = 0.1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        } else {
            // Second half of the popup lifetime
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0) {
            // Start disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }
    */
    private void LateUpdate()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;

        // Вычисляем угол поворота вокруг оси Y
        /*Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            // rotation *= Quaternion.Euler(0f, 180f, 0f);
            rotation *= Quaternion.Euler(0f, -Camera.main.transform.eulerAngles.y, 0f);
        // Применяем поворот к объекту текста
        */
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
       // transform.rotation = rotation;
    }

    private void Activate()
    {
        Sequence mySequence = DOTween.Sequence();
        transform.localScale = Vector3.zero;
        mySequence
            .Append(transform.DOScale(1, 0.3f))
            .Join(transform.DOMove(transform.position + new Vector3(Random.Range(-1, 1),Random.Range(-0.5f, 2),Random.Range(-1, 1)), 0.3f))
            .AppendInterval(0.2f)
            .Append(transform.DOScale(0, 0.3f))
            .OnComplete(() => Destroy(gameObject));
    }
    

}
