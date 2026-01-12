using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

[ExecuteAlways]
public class CardVisuals : MonoBehaviour
{
    [Header("Image Targets")]
    [SerializeField] Image _border;
    [SerializeField] Image _unitBackground;
    [SerializeField] Image _unitImage;
    [SerializeField] Image _powerBorder;
    [SerializeField] Image _nameContainer;
    [SerializeField] Image _narrativeContainer;

    [Header("Text Targets")]
    [SerializeField] TMP_Text _powerText;
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _abilityText;
    [SerializeField] TMP_Text _narrativeText;

    [Header("Assigned Sprites")]
    [SerializeField] Sprite _borderSprite;
    [SerializeField] Sprite _unitBackgroundSprite;
    [SerializeField] Sprite _unitSprite;
    [SerializeField] Sprite _powerBorderSprite;
    [SerializeField] Sprite _nameContainerSprite;
    [SerializeField] Sprite _narrativeContainerSprite;

    [Header("Assigned Text Values")]
    [SerializeField] int _power;
    [SerializeField] string _name;

    [SerializeField][TextArea] string _ability;
    [SerializeField][TextArea] string _narrative;

    [ContextMenu("Apply Card UI")]
    public void Apply()
    {
        ApplyImage(_border, _borderSprite, "Border Sprite");
        ApplyImage(_unitBackground, _unitBackgroundSprite, "Unit Background Sprite");
        ApplyImage(_unitImage, _unitSprite, "Unit Sprite");
        ApplyImage(_powerBorder, _powerBorderSprite, "Power Border Sprite");
        ApplyImage(_nameContainer, _nameContainerSprite, "Name Container Sprite");
        ApplyImage(_narrativeContainer, _narrativeContainerSprite, "Narrative Container Sprite");

        ApplyText(_powerText, _power.ToString(), "Power Text");
        ApplyText(_nameText, _name, "Name Text");
        ApplyText(_abilityText, _ability, "Ability Text");
        ApplyText(_narrativeText, _narrative, "Narrative Text");
    }

    public (Sprite unitBackgroundImage, Sprite uniteImage, int power) GetDataForUnit()
    {
        return (_unitBackgroundSprite, _unitSprite, _power);
    }

    void OnValidate()
    {
        if (!Application.isPlaying)
            Apply();
    }

    // ---------- Helpers ----------

    void ApplyImage(Image target, Sprite sprite, string undoName)
    {
        if (!target || !sprite)
            return;

#if UNITY_EDITOR
        Undo.RecordObject(target, undoName);
#endif
        target.sprite = sprite;
        MarkDirty(target);
    }

    void ApplyText(TMP_Text target, string value, string undoName)
    {
        if (!target)
            return;

#if UNITY_EDITOR
        Undo.RecordObject(target, undoName);
#endif
        target.text = value;
        MarkDirty(target);
    }

    void MarkDirty(Object obj)
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(obj);
#endif
    }
}
