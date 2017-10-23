using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Yarn.Unity.Example
{

    //     ,-'""`-.       you have been
    //    ;   {}   :   visited by the
    //   :          :  code skelton of
    //   :  _    _  ;  the abyss
    //   : (o)  (o) :            
    //   ::   '`   :;  good code and     
    //    !:      :!   bones will come
    //    `:`++++';'   
    //      `....'     but only if you comment
    //  _____________  'thank mr skeltal'
    // | MR. SKELTAL | in your code
    // 

    public class TimeKeeper : MonoBehaviour
    {

        public int dayCount = 0; //# of days passed
        public enum dayTime { Morning, Noon, Evening, Night }; //Time of day
        public dayTime currentTime = dayTime.Morning; //the current time, duh.

		void Update()
		{
			//GetComponent<Text> ().text = "Day " + dayCount + "\n" + currentTime;
		}

        [YarnCommand("advancetime")]
        public void AdvanceTime() //incements time forward
        {
            if ((int)currentTime >= 3)
            {
                currentTime = dayTime.Morning;
            }
            else
            {
                currentTime = (dayTime)((int)currentTime + 1);
            }
        }

        [YarnCommand("settime")]
        public void SetTime(string time) //sets time to accepted string
        {
            if (time == "Morning" || time == "morning")
            {
                if (!((int)currentTime <= 0))
                {
                    dayCount++;
                }
                currentTime = dayTime.Morning;
            }

            else if (time == "Noon" || time == "noon")
            {
                if (!((int)currentTime <= 1))
                {
                    dayCount++;
                }
                currentTime = dayTime.Noon;
            }

            else if (time == "Evening" || time == "evening")
            {
                if (!((int)currentTime <= 2))
                {
                    dayCount++;
                }
                currentTime = dayTime.Evening;
            }

            else if (time == "Night" || time == "night")
            {
                if (!((int)currentTime <= 3))
                {
                    dayCount++;
                }
                currentTime = dayTime.Night;
            }
        }

        [YarnCommand("newday")]
        public void newDay()
        {
            currentTime = dayTime.Morning;
            dayCount++;
        }

    }
}
