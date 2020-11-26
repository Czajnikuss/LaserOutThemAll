using UnityEngine;
public interface IHitable 
{
    void Hit();
    void Damage();
    Side side {get; set;}
   
}
public enum Side
{
    Player,
    Enemy, 
    Neutral
}

