using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ModelTable;
using static Unity.Burst.Intrinsics.Arm;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

public class Gear : AbstractMeshManager 
{
    protected NetworkAnimationController _networkAnimationReceive;
    protected int _ownerId;
    protected CharacterRaceAnimation _raceId;
    public const string weaponName = "weapon_";
    public const string shieldName = "shield_";
    public Transform[] allBone = new Transform[4];
    [Header("Weapons")]
    [Header("Meta")]
    private Weapon _rightHandWeapon;
    private Weapon _leftHandWeapon;
    private Weapon _leftHandShield;
    private string _origWeaponPrefabName = "";
    private string _origShieldPrefabName = "";
 
    public Action<int, Weapon>  OnEquipWeapon;
    public Action<string> OnUnequipWeapon;

    [Header("Models")]
    [Header("Right hand")]
    [SerializeField] private WeaponType _rightHandType;
    [SerializeField] protected Transform _rightHandBone;
    [SerializeField] protected Transform _rightHand;
    [Header("LeftHand")]
    [SerializeField] private WeaponType _leftHandType;
    [SerializeField] protected Transform _leftHandBone;
    [SerializeField] protected Transform _shieldBone;
    [SerializeField] protected Transform _leftHand;
    [SerializeField] protected string _weaponAnim;
    protected string _lastWeaponAnim = "";
    private int _weaponRange;

    public WeaponType WeaponType { get { return _leftHandType != WeaponType.none ? _leftHandType : _rightHandType; } }
    public string WeaponAnim { get { return _weaponAnim; } }
    public string LastWeaponAnim { get { return _lastWeaponAnim; } }

    public virtual void Initialize(int ownderId, CharacterRaceAnimation raceId) {
        TryGetComponent(out _networkAnimationReceive);
        _ownerId = ownderId;
        _raceId = raceId;
        _weaponAnim = "hand";
        _weaponRange = 40; //default
    }

    public void EquipShield(int weaponId)
    {
        Weapon weapon = ItemTable.Instance.GetWeapon(weaponId);
        ErrorPrint(weapon, weaponId);

        if (weaponId == 0 | IsShieldEquipped(weaponId) | IsWeaponEquipped(weaponId , true) | weapon == null)
        {
            return;
        }

        GameObject weaponPrefab = (GameObject)LoadMesh(EquipmentCategory.Weapon, weaponId);
        _origShieldPrefabName = weaponPrefab.name;
        WeaponType type = WeaponType.none;
        RefreshDataShield(weapon , WeaponType.shield);
        UpdateWeaponType(type);
        string shieldNameId = shieldName+weaponId;
        Transform[] refreshAllBone = RefreshBone(allBone);
        GameObject go = CreateCopy(weaponPrefab, shieldNameId, ObjectType.Weapon);
        _lastWeaponAnim = _weaponAnim;
        ActivateGameObject(go, type, true, refreshAllBone);
        OnEquipWeapon?.Invoke(-1, weapon);
    }

    public virtual void EquipWeapon(int weaponId, bool leftSlot) {

        Weapon weapon = ItemTable.Instance.GetWeapon(weaponId);

        ErrorPrint(weapon, weaponId);

        if (weaponId == 0 | IsWeaponEquipped(weaponId, leftSlot) | weapon == null) {
            return;
        }


        GameObject weaponPrefab = (GameObject)LoadMesh(EquipmentCategory.Weapon, weaponId);
        _origWeaponPrefabName = weaponPrefab.name;
        var weaponNameAndId = weaponName + weaponId;
        WeaponType type = weapon.Weapongrp.WeaponType;
        RefreshData(leftSlot, weapon);
        UpdateWeaponType(type);

        Transform[] refreshAllBone = RefreshBone(allBone);
        GameObject go = CreateCopy(weaponPrefab, weaponNameAndId, ObjectType.Weapon);
        _lastWeaponAnim = _weaponAnim;

        ActivateGameObject(go, type, leftSlot, refreshAllBone);
        OnEquipWeapon?.Invoke(-1 , weapon);
        Debug.Log("Request EquipWeapon");
    }



    private void ActivateGameObject(GameObject go, WeaponType type , bool leftSlot , Transform[] refreshAllBone)
    {
        if (go != null)
        {
            SetType(type, leftSlot, refreshAllBone);
            go.SetActive(true);
        }
    }

    private void ErrorPrint(Weapon weapon , int weaponId)
    {
        if (weapon == null)
        {
            Debug.LogWarning("Gear->EquipWeapon: Not Found item in database id " + weaponId);
        }

    }
    private void UpdateWeaponType(WeaponType weaponType) {
        _weaponAnim = WeaponTypeParser.GetWeaponAnim(weaponType);
        _weaponRange = WeaponTypeParser.WeaponRange(weaponType);
    }

    private Transform[] RefreshBone(Transform[] allBone)
    {
        allBone[0] = GetShieldBone();
        allBone[1] = GetLeftHandBone();
        allBone[2] = GetLeftHandBone();
        allBone[3] = GetRightHandBone();
        return allBone;
    }

