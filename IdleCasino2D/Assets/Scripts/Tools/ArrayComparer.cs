using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayComparer
{
    public static int CountMatches(int[] array1,  int[] array2)
    {
        int count = 0;

        Debug.Log("Массив комбинации:");
        for (int i = 0; i < array1.Length; i++)
        {
            Debug.Log(array1[i]);
        }

        Debug.Log("Массив :");
        for (int i = 0; i < array2.Length; i++)
        {
            Debug.Log(array2[i]);
        }

        Dictionary<int, int> mas2Dict = new Dictionary<int, int>();

        foreach (int num in array2)
        {
            if (mas2Dict.ContainsKey(num))
            {
                mas2Dict[num]++;
            }
            else
            {
                mas2Dict[num] = 1;
            }
        }

        foreach (int num in array1)
        {
            if(mas2Dict.ContainsKey(num) && mas2Dict[num] > 0)
            {
                count++;
                mas2Dict[num]--;
            }
        }

        return count;
    }

    public static Dictionary<int, bool> GetMatchDictionary(int[] mas1, int[] mas2)
    {
        Dictionary<int, bool> result = new Dictionary<int, bool>();

        List<int> mas1List = new List<int>(mas1);

        for (int i = 0; i < mas2.Length; i++)
        {
            if (mas1List.Contains(mas2[i]))
            {
                result[i] = true;
                mas1List.Remove(mas2[i]);
            }
            else
            {
                result[i] = false;
            }
        }

        return result;
    }

}
