using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dome
{
    public class SaveManager : MonoBehaviour
    {
        public GameManager gm;

        public List<GameObject> orbs;
        public List<string> milestones;

        public Dictionary<GameObject, string> orbLocations;

        private void Start()
        {
            gm = gameObject.GetComponent<GameManager>();
            orbLocations = new Dictionary<GameObject, string>();
        }

        public void SaveObjects()
        {
            foreach(OrbController orb in GameObject.FindObjectsOfType<OrbController>())
            {
                
                // If orb is attached to pylon, save the pylon
                Pylon orbParent = orb.gameObject.GetComponentInParent<Pylon>();
                if(orbParent != null)
                {
                    GameObject pylon = orbParent.gameObject;
                    if(orbLocations.ContainsKey(pylon)) 
                        orbLocations.Remove(pylon);
                    if (orbLocations.ContainsKey(orb.gameObject))
                        orbLocations.Remove(orb.gameObject);
                    pylon.transform.SetParent(null);
                    DontDestroyOnLoad(pylon);
                    orbLocations.Add(pylon, gm.curIWScene);
                    pylon.SetActive(false);
                }
                // Else if the orb is not held by the player or leaf boys, just save the orb
                else if(orb.transform.parent == null ||
                    (orb.transform.parent.GetComponent<PlayerController>() == null &&
                     orb.transform.parent.GetComponent<OrbLeaf>() == null))
                {
                    //Debug.Log("no pylon or player");
                    if(orbLocations.ContainsKey(orb.gameObject)) 
                        orbLocations.Remove(orb.gameObject);
                    DontDestroyOnLoad(orb.gameObject);
                    orbLocations.Add(orb.gameObject, gm.curIWScene);
                    orb.gameObject.SetActive(false);
                }
            }

            //foreach(GameObject key in orbLocations.Keys)
            //{
            //    Debug.Log(key.name + " location: " + orbLocations[key]);
            //}
        }

        public void LoadObjects(string sceneName)
        {
            List<GameObject> keysToRemove = new List<GameObject>();
            foreach (GameObject key in orbLocations.Keys)
            {
                if (key.GetComponent<Pylon>() != null && !key.GetComponent<Pylon>().HasOrb())
                {
                    keysToRemove.Add(key);
                }
            }

            foreach(GameObject key in keysToRemove)
            {
                orbLocations.Remove(key);
                Destroy(key);
            }

            foreach (GameObject key in orbLocations.Keys)
            {
                if(orbLocations[key] == sceneName)
                {
                    //Debug.Log(key.name + " location: " + orbLocations[key]);
                    key.SetActive(true);
                }
            }
        }

        public void SetMilestone(string curEvent)
        {
            milestones.Add(curEvent);
        }
    }
}