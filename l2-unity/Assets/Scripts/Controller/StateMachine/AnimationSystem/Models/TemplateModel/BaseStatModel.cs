using UnityEngine;


public class BaseStatModel
{
    


    private readonly GenderStats[] _genderData = new GenderStats[2];

    public BaseStatModel(GenderStats male, GenderStats female)
    {
        _genderData[0] = male;
        _genderData[1] = female;
    }

    public GenderStats GetData(int sex) => (sex == 1) ? _genderData[1] : _genderData[0];
}