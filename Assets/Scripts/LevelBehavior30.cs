using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehavior30 : MonoBehaviour
{
    [SerializeField]
    private Animator _wall1Animator;
    [SerializeField]
    private Animator _wall2Animator;
    [SerializeField]
    private Animator _wall3Animator;
    [SerializeField]
    private Animator _wall4Animator;


    private DateTime _nextPlayTime1;
    private DateTime _nextPlayTime2;
    private DateTime _nextPlayTime3;
    private DateTime _nextPlayTime4;

    private bool _isPlaying1;
    private bool _isPlaying2;
    private bool _isPlaying3;
    private bool _isPlaying4;


    // Start is called before the first frame update
    void Start()
    {
        _isPlaying1 = false;
        _isPlaying2 = false;
        _isPlaying3 = false;
        _isPlaying4 = false;
        _nextPlayTime1 = DateTime.Now.AddSeconds(1);
        _nextPlayTime2 = DateTime.MaxValue;
        _nextPlayTime3 = DateTime.MaxValue;
        _nextPlayTime4 = DateTime.MaxValue;
    }

    void Update()
    {
        if (_isPlaying1 == false && DateTime.Now.TimeOfDay.TotalSeconds >= _nextPlayTime1.TimeOfDay.TotalSeconds)
        {
            _isPlaying1 = true;
            _wall1Animator.Play("Moving Wall 5");
            _nextPlayTime2 = DateTime.Now.AddSeconds(2.5);
        }
        else
        {
            if (_isPlaying1 && _wall1Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                _isPlaying1 = false;
                _wall1Animator.Play("New State");
                _nextPlayTime1 = DateTime.MaxValue;
            }
        }


        if (_isPlaying2 == false && DateTime.Now.TimeOfDay.TotalSeconds >= _nextPlayTime2.TimeOfDay.TotalSeconds)
        {
            _isPlaying2 = true;
            _wall2Animator.Play("Moving Wall 6");
            _nextPlayTime3 = DateTime.Now.AddSeconds(2.5);
        }
        else
        {
            if (_isPlaying2 && _wall2Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                _isPlaying2 = false;
                _wall2Animator.Play("New State");
                _nextPlayTime2 = DateTime.MaxValue;
            }
        }

        if (_isPlaying3 == false && DateTime.Now.TimeOfDay.TotalSeconds >= _nextPlayTime3.TimeOfDay.TotalSeconds)
        {
            _isPlaying3 = true;
            _wall3Animator.Play("Moving Wall 7");
            _nextPlayTime4 = DateTime.Now.AddSeconds(2.5);
        }
        else
        {
            if (_isPlaying3 && _wall3Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                _isPlaying3 = false;
                _wall3Animator.Play("New State");
                _nextPlayTime3 = DateTime.MaxValue;
            }
        }

        if (_isPlaying4 == false && DateTime.Now.TimeOfDay.TotalSeconds >= _nextPlayTime4.TimeOfDay.TotalSeconds)
        {
            _isPlaying4 = true;
            _wall4Animator.Play("Moving Wall 8");
            _nextPlayTime1 = DateTime.Now.AddSeconds(2.5);
        }
        else
        {
            if (_isPlaying4 && _wall4Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                _isPlaying4 = false;
                _wall4Animator.Play("New State");
                _nextPlayTime4 = DateTime.MaxValue;
            }
        }

    }
}
