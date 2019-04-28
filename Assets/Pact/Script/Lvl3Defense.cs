using UnityEngine;

[CreateAssetMenu(menuName = "UnderBreak/Lvl3Defense")]
public class Lvl3Defense : Pact {

    [Header("Lvl 3 Defense effects variables")]
    [Header("Bonus - Damage return")]
    [Tooltip("Percentage of damage return")]
    [SerializeField] float _damageReturnPercent;

    // Bonus
    public override void Apply (Character character) {

        character.OnHit += (Character instigator, int amount) => { instigator.Hit(character, Mathf.RoundToInt(amount * _damageReturnPercent)); };

    }

    // Malus
    public override void Apply (GameUIControl UI) {

        UI.PlayerHealthBar.SetActive(false);
        UI.PlayerMinimap.SetActive(false);

    }

}
