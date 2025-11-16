# Avancement Détaillé - Phase 3 : Terrains et Collisions

**Branche** : `feature/phase3-terrains-et-collisions`
**Date de début** : 2025-11-16
**Statut** : 🚀 **EN COURS**

---

## Objectifs de la Phase 3

Cette phase vise à **améliorer le système de monde** en redéfinissant les types de terrains et en validant le système de collision :

- Redéfinition des modèles pour les différents types de terrains avec propriétés détaillées
- Validation et amélioration du système de collision
- Propriétés avancées des tiles (traversabilité, ressources, dangers, etc.)
- Gestion des transitions entre types de terrains

**Prérequis** : Phase 2 terminée ✅

---

## Session en cours

### Date : 2025-11-16

#### Objectif de la session
Démarrer la phase 3 en analysant le système actuel et en planifiant les améliorations.

#### Tâches complétées ✅

1. **Remplacement terrain Radioactive → Toxic**
   - ✅ TileType enum mis à jour (Radioactive → Toxic)
   - ✅ Description adaptée : "Zone toxique/contaminée (déchets toxiques, pollution chimique)"
   - ✅ TileColors mis à jour : RGB(150, 180, 40) - vert-jaune toxique
   - ✅ Tile.cs mis à jour (MovementCost)

2. **Fenêtre de légende des terrains**
   - ✅ LegendRenderer créé (affichage des 13 types de terrains)
   - ✅ Noms localisés en français
   - ✅ Design professionnel (fond semi-transparent, bordure)
   - ✅ Toggle avec F4
   - ✅ Game1 intégré avec dispose() et partage de font

3. **Système de propriétés de terrains avec probabilités**
   - ✅ TerrainProperties créé (3 probabilités: mobs, ressources, items)
   - ✅ TerrainDefinitions créé avec valeurs pour les 13 terrains
   - ✅ Tile.cs exposé les propriétés via property Properties
   - ✅ LegendRenderer mis à jour pour afficher les probabilités (format "M:XX% R:XX% I:XX%")
   - ✅ Valeurs définies par terrain :
     - Eau Profonde : M:10% R:0% I:0%
     - Eau Peu Profonde : M:20% R:10% I:5%
     - Sable : M:20% R:10% I:10%
     - Herbe : M:40% R:30% I:20%
     - Terre : M:30% R:20% I:10%
     - Forêt Dense : M:60% R:80% I:30%
     - Forêt Clairsemée : M:40% R:50% I:20%
     - Colline : M:30% R:40% I:10%
     - Montagne : M:20% R:70% I:20%
     - Pic Enneigé : M:10% R:20% I:5%
     - Marécage : M:70% R:40% I:30%
     - Ruines : M:80% R:20% I:70%
     - Zone Toxique : M:70% R:30% I:50%

#### Tâches à réaliser

### Priorité Haute
- [ ] **Analyse du système actuel**
  - [ ] Analyser les 13 types de tiles existants
  - [ ] Identifier les problèmes de collision actuels
  - [ ] Lister les propriétés manquantes par type de terrain

- [ ] **Redéfinition des types de terrains**
  - [ ] Créer un modèle de données enrichi pour TileType
  - [ ] Définir les propriétés de traversabilité par terrain
  - [ ] Définir les ressources disponibles par terrain
  - [ ] Définir les dangers/effets par terrain
  - [ ] Définir le coût de mouvement par terrain

- [ ] **Amélioration du système de collision**
  - [ ] Corriger les bugs de collision identifiés
  - [ ] Implémenter la collision par type de terrain
  - [ ] Ajouter la collision conditionnelle (équipement, compétences, etc.)
  - [ ] Gérer les transitions entre terrains

- [ ] **Tests et validation**
  - [ ] Tester tous les types de terrains
  - [ ] Valider les collisions dans tous les cas
  - [ ] Vérifier les performances

### Priorité Moyenne
- [ ] Création d'un système de propriétés de tiles
- [ ] Documentation des types de terrains
- [ ] Ajout de tests unitaires pour les collisions

### Priorité Basse
- [ ] Optimisations supplémentaires
- [ ] Ajout de logs pour debug des collisions

---

## État actuel du code

**Build** : ✅ Réussi (0 erreurs, 0 warnings)

**Projets** :
- `TinySurvivalWorld.Core` : 15 fichiers (5 enums, 5 modèles, 6 classes World)
- `TinySurvivalWorld.Data` : 7 fichiers (DbContext, Factory, 5 configurations)
- `TinySurvivalWorld.Shared` : 1 structure (Position)
- `TinySurvivalWorld.Game.Desktop` : 11 fichiers (Game1, 4 renderers, 1 caméra, 1 entité, 2 content, appsettings)

**Système de terrains actuel** :
- 13 types de tiles : DeepWater, ShallowWater, Sand, Grass, Dirt, Forest, SparseForest, Hill, Mountain, SnowPeak, Swamp, Ruins, Radioactive
- Propriété IsWalkable basique (binaire)
- Collision par vérification des 4 coins de la collision box
- Génération procédurale basée sur 3 couches de bruit (élévation, humidité, température)

---

## Décisions techniques à prendre

1. **Structure des propriétés de terrains** :
   - Classe dédiée `TerrainProperties` ou properties dans `TileType` ?
   - Stocker en base de données ou hardcodé dans le code ?

2. **Système de collision avancé** :
   - Collision conditionnelle par équipement/compétences ?
   - Gestion des dégâts environnementaux (radioactivité, marécages) ?

3. **Ressources par terrain** :
   - Intégration avec le système d'items existant ?
   - Taux d'apparition et régénération des ressources ?

4. **Performance** :
   - Cache des propriétés de terrains ?
   - Optimisation des calculs de collision ?

---

## Fichiers à modifier/créer

### À créer
- `Core/World/TerrainProperties.cs` (possible) : Propriétés détaillées par terrain
- `Core/World/TerrainDefinitions.cs` (possible) : Définitions des 13 terrains
- `Core/Enums/TerrainResource.cs` (possible) : Types de ressources extractibles

### À modifier
- `Core/World/Tile.cs` : Ajout de propriétés avancées
- `Core/World/WorldGenerator.cs` : Ajustements génération si nécessaire
- `Game.Desktop/Entities/PlayerCharacter.cs` : Amélioration collision

---

## Problèmes identifiés (Phase 2)

1. **Collision pas entièrement fonctionnelle**
   - Personnage peut parfois passer à travers certains terrains
   - Besoin de validation approfondie

2. **Propriétés de terrains limitées**
   - IsWalkable trop simpliste (binaire)
   - Pas de coût de mouvement différencié
   - Pas de ressources associées
   - Pas d'effets environnementaux

3. **Transitions abruptes**
   - Pas de gestion des bordures entre terrains
   - Pas de ralentissement progressif

---

## Statistiques

**Fichiers créés** : 0 (Phase 3 vient de démarrer)
**Lignes de code ajoutées** : 0
**Commits** : 0
**Tests** : 0
**Build** : ✅ Réussie (hérité de Phase 2)

---

## Références utiles

- [Phase 2 - Documentation](docs/progression/fondation-du-jeu.md)
- [Tile-based collision detection](https://developer.mozilla.org/en-US/docs/Games/Techniques/Tilemaps)
- [Procedural terrain generation](https://www.redblobgames.com/maps/terrain-from-noise/)

---

**Note** : Ce fichier doit être mis à jour régulièrement pendant le développement de cette phase.
