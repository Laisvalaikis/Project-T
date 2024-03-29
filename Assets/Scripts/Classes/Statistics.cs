using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Statistics
{
    public static string[] classes = { "KNIGHT", "SHAMAN", "SORCERESS", "PORTALIST", "CRUSADER", "HUNTRESS", "FIRE MAGE", "ENCHANTRESS", "ASSASSIN", "GUARDIAN", "STONEWEAVER" };//nu ir taip toliau
    //public string[] achievements;
    public float playTime;
    public float battleTime;
    //public int dayCount;
    public int[] charactersBoughtCountByClass;
    public int[] characterDeathsCountByClass;
    public int[] killCountByClass;
    public int[] charactersSelectedCountByClass;
    //public int[] enemiesSelectedCountByClass;
    //public int[] mapSelectedCount;
    //public int[] spellBoughtCount;
    //public int[] spellUsedCount;
    //public int[] townHallUpdatesBoughtCount;

    public Statistics()
    {
        playTime = 0;
        battleTime = 0;
        charactersBoughtCountByClass = new int[11];
        characterDeathsCountByClass = new int[11];
        killCountByClass = new int[11];
        charactersSelectedCountByClass = new int[11];
    }

    //public Statistics Add(Statistics statistics)
    //{
    //    Statistics result = new Statistics();
    //    for(int i = 0; i < 10; i++)
    //    {
    //        result.charactersBoughtCountByClass[i] = this.charactersBoughtCountByClass[i] + statistics.charactersBoughtCountByClass[i];
    //        result.characterDeathsCountByClass[i] = this.characterDeathsCountByClass[i] + statistics.characterDeathsCountByClass[i];
    //        result.killCountByClass[i] = this.killCountByClass[i] + statistics.killCountByClass[i];
    //        result.charactersSelectedCountByClass[i] = this.charactersSelectedCountByClass[i] + statistics.charactersSelectedCountByClass[i];
    //    }
    //    return result;
    //}

    public static int getClassIndex(string className)
    {
        return Array.IndexOf(classes, className);
    }
}
