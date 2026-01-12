using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitVisual : MonoBehaviour
{
    [Header("Unit Logic")]
    [SerializeField] UnitBase _unitBase;

    [Header("Image Targets")]
    [SerializeField] Image _unitBackgroundImage;
    [SerializeField] Image _unitImage;

    [Header("Text Targets")]
    [SerializeField] TMP_Text _powerText;
   
    //Sprite _unitBackgroundSprite;
    //Sprite _unitSprite;
    
    //int _power;

    private void SetVisualsBasedOnCard()
    {
        var cardData = _unitBase.DisplayUnitCard.GetComponent<CardVisuals>().GetDataForUnit();

        _unitBackgroundImage.sprite = cardData.unitBackgroundImage;

        _unitImage.sprite = cardData.uniteImage;

        _powerText.text = cardData.power.ToString();
    }
    private void ChangePower(int power)
    {
        _powerText.text = power.ToString();
    }

    private void OnEnable()
    {
        _unitBase.OnDisplayCardInitailised += SetVisualsBasedOnCard;
        _unitBase.OnPowerChanged += ChangePower;
    }

    private void OnDisable()
    {
        _unitBase.OnDisplayCardInitailised -= SetVisualsBasedOnCard;
        _unitBase.OnPowerChanged -= ChangePower;
    }
}
