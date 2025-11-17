# Guide de Génération de Monde (Mode DevMode)

## Table des matières

1. [Introduction](#introduction)
2. [Activation du mode DevMode](#activation-du-mode-devmode)
3. [Interface de configuration](#interface-de-configuration)
4. [Paramètres de génération](#parametres-de-generation)
5. [Exemples de configurations](#exemples-de-configurations)
6. [Conseils et astuces](#conseils-et-astuces)
7. [Référence technique](#reference-technique)

---

## Introduction

Le système de génération procédurale de **Tiny Survival World** utilise **3 couches de bruit de Perlin/Simplex** pour créer des mondes variés et réalistes :

- **Elevation** : Détermine l'altitude (océans, plaines, collines, montagnes)
- **Moisture** : Détermine l'humidité (déserts, forêts, marécages)
- **Temperature** : Détermine la température (zones froides, tempérées, chaudes)

Le **mode DevMode** permet d'ajuster en temps réel **15 paramètres** pour contrôler finement la génération et créer des mondes personnalisés.

---

## Activation du mode DevMode

### Étape 1 : Configurer appsettings.json

Ouvrez le fichier `src/TinySurvivalWorld.Game.Desktop/appsettings.json` et activez DevMode :

```json
{
  "GameSettings": {
    "DevMode": true
  }
}
```

### Étape 2 : Lancer le jeu

```bash
cd src/TinySurvivalWorld.Game.Desktop
dotnet run
```

L'écran de configuration s'affichera automatiquement au démarrage.

---

## Interface de configuration

### Layout

L'écran de configuration est divisé en 2 parties :

```
┌─────────────────────┬─────────────────────┐
│                     │                     │
│   PREVIEW           │   CONTROLS          │
│   (Carte en temps   │   (15 paramètres)   │
│    réel)            │                     │
│                     │   - Elevation (5)   │
│   Zoom: +/-         │   - Moisture (5)    │
│                     │   - Temperature (5) │
│                     │                     │
│                     │   Seed: _______     │
│                     │   [ START GAME ]    │
│                     │                     │
│                     │   Controls          │
└─────────────────────┴─────────────────────┘
```

### Contrôles clavier

| Touche | Action |
|--------|--------|
| **↑ / ↓** | Naviguer entre les paramètres |
| **← / →** | Ajuster la valeur sélectionnée |
| **+/-** | Zoom sur la preview |
| **R** | Reset tous les paramètres par défaut |
| **S** | Générer un nouveau seed aléatoire |
| **Enter** | Démarrer le jeu avec la configuration |
| **ESC** | Quitter |

---

## Paramètres de génération

Chaque couche (Elevation, Moisture, Temperature) dispose de **5 paramètres** :

### 1. Octaves (1-8)

**Contrôle la complexité du bruit** - Nombre de niveaux de détail superposés.

- **Faible (1-2)** : Terrain simple, grandes zones homogènes
- **Normal (3-4)** : Bon équilibre détail/performance
- **Élevé (5-8)** : Terrain très détaillé, variations fréquentes

**Défaut** : Elevation=4, Moisture=3, Temperature=2

---

### 2. Persistence (0.1-1.0)

**Contrôle l'influence des octaves** - Amplitude relative de chaque niveau de détail.

- **Faible (0.1-0.3)** : Les détails fins sont subtils
- **Normal (0.4-0.6)** : Équilibre entre grandes et petites variations
- **Élevé (0.7-1.0)** : Les détails fins dominent

**Défaut** : 0.5 pour toutes les couches

---

### 3. Lacunarity (1.0-4.0)

**Contrôle la fréquence des octaves** - Écart de fréquence entre les niveaux de détail.

- **Faible (1.0-1.5)** : Transitions douces entre détails
- **Normal (1.8-2.2)** : Variations naturelles
- **Élevé (2.5-4.0)** : Détails très contrastés

**Défaut** : 2.0 pour toutes les couches

---

### 4. Scale (0.1-2.0)

**Compression/extension des valeurs** - Contrôle la distribution des valeurs autour du centre.

**Formule** : `transformedValue = offset + (rawValue - 0.5) * scale`

- **< 1.0** : Compresse l'échelle → Biomes plus homogènes
  - Exemple : `scale=0.5` compresse 0.15-0.9 vers 0.325-0.7

- **= 1.0** : Échelle normale (aucun changement)

- **> 1.0** : Étend l'échelle → Biomes plus contrastés
  - Exemple : `scale=1.5` étend 0.15-0.9 vers 0.025-1.1 (clamped à 0-1)

**Défaut** : 1.0 pour toutes les couches

**Cas d'usage** :
```
Scale = 0.5  → Monde homogène (moins de variation)
Scale = 1.0  → Monde équilibré (par défaut)
Scale = 1.5  → Monde extrême (plus de variation)
```

---

### 5. Offset (0.0-1.0)

**Centre de l'échelle** - Décale toutes les valeurs vers le haut ou le bas.

- **< 0.5** : Décale vers les valeurs basses
  - Elevation : Plus d'eau, moins de montagnes
  - Moisture : Zones plus sèches
  - Temperature : Zones plus froides

- **= 0.5** : Centré (par défaut)

- **> 0.5** : Décale vers les valeurs hautes
  - Elevation : Plus de montagnes, moins d'eau
  - Moisture : Zones plus humides
  - Temperature : Zones plus chaudes

**Défaut** : 0.5 pour toutes les couches

**Exemples** :
```
Elevation Offset = 0.3  → Monde aquatique (beaucoup d'océans)
Elevation Offset = 0.7  → Monde montagneux (peu d'océans)

Moisture Offset = 0.3   → Monde aride (déserts, steppes)
Moisture Offset = 0.7   → Monde humide (forêts, marécages)

Temperature Offset = 0.3 → Monde froid (toundra, neige)
Temperature Offset = 0.7 → Monde chaud (jungles, déserts)
```

---

## Exemples de configurations

### 🌊 Monde Aquatique (Archipel)

Pour créer un monde avec beaucoup d'îles et d'océans :

```
Elevation Octaves = 4
Elevation Persistence = 0.5
Elevation Lacunarity = 2.0
Elevation Scale = 1.0
Elevation Offset = 0.35   ← Décalage vers le bas = plus d'eau
```

**Résultat** : 70% d'océans, petites îles dispersées.

---

### 🏔️ Monde Montagneux

Pour créer un monde avec de grandes chaînes de montagnes :

```
Elevation Octaves = 6      ← Plus de détails
Elevation Persistence = 0.6
Elevation Lacunarity = 2.2
Elevation Scale = 1.2      ← Plus de contraste
Elevation Offset = 0.65    ← Décalage vers le haut = plus de montagnes
```

**Résultat** : Hauts plateaux, pics élevés, vallées profondes.

---

### 🌳 Monde Forestier

Pour créer un monde avec de vastes forêts :

```
Moisture Octaves = 4
Moisture Persistence = 0.55
Moisture Lacunarity = 2.0
Moisture Scale = 1.0
Moisture Offset = 0.65     ← Humidité élevée = forêts
```

**Résultat** : Grandes zones de forêts denses et clairsemées.

---

### 🏜️ Monde Désertique

Pour créer un monde aride avec peu de végétation :

```
Moisture Octaves = 3
Moisture Persistence = 0.4
Moisture Lacunarity = 2.0
Moisture Scale = 0.8       ← Moins de variation
Moisture Offset = 0.3      ← Faible humidité = déserts

Temperature Offset = 0.65  ← Température élevée
```

**Résultat** : Vastes étendues de sable, peu de zones humides.

---

### ❄️ Monde Glaciaire

Pour créer un monde froid avec beaucoup de neige :

```
Temperature Octaves = 2
Temperature Persistence = 0.5
Temperature Lacunarity = 2.0
Temperature Scale = 0.7    ← Moins de variation de température
Temperature Offset = 0.25  ← Température basse = zones froides

Elevation Offset = 0.55    ← Légèrement montagneux
```

**Résultat** : Pics enneigés, toundra, peu de zones chaudes.

---

### 🌍 Monde Équilibré (Varié)

Pour créer un monde avec un bon mélange de tous les biomes :

```
Tous les paramètres aux valeurs par défaut :

Octaves : Elevation=4, Moisture=3, Temperature=2
Persistence : 0.5
Lacunarity : 2.0
Scale : 1.0
Offset : 0.5
```

**Résultat** : Distribution équilibrée de tous les types de terrains.

---

### 🏝️ Monde Homogène (Flat)

Pour créer un monde avec peu de variations :

```
Elevation Octaves = 2      ← Peu de détails
Elevation Scale = 0.5      ← Valeurs compressées
Moisture Scale = 0.5
Temperature Scale = 0.5
```

**Résultat** : Terrain plat, peu de variations extrêmes.

---

## Conseils et astuces

### 🎯 Stratégie d'ajustement

1. **Commencez par l'Elevation** : C'est le paramètre le plus visible
   - Ajustez **Offset** pour contrôler la proportion océan/terre
   - Ajustez **Scale** pour contrôler le relief

2. **Ajustez la Moisture** : Contrôle les biomes (déserts, forêts, marécages)
   - Offset élevé → Plus de forêts et marécages
   - Offset faible → Plus de déserts et steppes

3. **Ajustez la Temperature** : Affine les biomes (toundra, zones tempérées, tropicales)
   - Impact moins visible mais important pour la variété

4. **Utilisez la touche S** : Testez vos paramètres sur différents seeds

---

### ⚡ Performances

- **Octaves élevés (6-8)** : Plus de détails mais génération plus lente
- **Octaves faibles (1-3)** : Génération rapide mais moins de détails
- **Recommandation** : 3-4 octaves pour un bon équilibre

---

### 🎲 Seed personnalisé

- **Seed vide** : Génère un seed aléatoire basé sur l'horloge système
- **Seed numérique** : Permet de reproduire exactement le même monde
- **Astuce** : Notez le seed des mondes intéressants pour les rejouer

---

### 📊 Comprendre les valeurs

Les valeurs générées sont **normalisées entre 0.0 et 1.0** :

#### Elevation
```
0.00 - 0.15  → Eau profonde (DeepWater)
0.15 - 0.25  → Eau peu profonde (ShallowWater)
0.25 - 0.40  → Sable (Sand)
0.40 - 0.60  → Herbe/Terre (Grass/Dirt)
0.60 - 0.75  → Collines (Hill)
0.75 - 0.85  → Montagnes (Mountain)
0.85 - 1.00  → Pics enneigés (SnowPeak)
```

#### Moisture
```
0.00 - 0.30  → Zones sèches (Sand, Dirt)
0.30 - 0.60  → Zones normales (Grass)
0.60 - 0.80  → Zones humides (Forest)
0.80 - 1.00  → Zones très humides (Swamp)
```

#### Temperature
```
0.00 - 0.30  → Froid (SnowPeak, Tundra)
0.30 - 0.70  → Tempéré (Grass, Forest)
0.70 - 1.00  → Chaud (Desert, Jungle)
```

---

## Référence technique

### Formules de transformation

```csharp
// Génération du bruit brut (valeur entre 0 et 1)
rawValue = SimplexNoise.GenerateNormalized(x, y, octaves, persistence, lacunarity)

// Application de la transformation Scale/Offset
transformedValue = offset + (rawValue - 0.5) * scale

// Clamping pour garantir [0, 1]
finalValue = Math.Clamp(transformedValue, 0.0, 1.0)
```

### Plages de valeurs

| Paramètre | Min | Max | Défaut | Step | Type |
|-----------|-----|-----|--------|------|------|
| Octaves | 1 | 8 | 2-4 | 1 | int |
| Persistence | 0.1 | 1.0 | 0.5 | 0.05 | float |
| Lacunarity | 1.0 | 4.0 | 2.0 | 0.1 | float |
| Scale | 0.1 | 2.0 | 1.0 | 0.05 | float |
| Offset | 0.0 | 1.0 | 0.5 | 0.05 | float |

### 13 Types de terrains

| Type | Traversable | MovementCost | Description |
|------|-------------|--------------|-------------|
| DeepWater | ❌ | ∞ | Eau profonde (inaccessible) |
| ShallowWater | ✅ | 1.5 | Eau peu profonde (ralentit) |
| Sand | ✅ | 1.2 | Sable (plages, déserts) |
| Grass | ✅ | 1.0 | Herbe (terrain normal) |
| Dirt | ✅ | 1.0 | Terre (terrain normal) |
| Forest | ✅ | 1.3 | Forêt dense (ralentit légèrement) |
| SparseForest | ✅ | 1.1 | Forêt clairsemée |
| Hill | ✅ | 1.4 | Colline (ralentit) |
| Mountain | ✅ | 2.5 | Montagne (ralentit beaucoup) |
| SnowPeak | ❌ | ∞ | Pic enneigé (inaccessible) |
| Swamp | ✅ | 2.0 | Marécage (ralentit beaucoup) |
| Ruins | ✅ | 1.2 | Ruines (terrain normal) |
| Toxic | ✅ | 1.5 | Zone toxique (ralentit + danger) |

---

## Fichiers source

- **Configuration** : `src/TinySurvivalWorld.Core/World/WorldGenerationConfig.cs`
- **Générateur** : `src/TinySurvivalWorld.Core/World/WorldGenerator.cs`
- **Interface** : `src/TinySurvivalWorld.Game.Desktop/Screens/ConfigurationScreen.cs`
- **Terrains** : `src/TinySurvivalWorld.Core/World/TerrainDefinitions.cs`

---

## Support

Pour toute question ou problème :
- Consultez le fichier `BRANCH-PROGRESS.md` pour l'état actuel du développement
- Consultez `docs/GAME-SYSTEMS.md` pour plus de détails sur les systèmes de jeu

---

**Version** : Phase 3 - Terrains et Collisions
**Dernière mise à jour** : 2025-11-16
