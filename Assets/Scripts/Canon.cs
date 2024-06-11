using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Canon : MonoBehaviour
    {
        public float Calibr;
        public float DamageMultiplier;
        public float RandeMultiplier;
        public ShipLoot Powder;
        public AmmoShip AmmoType;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shot(null);
            }
        }

        public void Shot(ShipInventory shipInventory) // AmmoShip ammoShip) 
        {
           // shipInventory.DelItem(Powder);
           // shipInventory.DelItem(AmmoType);
            GameObject spawnedobject = Instantiate(AmmoType.Projectile, gameObject.transform.position, AmmoType.transform.rotation);
            spawnedobject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * AmmoType.AmmoRange * RandeMultiplier, ForceMode.Impulse);
            
        }



    }

}
