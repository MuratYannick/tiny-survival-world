# Schéma de base de données - Tiny Survival World

## Vue d'ensemble

Base de données : **MySQL**
Provider : **Pomelo.EntityFrameworkCore.MySql 9.0.0**
ORM : **Entity Framework Core 9.0.0**

---

## État actuel

**Statut** : Non implémenté
**Migrations** : Aucune

---

## Schéma planifié

### Tables principales

#### Players (Joueurs)
Stocke les informations des joueurs et leur progression.

| Colonne | Type | Contraintes | Description |
|---------|------|-------------|-------------|
| Id | GUID/UUID | PK | Identifiant unique |
| Name | VARCHAR(50) | NOT NULL, UNIQUE | Nom du joueur |
| Level | INT | NOT NULL, DEFAULT 1 | Niveau du joueur |
| Experience | INT | NOT NULL, DEFAULT 0 | Points d'expérience |
| Health | FLOAT | NOT NULL | Santé actuelle |
| MaxHealth | FLOAT | NOT NULL | Santé maximale |
| Hunger | FLOAT | NOT NULL | Niveau de faim (0-100) |
| Thirst | FLOAT | NOT NULL | Niveau de soif (0-100) |
| PositionX | FLOAT | NOT NULL | Position X dans le monde |
| PositionY | FLOAT | NOT NULL | Position Y dans le monde |
| WorldId | GUID/UUID | FK → Worlds | Monde actuel |
| CreatedAt | DATETIME | NOT NULL | Date de création |
| UpdatedAt | DATETIME | NOT NULL | Dernière mise à jour |

---

#### Worlds (Mondes)
Représente les différents mondes/sauvegardes de jeu.

| Colonne | Type | Contraintes | Description |
|---------|------|-------------|-------------|
| Id | GUID/UUID | PK | Identifiant unique |
| Name | VARCHAR(100) | NOT NULL, UNIQUE | Nom du monde |
| Seed | BIGINT | NOT NULL | Seed de génération |
| GameTime | BIGINT | NOT NULL | Temps de jeu écoulé (ticks) |
| Difficulty | TINYINT | NOT NULL | Difficulté (0=Facile, 1=Normal, 2=Difficile) |
| CreatedAt | DATETIME | NOT NULL | Date de création |
| LastPlayed | DATETIME | NOT NULL | Dernier accès |

---

#### Items (Items)
Catalogue des items disponibles dans le jeu.

| Colonne | Type | Contraintes | Description |
|---------|------|-------------|-------------|
| Id | INT | PK, AUTO_INCREMENT | Identifiant unique |
| Code | VARCHAR(50) | NOT NULL, UNIQUE | Code unique de l'item |
| Name | VARCHAR(100) | NOT NULL | Nom de l'item |
| Description | TEXT | NULL | Description |
| Type | VARCHAR(50) | NOT NULL | Type d'item (Resource, Tool, Weapon, Food, etc.) |
| Stackable | BOOLEAN | NOT NULL, DEFAULT true | Est empilable ? |
| MaxStackSize | INT | NOT NULL, DEFAULT 99 | Taille max de pile |
| IconPath | VARCHAR(255) | NULL | Chemin de l'icône |

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
Worlds 1───N Players
Players 1───N PlayerInventories
Items 1───N PlayerInventories
Items 1───N CraftingRecipes (result)
CraftingRecipes 1───N RecipeIngredients
Items 1───N RecipeIngredients
```

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
