using UnityEngine;

public class SpiderAnimationEvent : MonoBehaviour
{
    private Spider _spider;

    private void Start()
    {
        _spider = GetComponentInParent<Spider>();
    }

    private void Fire()
    {
        _spider.Attack();
    }
}
