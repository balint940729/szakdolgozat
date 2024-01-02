using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellBase {

    void SetTargets(List<UnitController> units);

    //public virtual void setPlayerTargets(List<UnitController> units) {
    //    allyTargets = units;
    //}

    void SetCaster(UnitController caster);

    void InitializeSpell();

    bool IsSpellTargets();
}