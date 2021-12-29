using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Rocket : MonoBehaviour
{
    [SerializeField] float rotSpeed = 100f;

    [SerializeField] float flySpeed = 100f;
    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip boomSound;
    [SerializeField] AudioClip finishSound;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State {Playing, Dead, NextLevel};

    State state = State.Playing;
    // Start is called before the first frame update
    void Start()
    {
        state = State.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Playing)
        {
            Launch();
            Rotation();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Playing)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "friendly":
                print("ok");
                break;
            case "Battery":
                print("+energy");
                break;
            case "Finish":
                state = State.NextLevel;
                audioSource.PlayOneShot(finishSound);
                Invoke("LoadNextLevel", 2f);
                break;
            default:
                audioSource.PlayOneShot(boomSound);
                state = State.Dead;
                Invoke("LoadFirstLevel", 2f);
            break;
        }
    }
    
void LoadNextLevel() // finish
{
    SceneManager.LoadScene(1);
}
    
void LoadFirstLevel() //lose
{
    SceneManager.LoadScene(0);
}
    void Launch()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * flySpeed);
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(flySound); 
        }
        else
            audioSource.Pause();
    }
    void Rotation()
    {
        float rotationSpeed = rotSpeed * Time.deltaTime;
        rigidBody.freezeRotation = true;   
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        rigidBody.freezeRotation = false;
    }

}
