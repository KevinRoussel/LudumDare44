using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="UnderBreak/ConeAttack")]
public class ConeAttack : Pact
{
    [SerializeField] float range = 1;

    public override void Apply(Character character)
    {
        base.Apply(character);
        character.ActivateConeAttack(range);
    }

}
