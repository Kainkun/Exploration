using System;

public static class YarnUtils
{
    public static bool HasRequiredCollectables(CollectableAmountPair[] requiredStackingCollectableAmountPairs,
        string[] requiredUniqueCollectables)
    {
        return HasRequiredCollectables(requiredUniqueCollectables, requiredStackingCollectableAmountPairs);
    }

    public static bool HasRequiredCollectables(string[] requiredUniqueCollectables,
        CollectableAmountPair[] requiredStackingCollectableAmountPairs)
    {
        return (HasRequiredUniqueCollectables(requiredUniqueCollectables) &&
                HasRequiredStackingCollectables(requiredStackingCollectableAmountPairs));
    }

    public static bool HasRequiredUniqueCollectables(string[] requiredUniqueCollectables)
    {
        for (int i = 0; i < requiredUniqueCollectables.Length; i++)
            if (!YarnAccess.TryGetValue(requiredUniqueCollectables[i], out bool variableIsTrue) || !variableIsTrue)
                return false;
        return true;
    }

    [Serializable]
    public struct CollectableAmountPair
    {
        public string collectableName;
        public float collectableAmount;
    }

    public static bool HasRequiredStackingCollectables(CollectableAmountPair[] requiredStackingCollectableAmountPairs)
    {
        for (int i = 0; i < requiredStackingCollectableAmountPairs.Length; i++)
        {
            CollectableAmountPair requirementPair = requiredStackingCollectableAmountPairs[i];
            string requiredStackingCollectable = requirementPair.collectableName;

            if (!YarnAccess.TryGetValue(requiredStackingCollectable, out float amount) || amount < requirementPair.collectableAmount)
                return false;
        }

        return true;
    }
}