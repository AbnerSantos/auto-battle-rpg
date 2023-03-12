# Trust in your party - The Auto Battle RPG!

Trust in your party is an Auto Battle RPG (Auto Chess) made originally as a Kokku Software Engineer Applicant Test.

## Description

In Trust in your party, you'll choose the settings for your battlefield or just accept the recommended ones, then choose the classes of your team members and also enemies!
After that, they'll spawn in the map and start fighting!

There are 3 classes:
### Warrior - High damaging melee fighter that uses his shield to defend himself.
  - Slash: Swings sword at enemy for 1d10 + 2 damage.
  - Impale: Low chance of critically impaling enemy for double (2d10 + 2) damage!.
### Ranger - Can easily traverse thick forests and uses a bow for ranged attacks that go over obstacles.
  - Shoot: Shoot arrow at enemy for 1d6 + 2 damage.
  - Hunter: The Ranger is used to the forest and can walk through it without any movement penalties.
  - Boyscout: The Ranger finds peace in starting his turn in the forest and heals 1d4 health.
### Mage - Frail but casts very powerful spells at range - if you have the mana, that is.
  - Concentrate: Concentrate and charge your mana by 1d8 + 3
  - Fireball: Shoots a ball of magic fire at the enemy for 2d8 + 2 damage!

## How to play

Just download from releases and extract the .zip and execute the .exe to play!

## For other coders

### Architecture

Inicially I drafted this software architecture to help design the systems involved.
Not everything from this draft was included, for example the leveling system, but it ended up really similar and helped as a guide.

![AutoBattleRPG drawio](https://user-images.githubusercontent.com/37094911/224541149-c00bef86-ab62-4846-b09c-16b095cd2dae.png)

### Terrain Generation and Pathfinding

For the terrain, I used **perlin noise** with a threshold to apply forest tiles where the threshold was higher. This created patches of forest instead of just random tiles and made for a better effect.
This game uses an **A\* algorithm**, with weights on the edges to be able to have difficult terrain that is harder to walk through (forests).

![image](https://user-images.githubusercontent.com/37094911/224541454-5ebc84ec-568c-4497-86b8-303378e2c91b.png)

### AI - Behavior Tree

To create the AI for the different classes, I created a behavior tree with selectors and action nodes. It's structured in a similar way as it is in the architecture file, check the BehaviorTree folder for more details.

# Have fun!
