Simple Military

//Gun Animation usage
To animate the guns you will need to set mechanim parameters in the character Animator and parent the waepon mesh under the right hand jnt

Parent weapon meshlocation -
/SimpleMilitary_SpecialForces04_White 
	/SimpleMilitary_Characters
		/Root_jnt
			/Hips_jnt
				/Body_jnt
					/Spine_jnt
						/UpperArm_Right_jnt
							/LowerArm_Right_jnt
								/Hand_Right_jnt

//Mechanim parameters

Shoot_b - Activates shoot

FullAuto_b - Enables full auto if avaliable (AssultRiffle01,02 and subMachineGun)

Reload_b - Reloads the current gun

Jump_b - will make the character jump 

Death_b - will kill the character

DeathType_int - Will select the type of death animation
1 = Back death
2 = Front death

Static_b - Will turn root motion on and off

Grounded_b - will play a falling animation when not grounded


Animation_int - selects an idle animation to play
0 = normal idle
1 = Crossed Arms
2 = HandsOnHips
3 = Check Watch
4 = Sexy Dance
5 = Smoking
6 = Salute
7 = Wipe Mount
8 = Leaning against wall
9 = Sitting on Ground

WeaponType_int - Sets the type of weapon animation to play
0 = No weapon
1 = Pistol
2 = AssultRifle01
3 = AssultRifle02
4 = Shotgun
5 = SniperRifle
6 = Rifle
7 = SubMachineGun
8 = RPG
9 = MiniGun
10 = Grenades
11 = Bow
12 = Melee

MeleeType
0 = Stab
1 = One Handed
2 = Two Handed

Head_Horizontal_f and Head_Vertical_f - Control the head direction

Each weapon animation will require a new Body_Horivontal_f and Body_Vertical_f value for Idle, Walk and Run

No weapon Idle = 0 , 0
No weapon Walk = 0 , 0
No weapon Run = 0 , 0

Pistol Idle =	0 , 0
Pistol Walk =	0 , 0
Pistol Run =	0 , 0.2

Grenades Idle =	0 , 0
(set speed to 0 when throwing a grenade)

All other weapons Idle = 0 , 0.6
All other weapons Walk = 0 , 0.6  
All other weapons Run = 0.3 , 0.6   


Thank you for purchasing Simple Military, check out our other packs
https://www.assetstore.unity3d.com/en/#!/publisher/5217