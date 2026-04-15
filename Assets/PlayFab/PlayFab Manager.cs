using System.Collections.Generic;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayFabManager : MonoBehaviour
{
    [Header("Register UI Elements")]
    [SerializeField] private TMP_InputField register_UsernameInputField;
    [SerializeField] private TMP_InputField register_PasswordInputField;
    [SerializeField] private TMP_InputField register_EmailInputField;
    [SerializeField] private UnityEvent OnRegisSuccess;

    [Header("Login UI Elements")]
    [SerializeField] private TMP_InputField login_UsernameInputField;
    [SerializeField] private TMP_InputField login_PasswordInputField;
    [SerializeField] private UnityEvent OnLogSuccess;

    public static PlayFabManager _PlayfabManager;
    public string username;


    private void Awake()
    {
        if(_PlayfabManager == null)
        {
            _PlayfabManager = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        //PlayFabSettings.DeveloperSecretKey = "F6TGI9NYOKQNCUMNGFGN7K5PG6GSCNN9U6619DHADBOY9BA8P8";

        PlayFabSettings.TitleId = "1B36C0";
    }

    public async void RegisterUser() //Metodo que ira en el boton para registrar al usuario
    {
        try
        {
            var resut = await RegisterPlayfabAccount();
            username = register_UsernameInputField.text;
            Debug.Log("Usuario Registrado");
            OnLogSuccess.Invoke();
        }
        catch (System.Exception error)
        {
            Debug.Log(error.Message);
        }
    }

    //Los metodos que realizan las request reciben una respuesta
    public async Task<RegisterPlayFabUserResult> RegisterPlayfabAccount()
    {
        //tipo de variable donde se almacena un resultado, y los devuelve
        var taskSource = new TaskCompletionSource<RegisterPlayFabUserResult>();

        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
        {
            Email = register_EmailInputField.text,
            Username = register_UsernameInputField.text.ToLower(),
            DisplayName = register_UsernameInputField.text,
            Password = register_PasswordInputField.text,
        };

        //descubrir como deben realizar la llamada a la API para que se pueda poner un await
        PlayFabClientAPI.RegisterPlayFabUser(request, resultCallback => 
        taskSource.SetResult(resultCallback), errorCallback => taskSource.SetException(new System.Exception(errorCallback.GenerateErrorReport())));

        return await taskSource.Task;
    }

    public async void PlayfabLogin()
    {
        try
        {
            var result = await LoginWithPlayfab();
            username = login_UsernameInputField.text;
            Debug.Log("Sesion iniciada");
            OnLogSuccess.Invoke();
        }
        catch (System.Exception error)
        {
            Debug.Log(error.Message);
        }
    }


    public async Task<LoginResult> LoginWithPlayfab()
    {
        //tipo de variable donde se almacena un resultado, y los devuelve
        var taskSource = new TaskCompletionSource<LoginResult>();

        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest()
        {
            Username = login_UsernameInputField.text.ToLower(),
            Password = login_PasswordInputField.text,
        };

        PlayFabClientAPI.LoginWithPlayFab(request, resultCallback => 
        taskSource.SetResult(resultCallback), errorCallback => taskSource.SetException(new System.Exception(errorCallback.GenerateErrorReport())));

        return await taskSource.Task;
    }
    
    public async void UpdateData(Dictionary<string, string> data)
    {
        try
        {
            var result = await UpdateUserData(data);
            Debug.Log("Data updated");
        }
        catch (System.Exception error)
        {
            Debug.Log(error.Message);
        }
    }

    public async Task<UpdateUserDataResult> UpdateUserData(Dictionary<string, string> data)
    {
        var taskSource = new TaskCompletionSource<UpdateUserDataResult>();

        UpdateUserDataRequest request = new UpdateUserDataRequest
        {
            Data = data
        };

        PlayFabClientAPI.UpdateUserData(request, resultCallback => 
        taskSource.SetResult(resultCallback), errorCallback => taskSource.SetException(new System.Exception(errorCallback.GenerateErrorReport())));

        return await taskSource.Task;
    }

    public async void GetData() //Se manda a llamar en un boton para imprimir toda la data en la nube
    {
        try
        {
            var result = await GetUserData();

            foreach (var item in result.Data)
            {
                Debug.Log(item.Key + ": " + item.Value.Value);
            }
        }
        catch (System.Exception error)
        {
            Debug.Log(error.Message);
        }
    }

    public async Task<GetUserDataResult> GetUserData()
    {
        var taskSource = new TaskCompletionSource<GetUserDataResult>();

        var request = new GetUserDataRequest();

        PlayFabClientAPI.GetUserData(request, resultCallback =>
        taskSource.SetResult(resultCallback), errorCallback => taskSource.SetException(new System.Exception(errorCallback.GenerateErrorReport())));

        return await taskSource.Task;
    }
}
