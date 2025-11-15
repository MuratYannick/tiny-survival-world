# Avancement Détaillé - Phase 2 : Fondations du jeu

**Branche** : `feature/phase2-foundations`
**Dernière mise à jour** : 2025-11-15
**Statut** : En cours - Branche créée

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

#### Tâches à réaliser

### Priorité Haute
- [ ] Créer les modèles de domaine dans `Core/`
  - [ ] Player (joueur)
  - [ ] World (monde)
  - [ ] Item (items de base)
  - [ ] Position (structure de position)
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

## Décisions techniques à prendre

### Questions en suspens
1. **Taille du monde** : Monde infini ou délimité ?
2. **Taille des chunks** : 16x16 ou 32x32 tiles ?
3. **Taille des tiles** : 32x32 ou 64x64 pixels ?
4. **Format de sauvegarde** : Base de données uniquement ou fichiers JSON + DB ?
5. **Assets graphiques** : Placeholder ou création initiale ?

---

## Notes de développement

### Problèmes rencontrés et solutions
_À documenter au fur et à mesure du développement_

---

## Statistiques

**Fichiers créés** : 0
**Lignes de code ajoutées** : 0
**Commits** : 0
**Tests** : 0

---

## Références utiles

- [MonoGame 2D Camera Tutorial](https://www.monogame.net/documentation/?page=Tutorials)
- [EF Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)
- [Perlin Noise for Terrain Generation](https://en.wikipedia.org/wiki/Perlin_noise)

---

**Note** : Ce fichier doit être mis à jour régulièrement pendant le développement de cette phase.
