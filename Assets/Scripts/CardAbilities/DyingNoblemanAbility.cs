using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingNoblemanAbility : CardAbility {

		public int goldGained;

		/*
		*	Ability that triggers on death.
		*	Gains 6 gold. It doesn't matter how to unit dies in the case, so we override OnKillAbility()
		*/
		public override void OnKillAbility(bool combat){
			player.GetComponent<PlayerField>().GainGold(goldGained);
		}

}
