using UnityEngine;

[CreateAssetMenu(menuName = "UnderBreak/Lvl2Dodge")]
public class Lvl2Dodge : Level2Pact {

    [Header("Lvl 2 Dodge effects variables")]
    [Header("Bonus - Flash Upgrade")]
    [Tooltip("Flash's upgrade effect strength (should be above 1)")]
    [SerializeField] float _flashUpgradeEffect;

    [Tooltip("Flash's upgrade effect duration")]
    [SerializeField] float _flashUpgradeEffectDuration;       

    public override void Apply (Character character) {

        base.Apply(character);

        character.FlashUpgradeEffect = new Vector2(_flashUpgradeEffect, _flashUpgradeEffectDuration);

    }

}
