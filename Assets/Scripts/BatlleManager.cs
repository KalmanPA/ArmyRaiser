using UnityEngine;

public class BatlleManager : MonoBehaviour
{
    [SerializeField] BattleButton _battleButton;

    [SerializeField] BattleSites[] _battleSites;

    private void RunBattles()
    {
        foreach (BattleSites battleSite in _battleSites)
        {
            battleSite.RunBattle();
        }
    }

    private void OnEnable()
    {
        _battleButton.OnBattleTriggered += RunBattles;
    }

    private void OnDisable()
    {
        _battleButton.OnBattleTriggered -= RunBattles;
    }
}
