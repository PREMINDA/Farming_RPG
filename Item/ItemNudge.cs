using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace Script.Item
{
    public class ItemNudge : MonoBehaviour
    {
        private WaitForSeconds _pause;
        private bool _isAnimating;

        private void Awake()
        {
            _pause = new WaitForSeconds(0.04f);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_isAnimating == false)
            {
                if (gameObject.transform.position.x < col.gameObject.transform.position.x)
                {
                    StartCoroutine(RotateAntiClockWise());
                }
                else
                {
                    StartCoroutine(RotateClockwise());
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (_isAnimating == false)
            {
                if (gameObject.transform.position.x > collision.gameObject.transform.position.x)
                {
                    StartCoroutine(RotateAntiClockWise());
                }
                else
                {
                    StartCoroutine(RotateClockwise());
                }
                


            }
        }

        private IEnumerator RotateClockwise()
        {
            _isAnimating = true;

            for (int i = 0; i < 4; i++)
            {
                gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);

                yield return _pause;
            }

            for (int i = 0; i < 5; i++)
            {
                gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);

                yield return _pause;
            }

            gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);

            yield return _pause;

            _isAnimating = false;
        }
        

        private IEnumerator RotateAntiClockWise()
        {
            _isAnimating = true;

            for (int i = 0; i < 4; i++)
            {
                gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);

                yield return _pause;
            }

            for (int i = 0; i < 5; i++)
            {
                gameObject.transform.GetChild(0).Rotate(0f, 0f, -2f);

                yield return _pause;
            }

            gameObject.transform.GetChild(0).Rotate(0f, 0f, 2f);

            yield return _pause;

            _isAnimating = false;
        }
    }

}