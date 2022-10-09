using System.Collections.Generic;
using UnityEngine;

public class GalaxyRandom
{
    public static List<T> ConvertToList<T>(T[] array)
    {
        List<T> output = new List<T>();
        output.AddRange(array);
        return output;
    }

    public static T[] ConvertToArray<T>(List<T> list)
    {
        T[] output = new T[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            output[i] = list[i];
        }
        return output;
    }

    //Weighted Random
    #region Weighted Random
    /// <summary>
    /// Returns a random weighted value from list.
    /// </summary>
    /// <returns>Returns an value based on the passed list.</returns>
    public static T GetRandomWeightedValue<T>(List<WeightedItem<T>> weightedValueList)
    {
        T output = default;

        //Getting a random weight value
        var totalWeight = 0;
        foreach (var entry in weightedValueList)
        {
            totalWeight += entry.weight;
        }
        var rndWeightValue = Random.Range(1, totalWeight + 1);

        //Checking where random weight value falls
        var processedWeight = 0;
        foreach (var entry in weightedValueList)
        {
            processedWeight += entry.weight;
            if (rndWeightValue <= processedWeight)
            {
                output = entry.returnValue;
                break;
            }
        }

        return output;
    }

    public static T GetRandomWeightedValue<T>(WeightedItem<T>[] weightedValueArray)
    {
        List<WeightedItem<T>> list = ConvertToList(weightedValueArray);

        return GetRandomWeightedValue(list);
    }
    #endregion

    //Non Repeating Random
    #region Shufflebag
    /// <summary>
    /// Returns a random value from list and won't repeat unless all values have been used once, or lis is manually reset.
    /// </summary>
    /// <param name="shufflebag">Parameter value to pass.</param>
    /// <returns>Returns an value based on the passed list.</returns>
    public static T GetValueFromShufflebag<T>(List<ShufflebagItem<T>> shufflebag)
    {
        T output = default;

        List<ShufflebagItem<T>> itemsInBag = new List<ShufflebagItem<T>>();
        foreach (var entry in shufflebag)
        {
            if(entry.inBag)
            {
                itemsInBag.Add(entry);
            }
        }

        if(itemsInBag.Count == 0)
        {
            ResetShufflebag(shufflebag);
            itemsInBag = shufflebag;
        }

        var rndValue = Random.Range(0, itemsInBag.Count);

        if(itemsInBag[rndValue] != null)
        {
            itemsInBag[rndValue].inBag = false;
            output = itemsInBag[rndValue].returnValue;
        }

        return output;
    }

    public static T GetValueFromShufflebag<T>(ShufflebagItem<T>[] shufflebagArray)
    {
        List<ShufflebagItem<T>> list = ConvertToList(shufflebagArray);

        return GetValueFromShufflebag(list);
    }

    public static void ResetShufflebag<T>(List<ShufflebagItem<T>> shufflebag)
    {
        for (int i = 0; i < shufflebag.Count; i++)
        {
            shufflebag[i].inBag = true;
        }
    }
    public static void ResetShufflebag<T>(ShufflebagItem<T>[] shufflebagArray)
    {
        List<ShufflebagItem<T>> list = ConvertToList(shufflebagArray);

        ResetShufflebag(list);
    }
    #endregion

    //Bell Curve Random
    #region Bell Curve Random
    public int GetBellCurveRandom(int min, int max, int numberOfRolls)
    {
        int output = 0;

        for (int i = 0; i < numberOfRolls; i++)
        {
            output += Random.Range(min, max);
        }

        return output;
    }

    public float GetBellCurveRandom(float min, float max, int numberOfRolls)
    {
        float output = 0;

        for (int i = 0; i < numberOfRolls; i++)
        {
            output += Random.Range(min, max);
        }

        return output;
    }
    #endregion

    //Random Vector
    #region Random Vectors
    public Vector2 GetRandomVector2(float xMin, float xMax, float yMin, float yMax)
    {
        Vector2 result;

        result = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));

        return result;
    }

    public Vector3 GetRandomVector3(float xMin, float xMax, float yMin, float yMax, float zMin, float zMax)
    {
        Vector3 result;

        result = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), Random.Range(zMin, zMax));

        return result;
    }
    #endregion

    //Random from List
    #region Random from List
    public static T GetRandomFromList<T>(List<T> list)
    {
        T output = default;

        int index = Random.Range(0, list.Count);

        output = list[index];

        return output;
    }

    public static T GetRandomFromList<T>(T[] array)
    {
        List<T> list = ConvertToList(array);

        return GetRandomFromList(list);
    }
    #endregion
}

[System.Serializable]
public class WeightedItem<TWeightedItem>
{
    public int weight = 1;
    public TWeightedItem returnValue;

    public WeightedItem(int itemWeight, TWeightedItem value)
    {
        weight = itemWeight;
        returnValue = value;
    }
}

[System.Serializable]
public class ShufflebagItem<TItem>
{
    public TItem returnValue;
    public bool inBag = true;

    public ShufflebagItem(TItem value)
    {
        returnValue = value;
        inBag = true;
    }
}