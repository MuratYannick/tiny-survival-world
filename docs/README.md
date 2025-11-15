# Documentation Technique - Tiny Survival World

Bienvenue dans la documentation technique du projet Tiny Survival World.

## Table des matières

### Architecture et Structure
- [Architecture du projet](ARCHITECTURE.md) - Structure multi-projet, dépendances et organisation du code
- [Schéma de base de données](DATABASE.md) - Modèle de données et structure MySQL
- [Systèmes de jeu](GAME-SYSTEMS.md) - Documentation des différents systèmes de gameplay

### Guides de développement
- [Guide de démarrage](GETTING-STARTED.md) - Configuration de l'environnement et premiers pas
- [Conventions de code](CODE-CONVENTIONS.md) - Standards et bonnes pratiques

### Référence API
- [Core API](api/CORE-API.md) - Documentation de TinySurvivalWorld.Core
- [Data API](api/DATA-API.md) - Documentation de TinySurvivalWorld.Data
- [Game API](api/GAME-API.md) - Documentation de TinySurvivalWorld.Game.Desktop

---

## À propos du projet

**Tiny Survival World** est un jeu de survie/gestion/RPG en 2D dans un univers post-apocalyptique.

### Technologies utilisées
- **Langage** : C# (.NET 9.0)
- **Moteur graphique** : MonoGame 3.8.4.1
- **Base de données** : MySQL (via Entity Framework Core 9.0)
- **Plateformes cibles** : Desktop, Mobile, Tablette

### Vision du jeu
Un jeu combinant trois genres :
- **Survie** : Gestion de la faim, soif, santé dans un monde hostile
- **Gestion** : Construction de base, gestion de ressources
- **RPG** : Progression du personnage, quêtes, NPCs

Vue de dessus 2D, univers post-apocalyptique.

---

## Contribution

Cette documentation doit être maintenue à jour par tous les développeurs contribuant au projet.

### Quand mettre à jour la documentation ?

1. **Ajout de nouvelles fonctionnalités** → Documenter dans GAME-SYSTEMS.md
2. **Modification de l'architecture** → Mettre à jour ARCHITECTURE.md
3. **Changements de base de données** → Mettre à jour DATABASE.md
4. **Nouvelles conventions** → Ajouter dans CODE-CONVENTIONS.md

---

**Dernière mise à jour** : 2025-11-15
**Version du projet** : 0.1.0-alpha
