using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pokemon.Scripts.MyUtils;
using Pokemon.Scripts.Saving;
using UnityEngine;

namespace Pokemon.Scripts.Map
{
    public class HubController : Singleton<HubController>, ISavable
    {
        private const string key = "HubController";
        private HashSet<string> unlockedHubs;
        public void Initialize()
        {
            unlockedHubs = RestoreState() as HashSet<string>;
            if (unlockedHubs == null)
            {
                unlockedHubs = new HashSet<string>();
            }
        }

        public void UnlockHub(string hubID)
        {
            if (!unlockedHubs.Contains(hubID))
            {
                unlockedHubs.Add(hubID);
                CaptureState();
            }
        }
        public bool ContainsHub(string hubID)
        {
            return unlockedHubs.Contains(hubID);
        }
        public void CaptureState()
        {
            if (unlockedHubs == null || unlockedHubs.Count == 0) return;
            string json = JsonConvert.SerializeObject(unlockedHubs, Formatting.Indented);
            PlayerPrefs.SetString(key, json);
        }

        public object RestoreState()
        {
            string json = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(json)) return null;

            HashSet<string> decodeHubs = JsonConvert.DeserializeObject<HashSet<string>>(json);
            return decodeHubs;
        }


    }
}