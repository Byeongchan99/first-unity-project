using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonsterBase
{
    public override IEnumerator AttackPattern()
    {
        yield return null;
    }
}
