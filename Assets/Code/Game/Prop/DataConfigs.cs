using UnityEngine;
using System.Collections.Generic;
using RO.Config;

namespace RO
{
	public class DataConfigs
	{
		public static string NAME = "DataConfigs";

		public static DataConfigs Instance{ get; private set; }

		public List<DataVO> datas;
		public SDictionary<int,DataVO> dataMaps = new SDictionary<int, DataVO> ();

		public DataConfigs ()
		{
			Instance = this;
			InitData (PropID.Hp, 1, "生命", 0, DataType.Int, false);
			InitData (PropID.Mp, 1, "魔法", 0, DataType.Int, false);
			InitData (PropID.PhyAtk, 1, "物理攻击", 0, DataType.Int, false);
			InitData (PropID.PhyDef, 1, "物理防御", 0, DataType.Int, false);
			InitData (PropID.MagAtk, 1, "魔法攻击", 0, DataType.Int, false);
			InitData (PropID.MagDef, 1, "魔法防御", 0, DataType.Int, false);
			InitData (PropID.MoveSpeed, 1, "移动速度", 0, DataType.Float, false);
			InitData (PropID.AtkRange, 1, "攻击距离", 0, DataType.Float, false);
			InitData (PropID.AtkRangeFactor, 1, "攻击距离系数", 0, DataType.Float, false);
			InitData (PropID.AtkCD, 1, "攻击间隔", 0, DataType.Float, false);
			InitData (PropID.AtkCDFactor, 1, "攻击间隔系数", 0, DataType.Float, false);
		}

		void InitData (int id, int typeID, string name, int priority, DataType type, bool isPercent)
		{
			DataVO dataVO = new DataVO ();
			dataVO.id = id;
			dataVO.typeID = typeID;
			dataVO.name = name;
			dataVO.priority = priority;
			dataVO.dataType = type;
			dataVO.isPercent = isPercent;
			dataMaps [id] = dataVO;
		}
	}
} // namespace RO
