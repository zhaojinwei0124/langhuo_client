using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Network
{
    public class GameBaseInfo:Single<GameBaseInfo>
    {

        public int userid{ get; set; }

        public string encrypt{ get; set; }

    }

}