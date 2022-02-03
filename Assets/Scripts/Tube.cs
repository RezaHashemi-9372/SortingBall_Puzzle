using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tube : MonoBehaviour
{
    [SerializeField, Range(0, 100)]
    private int score = 20;
    [SerializeField]
    private AudioClip success;

    private AudioSource audioSource;
    private Transform childPos;
    public static GameObject previousSelected;
    public static bool checking = false;
    private List<GameObject> ballContainer = new List<GameObject>();
    private GameMode gameMode;


    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = success;
        childPos = this.transform.Find("testPos");
        gameMode = FindObjectOfType<GameMode>();
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (checking)
        {
            return;
        }
        //Debug.Log("The ball containner for emty tube is: " + ballContainer.Count);
        if (previousSelected == null)
        {
            previousSelected = this.Raycast();
            previousSelected.GetComponent<Ball>().NextPosition(childPos.transform.position);
            //previousSelected.GetComponent<Rigidbody2D>().isKinematic = true;
            previousSelected.GetComponent<Rigidbody2D>().mass = 0;
            previousSelected.GetComponent<Rigidbody2D>().gravityScale = 0;
            checking = true;
        }
        else if (previousSelected != null )
        {
            if (this.Raycast() == null)
            {
                previousSelected.GetComponent<Ball>().NextPosition(this.childPos.transform.position);
                previousSelected = null;
            }
            else if (previousSelected == this.Raycast())
            {
                previousSelected.GetComponent<Rigidbody2D>().isKinematic = false;
                previousSelected = null;
            }
            else if (this.Raycast().GetComponent<SpriteRenderer>().color == previousSelected.GetComponent<SpriteRenderer>().color && this.ballContainer.Count < 4)
            {
                previousSelected.GetComponent<Ball>().NextPosition(this.childPos.transform.position);
                //previousSelected.GetComponent<Rigidbody2D>().isKinematic = false;
                previousSelected = null;
            }
            
            else
            {
                previousSelected.GetComponent<Rigidbody2D>().isKinematic = false;
                previousSelected = null;
                previousSelected = this.Raycast();
                previousSelected.GetComponent<Rigidbody2D>().mass = 0;
                previousSelected.GetComponent<Rigidbody2D>().gravityScale = 0;
                previousSelected.GetComponent<Ball>().NextPosition(this.childPos.transform.position);
            }
        }
        /*else
        {
            previousSelected.GetComponent<Ball>().NextPosition(this.childPos.transform.position);
            //previousSelected.GetComponent<Rigidbody2D>().isKinematic = false;
            
        }*/
    }
    
    private void DeSelect(GameObject obj)
    {
        obj.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Ball>().isTriggered = true;
        if (!ballContainer.Contains(collision.gameObject))
        {
            collision.transform.GetComponent<Ball>().isTriggered = true;
            ballContainer.Add(collision.gameObject);
        }
        CheckForEquality();
        /*if (CheckForEquality() == true)
        {
            this.GetComponent<BoxCollider2D>().enabled = false;
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ballContainer.Contains(collision.gameObject))
        {
            ballContainer.Remove(collision.gameObject);
        }
    }

    private void CheckForEquality()
    {
        if (ballContainer.Count == 0)
        {
            return;
        }
        if (ballContainer.Count == 4)
        {
            //Debug.Log("You are lucky guy bro it's 4.");
        }
        //int counter = 0;
        if (ballContainer.Count == 4)
        {
            if (ballContainer[0].GetComponent<Ball>().Color == ballContainer[1].GetComponent<Ball>().Color &&
                ballContainer[0].GetComponent<Ball>().Color == ballContainer[2].GetComponent<Ball>().Color)
            {
                if (ballContainer[0].GetComponent<Ball>().Color == ballContainer[3].GetComponent<Ball>().Color)
                {
                    //Debug.Log("It's full you can deactivate it's collider");
                    this.GetComponent<BoxCollider2D>().enabled = false;
                    audioSource.Play();
                    gameMode.AddScore(score);
                    gameMode.GameOver();
                }
            }
        }
    }


    public GameObject Raycast()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(childPos.transform.position, Vector2.down, 2.0f, ~LayerMask.GetMask("Tube"));
        if (hit && hit.collider.GetComponent<Ball>())
        {
            //hit.collider.GetComponent<Ball>().NextPosition(childPos.transform.position);
            return hit.collider.gameObject;
        }
        /*else if (!hit)
        {
            return null;
        }*/
        return null;
    }


}
