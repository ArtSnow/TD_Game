using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Threading.Tasks;

public class Multiplayer : MonoBehaviour
{
    public static string code;
    public static int mode;
    public static int opponentIndex;
    private static string baseUrl = "https://artsnow.pythonanywhere.com/";
    public async Task<int> StartGame()
    {
        JSONNode res;
        string url = "startGame";

        res = await REST_Get(url);
        if (res != null)
        {
            code = res["code"];
            mode = 0;
            opponentIndex = 2;
            return 1;
        } else
        {
            return 0;
        }
        
    }

    public async Task<int> FindGame(string inCode)
    {
        JSONNode res;
        string url = "findGame";

        Dictionary<string, string> data = new Dictionary<string, string>() 
        {
            {"code", inCode}
        };

        res = await REST_Post(url, data);

        if (res != null)
        {
            code = inCode;
            mode = 1;
            opponentIndex = 1;
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public async Task<int> CheckMode(string inCode)
    {
        JSONNode res;
        string url = "checkMode";

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"code", inCode}
        };

        res = await REST_Post(url, data);

        if (res != null)
        {
            return res["mode"];
        }
        else
        {
            return 0;
        }
    }

    public async Task<JSONNode> GetPlayer(string inCode)
    {
        JSONNode res;
        string url = "getPlayer";

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"code", inCode},
            {"player", "p" + opponentIndex.ToString() }
        };

        res = await REST_Post(url, data);

        if (res != null)
        {
            return res["answer"];
        }
        else
        {
            return 0;
        }
    }

    public async Task<JSONNode> GetMap(string inCode)
    {
        JSONNode res;
        string url = "getMap";

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"code", inCode}
        };

        res = await REST_Post(url, data);

        if (res != null)
        {
            return res["map"];
        }
        else
        {
            return 0;
        }
    }

    public async Task<JSONNode> GetWave(int index)
    {
        JSONNode res;
        string url = "getWave";

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"code", code},
            {"player", (1 + (opponentIndex % 2)).ToString()},
            {"index", index.ToString()}
        };

        res = await REST_Post(url, data);

        if (res != null)
        {
            return res;
        }
        else
        {
            return 0;
        }
    }

    public async Task<int> GetWavesCount(string inCode)
    {
        JSONNode res;
        string url = "getWavesCount";

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"code", inCode}
        };

        res = await REST_Post(url, data);

        if (res != null)
        {
            return res["count"];
        }
        else
        {
            return 0;
        }
    }

    public async Task<int> AddEnemy(int enemyIndex)
    {
        JSONNode res;
        string url = "addEnemy";

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"code", code},
            {"enemyIndex", enemyIndex.ToString()},
            {"opponentIndex", opponentIndex.ToString()}
        };

        res = await REST_Post(url, data);

        if (res != null)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public async Task<int> SetInfo()
    {
        JSONNode res;
        string url = "setInfo";

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"code", code},
            {"health", GameResources.i.getHealth().ToString()},
            {"energyIncome", GameResources.i.getEnergyIncome().ToString()},
            {"towersCount", GameResources.i.getTowersCount().ToString()},
            {"player", (1 + (opponentIndex % 2)).ToString()}
        };

        res = await REST_Post(url, data);

        if (res != null)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public async Task<int> GetInfo()
    {
        JSONNode res;
        string url = "getInfo";

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"code", code},
            {"player", opponentIndex.ToString()}
        };

        res = await REST_Post(url, data);

        if (res != null)
        {
            return res;
        }
        else
        {
            return 0;
        }
    }

    public async Task<bool> GetDefeat()
    {
        JSONNode res;
        string url = "getDefeat";

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"code", code},
            {"player", opponentIndex.ToString()}
        };

        res = await REST_Post(url, data);

        if (res != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<int> SetDefeat()
    {
        JSONNode res;
        string url = "setDefeat";

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"code", code},
            {"player", (1 + (opponentIndex % 2)).ToString()}
        };

        res = await REST_Post(url, data);

        if (res != null)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public async Task<int> EndGame()
    {
        JSONNode res;
        string url = "endGame";

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            {"code", code}
        };

        res = await REST_Post(url, data);

        if (res != null)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }


    private async Task<JSONNode> REST_Get(string url)
    {
        JSONNode res;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");
        UnityWebRequest www = UnityWebRequest.Get(baseUrl + url);
        var operation = www.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (www.isHttpError || www.isNetworkError)
        {
            res = null;
        } else
        {
            res = JSON.Parse(www.downloadHandler.text);
        }
        return res;
    }

    private async Task<JSONNode> REST_Post(string url, Dictionary<string, string> data)
    {
        JSONNode res;
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> entry in data)
        {
            form.AddField(entry.Key, entry.Value);
        }
        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + url, form))
        {
            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.isHttpError || www.isNetworkError)
            {
                res = null;
            }
            else
            {
                res = JSON.Parse(www.downloadHandler.text);
            }
            return res;
        }
    }
}
