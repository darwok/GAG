using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class EnemyAI : MonoBehaviour
    {
        public float moveSpeed = 1f;
        public float rayLenght;
        public LayerMask ground;
        public Transform groundRayOrigin;

        private new Rigidbody2D rigidbody;
        public Collider2D triggerCollider;

        private bool grouding, hitWall;
        private Vector2 currentDirection = Vector2.right;
        private bool blockMovement;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (blockMovement)
            {
                return;
            }

            rigidbody.linearVelocity = new Vector2(moveSpeed, rigidbody.linearVelocity.y);
        }

        void FixedUpdate()
        {
            if (blockMovement)
            {
                return;
            }

            grouding = GroundingRaycast(groundRayOrigin.position, Vector2.down, Color.green);
            hitWall = GroundingRaycast(transform.position, currentDirection, Color.red);

            if (!grouding || hitWall)
            {
                Flip();
            }
        }

        private bool GroundingRaycast(Vector3 origin, Vector3 direction, Color debugColor)
        {
            Ray ray = new Ray(origin, direction);

            Debug.DrawRay(ray.origin, ray.direction.normalized * rayLenght, debugColor);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, rayLenght, ground);

            return hit;
        }

        public void BlockMovement(bool vaue)
        {
            blockMovement = vaue;
        }

        private void Flip()
        {
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            moveSpeed *= -1;
            currentDirection *= -1;

        }
    }
}
