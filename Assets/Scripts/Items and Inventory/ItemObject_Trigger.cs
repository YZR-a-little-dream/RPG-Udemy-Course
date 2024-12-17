using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collison) {
        if(collison.GetComponent<Player>() != null)
        {
            if(collison.GetComponent<CharacterStats>().isDead)
                return;
                
            Debug.Log("Pick up item");
            myItemObject.PickupItem();
        }
    }
}
