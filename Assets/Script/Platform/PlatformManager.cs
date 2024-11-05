using System.Collections.Generic;
using UnityEngine;

namespace AshGreen.Platform
{
    public class PlatformManager : Singleton<PlatformManager>
    {
        public List<PlatformController> platformList = new List<PlatformController> ();
    }

}
