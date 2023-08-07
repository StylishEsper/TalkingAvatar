using System.Collections;
using System.IO;
using UnityEngine;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Amazon.Runtime;
using UnityEngine.Networking;

public class TextToSpeech : MonoBehaviour
{
    private BasicAWSCredentials credentials = new BasicAWSCredentials("", "");
    private AmazonPollyClient client;
    private AudioSource audioSource;

    [SerializeField] [TextArea] private string speech;
    private string dataPath = $"{Application.dataPath}/audio.mp3";
    private string oldText;

    [SerializeField] private bool speak;
    private bool isSpeaking;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        client = new AmazonPollyClient(credentials, RegionEndpoint.CACentral1);
    }

    private void Update()
    {
        if (speak && !isSpeaking)
        {
            StartCoroutine(Speak());
            speak = false;
        }
    }

    private IEnumerator Speak()
    {
        isSpeaking = true;

        if (oldText == speech)
        {
            audioSource.Play();

            yield return new WaitForSeconds(audioSource.clip.length);
            isSpeaking = false;

            yield break;
        }

        var request = new SynthesizeSpeechRequest()
        {
            Text = speech,
            Engine = Engine.Neural,
            VoiceId = VoiceId.Joey,
            OutputFormat = OutputFormat.Mp3
        };

        var response = client.SynthesizeSpeechAsync(request);
        yield return response.IsCompleted;
        WriteIntoFile(response.Result.AudioStream);

        using (var www = UnityWebRequestMultimedia.GetAudioClip(dataPath, AudioType.MPEG))
        {
            var op = www.SendWebRequest();
            yield return op.isDone;
            var clip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.clip = clip;
            audioSource.Play();
            yield return new WaitForSeconds(clip.length);
            isSpeaking = false;
            oldText = speech;
        }
    }

    private void WriteIntoFile(Stream stream)
    {
        using (var fileStream = new FileStream(dataPath, FileMode.Create))
        {
            byte[] buffer = new byte[8 * 1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, bytesRead);
            }
        }
    }
}