using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Potatoes
{
    public class Bot : MonoBehaviour
    {
        private GameObject bot;
        

        public NavMeshAgent Nav;
        private GameObject Target;

        public float Distance;

        public bool GotTarget;
        public bool TargetIsPotato;
        public bool Potatoes;


        void Start()
        {
            Nav = GetComponent<NavMeshAgent>();
            Distance = 10f;
            bot = this.gameObject;
            bot.tag = "Bot";
            Potatoes = false;
            bot.AddComponent<SphereCollider>();
            bot.GetComponent<SphereCollider>().isTrigger = true;
            bot.GetComponent<SphereCollider>().radius = Distance;
            bot.AddComponent<Rigidbody>();
            bot.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            bot.GetComponent<CapsuleCollider>().radius = 0.6f;
        }
        private void Update()
        {
            walk();
        }

        private void walk()
        {
            if (TargetIsPotato)
            {
                float distance = Vector3.Distance(transform.position, Target.transform.position);
                if (distance < Distance)
                {
                    Vector3 Direction = transform.position - Target.transform.position;
                    Vector3 NewPosition = transform.position + Direction;
                    Nav.SetDestination(NewPosition);
                }
            }

            if (Potatoes)
            {
                Nav.SetDestination(Target.transform.position);
            }
            if (!Potatoes && !TargetIsPotato)
            {
                //Случайное движение
            }
        }
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Bot")
            {
                if (other.gameObject.GetComponent<Bot>().Potatoes)
                {
                    TargetIsPotato = true;
                    Target = other.gameObject;
                    GotTarget = true;
                }
            }
            else if (other.gameObject.tag == "Player")
            {
                if (other.gameObject.GetComponent<Player>().Potatoes)
                {
                    TargetIsPotato = true;
                    Target = other.gameObject;
                    GotTarget = true;
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Bot")
            {
                GotTarget = false;
                if (other.gameObject.GetComponent<Bot>().Potatoes)
                {
                    TargetIsPotato = false;
                }
            }

            if (other.gameObject.tag == "Player")
            {
                GotTarget = false;
                if (other.gameObject.GetComponent<Player>().Potatoes)
                {
                    TargetIsPotato = false;
                }
            }
        }
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "Bot")
            {
                if (other.gameObject.GetComponent<Bot>().Potatoes == true)
                {
                    StartCoroutine(waitMyPotatoes());
                }
                else if (other.gameObject.GetComponent<Bot>().Potatoes == false)
                {
                    if (Potatoes)
                    {
                        StartCoroutine(waitNotMyPotatoes());
                    }
                }  
            }
            IEnumerator waitMyPotatoes()
            {
                yield return new WaitForSeconds(0.3f);
                Potatoes = true;
                other.gameObject.GetComponent<Bot>().Potatoes = false;
                bot.GetComponent<MeshRenderer>().material.color = Color.black;
                other.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
                StartCoroutine(stun());
            }
            IEnumerator waitNotMyPotatoes()
            {
                yield return new WaitForSeconds(0.3f);
                Potatoes = false;
                other.gameObject.GetComponent<Bot>().Potatoes = true;
                bot.GetComponent<MeshRenderer>().material.color = Color.white;
                other.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
            }
            IEnumerator stun()
            {
                bot.GetComponent<NavMeshAgent>().speed = 0;
                yield return new WaitForSeconds(2f);
                bot.GetComponent<NavMeshAgent>().speed = 3;
            }
        }
    }
}


