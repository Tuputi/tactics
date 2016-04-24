using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu]

public class Recipe : ScriptableObject
{

    public string Name;
    public Craftable Result;
    [TextArea]
    public string Description;

    public List<Craftable> Incredients;
}
