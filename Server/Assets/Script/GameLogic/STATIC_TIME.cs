using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TIME
{
    [System.Serializable]
    public class DAY
    {
        const double DayToYear = 365 / 7;//52.1428
        const double SecondToHour = 60 * 60;//187714.2857

        public int Day = 0;
        public int Hours = 0;
        public double Seconds = 0;

        public void Update()
        {
            Seconds += Time.deltaTime * DayToYear;
            if(Seconds > SecondToHour)
            {
                Seconds -= SecondToHour;
                Hours++;
                if(Hours >= 24)
                {
                    Hours = 0;
                    Day++;
                }
            }
        }
    }
    [System.Serializable]
    public class WEEK
    {
        public DAY Day = new DAY();
        public int Weeks;
        public void Update()
        {
            Day.Update();
            Weeks = (int)(Day.Day / 7) + 1;
        }
    }
    [System.Serializable]
    public class MONTH
    {
        public WEEK Week = new WEEK();
        public int Months;
        public void Update()
        {
            Week.Update();
            Months = (int)(Week.Day.Day / 30);
        }
    }
    [System.Serializable]
    public class YEAR
    {
        public MONTH Month = new MONTH();
        public int Years;
        public void Update()
        {
            Month.Update();
            Years = (int)(Month.Week.Day.Day / 365);
        }

        public int GetTimeInHours()
        {
            int YearsInHours = Years * (365 * 24);
            int DaysInHours = Month.Week.Day.Day * 24;

            return YearsInHours + DaysInHours + Month.Week.Day.Hours; 
        }

        public int GetHourDifference(YEAR _Other)
        {
            return Mathf.Abs(GetTimeInHours() - _Other.GetTimeInHours());
        }
    }

    public class STATIC_TIME : MonoBehaviour
    {
        [SerializeField] Text YearText;
        [SerializeField] Text MonthText;
        [SerializeField] Text DayText;
        [SerializeField] Text HourText;
        [SerializeField] Text SecondsText;

        public static YEAR Year = new YEAR();
        private int CurrentHour = 0;

        private void Start()
        {
            YEAR newYear = SaveTime.Load();
            if(newYear != null)
            {
                Year = newYear;
            }
        }

        private void Update()
        {
            Year.Update();

            YearText.text = "Year: " + Year.Years.ToString();
            MonthText.text = "Month: " + Year.Month.Months.ToString();
            DayText.text = "Day: " + Year.Month.Week.Day.Day.ToString();
            HourText.text = "Hours: " + ((int)Year.Month.Week.Day.Hours).ToString();
            SecondsText.text = "Seconds: " + ((int)Year.Month.Week.Day.Seconds).ToString();

            SaveTimeByHour();
        }

        private void SaveTimeByHour()
        {
            if(CurrentHour != Year.Month.Week.Day.Hours)
            {
                CurrentHour = Year.Month.Week.Day.Hours;
                SaveTime.Save(Year);
            }
        }
    }
}