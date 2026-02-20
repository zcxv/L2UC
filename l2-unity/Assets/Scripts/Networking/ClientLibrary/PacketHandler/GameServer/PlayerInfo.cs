using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInfo
{
      public NetworkIdentity Identity { get; set; }
      public PlayerStatus Status { get; set; }
      public PlayerStats Stats { get; set; }
      public PlayerAppearance Appearance { get; set; }
      public CharacterSkills Skills { get; set; }
}
