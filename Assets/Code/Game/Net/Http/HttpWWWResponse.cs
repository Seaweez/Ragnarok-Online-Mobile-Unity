using UnityEngine;
using System.Collections.Generic;
using LitJson;
using SLua;

namespace RO
{
    [SLua.CustomLuaClassAttribute]
    public class HttpWWWResponse
    {
        public string wwwError;   // Error from WWW
        public string resString;  // Raw response string
        public int Status;        // Status code
        public string message;    // Message from server
        public List<int> rewardIds; // List to store reward IDs
        private JsonData origindata; // Parsed JSON data

        public JsonData resData
        {
            set
            {
                if (value != null)
                {
                    if (value.Keys.Contains("status"))
                    {
                        Status = int.Parse(value["status"].ToString());
                    }

                    if (value.Keys.Contains("message"))
                    {
                        message = value["message"].ToString();
                        RO.LoggerUnused.Log("Some Wrong Msg：" + message);
                    }

                    if (value.Keys.Contains("reward_ids"))
                    {
                        rewardIds = new List<int>();
                        JsonData ids = value["reward_ids"];
                        for (int i = 0; i < ids.Count; i++)
                        {
                            rewardIds.Add((int)ids[i]);
                        }
                        // Log the count of reward IDs
                        Debug.Log("Count of Reward IDs: " + rewardIds.Count);
                        // Log all retrieved IDs for verification
                        Debug.Log("Retrieved Reward IDs: " + string.Join(", ", rewardIds));
                    }
                }
                origindata = value;
            }

            get
            {
                return origindata;
            }
        }

        public LuaTable GetRewardIdsLuaTable()
        {
            LuaTable rewardIdsTable = LuaSvr.mainState.doString("return {}") as LuaTable;
            for (int i = 0; i < rewardIds.Count; i++)
            {
                rewardIdsTable[i + 1] = rewardIds[i]; // LuaTable uses 1-based index
            }
            return rewardIdsTable;
        }
    }
}
