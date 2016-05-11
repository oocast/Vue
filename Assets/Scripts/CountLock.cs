using UnityEngine;
using System.Collections;

public class CountLock
{
    public bool value;
    int _count;

    public CountLock()
    {
        value = false;
        _count = 0;
    }

    public void Lock()
    {
        value = true;
        _count++;
    }

    public void Unlock()
    {
        _count--;
        if (_count <= 0)
        {
            value = false;
        }
    }
    
    public void UnlockAll()
    {
        _count = 0;
        value = false;
    }
}
