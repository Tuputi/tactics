using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu]

public class Recipe : ScriptableObject
{

    public string Name;
    public Craftable Result;
    public int MinimunPropertyPoints = 10;
    [TextArea]
    public string Description;

    public List<Craftable> Incredients;
}
