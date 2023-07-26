using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using Newtonsoft.Json;

//TODO - make save once a day - load first get date only - compare date - save
public class CloudSave : MonoBehaviour
{
    // public TextMeshProUGUI debugTxt, debugTxt2;
    // public TMP_InputField inputScore, inputKeyName;  

    //save complex data
    public async void SaveComplexDataCloud(Data dataToBeSave)
    {
        try
        {
            //set data for save
            var data = new Dictionary<string, object> { { "all", dataToBeSave } };
            //save
            await CloudSaveService.Instance.Data.ForceSaveAsync(data);
            Debug.Log("save data in cloud");
        }
        catch (System.Exception e)
        {
            Debug.Log("error save data in cloud : " + e);
        }

    }

    //load complex data
    public async void LoadComplexDataCloud()
    {
        try
        {
            //load data
            var query = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { "all" });
            //assign load data
            var stringData = query["all"];
            Debug.Log("load data from cloud");
            //convert data string to object
            var deserialized = JsonConvert.DeserializeObject<Data>(stringData);
            GameManager.instance.SetGameData(deserialized, false);
            GameManager.instance.isSuccessLoadCloud = true;
        }
        catch (System.Exception e)
        {
            Debug.Log("error load data from cloud : " + e);
            GameManager.instance.SaveState(true, false);
        }
    }

    //delete data
    public async void DeleteDataCloud()
    {
        try
        {
            await CloudSaveService.Instance.Data.ForceDeleteAsync("all");
            Debug.Log("delete data in cloud");
        }
        catch (System.Exception e)
        {
            Debug.Log("error delete data cloud : " + e);
        }
    }
}
