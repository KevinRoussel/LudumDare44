using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="UnderBreak/RageSkill")]
public class RageSkillPact : Pact
{
    public override void Apply(Character character)
    {
        base.Apply(character);
        character.SetSkill(Character.SkillChoice.Rage);
    }
}
