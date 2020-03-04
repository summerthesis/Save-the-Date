﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expendable_Carousel : MonoBehaviour
{
    [SerializeField] private PrimitiveType m_pTypeToCreate;
    [SerializeField] private float m_fRotationSpeed;
    [SerializeField] private float m_fCarouselRadius;
    [SerializeField] private float m_fPlatformSize;
    [SerializeField] private bool m_bShouldLookAtCenter;
    [SerializeField] private bool m_bShouldLookAwayFromCenter;
    [SerializeField] private GameObject[] m_Platforms;

    float angle;
    private float angleDiv;

    #region EditorVariables
    public int NumberOfPlatforms { get { return m_Platforms.Length; } }
    public float CarouselRadius { get { return m_fCarouselRadius; } }
    public float PlatformSize { get { return m_fPlatformSize; } }

    #endregion

    void Awake() {

        angle = m_Platforms.Length > 0 ? 2 * Mathf.PI / m_Platforms.Length : 0;
        for  (int i = 0; i < m_Platforms.Length; ++i) {
            angleDiv = i * angle;
            m_Platforms[i] = GameObject.CreatePrimitive(m_pTypeToCreate);
            m_Platforms[i].AddComponent<expendable_PlatformParenting>();
            m_Platforms[i].transform.parent = transform;
            m_Platforms[i].transform.position = this.gameObject.transform.position + 
                transform.TransformDirection(
                    new Vector3(Mathf.Cos(angleDiv), 0,
                                Mathf.Sin(angleDiv)))*
                m_fCarouselRadius;
            switch (m_pTypeToCreate) {
                case PrimitiveType.Sphere:
                case PrimitiveType.Capsule:
                case PrimitiveType.Plane:
                case PrimitiveType.Quad:
                    m_Platforms[i].transform.localScale = Vector3.one * m_fPlatformSize;
                    break;
                case PrimitiveType.Cylinder:
                    Destroy(m_Platforms[i].GetComponent<Collider>());
                    m_Platforms[i].transform.localScale = new Vector3(m_fPlatformSize, 0.5f, m_fPlatformSize);
                    m_Platforms[i].AddComponent<BoxCollider>();
                    break;
                case PrimitiveType.Cube:
                    m_Platforms[i].transform.localScale = new Vector3(m_fPlatformSize, 1, m_fPlatformSize);
                    break;
                default:
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.up, m_fRotationSpeed);

        if (m_pTypeToCreate != PrimitiveType.Quad) {
            for (int i = 0; i < m_Platforms.Length; ++i) {
                angleDiv = angle * i;
                m_Platforms[i].transform.rotation = Quaternion.identity; //REFACTOR
                if (m_bShouldLookAtCenter)
                {
                    //Vector3 endResult = m_Platforms[i].transform.position - this.gameObject.transform.position;
                    //m_Platforms[i].transform.LookAt(this.gameObject.transform.position);
                }
                else if (m_bShouldLookAwayFromCenter)
                {
                    //Vector3 endResult = m_Platforms[i].transform.position - this.gameObject.transform.position;
                    //m_Platforms[i].transform.LookAt(endResult * -1);
                }
                else {
                    m_Platforms[i].transform.rotation = Quaternion.identity;
                }
            }
        } else {
            foreach (GameObject g in m_Platforms) {
                Vector3 endResult = g.transform.position - this.gameObject.transform.position;
                g.transform.LookAt(endResult*-1);
                g.transform.Rotate(this.gameObject.transform.up, 180, Space.Self);
            }
        }
    }
}
