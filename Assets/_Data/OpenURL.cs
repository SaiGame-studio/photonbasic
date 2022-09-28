using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    public virtual void Open()
    {
        Application.OpenURL("https://discord.gg/NBT5ZPzBKK");
    }
}
