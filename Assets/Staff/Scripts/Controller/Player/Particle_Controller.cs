using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_Controller : MonoBehaviour
{
    public static Particle_Controller Instance;
    [Header("BUFF����ϵͳ")]
    public ParticleSystem buff_particle;
    private void Awake()
    {
        Instance = this;
    }
    public void Set_Particle_Color(ParticleSystem particleSystem,Color color)
    {
        particleSystem.startColor = color;
    }

    public void Set_Particle_visual(ParticleSystem particleSystem,bool enable)
    {
        particleSystem.gameObject.SetActive(enable);
    }
}