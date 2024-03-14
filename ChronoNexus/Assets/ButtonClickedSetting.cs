using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickedSetting : MonoBehaviour
{

    public bool isClicked;
    private SettignsButtons _parent;

    private TextMeshProUGUI _text;
    private Button _button;

    [SerializeField] private Color _ActiveButtonFont = Color.black;
    [SerializeField] private Color _ActiveButtonBack = Color.white;
    [SerializeField] private Color _DeactiveButtonFont = Color.white;
    [SerializeField] private Color _DeactiveButtonBack = Color.black;

    private void OnDrawGizmos()
    {
        _parent = GetComponentInParent<SettignsButtons>();
        _button = gameObject.GetComponent<Button>();
        _text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (!_button)
        {
            _button = gameObject.GetComponent<Button>();
        }
        if (!_parent)
        {
            _parent = GetComponentInParent<SettignsButtons>();
        }
        if (!_text)
        {
            _text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        }
        PaintButton(isClicked);
        _button.onClick.AddListener(Clicked);
    }
    public void Clicked()
    {
        if (!isClicked)
        {
            var b = _parent.RecolorAllButtons(isClicked);
            PaintButton(b);
        }
    }

    public void PaintButton(bool clicked)
    {
        if (clicked)
        {
            //button activated
            _button.GetComponent<Image>().color = _ActiveButtonBack;
            _text.color = _ActiveButtonFont;
            isClicked = true;
        }
        else
        {
            //button not active
            _button.GetComponent<Image>().color = _DeactiveButtonBack;
            _text.color = _DeactiveButtonFont;
            isClicked = false;
        }
    }
}
