using Scripts.Shrooms;
using UnityEngine;

public class ShroomSelectionView : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    public void Show()
    {
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    public void OnShroomSelected(int abilityType)
    {
        Hide();
        
        //todo switch character prefab
        ServiceLocator.Instance.GetService<ShroomAbilityService>().OnAbilityClicked((ShroomAbilityType)abilityType, null);
    }
}
