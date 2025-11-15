# Architecture du projet Tiny Survival World

## Vue d'ensemble

Le projet utilise une **architecture multi-projet en couches** pour faciliter la maintenabilité, la testabilité et l'évolution future vers d'autres plateformes (mobile, tablette).

```
TinySurvivalWorld/
├── src/
│   ├── TinySurvivalWorld.Shared/        # Couche utilitaires
│   ├── TinySurvivalWorld.Core/          # Couche logique métier
│   ├── TinySurvivalWorld.Data/          # Couche accès données
│   └── TinySurvivalWorld.Game.Desktop/  # Couche présentation (Desktop)
├── docs/                                # Documentation technique
├── PROGRESS.md                          # Suivi global du projet
└── BRANCH-PROGRESS.md                   # Suivi détaillé de la branche
```

---

## Projets

### 1. TinySurvivalWorld.Shared
**Type** : Bibliothèque de classe (.NET 9.0)
**Responsabilité** : Fournir des utilitaires, constantes et extensions partagées entre tous les projets

**Contenu typique** :
- Constantes globales
- Extensions de types .NET
- Helpers et utilitaires génériques
- Structures de données communes

**Dépendances** : Aucune (projet autonome)

**Namespace** : `TinySurvivalWorld.Shared`

---

### 2. TinySurvivalWorld.Core
**Type** : Bibliothèque de classe (.NET 9.0)
**Responsabilité** : Contenir toute la logique métier du jeu, indépendante de l'UI et de la persistance

**Contenu typique** :
- Modèles de domaine (Player, World, Item, etc.)
- Systèmes de jeu (mouvement, combat, inventaire, etc.)
- Game logic et règles métier
- Interfaces pour les services externes

**Dépendances** :
- ✅ TinySurvivalWorld.Shared

**Namespace** : `TinySurvivalWorld.Core`

**Principes** :
- Aucune dépendance vers la UI ou la base de données
- Code testable unitairement
- Implémentation des règles du jeu pure

---

### 3. TinySurvivalWorld.Data
**Type** : Bibliothèque de classe (.NET 9.0)
**Responsabilité** : Gérer l'accès aux données et la persistance MySQL

**Contenu typique** :
- DbContext Entity Framework Core
- Configurations d'entités
- Migrations de base de données
- Repositories et unités de travail
- Mappers entre entités DB et modèles du domaine

**Dépendances** :
- ✅ TinySurvivalWorld.Core
- ✅ TinySurvivalWorld.Shared
- ✅ Microsoft.EntityFrameworkCore (9.0.0)
- ✅ Pomelo.EntityFrameworkCore.MySql (9.0.0)
- ✅ MySqlConnector (2.5.0)

**Namespace** : `TinySurvivalWorld.Data`

**Principes** :
- Pattern Repository pour isoler la logique de persistance
- Unit of Work pour gérer les transactions
- Migrations pour versionner le schéma

---

### 4. TinySurvivalWorld.Game.Desktop
**Type** : Application MonoGame (.NET 9.0)
**Responsabilité** : Point d'entrée du jeu, gestion du rendu et de l'input pour Desktop

**Contenu typique** :
- Classe principale Game1 (hérite de XnaGame)
- Gestionnaires de scènes/écrans
- Composants UI MonoGame
- Gestionnaire d'input (clavier, souris, manette)
- Systèmes de rendu 2D
- Pipeline de contenu (textures, sons, fonts)

**Dépendances** :
- ✅ TinySurvivalWorld.Core
- ✅ TinySurvivalWorld.Data
- ✅ TinySurvivalWorld.Shared
- ✅ MonoGame.Framework.DesktopGL (3.8.4.1)

**Namespace** : `TinySurvivalWorld.Game.Desktop`

**Principes** :
- Séparation rendu / logique métier
- Utilisation de Core pour toute la logique
- Utilisation de Data pour la persistance

**Note importante** : Le namespace `TinySurvivalWorld.Game.Desktop` entre en conflit avec la classe `Game` de MonoGame. Solution : utiliser un alias `using XnaGame = Microsoft.Xna.Framework.Game;`

---

## Graphe de dépendances

```
┌─────────────────────────────────┐
│  Game.Desktop (Présentation)    │
│  - Rendu MonoGame               │
│  - Input handling               │
│  - UI                           │
└────────┬────────────────────────┘
         │ dépend de
         ├──────────────┬──────────────┐
         ▼              ▼              ▼
    ┌─────────┐   ┌─────────┐   ┌──────────┐
    │  Core   │   │  Data   │   │  Shared  │
    │ (Logic) │   │  (DB)   │   │ (Utils)  │
    └────┬────┘   └────┬────┘   └──────────┘
         │ dépend de   │ dépend de
         ├─────────────┤
         ▼             ▼
    ┌──────────────────────────┐
    │       Shared             │
    │ (Aucune dépendance)      │
    └──────────────────────────┘
```

---

## Flux de données

### Sauvegarde d'une partie
```
Game.Desktop (UI)
    ↓ Appel méthode
Core (Logique de sauvegarde)
    ↓ Appel repository
Data (Persistance MySQL)
    ↓ EF Core
MySQL Database
```

### Chargement d'une partie
```
MySQL Database
    ↓ Query EF Core
Data (Récupération entités)
    ↓ Mapping vers modèles domaine
Core (Modèles chargés)
    ↓ État du jeu
Game.Desktop (Rendu)
```

---

## Patterns architecturaux utilisés

### 1. Layered Architecture (Architecture en couches)
Séparation claire des responsabilités en couches distinctes.

### 2. Repository Pattern
Isolation de la logique d'accès aux données dans des repositories (dans Data).

### 3. Dependency Injection
Utilisation d'interfaces pour inverser les dépendances et faciliter les tests.

### 4. Entity-Component-System (ECS) - À implémenter
Pour gérer les entités du jeu de manière performante et modulaire.

---

## Évolution future

### Ajout de plateformes mobiles
L'architecture actuelle permet facilement d'ajouter :
- `TinySurvivalWorld.Game.Android`
- `TinySurvivalWorld.Game.iOS`

Ces projets partageront Core, Data et Shared, seule la couche de présentation change.

### Ajout de fonctionnalités
- **Multijoueur** → Ajouter `TinySurvivalWorld.Network`
- **Modding** → Ajouter `TinySurvivalWorld.Modding`
- **Éditeur de niveau** → Ajouter `TinySurvivalWorld.Editor`

---

## Technologies et frameworks

| Projet | Framework | Version | Dépendances principales |
|--------|-----------|---------|-------------------------|
| Shared | .NET | 9.0 | - |
| Core | .NET | 9.0 | Shared |
| Data | .NET | 9.0 | EF Core 9.0, Pomelo MySQL 9.0 |
| Game.Desktop | .NET | 9.0 | MonoGame 3.8.4.1 |

---

**Dernière mise à jour** : 2025-11-15
**Version** : 0.1.0-alpha
