using UnityEngine;

[CreateAssetMenu(menuName = "UnderBreak/Lvl3Dodge")]
public class Lvl3Dodge : Pact {

    [Header("Lvl 3 Dodge effects variables")]
    [Header("Bonus - Move Speed Bonus")]
    [Tooltip("Bonus by which the character's current moev speed will be multiplied")]
    [SerializeField] float _moveSpeedMultiplier;

    [Header("Malus - Fire Rate Reduction")]
    [Tooltip("Value by which the character's current fire rate will be multiplied")]
    [SerializeField] float _fireRateMalusMultiplier;

    public override void Apply (Character character) {

        character.NavMeshAgent.speed *= _moveSpeedMultiplier;

        character.FireRate *= _fireRateMalusMultiplier;

    }

}
