using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class AIModelGenerator : MonoBehaviour
{
    private string apiURL = "https://api.meshy.ai/openapi/v2/text-to-3d";
    private string apiKey = "msy_hXki4Ucd1JwWUXxvpLFgFPuJmCe5TWo32prZ"; // Replace with your actual API key

    public TMP_InputField promptInput;

    public void GenerateBtn()
    {
        GenerateModel(promptInput.text);
    }

    public void GenerateModel(string prompt)
    {
        StartCoroutine(SendRequest(prompt));
    }

    IEnumerator SendRequest(string prompt)
    {
        // Create JSON request body
        string jsonData = "{\"mode\": \"preview\", \"prompt\": \"" + prompt + "\", \"art_style\": \"realistic\", \"should_remesh\": true}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest request = new UnityWebRequest(apiURL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Sending request to: " + apiURL);
            Debug.Log("Request body: " + jsonData);

            yield return request.SendWebRequest();

            Debug.Log("Response Code: " + request.responseCode);
            Debug.Log("Response Text: " + request.downloadHandler.text);

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Extract model ID from response
                string jsonResponse = request.downloadHandler.text;
                string modelID = ExtractModelID(jsonResponse);

                if (!string.IsNullOrEmpty(modelID))
                {
                    Debug.Log("Model ID: " + modelID);
                    StartCoroutine(DownloadModel(modelID));
                }
            }
            else
            {
                Debug.LogError("Request failed: " + request.responseCode + " " + request.error);
            }
        }
    }

    string ExtractModelID(string jsonResponse)
    {
        // Simple JSON parsing (modify if needed)
        int startIndex = jsonResponse.IndexOf(": \"") + 3;
        int endIndex = jsonResponse.IndexOf("\"", startIndex);
        return jsonResponse.Substring(startIndex, endIndex - startIndex);
    }

    IEnumerator DownloadModel(string modelID)
    {
        string modelURL = "https://api.meshy.ai/openapi/v2/models/" + modelID; // Example URL format (check API docs)

        using (UnityWebRequest request = UnityWebRequest.Get(modelURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                byte[] modelData = request.downloadHandler.data;
                string filePath = Application.persistentDataPath + "/generated_model.glb";
                System.IO.File.WriteAllBytes(filePath, modelData);
                Debug.Log("Model saved at: " + filePath);

                LoadModel(filePath);
            }
            else
            {
                Debug.LogError("Model download failed: " + request.error);
            }
        }
    }

    void LoadModel(string path)
    {
        Debug.Log("Load 3D Model from: " + path);
        // Use GLTF or FBX importer to load the model dynamically
    }
}
