using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using LSL4Unity.Utils;

public class PlayerMovement : MonoBehaviour
{
    private Animator penAnimator;
    private bool isJump = false;
    private bool isTop = false;

    public bool isPlay = true;

    public float jumpHeight = 0;
    public float jumpSpeed = 0;
    private Vector2 startPosition;

    public string StreamName;// OpenViBE에서 StreamName과 동일해야 함
    ContinuousResolver resolver;
    double max_chunk_duration = 0.5;//OpenViBE에서 Epoch Interval과 동일
    private StreamInlet inlet;
    [@SerializeField] private GameObject retryUI;
    private float[,] data_buffer;//EEG data를 저장하기 위한 버퍼
    private double[] timestamp_buffer;
    float EEGpow;//EEG power를 계산하기 위한 변수


    void Awake()
    {
        if (!StreamName.Equals(""))
            resolver = new ContinuousResolver("name", StreamName);
        else
        {
            Debug.LogError("Object must specify a name for resolver to lookup a stream.");
            this.enabled = false;
            return;
        }
        StartCoroutine(ResolveExpectedStream());
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        penAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(isPlay == true){
            penAnimator.SetBool("isWalk", true);
        }
        if(inlet != null){//뇌파로 점프하기기
            int samples_returned = inlet.pull_chunk(data_buffer, timestamp_buffer);
            if (samples_returned > 0)
            {
                float x = data_buffer[samples_returned - 1, 0];
                EEGpow = x;
                Debug.Log(EEGpow);

                if(EEGpow > 9000){
                    isJump = true;
                    //  Run 애니메이션 -> Jump 애니메이션
                    penAnimator.SetBool("isJump", true);
                } 
                else if (transform.position.y <= startPosition.y){
                    isJump = false;
                    isTop = false;
                    transform.position = startPosition;
                    
                    // Jump 애니메이션 -> Run 애니메이션 
                    penAnimator.SetBool("isJump", false);

                }
            }
        }

      
        /*if (Input.GetKeyDown(KeyCode.Space)) // spacebar 누르면 점프하기
        {
            isJump = true;
            //  Run 애니메이션 -> Jump 애니메이션
            penAnimator.SetBool("isJump", true);

        }
        else if (transform.position.y <= startPosition.y)
        {
            isJump = false;
            isTop = false;
            transform.position = startPosition;
            
            // Jump 애니메이션 -> Run 애니메이션 
            penAnimator.SetBool("isJump", false);

        }
        
        */
        if (isJump)
        {
            if (transform.position.y <= jumpHeight - 0.1f && !isTop)
            {
                transform.position = Vector2.Lerp(transform.position,new Vector2(transform.position.x, jumpHeight), jumpSpeed * Time.deltaTime);
            }
            else
            {
                isTop = true;
            }

            if (transform.position.y > startPosition.y && isTop)
            {
                transform.position = Vector2.MoveTowards(transform.position,startPosition, jumpSpeed * Time.deltaTime);
            }
        } 
    }

      IEnumerator ResolveExpectedStream()
    {
        var results = resolver.results();
        while (results.Length == 0)
        {
            yield return new WaitForSeconds(.1f);
            results = resolver.results();
        }
        inlet = new StreamInlet(results[0]);
        var streamInfo = inlet.info();
        int buf_samples = (int)Mathf.Ceil((float)(streamInfo.nominal_srate() * max_chunk_duration));
        int n_channels = streamInfo.channel_count();
        data_buffer = new float[buf_samples, n_channels];
        timestamp_buffer = new double[buf_samples];
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            retryUI.SetActive(true);
            penAnimator.SetBool("isWalk", false);
            isPlay = false;
        }
    }
}
