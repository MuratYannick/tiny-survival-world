# Systèmes de jeu - Tiny Survival World

## Vue d'ensemble

Ce document décrit tous les systèmes de gameplay de Tiny Survival World. Chaque système est documenté avec ses mécaniques, son état d'implémentation et ses dépendances.

---

## État général

| Système | Statut | Priorité | Version cible |
|---------|--------|----------|---------------|
| Monde/Carte | 📝 Planifié | Haute | 0.2.0 |
| Joueur | 📝 Planifié | Haute | 0.2.0 |
| Mouvement | 📝 Planifié | Haute | 0.2.0 |
| Inventaire | 📝 Planifié | Haute | 0.3.0 |
| Ressources | 📝 Planifié | Haute | 0.3.0 |
| Crafting | 📝 Planifié | Moyenne | 0.4.0 |
| Survie | 📝 Planifié | Moyenne | 0.4.0 |
| Combat | 📝 Planifié | Moyenne | 0.5.0 |
| Construction | 📝 Planifié | Moyenne | 0.5.0 |
| NPCs | 📝 Planifié | Basse | 0.6.0 |
| Quêtes | 📝 Planifié | Basse | 0.6.0 |

**Légende** : 📝 Planifié | 🚧 En cours | ✅ Implémenté | ⚠️ Problème

---

## 1. Système de Monde/Carte

### Description
Génération et gestion du monde de jeu en 2D, incluant le terrain, les biomes et les ressources naturelles.

### Fonctionnalités planifiées
- Génération procédurale basée sur seed
- Système de chunks pour optimiser les performances
- Différents biomes (forêt, désert, montagnes, ruines urbaines)
- Ressources naturelles distribuées selon les biomes
- Système jour/nuit

### Implémentation
**Fichiers** :
- `Core/World/WorldGenerator.cs` (à créer)
- `Core/World/Chunk.cs` (à créer)
- `Core/World/Biome.cs` (à créer)
- `Core/World/Tile.cs` (à créer)

**Namespace** : `TinySurvivalWorld.Core.World`

### Dépendances
- Aucune (système fondamental)

### Notes techniques
- Utiliser Perlin/Simplex noise pour la génération
- Taille de chunk : 16x16 tiles
- Taille de tile : 32x32 pixels
- Chargement/déchargement dynamique des chunks

---

## 2. Système de Joueur

### Description
Gestion de l'entité joueur, ses statistiques, sa progression et son état.

### Fonctionnalités planifiées
- Statistiques de base (santé, faim, soif, énergie)
- Système de niveau et expérience
- Compétences/skills
- États (normal, affamé, assoiffé, blessé, etc.)

### Implémentation
**Fichiers** :
- `Core/Player/Player.cs` (à créer)
- `Core/Player/PlayerStats.cs` (à créer)
- `Core/Player/PlayerState.cs` (à créer)
- `Core/Player/PlayerSkills.cs` (à créer)

**Namespace** : `TinySurvivalWorld.Core.Player`

### Dépendances
- Système de Monde (position)
- Système de Survie (statistiques)

---

## 3. Système de Mouvement

### Description
Gestion du déplacement du joueur dans le monde, détection de collisions, pathfinding.

### Fonctionnalités planifiées
- Déplacement en 8 directions (WASD/Flèches)
- Détection de collisions avec le terrain
- Vitesse variable selon le terrain
- Animation de marche/course
- Support gamepad et touch (mobile)

### Implémentation
**Fichiers** :
- `Core/Movement/MovementSystem.cs` (à créer)
- `Core/Movement/CollisionDetector.cs` (à créer)
- `Game.Desktop/Input/InputManager.cs` (à créer)

**Namespace** :
- Core : `TinySurvivalWorld.Core.Movement`
- Game : `TinySurvivalWorld.Game.Desktop.Input`

### Dépendances
- Système de Monde (terrain et collisions)
- Système de Joueur (position et vitesse)

