using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Text;



public class LLM_manager : MonoBehaviour
{

    public GameObject partText;
    public TMP_InputField inputPrompt;
    public TMP_InputField text;
    public Transform scrollContent;
    string currentContext;

    [System.Serializable]
    public class RequestData
    {
        public string model;
        public string prompt;
        public bool stream = false;
        
    }

    public string url = "http://localhost:11434/api/generate";
    public string model = "hf.co/QuantFactory/saiga_llama3_8b-GGUF:Q6_K";
    public string prompt = "Привет, модель";

    private void Start()
    {
        ScenarioOrientaionSystemIssue();
    }
    public void ScenarioOrientaionSystemIssue()
    {
        //Сбой в системе ореинтации
        currentContext += "-Земля, у нас тут нарушении в системе ориентации, приём...";

        GameObject newPartText = Instantiate(partText, scrollContent.position, Quaternion.identity, scrollContent);
        TMP_Text newText = newPartText.GetComponent<TMP_Text>();
        newText.text = "-Земля, у нас тут нарушении в системе ориентации, приём...";


    }

    public void Send()
    {
        //Берём ответ
        prompt = inputPrompt.text;
       
        //спавн обьекта
        GameObject newPartText = Instantiate(partText, scrollContent.position, Quaternion.identity, scrollContent);
        TMP_Text newText = newPartText.GetComponent<TMP_Text>();
        newText.text = "- " + prompt;
        //формируем запрос нейронке
        prompt = "Текущий контекст диалога: " + currentContext + "Ответ игрока: " + inputPrompt.text + "Твоя задача сформировавть ответ, " +
            "не отходя и развивая сюжет, " +
            "ответ должен быть формата ответа станции из космоса от лица космонавта";
        //Дополняем котекст 
        currentContext += "Ответ игрока: " + inputPrompt.text;
        //Отправляем запрос
        StartCoroutine("SendPrompt");
        //Очищаем поле ввода
        inputPrompt.text = "";

    }

    [System.Serializable]
    public class OllamaResponse
    {
        public string model;
        public string create_at;
        public string response;
        public bool done;
    }

    public OllamaResponse ParseOllamaResponse(string json)
    {
        return JsonUtility.FromJson<OllamaResponse>(json);
    }
    private IEnumerator SendPrompt()
    {
        var reqObj = new RequestData
        {
            model = model,
            prompt = prompt,
            stream = false
        };

        string json = JsonUtility.ToJson(reqObj);
        byte[] body = Encoding.UTF8.GetBytes(json);

        using (var www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(body);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            print("Запрос отправлен... ");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ну да, всё как всегда... ");
            }
            else 
            { 
                Debug.Log("Ответ модели:");
                OllamaResponse response = ParseOllamaResponse(www.downloadHandler.text);
                Debug.Log(response.response);

                //спавним текст
                GameObject newPartText = Instantiate(partText, scrollContent.position, Quaternion.identity, scrollContent);
                TMP_Text newText = newPartText.GetComponent<TMP_Text>();
                newText.text = "- " + response.response;

                //Обновляем контекст
                currentContext += response.response;
            }
        }
    }
}
 