using UnityEngine;

namespace Assets
{
    public class Sail
    {
        public GameObject SailUp;
        public GameObject SailDown;
        public bool IsSailsUp;

        public Sail(GameObject sailUp, GameObject sailDown)
        {
            SailUp = sailUp;
            SailDown = sailDown;
            IsSailsUp = false;
            SwitchStatus();         
        }


        public void SwitchStatus()
        {
            if(IsSailsUp)
            {
                SailUp.SetActive(true);
                SailDown.SetActive(false);
            }else
            {
                SailDown.SetActive(true);
                SailUp.SetActive(false);
            }
        }
    }
}'
    ';lkj;jkj;lihlih