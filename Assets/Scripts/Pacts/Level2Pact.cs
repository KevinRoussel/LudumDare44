using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UnderBreak/Pact/Level 2")]
public class Level2Pact : Pact
{
    [SerializeField] Character.OffsetType type;

    public override void Apply(Character character) {
        character.SetOffset(type);
    }
}
