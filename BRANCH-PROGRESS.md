# Avancement Détaillé - Phase 2 : Fondations du jeu

**Branche** : `feature/phase2-foundations`
**Dernière mise à jour** : 2025-11-16
**Statut** : En cours - Personnage joueur contrôlable avec collisions!

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

5. **Refactoring majeur : Player → Character (support PNJ)**
   - ✅ **Raison** : Résolution de circularité `Clans.LeaderId ↔ Player.ClanId`
   - ✅ Modèle `Player` renommé en `Character`
   - ✅ Nouvelles propriétés ajoutées :
     - `IsPlayer` : bool pour distinguer joueurs/PNJ (défaut: true)
     - `IsClanLeader` : bool pour marquer le leader d'un clan (défaut: false)
     - `IsFactionLeader` : bool pour marquer le leader d'une faction (défaut: false)
   - ✅ Modèle `Clan` modifié :
     - Suppression de `LeaderId` (circularité résolue)
     - Suppression de la navigation `Leader`
     - Ajout de `Tag` (string nullable, max 5 caractères)
     - `CreatedAt` renommé en `FoundedDate` pour cohérence
   - ✅ Modèle `World` modifié :
     - `Players` renommé en `Characters`
     - Ajout de `CharacterCount` (propriété calculée)
     - `PlayerCount` maintenant calculé avec `Characters.Count(c => c.IsPlayer)`
   - ✅ `GameDbContext` : DbSet<Character> Characters
   - ✅ Configurations EF Core mises à jour :
     - `CharacterConfiguration` créée (remplace PlayerConfiguration)
     - `ClanConfiguration` : suppression de la relation Leader
     - `FactionConfiguration` : mise à jour des relations
     - `WorldConfiguration` : mise à jour des relations
   - ✅ Migration `RefactorPlayerToCharacter` créée et appliquée
   - ✅ Table MySQL `Players` renommée en `Characters`
   - ✅ Colonnes ajoutées : `IsPlayer`, `IsClanLeader`, `IsFactionLeader`
   - ✅ Colonne `LeaderId` supprimée de `Clans`
   - ✅ Colonne `Tag` ajoutée à `Clans`
   - ✅ Build réussie (0 erreurs, 0 warnings)

6. **Système monde procédural avec chunks et tiles**
   - ✅ Enum `TileType` créé (13 types : DeepWater, ShallowWater, Sand, Grass, Dirt, Forest, SparseForest, Hill, Mountain, SnowPeak, Swamp, Ruins, Radioactive)
   - ✅ Classe `Tile` créée :
     - Propriétés : Type, WorldX/Y, Elevation, Moisture, Temperature
     - Propriétés calculées : IsWalkable, MovementCost, CanHaveResources
     - Méthode `DetermineTypeFromBiome()` : génération basée sur élévation/humidité/température
   - ✅ Classe `Chunk` créée (32x32 tiles) :
     - Système de coordonnées : monde ↔ chunk ↔ local
     - Conversion automatique entre coordonnées
     - Gestion des coordonnées négatives
     - LastAccessed pour unload intelligent
   - ✅ Classe `SimplexNoise` créée :
     - Algorithme de bruit de Perlin 2D
     - Support du bruit fractal (octaves multiples)
     - Méthodes : Generate, GenerateFractal, GenerateNormalized
     - Table de permutation basée sur seed
   - ✅ Classe `WorldGenerator` créée :
     - Génération procédurale basée sur 3 couches de bruit (élévation, humidité, température)
     - Système de biomes déterministe
     - Ajout aléatoire de ruines (5% de chance par chunk)
     - Méthode FindValidSpawnPoint() pour spawn sécurisé
   - ✅ Classe `ChunkManager` créée :
     - Streaming de chunks (load/unload automatique)
     - Cache thread-safe (ConcurrentDictionary)
     - View distance configurable (défaut: 3 chunks)
     - Unload des chunks inactifs (défaut: 5 minutes)
     - Préchargement asynchrone
   - ✅ Classe `WorldConstants` créée :
     - ChunkSize = 32 tiles
     - TileSize = 32 pixels
     - Paramètres de génération (scales, octaves, persistence, lacunarity)
   - ✅ Build réussie (0 erreurs, 0 warnings)

