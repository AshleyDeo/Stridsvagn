using System.Collections.Generic;
using UnityEngine;

namespace strids {
	[CreateAssetMenu(fileName = "New Tank List", menuName = "Assets/Tank List")]
	public class TankListSO : ScriptableObject {
        public List<TankType> TankList;
    }
}