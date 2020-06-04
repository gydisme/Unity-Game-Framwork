/*
 * design pattern sample:AccessLimited class and Manager
 * Date 20200603
 * Author: Chun-Lung Tseng as Gyd
 * Email: kingterrygyd@gmail.com
 * Twitter: @kingterrygyd
 * Facebook: facebook.com/barbariangyd
 */

using UnityEngine;

public class AccessLimitedManager : AccessLimited.Manager { }

public class AccessLimited
{
    private int _limitedIntData = 0;
    private AccessLimited()
    {

    }

    public class Manager : MonoBehaviour
    {
        public AccessLimited Create()
        {
            AccessLimited accessLimited = new AccessLimited();
            return accessLimited;
        }

        public void SetIntData(AccessLimited accessLimited, int value)
        {
            accessLimited._limitedIntData = value;
        }
    }
}
