using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAppearance : Appearance {
    public int Race;
    public int Sex;
    public int Face;
    public int BaseClass;
    public bool Running = true;
    public int HairStyle;
    public int HairColor;
    public int Chest;
    public int Legs;
    public int Gloves;
    public int Feet;
    public PaperDollSelection PaperDoll { get; } = new();
}
