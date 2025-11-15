# Instructions Claude Code - Tiny Survival World

## À propos du projet

**Tiny Survival World** est un mini-jeu combinant survie, gestion et RPG dans un univers post-apocalyptique.

### Technologies
- **Langage** : C#
- **Framework graphique** : MonoGame (2D, vue de dessus)
- **Base de données** : MySQL (en local)
- **Plateformes cibles** : Mobile, Tablette, Ordinateur

### Architecture
Le projet utilise une architecture multi-projet pour faciliter l'évolution :
- `TinySurvivalWorld.Core` : Logique métier et modèles
- `TinySurvivalWorld.Data` : Accès aux données MySQL via EF Core
- `TinySurvivalWorld.Shared` : Utilitaires et constantes partagées
- `TinySurvivalWorld.Game.Desktop` : Application MonoGame Desktop

### Dépendances
- Core → Shared
- Data → Core + Shared
- Game.Desktop → Core + Data + Shared

## Instructions pour Claude Code

### Documentation
1. **TOUJOURS** mettre à jour la documentation technique dans le répertoire `docs/` lors de l'ajout de nouvelles fonctionnalités
2. La documentation doit être structurée et claire, destinée aux développeurs
3. Créer/mettre à jour les fichiers appropriés dans `docs/` selon les changements :
   - Architecture système
   - Modèles de données
   - Systèmes de jeu
   - API et interfaces
   - Guides de développement

### Suivi de progression
1. **PROGRESS.md** : Mettre à jour après chaque fonctionnalité majeure complétée
   - Maintenir une vision globale de l'avancement du projet
   - Marquer les jalons atteints

2. **BRANCH-PROGRESS.md** : Mettre à jour régulièrement pendant le développement
   - Documenter les changements détaillés de la branche en cours
   - Lister les tâches en cours et à venir
   - Noter les décisions techniques prises

### Conventions de code
- Utiliser les conventions C# standard (PascalCase pour classes/méthodes, camelCase pour variables privées)
- Commenter le code complexe ou les algorithmes importants
- Préférer la clarté à la concision excessive
- Suivre les principes SOLID pour l'architecture

### Workflow Git
- Branche principale : `main`
- Remote principal : `origin` (GitHub)
- Créer des branches de fonctionnalité pour les nouvelles features
- Commits clairs et descriptifs en français

### Rappels importants
- Le jeu est en 2D vue de dessus
- L'univers est post-apocalyptique (survie + gestion + RPG)
- La base de données MySQL est locale
- L'architecture doit rester modulaire pour faciliter l'ajout de plateformes (mobile/tablette)
