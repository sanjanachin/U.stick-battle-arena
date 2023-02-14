using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Game.Player;
using UnityEngine;
using Object = System.Object;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Transform[] _wayPoints;
        [SerializeField] private Transform _transform;
        [SerializeField] private float _speed;
        [SerializeField] private bool _loop;
        private Queue<Transform> _wayPointQueue;
        private Transform _lastTrans;
        private float _time = 0;
        private float _distance = 0;
        
        private Collider2D _collider;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();

            if (_wayPoints.Length == 0)
            {
                Debug.LogError($"Not way points for this moving platform!");
                gameObject.SetActive(false);
                return;
            }
            
            _wayPointQueue = new Queue<Transform>(_wayPoints);
            if (!_loop)
            {
                // add waypoints in reverse order for reverse movement
                for (int i = _wayPoints.Length - 2; i > 0; i--)
                    _wayPointQueue.Enqueue(_wayPoints[i]);
            }

            NextWayPoint();
        }

        private void NextWayPoint()
        {
            // recycle the transform into queue
            _lastTrans = _wayPointQueue.Dequeue();
            _wayPointQueue.Enqueue(_lastTrans);
            
            // set position to exactly the position of way point
            _transform.position = _lastTrans.position;

            // calculate distance for lerp
            _distance = Vector2.Distance(
                _lastTrans.position,
                _wayPointQueue.Peek().position);
            _time = 0;
        }

        private void FixedUpdate()
        {
            float rate = _time * _speed / _distance;
            
            _transform.position = Vector3.Lerp(
                _lastTrans.position, 
                _wayPointQueue.Peek().position,
                rate);
            
            if (rate >= 1)
                NextWayPoint();
            _time += Time.deltaTime;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            PlayerController player = col.gameObject.GetComponent<PlayerController>();
            if (player == null) return;

            player.transform.position +=
                (_wayPointQueue.Peek().position - _lastTrans.position).normalized 
                * _speed * Time.deltaTime;
        }

        private void OnDrawGizmosSelected()
        {
            if (_wayPoints == null) return;
            if (_wayPoints.Length == 0) return;

            for (int i = 0; i < _wayPoints.Length - 1; i++)
            {
                if (_wayPoints[i] == null) continue;
                Gizmos.DrawLine(
                    _wayPoints[i].position,
                    _wayPoints[i + 1].position
                    );
            }
            
            if (_loop)
                Gizmos.DrawLine(
                    _wayPoints.First().position,
                    _wayPoints.Last().position
                );
        }
    }
}