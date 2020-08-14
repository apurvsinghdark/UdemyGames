using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource rocketAudio;
    
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;
    
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;
    
    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem deathParticle;

    bool collisionDisable = false;

    enum State { Alive , Trancending , Dead}
    [SerializeField]State state = State.Alive;

    private Rocket rocket;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rocketAudio = GetComponent<AudioSource>();
        rocket = GetComponent<Rocket>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondOnThrustApply();
            RespondOnRotationApply();
            // rocketAudio.enabled = false;
        }
        if (Debug.isDebugBuild)
        {
            RespondOnDebugKey();
        }
    }

    private void RespondOnDebugKey()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionDisable) { return; }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dead;
        rocketAudio.Stop();
        rocketAudio.PlayOneShot(death);
        deathParticle.Play();
        Invoke("DeadScene", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Trancending;
        rocketAudio.Stop();
        rocketAudio.PlayOneShot(success);
        successParticle.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void DeadScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        var loadNextScene = currentScene + 1;
        if(loadNextScene == SceneManager.sceneCountInBuildSettings ) { loadNextScene = 0; }   
        SceneManager.LoadScene(loadNextScene);
    }

    void RespondOnThrustApply()
    {
        float rotationThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            mainEngineParticle.Play();
            ApplyThrust(rotationThisFrame);
        }
        else
        {
            StopApplyThrust();
        }
    }

    private void StopApplyThrust()
    {
        if (rocketAudio.isPlaying)
            rocketAudio.Stop();

        mainEngineParticle.Stop();
    }

    void ApplyThrust(float rotationThisFrame)
    {
        rigidbody.AddRelativeForce(Vector3.up * rotationThisFrame);
        if (!rocketAudio.isPlaying) 
        { 
            rocketAudio.PlayOneShot(mainEngine);
        }
    }

    private void RespondOnRotationApply()
    {
        rigidbody.angularVelocity = Vector3.zero;

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        
        //Rotate
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }
}
