# Avancement Détaillé - Phase 2 : Fondations du jeu

**Branche** : `feature/phase2-foundations`
**Dernière mise à jour** : 2025-11-15
**Statut** : En cours - Modèles de domaine créés

---

## Objectifs de la Phase 2

Cette phase vise à mettre en place les **fondations du jeu** :
- Modèles de données de base (Player, World, Items)
- Schéma de base de données MySQL avec Entity Framework Core
- Système de monde/carte basique
- Système de joueur
- Rendu MonoGame minimal pour visualiser le monde et le joueur

---

## Session en cours

### Date : 2025-11-15

#### Objectif de la session
Initialiser la branche de la phase 2 et commencer l'implémentation des modèles de domaine.

#### Tâches complétées ✅

1. **Modèles de domaine - Système de factions et clans**
   - ✅ Enum `Ethnicity` (Éveillés / Inaltérés)
   - ✅ Enum `ClanEthnicityType` (restrictions ethniques des clans)
   - ✅ Modèle `Faction` avec logique de recrutement
   - ✅ Modèle `Clan` avec règles d'affiliation
   - ✅ Modèle `Player` avec statistiques de base et affiliations
   - ✅ Logique métier pour vérifier les règles de rejoindre faction/clan

2. **Modèles de domaine - Monde et Items**
   - ✅ Structure `Position` (Shared) avec opérateurs et méthodes utilitaires
   - ✅ Enum `Difficulty` (Easy, Normal, Hard, Hardcore)
   - ✅ Enum `ItemType` (Resource, Tool, Weapon, Armor, Food, etc.)
   - ✅ Modèle `World` avec génération procédurale (seed, taille, difficulté)
   - ✅ Modèle `Item` (catalogue d'items avec propriétés variées)
   - ✅ Relation Player → World ajoutée
   - ✅ Build réussie sans erreurs (2 sessions)

#### Tâches à réaliser

### Priorité Haute
- [ ] Créer le DbContext EF Core dans `Data/`
- [ ] Configurer les entités EF Core
- [ ] Créer la migration initiale
- [ ] Implémenter le système de monde basique
  - [ ] Génération simple de terrain
  - [ ] Structure de chunks
  - [ ] Tiles
- [ ] Créer le système de joueur basique
  - [ ] Statistiques de base
  - [ ] Position et mouvement
- [ ] Mettre en place le rendu MonoGame basique
  - [ ] Caméra 2D
  - [ ] Rendu du monde
  - [ ] Rendu du joueur

### Priorité Moyenne
- [ ] Système de configuration (appsettings.json)
- [ ] Connection string MySQL
- [ ] Logging de base

### Priorité Basse
- [ ] Tests unitaires pour les modèles
- [ ] Documentation API

---

## État actuel du code

**Build** : ✅ Réussi (0 erreurs, 0 warnings)

**Projets** :
- `TinySurvivalWorld.Core` : Vide
- `TinySurvivalWorld.Data` : Vide
- `TinySurvivalWorld.Shared` : Vide
- `TinySurvivalWorld.Game.Desktop` : Template MonoGame de base

---

## Décisions techniques prises

### Système de factions et clans
**Contexte du jeu** :
- 2 ethnies : Éveillés (mutants) et Inaltérés (non-mutants)
- 2 factions exclusives par ethnie :
  - "Éclaireurs de l'Aube Nouvelle" (Éveillés)
  - "Veilleurs de l'Ancien Monde" (Inaltérés)
- Clans affiliés ou indépendants avec règles de recrutement complexes

**Implémentation** :
- **Faction** : ID int (seulement 2 factions fixes), ethnie requise, collections de clans et membres
- **Clan** : ID Guid, FactionId nullable, type ethnique pour clans indépendants
- **Player** : ID Guid, ethnie obligatoire, FactionId et ClanId nullables
- **Règles de recrutement** :
  - Clans de faction : recrutent uniquement membres de leur faction
  - Clans indépendants : recrutent uniquement sans faction, avec restrictions ethniques
  - Méthodes `CanJoin()` pour valider les règles métier

### Questions en suspens
1. **Taille du monde** : Monde infini ou délimité ?
2. **Taille des chunks** : 16x16 ou 32x32 tiles ?
3. **Taille des tiles** : 32x32 ou 64x64 pixels ?
4. **Format de sauvegarde** : Base de données uniquement ou fichiers JSON + DB ?
5. **Assets graphiques** : Placeholder ou création initiale ?

---

## Notes de développement

### Fichiers créés

**Shared/Structures/** :
- `Position.cs` : Structure position 2D avec distance, lerp, opérateurs

**Core/Enums/** :
- `Ethnicity.cs` : Éveillés vs Inaltérés
- `ClanEthnicityType.cs` : Restrictions ethniques des clans
- `Difficulty.cs` : Niveaux de difficulté du monde
- `ItemType.cs` : Types d'items (Resource, Tool, Weapon, etc.)

**Core/Models/** :
- `Faction.cs` : Modèle avec validation d'ethnie
- `Clan.cs` : Modèle avec logique de recrutement complexe
- `Player.cs` : Modèle avec statistiques de survie, affiliations et monde
- `World.cs` : Modèle de monde avec seed, difficulté, taille
- `Item.cs` : Catalogue d'items avec propriétés variées

### Problèmes rencontrés et solutions
_Aucun problème rencontré - Build réussie du premier coup_

---

## Statistiques

**Fichiers créés** : 10 (1 structure, 4 enums, 5 modèles)
**Lignes de code ajoutées** : ~800
**Commits** : 2 (initialisation branche + factions/clans)
**Tests** : 0
**Build** : ✅ Réussie (0 erreurs, 0 warnings)

---

## Références utiles

- [MonoGame 2D Camera Tutorial](https://www.monogame.net/documentation/?page=Tutorials)
- [EF Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Perlin Noise for Terrain Generation](https://en.wikipedia.org/wiki/Perlin_noise)

---

**Note** : Ce fichier doit être mis à jour régulièrement pendant le développement de cette phase.
