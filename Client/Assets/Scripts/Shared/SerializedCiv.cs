using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    [System.Serializable]
    public class SerializedCiv
    {
        public string Name;

        public int CurrentTask;
        public System.DateTime StartTime;
        public System.DateTime FinishTime;

        public int Resource;
        public int HoursToCompleteTask;

        public int Focus;
        public int Strength;
        public int Stamina;
        public int Agility;
        public int Charisma;
        public int Intelegence;

    }
}