using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="UnderBreak/FlashSkill")]
public class FlashSkillPact : Pact
{
    public override void Apply(Character character)
    {
        base.Apply(character);
        character.SetSkill(Character.SkillChoice.Flash);
    }
}