7. **Système de rendu MonoGame avec caméra 2D**
   - ✅ Classe `Camera2D` créée :
     - Position, rotation et zoom
     - TransformMatrix pour transformation view
     - Méthodes : Move, CenterOn, GetVisibleArea
     - Conversion coordonnées écran ↔ monde
   - ✅ Classe `TileColors` créée :
     - Mapping couleur pour les 13 types de tiles
     - Variation de couleur basée sur coordonnées (pseudo-aléatoire)
     - Placeholder avant sprites
   - ✅ Classe `TileRenderer` créée :
     - Frustum culling : rendu uniquement des chunks visibles
     - DrawChunk : rendu de tous les tiles d'un chunk
     - DrawChunkGrid : grille de debug pour visualiser les chunks
     - Statistiques de rendu (chunks/tiles rendus par frame)
   - ✅ Classe `Game1` modifiée :
     - Intégration ChunkManager, Camera2D, TileRenderer
     - Résolution 1280x720
     - Contrôles caméra : WASD/Flèches (déplacement), Q/E (zoom)
     - Toggles debug : F1 (overlay debug), F2 (grille chunks)
     - Overlay debug avec FPS, position caméra, statistiques rendu
     - SamplerState.PointClamp pour pixel art
   - ✅ Configuration projet :
     - Nullable enabled
     - ImplicitUsings enabled
   - ✅ Build réussie (0 erreurs, 0 warnings)
   - ✅ **Monde procédural visualisable en temps réel!**

8. **Système de mouvement du personnage joueur**
   - ✅ Classe `PlayerCharacter` créée :
     - Position dans le monde (Vector2, en pixels)
     - MoveSpeed configurable (défaut: 150 px/s)
     - Size du personnage (défaut: 24px)
     - CollisionBox calculée dynamiquement
     - Update() : gestion des inputs ZQSD/flèches
     - TryMove() : mouvement séparé par axe (X puis Y)
     - CanMoveTo() : vérification collision sur les 4 coins
     - IsTileWalkable() : vérification walkability via ChunkManager
   - ✅ Classe `PlayerRenderer` créée :
     - Rendu rectangle coloré (bleu) pour le personnage
     - Bordure blanche pour meilleure visibilité
     - Point central (jaune) pour position exacte
     - Placeholder avant sprites/animations
   - ✅ Classe `Game1` modifiée :
     - Spawn du personnage au point valide (FindSpawnPoint)
     - Deux modes de caméra :
       - **Mode Follow** (défaut) : caméra suit le personnage automatiquement
       - **Mode Free** (F3) : caméra libre avec WASD
     - Contrôles personnage : ZQSD/flèches pour déplacement
     - Contrôles caméra : Q/E pour zoom (les deux modes)
     - Streaming de chunks centré sur le joueur (mode follow) ou caméra (mode free)
     - Debug overlay mis à jour : position joueur, mode caméra
     - PlayerRenderer.Dispose() dans cleanup
   - ✅ Détection de collision fonctionnelle :
     - Personnage bloqué par DeepWater, Mountain, SnowPeak
     - Mouvement fluide sur tiles walkable
     - Vérification des 4 coins de la collision box
     - Mouvement diagonal normalisé
   - ✅ Build réussie (0 erreurs, 0 warnings)
   - ✅ **Personnage contrôlable avec collisions!**

#### Tâches à réaliser

### Priorité Haute
- [x] Mettre en place le rendu MonoGame basique
  - [x] Caméra 2D
  - [x] Rendu du monde (chunks/tiles)
  - [x] Rendu du personnage
- [x] Système de mouvement du personnage
  - [x] Input clavier/souris
  - [x] Collision avec les tiles non traversables
  - [ ] Animation basique (sprites animés)
- [ ] Système de sprites
  - [ ] Charger les sprites des tiles
  - [ ] Charger le sprite du personnage
  - [ ] Animation du personnage (marche, idle)

### Priorité Moyenne
- [x] Système de configuration (appsettings.json)
- [x] Connection string MySQL
- [ ] Logging de base
- [ ] Système d'inventaire de base
- [ ] Système de ramassage d'items
- [ ] UI/HUD pour afficher stats joueur

### Priorité Basse
- [ ] Tests unitaires pour les modèles
- [ ] Documentation API
- [ ] Système de sauvegarde/chargement
- [ ] Son et musique

---

## État actuel du code

**Build** : ✅ Réussi (0 erreurs, 0 warnings)

**Projets** :
- `TinySurvivalWorld.Core` : 15 fichiers (5 enums, 5 modèles, 6 classes World)
- `TinySurvivalWorld.Data` : 7 fichiers (DbContext, Factory, 5 configurations)
- `TinySurvivalWorld.Shared` : 1 structure (Position)
- `TinySurvivalWorld.Game.Desktop` : 7 fichiers (Game1, 3 renderers, 1 caméra, 1 entité, appsettings)

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
- **Clan** : ID Guid, FactionId nullable, type ethnique pour clans indépendants, Tag optionnel
- **Character** (renommé depuis Player) : ID Guid, ethnie obligatoire, FactionId et ClanId nullables
  - `IsPlayer` : distingue joueurs (true) vs PNJ (false)
  - `IsClanLeader` : marque le leader du clan (remplace Clan.LeaderId - circularité résolue)
  - `IsFactionLeader` : marque le leader de la faction
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
- **Cascade delete** uniquement pour World → Characters (suppression logique)
- **SetNull** pour Faction → Clans/Characters et Clan → Characters (préservation des données)

