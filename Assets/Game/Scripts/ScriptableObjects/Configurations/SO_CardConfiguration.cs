using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardConfiguration", menuName = "ScriptableObjects/Configurations/CardConfiguration")]
public class SO_CardConfiguration : ScriptableObject
{
    public List<SO_Card> CardsList;
}
