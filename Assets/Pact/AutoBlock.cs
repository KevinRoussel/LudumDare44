using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="UnderBreak/AutoBlock")]
public class AutoBlock : Pact
{
    [SerializeField] float _cooldownDuration=5f;

    public override void Apply(Character character)
    {
        base.Apply(character);
        character.LaunchAutoBlock(_cooldownDuration);
    }
}