**Schéma de base de données (après refactoring)** :
- **Factions** : ID int, unique sur Name
- **Clans** : ID Guid, FactionId nullable, Tag (5 car. max), pas de LeaderId
- **Characters** : ID Guid, WorldId obligatoire (cascade), FactionId/ClanId nullables, IsPlayer, IsClanLeader, IsFactionLeader
- **Worlds** : ID Guid, seed pour génération procédurale
- **Items** : ID Guid, unique sur Code, index sur Type

### Système monde procédural

**Décisions techniques** :
- **Chunks de 32x32 tiles** : équilibre entre performances et granularité
- **Tiles de 32x32 pixels** : standard pour jeux 2D, compatible avec sprites
- **Monde infini** : génération procédurale à la demande (streaming de chunks)
- **Bruit de Perlin** : algorithme de SimplexNoise pour génération déterministe
- **3 couches de bruit** : Elevation, Moisture, Temperature pour variété des biomes
- **13 types de tiles** : variété suffisante pour monde post-apocalyptique
- **Streaming intelligent** : chunks chargés/déchargés selon distance de la caméra
- **Thread-safe** : ConcurrentDictionary pour accès multi-thread

**Architecture** :
- `Tile` : unité de base du monde (propriétés physiques + gameplay)
- `Chunk` : conteneur de 32x32 tiles (optimisation mémoire/rendu)
- `SimplexNoise` : générateur de bruit pour procédural
- `WorldGenerator` : logique de génération des chunks
- `ChunkManager` : gestion du cycle de vie des chunks (load/unload/cache)
- `WorldConstants` : paramètres de configuration centralisés

### Système de rendu MonoGame

**Décisions techniques** :
- **Caméra 2D avec matrices** : TransformMatrix pour transformations view standardisées
- **Frustum culling** : rendu uniquement des chunks visibles (optimisation performance)
- **SamplerState.PointClamp** : sampling pixel-perfect pour style pixel art
- **Placeholder colors** : couleurs avant implémentation des sprites
- **Variation de couleur** : pseudo-aléatoire basée sur coordonnées pour variation visuelle
- **Debug overlay** : statistiques de rendu en temps réel (FPS, chunks/tiles rendus)
- **Résolution fixe** : 1280x720 pour développement

**Architecture** :
- `Camera2D` : gestion de la vue (position, zoom, rotation, conversions coordonnées)
- `TileColors` : mapping couleur pour chaque type de tile
- `TileRenderer` : rendu optimisé des chunks visibles
- `PlayerRenderer` : rendu du personnage (placeholder rectangle)

### Système de mouvement du personnage

**Décisions techniques** :
- **Mouvement basé sur la physique** : vitesse en pixels/seconde avec deltaTime
- **Collision par coins** : vérification des 4 coins de la collision box (précision)
- **Séparation d'axes** : mouvement X puis Y séparément (sliding le long des murs)
- **Normalisation diagonale** : vitesse constante quelle que soit la direction
- **Tile-based collision** : détection basée sur les propriétés IsWalkable des tiles
- **Deux modes de caméra** :
  - **Follow mode** : caméra centrée automatiquement sur le joueur
  - **Free mode** : caméra libre pour exploration/debug
- **Streaming centré** : chunks chargés autour du joueur (follow) ou caméra (free)

**Architecture** :
- `PlayerCharacter` : logique de mouvement, collision, update
- `PlayerRenderer` : rendu visuel du personnage
- Intégration dans `Game1` : gestion des deux modes caméra

### Questions en suspens
1. ~~**Taille du monde**~~ : ✅ Monde infini (génération procédurale)
2. ~~**Taille des chunks**~~ : ✅ 32x32 tiles
3. ~~**Taille des tiles**~~ : ✅ 32x32 pixels
4. **Format de sauvegarde** : Base de données uniquement ou fichiers JSON + DB ?
5. **Assets graphiques** : Placeholder ou création initiale ?
6. **Compression des chunks** : Sauvegarder les chunks modifiés ?

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
- `TileType.cs` : Types de tuiles du monde (13 types de terrains/biomes)

