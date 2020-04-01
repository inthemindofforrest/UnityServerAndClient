using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLogic
{
    [System.Serializable]
    public abstract class Task
    {

        Dictionary<TaskList.LIST, Task> TaskList = new Dictionary<TaskList.LIST, Task>();

        public DateTime StartTime;
        public DateTime FinishTime;

        public Inventory.Items Resource;
        public int HoursToCompleteTask;

        public void Constructor()
        {
            StartTime = DateTime.Now;
            TaskConstructor();
            FinishTime.AddHours(HoursToCompleteTask);
        }

        protected Task()
        {
            Resource = new Inventory.Empty();
            Constructor();
        }

        public bool FinishedTask()
        {
            return (FinishTime - StartTime).TotalHours >= HoursToCompleteTask;
        }

        public Inventory.Items RecieveItem()
        {
            return Resource;
        }


        public Task GetClassFromEnum(TaskList.LIST _Task)
        {
            if (TaskList.ContainsKey(_Task))
            {
                return TaskList[_Task];
            }
            else
            {
                return null;
            }
        }

        public TaskList.LIST GetEnumFromClass(Task _Task)
        {
            return TaskList.FirstOrDefault(x => x.Value == _Task).Key;
        }


        protected abstract void TaskConstructor();
    }

    [System.Serializable]
    public class Idle : Task
    {
        protected override void TaskConstructor()
        {
            HoursToCompleteTask = 0;//20 hours until complete task
            Resource = null;
        }
    }

    [System.Serializable]
    public class CollectWood : Task
    {
        protected override void TaskConstructor()
        {
            HoursToCompleteTask = 20;//20 hours until complete task
            Resource = new Inventory.Wood();
        }
    }
}