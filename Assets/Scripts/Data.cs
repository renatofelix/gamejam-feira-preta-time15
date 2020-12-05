using System;

namespace Game
{
    public enum Gender
    {
        CisMan,
        CisWoman,
        TransMan,
        TransWoman,
        NonBinary,
        Other,
    }

    public enum ColorOrRace
    {
        White,
        Black,
        Brown,
        Yellow,
        Indigenous,
        Other,
    }

    public enum Education
    {
        Illiterate,
        Primary,
        Secondary,
        Higher,
    }

    public enum SocialClass
    {
        Working,
        LowerMiddle,
        UpperMidle,
        LowerUpper,
        Upper,
    }

    [Serializable]
    public class Person
    {
        public int id;
        public string name;
        public int age;
        public Gender gender;
        public ColorOrRace colorOrRace;
        public Education education;
        public SocialClass socialClass;
        public int wealth;

        public void AddWealth(int amount)
        {
        }

        public void RemoeWealth(int amount)
        {
        }
    }
}