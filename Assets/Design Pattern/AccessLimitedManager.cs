/*
 * design pattern sample:AccessLimited class and Manager
 * Date 20200603
 * Author: Chun-Lung Tseng as Gyd
 * Email: kingterrygyd@gmail.com
 * Twitter: @kingterrygyd
 * Facebook: facebook.com/barbariangyd
 * Github: gydisme
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class AccessLimitedManager : AccessLimited.Manager { }

public class AccessLimited
{
    private int _limitedIntData = 0;
    private AccessLimited()
    {

    }

    public void SetIntData(int value)
    {
        _limitedIntData = value;
    }
    
    private void Destroy()
    {
        // do some cleanup
    }

    #region Manager
    public class Manager : MonoBehaviour
    {
        public static Manager Instance { get; private set; }
        private List<AccessLimited> _created = null;
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning( "Instance Exist, Destroy self");
                Destroy(this);
                return;
            }

            Instance = this;

            _created = new List<AccessLimited>();
        }

        private void OnDestroy()
        {
            if(Instance == this)
            {
                foreach( var item in _created)
                {
                    item.Destroy();
                }

                _created.Clear();
                Instance = null;
            }
        }

        public AccessLimited Create()
        {
            AccessLimited accessLimited = new AccessLimited();
            return accessLimited;
        }

        // another design pattern
        public static AccessLimited AnotherCreate()
        {
            if( Instance != null)
            {
                return Instance._CreateInternal();
            }

            Debug.LogError("Instance is null");
            return null;
        }

        private AccessLimited _CreateInternal()
        {
            AccessLimited accessLimited = new AccessLimited();
            return accessLimited;
        }
    }
    #endregion
}


/*
 * Sample
 */

public class AccessLimitedManagerSample
{
    public void Main()
    {
        AccessLimited accessLimited = AccessLimitedManager.Instance.Create();
        accessLimited.SetIntData(0);

        // another design pattern
        AccessLimited accessLimited2 = AccessLimitedManager.AnotherCreate();
        accessLimited2.SetIntData(0);
    }
}
