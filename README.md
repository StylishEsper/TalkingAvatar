A controllable character that can speak. Created with Amazon Polly tts and Oculus lip sync.

Steps:

1. Open the project in Unity.

2. To get Amazon Polly working, open Assets/Scripts/TextToSpeech.cs and add your Amazon Access Key and Secret Key in the BasicAWSCredentials object.

3. Open the Playground scene in Unity and find Cesar in the hierarchy.

4. Under Cesar, you will find a GameObject named Vocal.

5. Click on Vocal, and from the inspector, scroll to find the TextToSpeech component.

6. Under speech, type the dialogue you want the character to speak.

7. Enter Play mode and check the "Speak" checkbox in the TextToSpeech component to make the character start speaking.
