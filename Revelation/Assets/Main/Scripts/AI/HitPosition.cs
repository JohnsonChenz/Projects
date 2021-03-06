using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPosition : MonoBehaviour {

	public EnemyStat enemystat;
	public ybotDamage ybotdamage;
	public PipeHp pipehp;
	public TownBoss townboss;
	public float mutiplication;

	public float takeaway;
	public float burntime;
	public ParticleSystem FireEffect;

	public float test;

	// Use this for initialization
	void Start () {
		
	}

	public void Damage(float damage)
	{
		if (enemystat) {
			enemystat.TakeAwayHealth (damage * mutiplication);
		}

		if (ybotdamage) {
			ybotdamage.BulletHit (damage * mutiplication);
		}

		if (pipehp) {
			if (!pipehp.townboss.isInvincible) {
				pipehp.TakeAwayHealth (damage * mutiplication);
			}
		}

		if (townboss) {
			if (!townboss.isInvincible && townboss.Stage == 3) {
				townboss.TakeAwayHealth (damage * mutiplication);
			}
		}
	}

	void Update()
	{
		if (pipehp) {
			if (!pipehp.townboss.isInvincible) {
				if (burntime > 0) {
					burntime -= Time.deltaTime;
					if (pipehp.side == "Right") {
						pipehp.townboss.Pipe_Right_HP -= takeaway * Time.deltaTime * 5 * 0.1f;
					}

					if (pipehp.side == "Left") {
						pipehp.townboss.Pipe_Left_HP -= takeaway * Time.deltaTime * 5 * 0.1f;
					}
					//burntime = 0;
				} else if (burntime < 0){
					if (FireEffect) {
						FireEffect.Stop ();
					}
					burntime = 0;
				}
			}
		}

		if (townboss) {
			if (!townboss.isInvincible && townboss.Stage == 3) {
				if (burntime > 0) {
					burntime -= Time.deltaTime;
					townboss.hp -= takeaway * Time.deltaTime * 5 * 0.1f;
				} else if (burntime < 0){
					if (FireEffect) {
						FireEffect.Stop ();
					}
					burntime = 0;
				}
			}
			if (townboss.hp < (townboss.Max_hp * 0.5f) && townboss.Crystalstage != 2) {
				townboss.Crystalstage = 2;
				townboss.CrystalStage (townboss.Crystalstage);
			}
		}


	}

	public void TakeAwayHealthBurn(float TakeAway, float BurnTime)
	{
		if (pipehp) {
			if (!pipehp.townboss.isInvincible) {
				takeaway = TakeAway;
				if (pipehp.side == "Right") {
					pipehp.townboss.Pipe_Right_HP -= takeaway * 0.1f;
				}

				if (pipehp.side == "Left") {
					pipehp.townboss.Pipe_Left_HP -= takeaway * 0.1f;
				}

				burntime = BurnTime;
				if (FireEffect) {
					FireEffect.Play ();
				}
			}
		}

		if (townboss) {
			if (!townboss.isInvincible && townboss.Stage == 3) {
				takeaway = TakeAway;
				townboss.hp -= takeaway * Time.deltaTime * 5 * 0.1f;
				burntime = BurnTime;
				if (FireEffect) {
					FireEffect.Play ();
				}
			}
			if (townboss.hp < (townboss.Max_hp * 0.5f) && townboss.Crystalstage != 2) {
				townboss.Crystalstage = 2;
				townboss.CrystalStage (townboss.Crystalstage);
			}
		}


	}


}