### Notes techniques
- Utiliser un système de vélocité
- 60 FPS cible pour le mouvement fluide
- Delta time pour l'indépendance du framerate

---

## 4. Système d'Inventaire

### Description
Gestion de l'inventaire du joueur, stockage, empilement d'items.

### Fonctionnalités planifiées
- Grille d'inventaire (NxM slots)
- Empilement d'items (stackable)
- Tri automatique
- Filtrage par catégorie
- Équipement rapide (hotbar)

### Implémentation
**Fichiers** :
- `Core/Inventory/Inventory.cs` (à créer)
- `Core/Inventory/InventorySlot.cs` (à créer)
- `Core/Inventory/Item.cs` (à créer)
- `Game.Desktop/UI/InventoryUI.cs` (à créer)

**Namespace** : `TinySurvivalWorld.Core.Inventory`

### Dépendances
- Système de Joueur (propriétaire)
- Items (base de données)

### Notes techniques
- Taille initiale : 20 slots (4x5)
- Hotbar : 5 slots d'accès rapide
- Pattern Observer pour notifier les changements à l'UI

---

## 5. Système de Ressources

### Description
Récolte de ressources dans le monde (bois, pierre, nourriture, etc.).

### Fonctionnalités planifiées
- Interaction avec les ressources du monde
- Animation de récolte
- Durée de récolte variable
- Outils requis pour certaines ressources
- Durabilité des outils
- Ressources qui se régénèrent

### Implémentation
**Fichiers** :
- `Core/Resources/ResourceNode.cs` (à créer)
- `Core/Resources/HarvestingSystem.cs` (à créer)
- `Core/Resources/Tool.cs` (à créer)

**Namespace** : `TinySurvivalWorld.Core.Resources`

### Dépendances
- Système de Monde (placement des ressources)
- Système d'Inventaire (stockage des ressources récoltées)
- Système de Joueur (interaction)

---

## 6. Système de Crafting

### Description
Fabrication d'items à partir de ressources et de recettes.

### Fonctionnalités planifiées
- Recettes à débloquer
- Interface de craft
- Temps de fabrication
- Niveau/compétence requis
- Stations de craft (établi, forge, etc.)

### Implémentation
**Fichiers** :
- `Core/Crafting/Recipe.cs` (à créer)
- `Core/Crafting/CraftingSystem.cs` (à créer)
- `Core/Crafting/CraftingStation.cs` (à créer)
- `Game.Desktop/UI/CraftingUI.cs` (à créer)

**Namespace** : `TinySurvivalWorld.Core.Crafting`

### Dépendances
- Système d'Inventaire (consommation/production d'items)
- Système de Joueur (niveau et compétences)
- Base de données (recettes)

---

## 7. Système de Survie

### Description
Gestion des besoins vitaux du joueur (faim, soif, santé, température).

