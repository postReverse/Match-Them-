using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{

    [Header("Data")]
    [SerializeField] private EItemName itemName;
    public EItemName ItemName => itemName;

    [Header("Elements")]
    [SerializeField] private Renderer Renderer;
    [SerializeField] private Collider Collider;
    private Material baseMaterial;

    private void Awake()
    {
        baseMaterial = Renderer.material;
    }
    public void DisableShadows()
    {
        Renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    public void DisablePhysics()
    {

        GetComponent<Rigidbody>().isKinematic = true;
        Collider.enabled = false;
    
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
