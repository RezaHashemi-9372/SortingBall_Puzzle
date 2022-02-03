using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private AudioClip move;

    private AudioSource audioSource;
    private Vector3 savedScale;
    public Vector3 nextPosition;
    private int counter = 0;
    public bool isSetted = false;
    public bool isTriggered = false;

    private void Awake()
    {
        savedScale = this.transform.localScale;
        audioSource = FindObjectOfType<AudioSource>();
        audioSource.clip = move;
    }
    public Color Color
    {
        get
        {
            return this.GetComponent<SpriteRenderer>().color;
        }
        set
        {
            this.GetComponent<SpriteRenderer>().color = value;
        }
    }
    void Update()
    {
        if (isSetted)
        {
            this.GetComponent<CircleCollider2D>().enabled = false;
            this.transform.position = Vector3.Lerp(this.transform.position, nextPosition, 40.0f * Time.deltaTime);

            if (this.transform.position == nextPosition)
            {
                this.GetComponent<CircleCollider2D>().enabled = true;
                isSetted = false;
                Tube.checking = false;
                //this.GetComponent<Rigidbody2D>().isKinematic = false ? true : false;
                if (this.GetComponent<Rigidbody2D>().isKinematic == false)
                {
                    this.GetComponent<Rigidbody2D>().isKinematic = true;
                    this.GetComponent<Rigidbody2D>().mass = 3;
                    this.GetComponent<Rigidbody2D>().gravityScale = 3;
                }
                else if (this.GetComponent<Rigidbody2D>().isKinematic == true)
                {
                    this.GetComponent<Rigidbody2D>().isKinematic = false;
                }
            }
        }

        if (isTriggered)
        {
            //Debug.Log("Changing the scale is working..!!");
            counter++;
           // Debug.Log("The counter is: " + counter); 
            this.transform.localScale = Vector3.Lerp(this.transform.localScale,
                new Vector3(.225f,.1f),
                60.0f * Time.deltaTime);
            //this.transform.localScale = savedScale;

            if (this.transform.localScale.x  <= .226f)
            {
                this.transform.localScale = savedScale;
            }
        }

    }

    public void NextPosition(Vector3 newpos)
    {
        isSetted = true;
        nextPosition = newpos;
        //this.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isTriggered = false;
        this.transform.localScale = savedScale;
        audioSource.Play();
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        audioSource.Play();
    }

    public Ball SetColor(Color color)
    {
        this.Color = color;
        return this;
    }
}
