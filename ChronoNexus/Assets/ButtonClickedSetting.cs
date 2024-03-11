using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickedSetting : MonoBehaviour
{

    public bool isClicked;
    private SettignsButtons _parent;

    private TextMeshProUGUI _text;
    private Button _button;

    [SerializeField] private Color _white = Color.white;
    [SerializeField] private Color _black;

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
        var b = _parent.RecolorAllButtons(isClicked);
        PaintButton(b);

    }

    public void PaintButton(bool clicked)
    {
        if (clicked)
        {
            _button.GetComponent<Image>().color = _white;
            _text.color = _black;
            isClicked = true;
        }
        else
        {
            _button.GetComponent<Image>().color = _black;
            _text.color = _white;
            isClicked = false;
        }
    }
}
