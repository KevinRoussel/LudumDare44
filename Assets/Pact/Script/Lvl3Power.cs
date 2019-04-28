using UnityEngine;

[CreateAssetMenu(menuName = "UnderBreak/Lvl3Power")]
public class Lvl3Power : Pact {

    [Header("Lvl 3 Power  effects variables")]
    [Header("Bonus - Rage Upgrade")]
    [Tooltip("Rage's cooldown reduced percentage")]
    [Range(0, 1)]
    [SerializeField] float _rageCooldownDivider;

    [Tooltip("Character's projectiles will have their size multiplied by this value while rage is on")]
    [SerializeField] float _rageProjectilesSizeMultiplier;

    public override void Apply (Character character) {

        character.Lvl3RageProjectilesSizeMultiplier = _rageProjectilesSizeMultiplier;

        character.RageCooldown /= _rageCooldownDivider;

    }

}
