using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Library/UpgradeLibrary")]
public class UpgradeLibrary : ScriptableObject
{
    public List<UpgradeCard> upgrades;
}
