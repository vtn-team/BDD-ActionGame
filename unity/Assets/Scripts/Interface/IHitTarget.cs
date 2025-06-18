using UnityEngine;

public interface IHitTarget
{
    bool Damage(int attackPower);
    int GetGeneratorID();
    float GetHPPercentage();
}