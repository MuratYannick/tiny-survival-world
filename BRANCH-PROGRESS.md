# Avancement Détaillé - Phase 2 : Fondations du jeu

**Branche** : `feature/phase2-foundations`
**Dernière mise à jour** : 2025-11-15
**Statut** : En cours - Base de données MySQL créée

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

3. **Configuration Entity Framework Core**
   - ✅ `GameDbContext` créé avec DbSets pour toutes les entités
   - ✅ 5 configurations FluentAPI créées :
     - `FactionConfiguration` : table Factions, relations One-to-Many avec Clans/Players
     - `ClanConfiguration` : table Clans, relation One-to-One avec Leader
     - `PlayerConfiguration` : table Players, relation Many-to-One avec World (Cascade)
     - `WorldConfiguration` : table Worlds, valeurs par défaut, relation One-to-Many avec Players
     - `ItemConfiguration` : table Items, index unique sur Code, valeurs par défaut
   - ✅ `appsettings.json` et `appsettings.Development.json` configurés avec connection strings MySQL
   - ✅ Packages NuGet installés :
     - `Microsoft.EntityFrameworkCore` 9.0.0
     - `Microsoft.EntityFrameworkCore.Design` 9.0.0
     - `Pomelo.EntityFrameworkCore.MySql` 9.0.0
     - `MySqlConnector` 2.5.0
     - `Microsoft.Extensions.Configuration.Json` 10.0.0
     - `Microsoft.Extensions.Configuration.Binder` 10.0.0
   - ✅ `GameDbContextFactory` créé pour migrations EF Core (IDesignTimeDbContextFactory)
   - ✅ Migration initiale `InitialCreate` générée avec succès
   - ✅ Build réussie sans erreurs

4. **Application de la migration et création de la base de données**
   - ✅ WampServer démarré avec MySQL
   - ✅ Migration `InitialCreate` appliquée avec succès
   - ✅ Base de données `tinysurvivalworld_dev` créée
   - ✅ 5 tables créées dans MySQL :
     - `Factions` : table des factions avec index unique sur Name
     - `Items` : catalogue d'items avec index unique sur Code, index sur Type
     - `Worlds` : table des mondes avec seed et configuration
     - `Clans` : table des clans avec FK vers Factions et LeaderId
     - `Players` : table des joueurs avec FK vers World (Cascade), Faction, Clan
   - ✅ Table `__EFMigrationsHistory` créée automatiquement par EF Core

#### Tâches à réaliser

### Priorité Haute
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
- `TinySurvivalWorld.Core` : 9 fichiers (4 enums, 5 modèles)
- `TinySurvivalWorld.Data` : 7 fichiers (DbContext, Factory, 5 configurations)
- `TinySurvivalWorld.Shared` : 1 structure (Position)
- `TinySurvivalWorld.Game.Desktop` : MonoGame + appsettings configurés

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

### Configuration Entity Framework Core

**Choix techniques** :
- **FluentAPI** au lieu d'attributs pour la configuration des entités
- **Pomelo MySQL Provider** 9.0.0 pour MySQL
- **Version MySQL fixe** (8.0.40) dans le DbContextFactory pour éviter connexion au design-time
- **Pattern DbContextFactory** pour permettre les migrations sans serveur MySQL actif
- **Cascade delete** uniquement pour World → Players (suppression logique)
- **SetNull** pour Faction → Clans/Players (préservation des données)
- **Restrict** pour Clan.Leader → Player (éviter suppression accidentelle)

**Schéma de base de données** :
- **Factions** : ID int, unique sur Name
- **Clans** : ID Guid, FactionId nullable, LeaderId avec restriction
- **Players** : ID Guid, WorldId obligatoire (cascade), FactionId/ClanId nullables
- **Worlds** : ID Guid, seed pour génération procédurale
- **Items** : ID Guid, unique sur Code, index sur Type

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

**Data/** :
- `GameDbContext.cs` : DbContext principal avec DbSets pour toutes les entités
- `GameDbContextFactory.cs` : Factory pour design-time DbContext (migrations)

**Data/Configurations/** :
- `FactionConfiguration.cs` : Configuration FluentAPI pour Faction
- `ClanConfiguration.cs` : Configuration FluentAPI pour Clan
- `PlayerConfiguration.cs` : Configuration FluentAPI pour Player
- `WorldConfiguration.cs` : Configuration FluentAPI pour World
- `ItemConfiguration.cs` : Configuration FluentAPI pour Item

**Data/Migrations/** :
- `20251115162405_InitialCreate.cs` : Migration initiale
- `20251115162405_InitialCreate.Designer.cs` : Designer de migration
- `GameDbContextModelSnapshot.cs` : Snapshot du modèle EF Core

**Game.Desktop/** :
- `appsettings.json` : Configuration production avec connection string MySQL
- `appsettings.Development.json` : Configuration développement avec logging détaillé

### Problèmes rencontrés et solutions

1. **dotnet-ef non installé**
   - **Problème** : L'outil global dotnet-ef n'était pas installé
   - **Solution** : `dotnet tool install --global dotnet-ef --version 9.0.0`

2. **Incompatibilité de version dotnet-ef**
   - **Problème** : Installation initiale de dotnet-ef 10.0.0 incompatible avec EF Core 9.0.0
   - **Erreur** : `FileNotFoundException: System.Runtime, Version=10.0.0.0`
   - **Solution** : Désinstallation de 10.0.0 et installation de 9.0.0 spécifiquement

3. **EF Core Design manquant dans Game.Desktop**
   - **Problème** : Le projet startup n'avait pas la référence Microsoft.EntityFrameworkCore.Design
   - **Solution** : Ajout du package au projet Game.Desktop

4. **ServerVersion.AutoDetect nécessite MySQL actif**
   - **Problème** : Le DbContextFactory utilisait AutoDetect qui essayait de se connecter à MySQL
   - **Erreur** : `Unable to connect to any of the specified MySQL hosts`
   - **Solution** : Utilisation d'une version MySQL fixe (8.0.40) dans le factory pour design-time

---

## Statistiques

**Fichiers créés** : 22 (1 structure, 4 enums, 5 modèles, 1 DbContext, 1 Factory, 5 configurations, 3 migrations, 2 appsettings)
**Lignes de code ajoutées** : ~1500
**Commits** : 3 (init + modèles + EF Core config)
**Tests** : 0
**Build** : ✅ Réussie (0 erreurs, 0 warnings)
**Base de données** : ✅ Créée avec 5 tables (MySQL 8.0 via WampServer)

---

## Références utiles

- [MonoGame 2D Camera Tutorial](https://www.monogame.net/documentation/?page=Tutorials)
- [EF Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Perlin Noise for Terrain Generation](https://en.wikipedia.org/wiki/Perlin_noise)

---

**Note** : Ce fichier doit être mis à jour régulièrement pendant le développement de cette phase.
