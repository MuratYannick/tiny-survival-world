# Schéma de base de données - Tiny Survival World

## Vue d'ensemble

Base de données : **MySQL**
Provider : **Pomelo.EntityFrameworkCore.MySql 9.0.0**
ORM : **Entity Framework Core 9.0.0**

---

## État actuel

**Statut** : Modèles créés, en attente de configuration EF Core
**Migrations** : Aucune
**Modèles implémentés** : Player, Faction, Clan

---

## Schéma planifié

### Tables principales

#### Factions
Représente les deux factions majeures du jeu, exclusives par ethnie.

| Colonne | Type | Contraintes | Description |
|---------|------|-------------|-------------|
| Id | INT | PK, AUTO_INCREMENT | Identifiant unique |
| Name | VARCHAR(100) | NOT NULL, UNIQUE | Nom de la faction |
| Description | TEXT | NOT NULL | Description et philosophie |
| RequiredEthnicity | TINYINT | NOT NULL | Ethnie requise (0=Éveillés, 1=Inaltérés) |
| FoundedDate | DATETIME | NOT NULL | Date de fondation |

**Données fixes** :
1. "Éclaireurs de l'Aube Nouvelle" (RequiredEthnicity = 0 - Éveillés)
2. "Veilleurs de l'Ancien Monde" (RequiredEthnicity = 1 - Inaltérés)

---

#### Clans
Représente les clans du jeu, affiliés à une faction ou indépendants.

| Colonne | Type | Contraintes | Description |
|---------|------|-------------|-------------|
| Id | GUID/UUID | PK | Identifiant unique |
| Name | VARCHAR(100) | NOT NULL, UNIQUE | Nom du clan |
| Description | TEXT | NULL | Description du clan |
| FactionId | INT | NULL, FK → Factions | Faction affiliée (null = indépendant) |
| EthnicityType | TINYINT | NOT NULL | Type ethnique (0=Éveillés, 1=Inaltérés, 2=Mixte) |
| MaxMembers | INT | NOT NULL, DEFAULT 50 | Nombre max de membres |
| LeaderId | GUID/UUID | NOT NULL, FK → Players | Leader du clan |
| CreatedAt | DATETIME | NOT NULL | Date de création |

**Règles de recrutement** :
- Si FactionId != NULL : recrute uniquement membres de la faction
- Si FactionId = NULL : recrute uniquement joueurs sans faction, selon EthnicityType

---

#### Players (Joueurs)
Stocke les informations des joueurs et leur progression.

| Colonne | Type | Contraintes | Description |
|---------|------|-------------|-------------|
| Id | GUID/UUID | PK | Identifiant unique |
| Name | VARCHAR(50) | NOT NULL, UNIQUE | Nom du joueur |
| Ethnicity | TINYINT | NOT NULL | Ethnie (0=Éveillés, 1=Inaltérés) |
| FactionId | INT | NULL, FK → Factions | Faction du joueur (null si aucune) |
| ClanId | GUID/UUID | NULL, FK → Clans | Clan du joueur (null si aucun) |
| Level | INT | NOT NULL, DEFAULT 1 | Niveau du joueur |
| Experience | INT | NOT NULL, DEFAULT 0 | Points d'expérience |
| Health | FLOAT | NOT NULL, DEFAULT 100 | Santé actuelle |
| MaxHealth | FLOAT | NOT NULL, DEFAULT 100 | Santé maximale |
| Hunger | FLOAT | NOT NULL, DEFAULT 0 | Niveau de faim (0-100) |
| Thirst | FLOAT | NOT NULL, DEFAULT 0 | Niveau de soif (0-100) |
| PositionX | FLOAT | NOT NULL, DEFAULT 0 | Position X dans le monde |
| PositionY | FLOAT | NOT NULL, DEFAULT 0 | Position Y dans le monde |
| CreatedAt | DATETIME | NOT NULL | Date de création |
| UpdatedAt | DATETIME | NOT NULL | Dernière mise à jour |
| LastLogin | DATETIME | NOT NULL | Dernière connexion |

---

#### Worlds (Mondes)
Représente les différents mondes/sauvegardes de jeu.

| Colonne | Type | Contraintes | Description |
|---------|------|-------------|-------------|
| Id | GUID/UUID | PK | Identifiant unique |
| Name | VARCHAR(100) | NOT NULL, UNIQUE | Nom du monde |
| Seed | BIGINT | NOT NULL | Seed de génération procédurale |
| GameTime | BIGINT | NOT NULL, DEFAULT 0 | Temps de jeu en millisecondes |
| Difficulty | TINYINT | NOT NULL, DEFAULT 1 | Difficulté (0=Easy, 1=Normal, 2=Hard, 3=Hardcore) |
| IsHardcore | BOOLEAN | NOT NULL, DEFAULT false | Mode permadeath activé |
| WorldSizeX | INT | NOT NULL, DEFAULT 0 | Largeur en chunks (0=infini) |
| WorldSizeY | INT | NOT NULL, DEFAULT 0 | Hauteur en chunks (0=infini) |
| SpawnPointX | FLOAT | NOT NULL, DEFAULT 0 | Position spawn X |
| SpawnPointY | FLOAT | NOT NULL, DEFAULT 0 | Position spawn Y |
| GameVersion | VARCHAR(20) | NOT NULL | Version du jeu |
| CreatedAt | DATETIME | NOT NULL | Date de création |
| LastPlayed | DATETIME | NOT NULL | Dernier accès |

---

#### Items (Items)
Catalogue des items disponibles dans le jeu (données de référence).

