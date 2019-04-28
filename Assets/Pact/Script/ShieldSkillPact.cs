using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="UnderBreak/ShieldSkill")]
public class ShieldSkillPact : Pact
{
    public override void Apply(Character character)
    {
        base.Apply(character);
        character.SetSkill(Character.SkillChoice.Shield);
    }
}
