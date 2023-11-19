using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if !UNITY_WEBGL || UNITY_EDITOR
using UnityEngine.Windows.Speech;
#endif
using TMPro;

public class WordRecognition : MonoBehaviour
{
#if !UNITY_WEBGL || UNITY_EDITOR
    [SerializeField] private TMP_Text result;

    private KeywordRecognizer _keyRecognizer;
    private Dictionary<string, Action> wordToAction;

    private void Start()
    {
        wordToAction = new Dictionary<string, Action>();
        wordToAction.Add("Hola", Hello);

        _keyRecognizer = new KeywordRecognizer(wordToAction.Keys.ToArray());
        _keyRecognizer.OnPhraseRecognized += ProcessPhrase;
        _keyRecognizer.Start();
    }

    private void ProcessPhrase(PhraseRecognizedEventArgs args)
    {
        result.text = args.text;
        if(wordToAction.ContainsKey(args.text))
        {
            wordToAction[args.text].Invoke();
        }       
    }

    private void Hello()
    {
        Debug.Log("Hola");
    }
#endif
}
