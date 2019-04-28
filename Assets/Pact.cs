using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pact : ScriptableObject
{
    public virtual void Apply(Room room) { }
    public virtual void Apply(Character character) { }
    public virtual void Apply(GameUIControl UI) { }
    public virtual void Apply(InputManager inputManager) { }

}
