using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenuMB : MonoBehaviour
{
    public Ring Data;
    public RingCakePiece RingCakePiecePrefab;
    public float GapWidthDegree = 1f;
    public Action<string> callback;
    protected RingCakePiece[] Pieces;
    protected RingMenuMB Parent;
    public string Path;

    void Start()
    {
        var stepLength = 360f / Data.Elements.Length;
        var iconDist = Vector3.Distance(RingCakePiecePrefab.Icon.transform.position, RingCakePiecePrefab.CakePiece.transform.position);

        //Position it
        Pieces = new RingCakePiece[Data.Elements.Length];

        for (int i = 0; i < Data.Elements.Length; i++)
        {
            Pieces[i] = Instantiate(RingCakePiecePrefab, transform);
            //set root element
            Pieces[i].transform.localPosition = Vector3.zero;
            Pieces[i].transform.localRotation = Quaternion.identity;

            //set cake piece
            Pieces[i].CakePiece.fillAmount = 1f / Data.Elements.Length - GapWidthDegree / 360f;
            Pieces[i].CakePiece.transform.localPosition = Vector3.zero;
            Pieces[i].CakePiece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + GapWidthDegree / 2f + i * stepLength);
            Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.5f);

            //set icon
            Pieces[i].Icon.transform.localPosition = Pieces[i].CakePiece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
            Pieces[i].Icon.sprite = Data.Elements[i].Icon;

        }
    }

    private void Update()
    {
        var stepLength = 360f / Data.Elements.Length;
        var mouseAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up, Input.mousePosition - transform.position, Vector3.forward) + stepLength / 2f);
        var activeElement = (int)(mouseAngle / stepLength);
        for (int i = 0; i < Data.Elements.Length; i++)
        {
            if (i == activeElement)
                Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.75f);
            else
                Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.5f);
        }


        if (Input.GetMouseButtonDown(0))
        {
            var path = Path + "/" + Data.Elements[activeElement].Name;
            if (Data.Elements[activeElement].NextRing != null)
            {
                var newSubRing = Instantiate(gameObject, transform.parent).GetComponent<RingMenuMB>();
                newSubRing.Parent = this;
                for (var j = 0; j < newSubRing.transform.childCount; j++)
                    Destroy(newSubRing.transform.GetChild(j).gameObject);
                newSubRing.Data = Data.Elements[activeElement].NextRing;
                newSubRing.Path = path;
                newSubRing.callback = callback;
            }
            else
            {
                callback?.Invoke(path);
            }
            gameObject.SetActive(false);
        }
    }

    private float NormalizeAngle(float a) => (a + 360f) % 360f;
    //public Ring Data;
    //public RingCakePiece RingCakePiecePrefab;
    //public float GapWidthDegree = 1f;
    //public Action<string> Callback;
    //protected RingCakePiece[] Pieces;
    //protected RingMenuMB Parent;
    //[HideInInspector]
    //public string Path;

    //private void Start()
    //{
    //    var stepLength = 360f / Data.Elements.Length;
    //    var iconDist = Vector3.Distance(RingCakePiecePrefab.Icon.transform.position, RingCakePiecePrefab.CakePiece.transform.position);

    //    //Position it
    //    Pieces = new RingCakePiece[Data.Elements.Length];

    //    for (int i = 0; i < Data.Elements.Length; i++)
    //    {
    //        Pieces[i] = Instantiate(RingCakePiecePrefab, transform);
    //        //set root element
    //        Pieces[i].transform.localPosition = Vector3.zero;
    //        Pieces[i].transform.localRotation = Quaternion.identity;

    //        //set cake piece
    //        Pieces[i].CakePiece.fillAmount = 1f / Data.Elements.Length - GapWidthDegree / 360f;
    //        Pieces[i].CakePiece.transform.localPosition = Vector3.zero;
    //        Pieces[i].CakePiece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + GapWidthDegree / 2f + i * stepLength);
    //        Pieces[i].CakePiece.color = new Color(1f, 1f, 1f, 0.5f);

    //        //set icon
    //        Pieces[i].Icon.transform.localPosition = Pieces[i].CakePiece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
    //        Pieces[i].Icon.sprite = Data.Elements[i].Icon;

    //    }
    //}

    //private void Update()
    //{
    //    var stepLength = 360f / Data.Elements.Length;
    //    var NewMouse = Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f);
    //    var mouseAngle = NormalizeAngle(Vector3.SignedAngle(Vector3.up, Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f), Vector3.forward) + stepLength / 2f);

    //    var activeElement = (int)(mouseAngle / stepLength);
    //    for (int i = 0; i < Data.Elements.Length; i++)
    //    {
    //        if (i == activeElement)
    //            Pieces[i].CakePiece.color = new Color(0.05f, 0.05f, 0.05f, 1f);
    //        else
    //            Pieces[i].CakePiece.color = new Color(.1f, .1f, .1f, 1f);
    //    }


    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        var path = Path + "/" + Data.Elements[activeElement].Name;
    //        if (Data.Elements[activeElement].NextRing != null)
    //        {
    //            var newSubRing = Instantiate(gameObject, transform.parent).GetComponent<RingMenuMB>();
    //            newSubRing.Parent = this;
    //            for (var j = 0; j < newSubRing.transform.childCount; j++)
    //                Destroy(newSubRing.transform.GetChild(j).gameObject);
    //            newSubRing.Data = Data.Elements[activeElement].NextRing;
    //            newSubRing.Path = path;
    //            newSubRing.Callback = Callback;
    //        }
    //        else
    //        {
    //            Callback?.Invoke(path);
    //        }
    //        gameObject.SetActive(false);
    //    }
    //}
    //private float NormalizeAngle(float _a) => (_a + 360f) % 360f;
}
