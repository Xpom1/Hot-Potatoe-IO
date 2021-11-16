using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Potatoes
{
    public class Player : MonoBehaviour
    {
        private GameObject player;
        private GameObject bot;
        private float speedMove;
        private float gravityForse;
        private float jumpPower;
        private Vector3 moveVector;
        private CharacterController ch_controller;
        [SerializeField] private MobileController mController;
        private GameObject gg;

        public bool Potatoes;

        //private Transform cam;

        void Start()
        {
            //cam = Camera.main.transform;

            Potatoes = true;
            player = this.gameObject;
            player.tag = "Player";

            player.AddComponent<CharacterController>();
            player.AddComponent<Rigidbody>();
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ch_controller = GetComponent<CharacterController>();

            player.GetComponent<CapsuleCollider>().radius = 0.6f;

            player.GetComponent<MeshRenderer>().material.color = Color.black;

            jumpPower = 5f;
            speedMove = 5f;

            mController = GameObject.FindGameObjectWithTag("Joystick").GetComponent<MobileController>();

        }

        void Update()
        {
            ChMove();
            Graviti();
        }


        private void OnCollisionEnter(Collision collision)
        { 
            if (collision.gameObject.tag == "Bot")
            {
                bot = collision.gameObject;
                if (collision.gameObject.GetComponent<Bot>().Potatoes == true)
                {
                    Potatoes = true;
                    collision.gameObject.GetComponent<Bot>().Potatoes = false;
                    collision.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                    player.GetComponent<MeshRenderer>().material.color = Color.black;
                    StartCoroutine(stun());
                }
                else if (collision.gameObject.GetComponent<Bot>().Potatoes == false)
                {
                    if (Potatoes)
                    {
                        Potatoes = false;
                        collision.gameObject.GetComponent<Bot>().Potatoes = true;
                        collision.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
                        player.GetComponent<MeshRenderer>().material.color = Color.white;
                        StartCoroutine(stunBot());
                    }
                }
                IEnumerator stun()
                {
                    speedMove = 0f;
                    yield return new WaitForSeconds(2f);
                    speedMove = 5f;
                }
                IEnumerator stunBot()
                {
                    gg = collision.gameObject;
                    gg.GetComponent<Bot>().Nav.speed = 0;
                    yield return new WaitForSeconds(2f);
                    gg.GetComponent<Bot>().Nav.speed = 3;
                }
            }
            
        }
        private void ChMove()
        {
            //cam.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

            moveVector = Vector3.zero;
            moveVector.x = mController.Horizontal() * speedMove;
            moveVector.z = mController.Vertical() * speedMove;
            moveVector.y = gravityForse;

            ch_controller.Move(moveVector * Time.deltaTime);
        }
        private void Graviti()
        {
            if (!ch_controller.isGrounded) gravityForse -= 20f * Time.deltaTime;
            else gravityForse = -1f;
            if (Input.GetKeyDown(KeyCode.Space) && ch_controller.isGrounded) gravityForse = jumpPower;
        }
    }
}

