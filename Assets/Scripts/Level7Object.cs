using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level7Object : MonoBehaviour
{
    Level7 Level7;

    public int tagValue; // ������ ���������� ��� �������� �������� ����

    private void Awake()
    {
        Level7 = FindObjectOfType<Level7>();
    }

    private void OnMouseDown()
    {
        string tag = gameObject.tag;
        int.TryParse(tag, out int parsedValue);
        tagValue = parsedValue;

        Level7.CheckClicked(tagValue);
    }
}
