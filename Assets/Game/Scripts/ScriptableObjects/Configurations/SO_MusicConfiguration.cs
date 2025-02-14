using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMusicConfiguration", menuName = "ScriptableObjects/Configurations/MusicConfiguration")]
public class SO_MusicConfiguration : ScriptableObject
{
    public List<SO_Sound> MusicList;
}
