using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Item : MonoBehaviour
{

    [Header("Elements")]
    [SerializeField] private Renderer Renderer;
    private Material baseMaterial;

    private void Awake()
    {
        baseMaterial = Renderer.material;
    }
    public void DisableShadows()
    {

    }

    public void DisablePhysics()
    {
        //rig.isKinematic = true;
        //collider.enabled = false;

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    
    }

    public void Select(Material outlineMaterial)
    {
        
        Renderer.materials = new Material[] { baseMaterial, outlineMaterial };
    }

    public void DeSelect()
    {
        Renderer.materials = new Material[] { baseMaterial };
    }

}
