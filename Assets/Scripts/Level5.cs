using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level5 : MonoBehaviour
{
    [SerializeField] private Text timerText;
    [SerializeField] private Sprite[] trueFalse;
    [SerializeField] private GameObject winPanel, loosPanel, frukty, levelBar, daleeButton;

    [SerializeField] private AudioClip audioIntro;
    [SerializeField] private AudioClip audioWin;
    [SerializeField] private AudioClip audioLose;

    [SerializeField] private AudioClip[] audioFrukty;
    private AudioSource audioSource;

    [SerializeField] private int currentNumber = 0; // ������� ����� �������. �������� � 1 ��� ������� �����
    private bool isGameActive = false;              // ����, �����������, ������� �� ������� ������� ���������

    private float cTime;                            // ����� ��������� �������
    private const float startTime = 4f;             // ��������� ����� 3 �������
    private bool isTimerActive;                     // ��������� �������


    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioIntro;
    }

    public void StartEmiter()
    {
        daleeButton.SetActive(true);
    }

    public void ClickStartGame()
    {
        levelBar.SetActive(true);
        frukty.SetActive(true);
        if (winPanel != null) winPanel.SetActive(false);
        if (loosPanel != null) loosPanel.SetActive(false);

        StartCoroutine(PlayAudioAndActivate());  // ��������� �������� ��� ��������������� ����� � �������� � ���������� �����
    }

    private void Update()
    {
        Timer();
    }

    private void Timer()
    {
        if (isTimerActive)
        {
            cTime -= Time.deltaTime;

            if (cTime <= 0)
            {
                Image currentImageComponent = levelBar.transform.GetChild(currentNumber).GetComponent<Image>();
                NextFaceFalse(currentImageComponent);
            }

            timerText.text = Mathf.Max(0, Mathf.FloorToInt(cTime)).ToString();
        }
    }

    private IEnumerator PlayAudioAndActivate()  // ���� �� ������������, ����� ���������� ����
    {
        audioSource.Play();

        yield return new WaitForSeconds(audioIntro.length);

        StartNextRound();
        isTimerActive = true;
    }

    private void StartNextRound()
    {
        if (currentNumber < 6)
        {
            cTime = startTime;
            audioSource.clip = audioFrukty[currentNumber];
            audioSource.Play();
            isGameActive = true;
        }
        else
        {
            CheckWin();  // �������� ����� ��� ��������� ������
        }
    }

    private void NextFaceTrue(Image face)       // ����� ��� ��������� ����������� ������
    {
        currentNumber++;
        face.sprite = trueFalse[1];             // ������������� ������ ��� ����������� ������
        StartNextRound();                       // ��������� � ���������� ������
    }

    private void NextFaceFalse(Image face)      // ����� ��� ��������� ������������� ������
    {
        currentNumber++;
        face.sprite = trueFalse[0];                   // ������������� ������ ��� ������������� ������
        StartNextRound();                             // ��������� � ���������� ������
    }

    public void CheckClicked(int value)
    {
        if (!isGameActive) return;

        Image currentImageComponent = levelBar.transform.GetChild(currentNumber).GetComponent<Image>(); // �������� ��������� ����������� ��� �������� ������� ������

        if (value == currentNumber) // ���� �������� ��������� � ������� �������
        {
            NextFaceTrue(currentImageComponent); // ��������� � ������ ��� ����������� ������
        }
        else // ���� �������� �� ���������
        {
            NextFaceFalse(currentImageComponent); // ��������� � ������ ��� ������������� ������
        }
    }

    private void CheckWin()
    {
        isGameActive = false; // ������������ ��������� ����
        isTimerActive = false;
        bool looseSlot = false;

        for (int i = 0; i < levelBar.transform.childCount; i++)
        {
            Image currentImageComponent = levelBar.transform.GetChild(i).GetComponent<Image>();
            if (currentImageComponent.sprite != trueFalse[1]) looseSlot = true;
        }

        if (looseSlot)
        {
            loosPanel.SetActive(true);
            audioSource.clip = audioLose;
            audioSource.Play();
        }
        else
        {
            winPanel.SetActive(true);
            audioSource.clip = audioWin;
            audioSource.Play();
        }
    }
}