    protected virtual Transform GetLeftHandBone() {
        if (_leftHandBone == null) {
            _leftHandBone = transform.FindRecursive("Bow Bone");
        }
        return _leftHandBone;
    }

    protected virtual Transform GetRightHandBone() {
        if (_rightHandBone == null) {
            _rightHandBone = transform.FindRecursive("Sword Bone");
        }
        return _rightHandBone;
    }

    protected virtual Transform GetShieldBone() {
        if (_shieldBone == null) {
            _shieldBone = transform.FindRecursive("Shield Bone");
        }
        return _shieldBone;
    }

    public float GetWeaponRange() {
        return VectorUtils.ConvertL2jDistance(_weaponRange);
    }


    public  void UnequipWeapon(bool leftSlot , int weaponId)
    {

        string weapondNameId = weaponName + weaponId;


        Transform weapon = (leftSlot ? GetLeftHandBone() : GetRightHandBone())?.Find(weapondNameId);
        int findCount = FindAllWeaponCount(weaponName, shieldName);

        if (weapon != null)
        {
            DestroyObject(weapon.gameObject, _origWeaponPrefabName);
            RefreshHandWeapon(leftSlot, null);

            if (findCount == 1)
            {
                RefreshData(leftSlot, null);
                _lastWeaponAnim = _weaponAnim;
                UpdateWeaponType(WeaponType.hand);
                OnUnequipWeapon?.Invoke("");
            }
        }
    }





    public void UnequipShield(int shieldId)
    {
        string shieldNameId = shieldName + shieldId;
        Transform shield = GetShieldBone()?.Find(shieldNameId);

        int findCount = FindAllWeaponCount(weaponName, shieldName);

        if (shield != null)
        {


            Debug.LogWarning("Gear: UnequipShield->Unequip shield");
            DestroyObject(shield.gameObject, _origShieldPrefabName);

            RefreshHandShield(null);
            //if no equip sword and no equip shield. findCount = 1 > then remove current shield 
            if (findCount == 1)
            {
                RefreshDataShield(null);
                _lastWeaponAnim = _weaponAnim;
                UpdateWeaponType(WeaponType.hand);
                OnUnequipWeapon?.Invoke("");
            }

        }
    }

    private void DestroyObject(GameObject go , string origPrefabName)
    {
        if (ObjectPoolManager.Instance != null)
        {
            go.name = origPrefabName;
            if (!ObjectPoolManager.Instance.ReturnToPool(ObjectType.Weapon, go))
            {
                Destroy(go);
            }
        }
        else
        {
            Destroy(go);
        }
    }

    public bool IsWeaponEquipped(int weaponId, bool leftSlot)
    {
        Weapon weapon = leftSlot ? _leftHandWeapon : _rightHandWeapon;
        return weapon != null && weapon.Id == weaponId;
    }

    public bool IsShieldEquipped(int weaponId)
    {
        Weapon weapon = _leftHandShield;
        return weapon != null && weapon.Id == weaponId;
    }


    private void RefreshData(bool leftSlot , Weapon weapon)
    {
        // Updating weapon type
        if (leftSlot)
        {
            _leftHandWeapon = weapon;
            var typeL = (weapon == null) ? WeaponType.hand : weapon.Weapongrp.WeaponType;
            _leftHandType = typeL;
        }
        else
        {
            _rightHandWeapon = weapon;
            var typeR = (weapon == null) ? WeaponType.hand : weapon.Weapongrp.WeaponType;
            _rightHandType = typeR;
        }
    }

    private void RefreshHandWeapon(bool leftSlot, Weapon weapon)
    {
        if (leftSlot)
        {
            _leftHandWeapon = weapon;
        }
        else
        {
            _rightHandWeapon = weapon;
        }
    }

    private void RefreshDataShield(Weapon weapon , WeaponType weaponType = WeaponType.hand)
    {
        _leftHandShield = weapon;
        _leftHandWeapon = weapon;
        _leftHandType = weaponType;
    }

    private void RefreshHandShield(Weapon weapon)
    {
        _leftHandShield = weapon;
        _leftHandWeapon = weapon;
    }


    private IEnumerable<Transform> FindTransformsWithName(Transform parent, string pattern)
    {
        if (parent == null) yield break;

        // Check current transform
        if (parent.name.Contains(pattern))
        {
            yield return parent;
        }

        // Recursively check all children
        foreach (Transform child in parent)
        {
            foreach (Transform match in FindTransformsWithName(child, pattern))
            {
                yield return match;
            }
        }
    }
    private int FindAllWeaponCount(string allWeapons , string allShields)
    {
        var listWeapons = FindTransformsWithName((false ? GetLeftHandBone() : GetRightHandBone()), allWeapons).ToList();
        var listShield = FindTransformsWithName(GetShieldBone(), allShields).ToList();
        return listWeapons.Count + listShield.Count;
    }


}
