using UnityEngine;
using Whisper.Utils;
using Whisper;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class WhisperRecognition : MonoBehaviour
{
#if !UNITY_WEBGL || UNITY_EDITOR
    private PlayerInputs _playerInputs;

    [SerializeField] private WhisperManager whisper;
    [SerializeField] MicrophoneRecord microphoneRecord;
    public bool streamSegments = true;

    [SerializeField] private TMP_Text result;

    private string _buffer;

    public static Action<string> OnPlayerTalked;

    private void Awake()
    {
        _playerInputs = new PlayerInputs();
        whisper.OnNewSegment += NewSegment;
        microphoneRecord.OnRecordStop += StopRecordAsync;
        _playerInputs.Enable();
        _playerInputs.Speak.Greet.performed += Speak;
        _playerInputs.Speak.Greet.canceled += FinishTalking;
    }

    private void OnDisable()
    {
        whisper.OnNewSegment -= NewSegment;
        microphoneRecord.OnRecordStop -= StopRecordAsync;
        _playerInputs.Speak.Greet.performed -= Speak;
        _playerInputs.Speak.Greet.canceled -= FinishTalking;
        _playerInputs.Disable();
    }

    private void Speak(InputAction.CallbackContext context)
    {
        if (microphoneRecord.IsRecording) return;
        microphoneRecord.StartRecord();
    }

    private void FinishTalking(InputAction.CallbackContext context)
    {
        microphoneRecord.StopRecord();
    }

    private async void StopRecordAsync(AudioChunk recordedAudio)
    {
        _buffer = "";

        var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
        if (res == null)
            return;

        result.text = res.Result;
        OnPlayerTalked?.Invoke(res.Result);
    }

    private void NewSegment(WhisperSegment segment)
    {
        if (!streamSegments)
            return;

        _buffer += segment.Text;
        result.text = _buffer + "...";
    }
#endif
}
