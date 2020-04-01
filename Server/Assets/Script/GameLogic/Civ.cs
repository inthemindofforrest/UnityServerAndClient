using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    [System.Serializable]
    public class Attributes
    {
        public int Focus;
        public int Strength;
        public int Stamina;
        public int Agility;
        public int Charisma;
        public int Intelegence;

        public void AssignAtts()
        {
            int Min = 1;
            int Max = 15;

            Focus = Random.Range(Min, Max);
            Strength = Random.Range(Min, Max);
            Stamina = Random.Range(Min, Max);
            Agility = Random.Range(Min, Max);
            Charisma = Random.Range(Min, Max);
            Intelegence = Random.Range(Min, Max);
        }
        public void AssignStarterAtts()
        {
            AssignAtts();

            List<int> RandomRange = new List<int>();
            RandomRange.Add(1);
            RandomRange.Add(2);
            RandomRange.Add(3);
            RandomRange.Add(4);
            RandomRange.Add(5);
            RandomRange.Add(6);


            int Bonus1, Bonus2;
            RandomRange.Remove(Bonus1 = Random.Range(1, RandomRange.Count));
            Bonus2 = Random.Range(1, RandomRange.Count);

            switch (Bonus1)
            {
                case 1:
                    Focus = 15;
                    break;
                case 2:
                    Strength = 15;
                    break;
                case 3:
                    Stamina = 15;
                    break;
                case 4:
                    Agility = 15;
                    break;
                case 5:
                    Charisma = 15;
                    break;
                case 6:
                    Intelegence = 15;
                    break;

                default:
                    break;
            }
            switch (Bonus2)
            {
                case 1:
                    Focus = 15;
                    break;
                case 2:
                    Strength = 15;
                    break;
                case 3:
                    Stamina = 15;
                    break;
                case 4:
                    Agility = 15;
                    break;
                case 5:
                    Charisma = 15;
                    break;
                case 6:
                    Intelegence = 15;
                    break;

                default:
                    break;
            }
        }
    }
    [System.Serializable]
    public class Civ
    {
        public string Name;

        public Task CurrentTask;

        public Attributes Att = new Attributes();

        public Civ()
        {
            CurrentTask = new Idle();
            CurrentTask.Resource = new Inventory.Empty();
            Att = new Attributes();
        }

        //public bool IsTaskFinished()
        //{
        //    return CurrentTask.FinishedTask();
        //}
        //public Inventory.Items RecieveItemFromTask()
        //{
        //    if(IsTaskFinished())
        //    {
        //        Inventory.Items CurrentItem = CurrentTask.RecieveItem();
        //        CurrentTask = null;//empty out Task
        //        return CurrentItem;
        //    }
        //    return null;
        //}
        //public void AssignTask(Task _NewTask)
        //{
        //    if(CurrentTask == null)
        //    {
        //        CurrentTask.Constructor();
        //    }
        //}

        public void AssignAtts()
        {
            Att.AssignAtts();
            AssignName();
        }
        public void AssignStarterAtts()
        {
            Att.AssignStarterAtts();
            AssignName();
        }

        void AssignName()
        {
            Name = NameGenerator.GetRandomName;
        }

        public void CreateFromSerializedCiv(SerializedCiv _Civ)
        {
            Name = _Civ.Name;

            CurrentTask = CurrentTask.GetClassFromEnum((TaskList.LIST)_Civ.CurrentTask);
            CurrentTask.StartTime = _Civ.StartTime;
            CurrentTask.FinishTime = _Civ.FinishTime;
            CurrentTask.Resource = CurrentTask.Resource.GetClassFromEnum((InventoryList.LIST)_Civ.Resource);
            CurrentTask.HoursToCompleteTask = _Civ.HoursToCompleteTask;

            Att.Focus = _Civ.Focus;
            Att.Strength = _Civ.Strength;
            Att.Stamina = _Civ.Stamina;
            Att.Agility = _Civ.Agility;
            Att.Charisma = _Civ.Charisma;
            Att.Intelegence = _Civ.Intelegence;
        }

        public static SerializedCiv CreateSerializedCivFromCiv(Civ _Civ)
        {
            SerializedCiv NewCiv = new SerializedCiv();

            NewCiv.Name = _Civ.Name;

            NewCiv.CurrentTask = (int)_Civ.CurrentTask.GetEnumFromClass(_Civ.CurrentTask);
            NewCiv.StartTime = _Civ.CurrentTask.StartTime;
            NewCiv.FinishTime = _Civ.CurrentTask.FinishTime;
            NewCiv.Resource = (int)_Civ.CurrentTask.Resource.GetEnumFromClass(_Civ.CurrentTask.Resource);
            NewCiv.HoursToCompleteTask = _Civ.CurrentTask.HoursToCompleteTask;

            NewCiv.Focus = _Civ.Att.Focus;
            NewCiv.Strength = _Civ.Att.Strength;
            NewCiv.Stamina = _Civ.Att.Stamina;
            NewCiv.Agility = _Civ.Att.Agility;
            NewCiv.Charisma = _Civ.Att.Charisma;
            NewCiv.Intelegence = _Civ.Att.Intelegence;

            return NewCiv;
        }

    }
}