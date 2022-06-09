using System.Collections;
using System.Collections.Generic;

public class AbilityENums
{}

public enum AbilityType
{
    shoot,
    channel
}

public enum AbilityProbability
{
    often,
    rare,
    extraRare
}

public enum AbilityState
{
    ready,
    active,
    cooldown
}