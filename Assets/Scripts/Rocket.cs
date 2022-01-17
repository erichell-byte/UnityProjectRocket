using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class Rocket : MonoBehaviour
{
    [SerializeField] float rotSpeed = 100f;

    [SerializeField] Text energyText;

    [SerializeField] float flySpeed = 100f;
    [SerializeField] int energyTotal = 2000;
    [SerializeField] float energyApply = 10;
    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip boomSound;
    [SerializeField] AudioClip finishSound;
    [SerializeField] ParticleSystem flyParticles;
    [SerializeField] ParticleSystem finishParticles;
    [SerializeField] ParticleSystem boomParticles;

    bool collisionOff = false;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State {Playing, Dead, NextLevel};

    State state = State.Playing;
    // Start is called before the first frame update
    void Start()
    {
        energyText.text = energyTotal.ToString();
        state = State.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Playing && energyTotal > 0)
        {
            Launch();
            Rotation();
            if (Debug.isDebugBuild)
                DebugKeys();
        }
    }

    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionOff = !collisionOff;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Playing || collisionOff)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "friendly":
                print("ok");
                break;
            case "Battery":
                PlusEnergy(800, collision.gameObject) ;
                break;
            case "Finish":
                Finish();
                break;
            default:
                Lose();
                break;
        }
    }
    

void PlusEnergy(int energyToAdd, GameObject batteryObj)
{
        batteryObj.GetComponent<BoxCollider>().enabled = false;
        energyTotal += energyToAdd;
        energyText.text = energyTotal.ToString();
        Destroy(batteryObj);
}
void Lose()
{
    audioSource.Stop();
    audioSource.PlayOneShot(boomSound);
    state = State.Dead;
    boomParticles.Play();
    Invoke("LoadFirstLevel", 2f); 
}
void Finish()
{
    state = State.NextLevel;
    audioSource.Stop();
    audioSource.PlayOneShot(finishSound);
    finishParticles.Play();
    Invoke("LoadNextLevel", 2f);
}
void LoadNextLevel() // finish
{
    int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    if (currentLevelIndex + 1 == SceneManager.sceneCountInBuildSettings)
    {
        // print(SceneManager.sceneCountInBuildSettings);
        currentLevelIndex = -1;
    }
    SceneManager.LoadScene(currentLevelIndex + 1); 
}
    
void LoadFirstLevel() //lose
{
    SceneManager.LoadScene(0);
}
    void Launch()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            energyTotal -= Mathf.RoundToInt(energyApply * Time.deltaTime);
            energyText.text = energyTotal.ToString();
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
                audioSource.PlayOneShot(flySound);
            flyParticles.Play();
        }
        else
        {
            audioSource.Pause();
            flyParticles.Stop();
        }
        
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
