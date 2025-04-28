using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KS
{
    public class AIBossCombatAnimationEvents : MonoBehaviour
    {
        AIBossManager manager;
        Animator anim;
        AIBossAnimationEvents bossAnimEvents;

        public Transform WeaponOutput;
        public MeleeDamageCollider meleeHitbox;
        public MeleeDamageCollider ArmHitbox;
        public MeleeDamageCollider meleeHitboxFinish;
        public GameObject AuraBlade;

        [Header("Mechanics")]
        public GameObject BossMechanics;

        [Header("VFX Buildup")]
        public GameObject meleeBU_Parent;

        [Header("Melee VFX")]
        public GameObject Melee1;
        public GameObject Melee2;
        public GameObject Melee3A;
        public GameObject Melee3B;

        [Header("Magic Smmon")]
        public GameObject currentSummonBuildup;
        public List<GameObject> spawners;

        private void Awake()
        {
            manager = GetComponentInParent<AIBossManager>();
            anim = GetComponent<Animator>();
            bossAnimEvents = GetComponent<AIBossAnimationEvents>();

        }

        #region Melee

        public void SetupMeleeHitboxes()
        {
            meleeHitbox.Init(DestroyHitbox, manager, manager.statManager.baseAttack);
            ArmHitbox.Init(DestroyHitbox, manager, manager.statManager.baseAttack);
            MeleeDeactive();
            meleeHitboxFinish.Init(DestroyHitbox, manager, manager.statManager.baseAttack);
            MeleeFinishDeactive();
        }

        public void BuildUpVFX()
        {
            GameObject vfx = Instantiate(manager.combatManager.GetMeleeBuildupVFX(), meleeBU_Parent.transform);
            Destroy(vfx, .5f);
           
        }

        //turns ON melee hitbox
        public void MeleeActive()
        {
            meleeHitbox.ResetBeforeUse(); 
            meleeHitbox.SetAttackPower(manager.combatManager.currentMeleeAttack.baseDamage);
            meleeHitbox.gameObject.SetActive(true);

            ArmHitbox.ResetBeforeUse();
            ArmHitbox.SetAttackPower(manager.combatManager.currentMeleeAttack.baseDamage);
            ArmHitbox.gameObject.SetActive(true);
        }

        //turns OFF melee hitbox
        public void MeleeDeactive()
        {
            meleeHitbox.gameObject.SetActive(false);
            ArmHitbox.gameObject.SetActive(false);
        }

        //turns ON melee hitbox Finish (last combo attack)
        public void MeleeFinishActive()
        {
            meleeHitboxFinish.ResetBeforeUse();
            meleeHitboxFinish.SetAttackPower(manager.combatManager.currentMeleeAttack.baseDamage);
            meleeHitboxFinish.gameObject.SetActive(true);
            AuraBlade.SetActive(true);

            ArmHitbox.ResetBeforeUse();
            ArmHitbox.SetAttackPower(manager.combatManager.currentMeleeAttack.baseDamage);
            ArmHitbox.gameObject.SetActive(true);
        }

        //turns OFF melee hitbox Finish (last combo attack)
        public void MeleeFinishDeactive()
        {
            meleeHitboxFinish.gameObject.SetActive(false);
            AuraBlade.SetActive(false);
            ArmHitbox.gameObject.SetActive(false);
        }

        //sets Combo flag boolean to true,
        //so that the behaviour can look for to see if it ended
        public void SetComboFlag()
        {
            manager.comboFlag = true;
        }

        //instanciates slash & Stab VFX
        public void MeleeVFX(int comboNum)
        {
            GameObject vfx = null;
            switch (comboNum)
            {
                case 1:
                    //melee attack 1
                   vfx = Instantiate(Melee1, manager.transform);
                    break;
                case 2:
                    //melee attack 2
                    vfx = Instantiate(Melee2, manager.transform);
                    break;
                case 3:
                    //melee attack 3A, Stab
                    vfx = Instantiate(Melee3A, manager.transform);
                    break;
                case 4:
                    //melee attack 3B, slash
                    vfx = Instantiate(Melee3B, manager.transform);
                    break;
            }
            
            Destroy(vfx, .5f);
        }

        #endregion

        #region Fan Abilities

        //initiates the throwing fan sequence.
        public void FanThrow()
        {
            manager.combatManager.HandleThrowFan();
        }

        #endregion

        #region Magic: Summon

        //animation event function, Instatiate's summon buildup vfx
        public void SummonBuildUp()
        {
            currentSummonBuildup = manager.combatManager.currentSummonAttack.buildUp;

            for (int i = 0; i < spawners.Count; i++)
            {
                if (spawners[i].transform.childCount == 0)
                {
                    Instantiate(currentSummonBuildup, spawners[i].transform);
                }
            }

        }

        //animation event function, activate summon magic
        public void ActivateMagicSummon()
        {
            manager.combatManager.ActivateMagicSummon(spawners);
        }

        #endregion

        #region Mechanics

        //THE COMPONENT CALLED "BMECH_TornadoMechnic" NEED TO BE CHANGED TO A BASE SCRIPT
        //animation evet function, executes a mechanic based on given ID
        public void ExecuteMechanic(string type)
        {
            Debug.Log("Execute Mechanic");

            for (int i = 0; i < manager.combatManager.Mechanics.Count; i++)
            {
                if (type == "cast")
                {
                    if (manager.combatManager.Mechanics[i].ID == manager.combatManager.currentMagicAttack.ID)
                    {
                        manager.combatManager.Mechanics[i].PlayMechanic();
                        manager.ActiveMechanic = true;
                        manager.LongCastFinish = true;
                    }

                }
                else if (type == "dash")
                {
                    if (manager.combatManager.Mechanics[i].ID == manager.combatManager.DashAttackSO.ID)
                    {
                        manager.animationEvents.CharInvisible();
                        manager.statManager.InvulnOFF();

                        manager.combatManager.Mechanics[i].PlayMechanic();
                        manager.ActiveMechanic = true;
                    }
                }
               
            }

        }

        #endregion

        public void DestroyHitbox(BaseDamageCollider obj)
        {
            Destroy(obj.gameObject);
        }

    }
}