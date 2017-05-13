using UnityEngine;
using System.Collections;

public class HeroCombatController : MonoBehaviour
{
    #region Designer Variables
    [SerializeField]private Weapon wCurrentWeapon;
    [SerializeField]private Transform tWeaponSocket;
    #endregion

    private const string sWeaponSocketName = "WeaponSocket";

    private void Start()
    {
        if (!tWeaponSocket)
        {
            Debug.LogError("Missing Weapon Socket refference on Hero character.");
            Debug.Break();
        }

        if (wCurrentWeapon != null)
        {
            wCurrentWeapon = Instantiate(wCurrentWeapon) as Weapon;
            EquipWeapon();
        }
        else
        {
            //Equip base gun.
        }
    }

    private void Update()
    {
        float fireAxis = Input.GetAxis("Fire");

        if (fireAxis < -0.3f)
        {
            wCurrentWeapon.DoAttack();
        }
    }

    /// <summary>
    /// Sets the weapon.
    /// </summary>
    /// <param name="newWeapon">New weapon.</param>
    public void SetWeapon(Weapon newWeapon)
    {
        wCurrentWeapon = newWeapon;
    }

    private void EquipWeapon()
    {
        //position new weapon at the weapon socket
        wCurrentWeapon.transform.position = tWeaponSocket.position;
        wCurrentWeapon.transform.parent = tWeaponSocket;
        wCurrentWeapon.transform.localRotation = Quaternion.identity;
    }
        
}
