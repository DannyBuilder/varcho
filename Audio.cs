using UnityEngine;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    public AudioClip letterSound;

    private Button button;
    private AudioSource audioSource;

    private void Start()
    {
        button = GetComponent<Button>();
        audioSource = GetComponent<AudioSource>();

        button.onClick.AddListener(PlayLetterSound);
    }

    private void PlayLetterSound()
    {
        audioSource.clip = letterSound;
        audioSource.Play();
    }
}
