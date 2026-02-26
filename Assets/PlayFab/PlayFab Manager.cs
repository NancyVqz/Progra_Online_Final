using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System.Threading.Tasks;

public class PlayFabManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;

    //private void Start()
    //{
    //    if(PlayFabSettings.DeveloperSecretKey == null)
    //    {
    //        PlayFabSettings.DeveloperSecretKey = "F6TGI9NYOKQNCUMNGFGN7K5PG6GSCNN9U6619DHADBOY9BA8P8";
    //    }
    //    if(PlayFabSettings.TitleId == null)
    //    {
    //        PlayFabSettings.TitleId = "1B36C0";
    //    }
    //}

    //public async Task CreatePlayfabAccount()
    //{
    //    RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
    //    {
    //        Username = usernameInput.text.ToLower(),
    //        DisplayName = usernameInput.text,
    //        Password = passwordInput.text,
    //    };

    //    PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterPlayerSucess, OnError);
    //}

    //private void OnRegisterPlayerSucess(RegisterPlayFabUserResult result)
    //{ 

    //}

    //private void OnError(PlayFabError error)
    //{

    //}
}
