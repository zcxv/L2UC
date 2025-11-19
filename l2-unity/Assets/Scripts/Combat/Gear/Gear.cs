using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Gear : AbstractMeshManager 
{
    protected NetworkAnimationController _networkAnimationReceive;
    protected int _ownerId;
    protected CharacterRaceAnimation _raceId;
    public const string weaponName = "weapon";
    public Transform[] allBone = new Transform[4];
    [Header("Weapons")]
    [Header("Meta")]
    [SerializeField] private Weapon _rightHandWeapon;
    [SerializeField] private Weapon _leftHandWeapon;


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
    private int _weaponRange;

    public WeaponType WeaponType { get { return _leftHandType != WeaponType.none ? _leftHandType : _rightHandType; } }
    public string WeaponAnim { get { return _weaponAnim; } }
    public int OwnerId { get { return _ownerId; } set { _ownerId = value; } }
    public CharacterRaceAnimation RaceId { get { return _raceId; } set { _raceId = value; } }

    public virtual void Initialize(int ownderId, CharacterRaceAnimation raceId) {
        TryGetComponent(out _networkAnimationReceive);
        _ownerId = ownderId;
        _raceId = raceId;
        _weaponAnim = "hand";
        _weaponRange = 40; //default
    }

    public virtual void EquipWeapon(int weaponId, bool leftSlot) {

        Weapon weapon = ItemTable.Instance.GetWeapon(weaponId);

        ErrorPrint(weapon, weaponId);

        if (weaponId == 0 | IsWeaponEquipped(weaponId, leftSlot) | weapon == null) {
            return;
        }


        GameObject weaponPrefab = (GameObject)LoadMash(EquipmentCategory.Weapon , weaponId);

        WeaponType type = weapon.Weapongrp.WeaponType;
        RefreshData(leftSlot, weapon);
        UpdateWeaponType(type);
        Transform[] refreshAllBone = RefreshBone(allBone);

        GameObject go = CreateCopy(weaponPrefab, weaponName);
        SetType(type, leftSlot, refreshAllBone);
        go.SetActive(true);
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


    public  void UnequipWeapon(bool leftSlot)
    {
        Transform weapon = (leftSlot ? GetLeftHandBone() : GetRightHandBone())?.Find("weapon");
        if (weapon != null)
        {
            Debug.LogWarning("Unequip weapon");
            Destroy(weapon.gameObject);
            RefreshData(leftSlot, null);
        }
    }

    public bool IsWeaponEquipped(int weaponId, bool leftSlot)
    {
        Weapon weapon = leftSlot ? _leftHandWeapon : _rightHandWeapon;
        return weapon != null && weapon.Id == weaponId;
    }


    private void RefreshData(bool leftSlot , Weapon weapon)
    {
        // Updating weapon type
        if (leftSlot)
        {
            _leftHandWeapon = weapon;
            _leftHandType = (weapon == null) ? WeaponType.none : weapon.Weapongrp.WeaponType;
        }
        else
        {
            _rightHandWeapon = weapon;
            _rightHandType = (weapon == null) ? WeaponType.none : weapon.Weapongrp.WeaponType;
        }
    }

}
