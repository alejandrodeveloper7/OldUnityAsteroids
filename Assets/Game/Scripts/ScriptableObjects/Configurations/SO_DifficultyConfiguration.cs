using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDifficultyConfiguration", menuName = "ScriptableObjects/Configurations/DifficultyConfiguration")]
public class SO_DifficultyConfiguration : ScriptableObject
{
    public List<SO_Difficulty> DifficultyList;
}