| Colonne | Type | Contraintes | Description |
|---------|------|-------------|-------------|
| Id | INT | PK, AUTO_INCREMENT | Identifiant unique |
| Code | VARCHAR(50) | NOT NULL, UNIQUE | Code unique (ex: "wood_plank") |
| Name | VARCHAR(100) | NOT NULL | Nom affiché |
| Description | TEXT | NULL | Description |
| Type | TINYINT | NOT NULL | Type d'item (0-99, enum ItemType) |
| IsStackable | BOOLEAN | NOT NULL, DEFAULT true | Est empilable ? |
| MaxStackSize | INT | NOT NULL, DEFAULT 99 | Taille max de pile |
| Weight | FLOAT | NOT NULL, DEFAULT 1.0 | Poids unitaire |
| BaseValue | INT | NOT NULL, DEFAULT 0 | Valeur marchande |
| IconPath | VARCHAR(255) | NULL | Chemin de l'icône |
| MaxDurability | INT | NOT NULL, DEFAULT 0 | Durabilité max (0=indestructible) |
| Damage | FLOAT | NOT NULL, DEFAULT 0 | Dégâts (armes) |
| Defense | FLOAT | NOT NULL, DEFAULT 0 | Défense (armures) |
| HealthRestore | FLOAT | NOT NULL, DEFAULT 0 | Soin à la consommation |
| HungerRestore | FLOAT | NOT NULL, DEFAULT 0 | Réduit faim |
| ThirstRestore | FLOAT | NOT NULL, DEFAULT 0 | Réduit soif |

---

#### PlayerInventories (Inventaires)
Gère l'inventaire des joueurs.

| Colonne | Type | Contraintes | Description |
|---------|------|-------------|-------------|
| Id | GUID/UUID | PK | Identifiant unique |
| PlayerId | GUID/UUID | FK → Players | Joueur propriétaire |
| ItemId | INT | FK → Items | Item stocké |
| Quantity | INT | NOT NULL | Quantité |
| SlotIndex | INT | NOT NULL | Index du slot d'inventaire |

**Index** : Unique sur (PlayerId, SlotIndex)

---

#### Crafting Recipes (Recettes)
Définit les recettes de craft disponibles.

| Colonne | Type | Contraintes | Description |
|---------|------|-------------|-------------|
| Id | INT | PK, AUTO_INCREMENT | Identifiant unique |
| ResultItemId | INT | FK → Items | Item résultat |
| ResultQuantity | INT | NOT NULL | Quantité produite |
| CraftTime | FLOAT | NOT NULL | Temps de craft (secondes) |
| RequiredLevel | INT | NOT NULL, DEFAULT 1 | Niveau requis |

---

#### RecipeIngredients (Ingrédients de recette)
Ingrédients nécessaires pour chaque recette.

| Colonne | Type | Contraintes | Description |
|---------|------|-------------|-------------|
| Id | INT | PK, AUTO_INCREMENT | Identifiant unique |
| RecipeId | INT | FK → CraftingRecipes | Recette concernée |
| ItemId | INT | FK → Items | Item requis |
| Quantity | INT | NOT NULL | Quantité requise |

---

### Relations

```
Factions 1───N Clans (affiliés)
Factions 1───N Players (membres)
Clans 1───N Players (membres)
Clans 1───1 Players (leader)
Worlds 1───N Players
Players 1───N PlayerInventories
Items 1───N PlayerInventories
Items 1───N CraftingRecipes (result)
CraftingRecipes 1───N RecipeIngredients
Items 1───N RecipeIngredients
```

**Relations clés** :
- Un joueur a **une** ethnie (obligatoire)
- Un joueur peut avoir **au plus une** faction (selon son ethnie)
- Un joueur peut avoir **au plus un** clan (selon faction/ethnie)
- Une faction est exclusive à **une** ethnie
- Un clan peut être affilié à **une** faction ou être indépendant
- Chaque clan a **un** leader (joueur)

---

## Migrations

### À créer
- [ ] Migration initiale : Création des tables de base
- [ ] Migration : Ajout du système de construction
- [ ] Migration : Ajout du système de quêtes
- [ ] Migration : Ajout des NPCs

---

## Indexation (à optimiser)

### Index planifiés
- `Players.Name` (UNIQUE)
- `Players.WorldId` (FK index auto)
- `Worlds.Name` (UNIQUE)
- `Items.Code` (UNIQUE)
- `PlayerInventories.PlayerId, SlotIndex` (UNIQUE COMPOSITE)
- `PlayerInventories.ItemId` (FK index auto)

---

## Connection String

**Format** :
```
Server=localhost;Port=3306;Database=tinysurvivalworld;User=root;Password=***;
```

**Configuration** : À définir dans `appsettings.json` du projet Game.Desktop

---

## EF Core Configuration

### DbContext
**Fichier** : `TinySurvivalWorld.Data/GameDbContext.cs` (à créer)

### Migrations
**Commandes** :
```bash
# Créer une migration
dotnet ef migrations add InitialCreate --project src/TinySurvivalWorld.Data

# Appliquer les migrations
dotnet ef database update --project src/TinySurvivalWorld.Data

# Supprimer la dernière migration
dotnet ef migrations remove --project src/TinySurvivalWorld.Data
```

---

## Notes de conception

### Choix de GUID vs INT pour les IDs
- **Players, Worlds, PlayerInventories** : GUID
  - Raison : Permet la génération côté client, évite les conflits en multi-joueur futur
- **Items, Recipes** : INT
  - Raison : Données de référence, plus compact, performance en jointure

### Normalisation
Le schéma suit la 3ème forme normale (3NF) pour éviter la redondance.

### Performance
- Indexation sur les clés étrangères (automatique avec EF Core)
- Index composite sur PlayerInventories pour les recherches fréquentes
- Considérer le cache applicatif pour Items (rarement modifiés)

---

**Dernière mise à jour** : 2025-11-15
**Version** : 0.1.0-alpha
**Statut** : Planification