### Fonctionnalités planifiées
- Jauge de faim (diminue avec le temps et l'effort)
- Jauge de soif (diminue plus vite que la faim)
- Régénération de santé si bien nourri
- Dégâts si affamé/assoiffé
- Système de température (chaud/froid)
- Effets météo

### Implémentation
**Fichiers** :
- `Core/Survival/SurvivalSystem.cs` (à créer)
- `Core/Survival/HungerManager.cs` (à créer)
- `Core/Survival/ThirstManager.cs` (à créer)
- `Core/Survival/TemperatureManager.cs` (à créer)

**Namespace** : `TinySurvivalWorld.Core.Survival`

### Dépendances
- Système de Joueur (statistiques)
- Système de Monde (météo, température ambiante)
- Système d'Inventaire (consommation de nourriture/eau)

### Notes techniques
- Update toutes les secondes pour optimiser
- Taux de diminution configurable selon la difficulté
- Seuils : 0-25% = critique, 25-50% = faible, 50-100% = normal

---

## 8. Système de Combat

### Description
Combat contre des ennemis, PvE (et éventuellement PvP).

### Fonctionnalités planifiées
- Armes de mêlée et à distance
- Système de dégâts
- Animations d'attaque
- Hitbox et détection de collision
- IA basique des ennemis
- Drops des ennemis vaincus

### Implémentation
**Fichiers** :
- `Core/Combat/CombatSystem.cs` (à créer)
- `Core/Combat/Weapon.cs` (à créer)
- `Core/Combat/Enemy.cs` (à créer)
- `Core/Combat/DamageCalculator.cs` (à créer)

**Namespace** : `TinySurvivalWorld.Core.Combat`

### Dépendances
- Système de Joueur (santé, attaque)
- Système d'Inventaire (armes équipées)
- Système de Mouvement (positionnement)

---

## 9. Système de Construction

### Description
Construction de structures (abris, murs, stockage, etc.).

### Fonctionnalités planifiées
- Placement de structures
- Mode construction/destruction
- Coût en ressources
- Grille de placement
- Aperçu de placement (ghost)
- Durabilité des structures

### Implémentation
**Fichiers** :
- `Core/Building/BuildingSystem.cs` (à créer)
- `Core/Building/Structure.cs` (à créer)
- `Core/Building/BuildingPlacer.cs` (à créer)
- `Game.Desktop/BuildMode/BuildModeController.cs` (à créer)

**Namespace** : `TinySurvivalWorld.Core.Building`

### Dépendances
- Système de Monde (modification du terrain)
- Système d'Inventaire (coût en ressources)
- Système de Crafting (recettes de structures)

---

## 10. Système de NPCs

### Description
Personnages non-joueurs, marchands, alliés, ennemis.

### Fonctionnalités planifiées
- IA de base (patrouille, fuite, attaque)
- Dialogues
- Commerce/échange
- Quêtes données par NPCs
- Relations (ami/neutre/ennemi)

### Implémentation
**Fichiers** :
- `Core/NPCs/NPC.cs` (à créer)
- `Core/NPCs/NPCBehavior.cs` (à créer)
- `Core/NPCs/DialogueSystem.cs` (à créer)
- `Core/NPCs/TradeSystem.cs` (à créer)

**Namespace** : `TinySurvivalWorld.Core.NPCs`

### Dépendances
- Système de Mouvement (déplacement NPCs)
- Système de Combat (NPCs ennemis)
- Système d'Inventaire (commerce)

---

## 11. Système de Quêtes

### Description
Missions et objectifs pour guider le joueur.

### Fonctionnalités planifiées
- Quêtes principales (storyline)
- Quêtes secondaires
- Objectifs multiples
- Récompenses (XP, items, etc.)
- Tracking de progression
- Journal de quêtes

### Implémentation
**Fichiers** :
- `Core/Quests/Quest.cs` (à créer)
- `Core/Quests/QuestObjective.cs` (à créer)
- `Core/Quests/QuestManager.cs` (à créer)
- `Game.Desktop/UI/QuestJournalUI.cs` (à créer)

**Namespace** : `TinySurvivalWorld.Core.Quests`

### Dépendances
- Système de Joueur (XP, récompenses)
- Système de NPCs (donneurs de quêtes)
- Tous les autres systèmes (objectifs variés)

---

## Priorisation

### Phase 1 (v0.2.0) - Fondations
1. Système de Monde
2. Système de Joueur
3. Système de Mouvement

### Phase 2 (v0.3.0) - Collecte
4. Système d'Inventaire
5. Système de Ressources

### Phase 3 (v0.4.0) - Survie
6. Système de Crafting
7. Système de Survie

### Phase 4 (v0.5.0) - Action
8. Système de Combat
9. Système de Construction

### Phase 5 (v0.6.0) - Contenu
10. Système de NPCs
11. Système de Quêtes

---

**Dernière mise à jour** : 2025-11-15
**Version** : 0.1.0-alpha
