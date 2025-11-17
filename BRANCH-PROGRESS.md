# Avancement Détaillé - Phase 4 : Temps et Survie

**Branche** : `feature/phase4-temps-et-survie`
**Date de début** : 2025-11-17
**Statut** : 🚀 **EN COURS**

---

## Objectifs de la Phase 4

Cette phase vise à **implémenter les systèmes de temps et de survie** pour donner vie au personnage et créer une vraie boucle de gameplay survival :

### 1. Gestion du temps dans le jeu
- Système de cycle jour/nuit
- Horloge interne du jeu (heures, jours, saisons)
- Vitesse du temps ajustable
- Événements temporels (lever/coucher de soleil, etc.)

### 2. Systèmes de survie du personnage
- **Santé (HP)** : Points de vie, régénération, dégâts
- **Faim** : Besoin de nourriture, effets de la faim
- **Soif** : Besoin d'eau, déshydratation
- **Énergie/Stamina** : Fatigue, besoin de repos
- **Stress** : Niveau de stress, facteurs de stress

### 3. États du personnage
- **États positifs** : Rassasié, Reposé, Hydraté, Régénération
- **États négatifs** : Affamé, Assoiffé, Épuisé, Empoisonné, Malade, Saignement
- **États neutres** : Normal, En course, En combat
- Système de buffs/debuffs avec durée
- Effets visuels pour les états

**Prérequis** : Phase 3 terminée ✅

---

## Session en cours

### Date : 2025-11-17

#### Objectif de la session
Démarrer la phase 4 en concevant l'architecture des systèmes de temps et de survie.

#### Tâches complétées ✅

1. **Système de temps continu implémenté**
   - ✅ TimeOfDay enum créé (6 périodes: Night, Dawn, Morning, Afternoon, Dusk, Evening)
   - ✅ TimeManager créé avec horloge continue basée sur DateTime.UtcNow
   - ✅ Ratio temps : 1 jour IG (24h) = 20h IRL
   - ✅ Date d'initialisation : 01/01/2025 00:00 UTC IRL → 01/01/2125 00:00 UTC IG
   - ✅ Temps s'écoule même hors ligne (persistant)
   - ✅ Événements : OnHourChanged, OnDayChanged, OnTimeOfDayChanged
   - ✅ Propriétés exposées : CurrentGameTime, CurrentDay, CurrentHour, CurrentTimeOfDay, IsDay, IsNight, IsTwilight
   - ✅ Méthodes utilitaires : GetFormattedTime(), GetFormattedDateTime(), GetTimeOfDayName()
   - ✅ Conversions temps IG ↔ temps IRL
   - ✅ TimeManager intégré dans Game1 (Initialize + Update)
   - ✅ Affichage debug overlay (Time + Period)
   - ✅ Build réussi : 0 erreurs, 0 avertissements

---

## Architecture technique envisagée

### Systèmes principaux

1. **TimeManager** (singleton)
   - CurrentTime (TimeSpan)
   - CurrentDay (int)
   - TimeOfDay (enum: Dawn, Day, Dusk, Night)
   - TimeScale (float, vitesse du temps)
   - Events: OnHourChanged, OnDayChanged, OnTimeOfDayChanged

2. **SurvivalManager** (component sur Player)
   - HealthComponent
   - HungerComponent
   - ThirstComponent
   - StaminaComponent
   - StressComponent

3. **CharacterStateManager** (component sur Player)
   - ActiveStates (List<CharacterState>)
   - ApplyState(CharacterState)
   - RemoveState(CharacterState)
   - Update() pour gérer durées

4. **CharacterState** (base class)
   - StateType (enum)
   - Duration (float, -1 = permanent)
   - Effects (dictionnaire de modificateurs)

### Intégrations avec systèmes existants

- **TerrainProperties** : Utilisation IsToxic pour état Empoisonné
- **TerrainProperties** : Utilisation IsDifficultTerrain pour Stamina drain
- **PlayerCharacter** : Intégration SurvivalManager
- **Game1** : Intégration TimeManager (Update)

---

## États du personnage planifiés

### États positifs ✅
- **Rassasié** (Well Fed) : Faim > 80%, +10% régénération HP
- **Hydraté** (Well Hydrated) : Soif > 80%, +10% régénération Stamina
- **Reposé** (Well Rested) : Énergie > 90%, +20% régénération Stamina
- **Régénération** (Regenerating) : HP régénère activement

### États négatifs ⚠️
- **Affamé** (Starving) : Faim < 20%, -50% régénération HP, -1 HP/sec si 0%
- **Assoiffé** (Dehydrated) : Soif < 20%, -50% Stamina max, -1 HP/sec si 0%
- **Épuisé** (Exhausted) : Énergie < 10%, vitesse réduite 50%, pas de course
- **Fatigué** (Tired) : Énergie < 30%, -20% vitesse course
- **Empoisonné** (Poisoned) : -2 HP/sec, durée variable
- **Saignement** (Bleeding) : -1 HP/sec jusqu'à soin
- **Malade** (Sick) : -50% régénération, -30% Stamina max
- **Stressé** (Stressed) : Stress > 70%, -10% précision (futur combat)

### États neutres/situationnels
- **Normal** : État par défaut, aucun effet
- **En course** (Sprinting) : Consommation Stamina accrue
- **En combat** (In Combat) : Stress augmente, régénération réduite

---

## État actuel du code (hérité Phase 3)

**Build** : ✅ Réussi (0 erreurs, 0 warnings)

**Fonctionnalités héritées** :
- ✅ Système de génération procédurale complet
- ✅ 13 types de terrains avec 8 propriétés chacun
- ✅ Propriétés IsToxic et IsDifficultTerrain (prêtes pour Phase 4)
- ✅ Système de collision validé
- ✅ Blending visuel entre terrains
- ✅ Mode DevMode avec configuration avancée
- ✅ Système de logging

**À intégrer en Phase 4** :
- Utiliser `tile.IsToxic` pour état Empoisonné
- Utiliser `tile.IsDifficultTerrain` pour drain Stamina
- Utiliser temps (jour/nuit) pour stress et fatigue

---

## Statistiques (Phase 4)

**Fichiers créés** : 2
- Core/Time/TimeOfDay.cs
- Core/Time/TimeManager.cs

**Fichiers modifiés** : 1
- Game.Desktop/Game1.cs

**Lignes de code ajoutées** : ~280
**Commits** : 1
**Tests** : Temps s'écoule correctement, événements fonctionnels
**Build** : ✅ 0 erreurs, 0 avertissements

---

**Note** : Ce fichier doit être mis à jour régulièrement pendant le développement de cette phase.