**Core/Models/** :
- `Faction.cs` : Modèle avec validation d'ethnie
- `Clan.cs` : Modèle avec logique de recrutement complexe (Tag ajouté, LeaderId supprimé)
- `Character.cs` : Modèle avec statistiques de survie, affiliations et monde (renommé depuis Player.cs, avec IsPlayer, IsClanLeader, IsFactionLeader)
- ~~`Player.cs`~~ : Supprimé et remplacé par Character.cs
- `World.cs` : Modèle de monde avec seed, difficulté, taille (Players → Characters)
- `Item.cs` : Catalogue d'items avec propriétés variées

**Core/World/** :
- `Tile.cs` : Représentation d'une tuile (Type, Elevation, Moisture, Temperature, IsWalkable, MovementCost, DetermineTypeFromBiome())
- `Chunk.cs` : Conteneur de 32x32 tiles avec conversions de coordonnées (world ↔ chunk ↔ local)
- `SimplexNoise.cs` : Générateur de bruit de Perlin 2D avec support fractal
- `WorldGenerator.cs` : Générateur procédural de chunks (3 couches de bruit, biomes, ruines aléatoires, FindValidSpawnPoint())
- `ChunkManager.cs` : Gestionnaire de streaming de chunks (load/unload, cache thread-safe, préchargement async)
- `WorldConstants.cs` : Constantes (ChunkSize, TileSize, scales de bruit, octaves, persistence, lacunarity)

**Data/** :
- `GameDbContext.cs` : DbContext principal avec DbSets pour toutes les entités
- `GameDbContextFactory.cs` : Factory pour design-time DbContext (migrations)

**Data/Configurations/** :
- `FactionConfiguration.cs` : Configuration FluentAPI pour Faction (mise à jour pour Character)
- `ClanConfiguration.cs` : Configuration FluentAPI pour Clan (suppression relation Leader, ajout Tag)
- `CharacterConfiguration.cs` : Configuration FluentAPI pour Character (remplace PlayerConfiguration.cs)
- ~~`PlayerConfiguration.cs`~~ : Supprimé et remplacé par CharacterConfiguration.cs
- `WorldConfiguration.cs` : Configuration FluentAPI pour World (mise à jour pour Characters)
- `ItemConfiguration.cs` : Configuration FluentAPI pour Item

**Data/Migrations/** :
- `20251115162405_InitialCreate.cs` : Migration initiale
- `20251115162405_InitialCreate.Designer.cs` : Designer de migration
- `20251115170728_RefactorPlayerToCharacter.cs` : Migration Player → Character (renommage table, ajout colonnes, suppression LeaderId)
- `20251115170728_RefactorPlayerToCharacter.Designer.cs` : Designer de migration refactoring
- `GameDbContextModelSnapshot.cs` : Snapshot du modèle EF Core (mis à jour)

**Game.Desktop/** :
- `appsettings.json` : Configuration production avec connection string MySQL
- `appsettings.Development.json` : Configuration développement avec logging détaillé
- `Game1.cs` : Classe principale du jeu (Update, Draw, gestion caméra, personnage, monde)
- `TinySurvivalWorld.Game.Desktop.csproj` : Configuration projet (Nullable, ImplicitUsings enabled)

**Game.Desktop/Rendering/** :
- `Camera2D.cs` : Caméra 2D avec zoom, rotation, TransformMatrix
- `TileColors.cs` : Mapping couleur pour les types de tiles (placeholder)
- `TileRenderer.cs` : Rendu optimisé des chunks et tiles avec frustum culling
- `PlayerRenderer.cs` : Rendu du personnage joueur (rectangle coloré placeholder)

**Game.Desktop/Entities/** :
- `PlayerCharacter.cs` : Logique de mouvement et collision du personnage joueur

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

**Fichiers créés** : 37 (1 structure, 5 enums, 5 modèles, 6 classes World, 1 DbContext, 1 Factory, 5 configurations, 5 migrations, 2 appsettings, 1 Game1, 4 renderers, 1 entité)
**Fichiers supprimés** : 2 (Player.cs, PlayerConfiguration.cs - remplacés par Character.cs et CharacterConfiguration.cs)
**Lignes de code ajoutées** : ~3350
**Commits** : 6 (init + modèles + EF Core config + refactoring Character + monde + rendu + mouvement)
**Tests** : 0
**Build** : ✅ Réussie (0 erreurs, 0 warnings)
**Base de données** : ✅ Mise à jour avec table Characters
**Système monde** : ✅ Génération procédurale fonctionnelle (chunks 32x32, 13 types de tiles, streaming intelligent)
**Système rendu** : ✅ MonoGame avec caméra 2D, frustum culling, debug overlay
**Système mouvement** : ✅ Personnage contrôlable avec collisions tile-based, deux modes caméra

---

## Références utiles

- [MonoGame 2D Camera Tutorial](https://www.monogame.net/documentation/?page=Tutorials)
- [EF Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Perlin Noise for Terrain Generation](https://en.wikipedia.org/wiki/Perlin_noise)

---

**Note** : Ce fichier doit être mis à jour régulièrement pendant le développement de cette phase.
