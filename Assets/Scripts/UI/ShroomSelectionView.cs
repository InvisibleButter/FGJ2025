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
        
        ServiceLocator.Instance.GetService<ShroomSpawner>().OnShroomSelected((ShroomAbilityType)abilityType);
    }
}
