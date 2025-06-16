# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a BDD (Behavior-Driven Development) Unity action game project developed using specifications-first approach where all Unity C# scripts are generated from markdown specifications. The game features a cube-shaped player character that rotates during movement and utilizes a skill-based combat system.

## Architecture

### Specification-Driven Development Structure
- **spec/**: Contains all specifications and requirements
  - **code/**: Individual class specifications (becomes Unity C# scripts in `Assets/Scripts/`)
  - **gamedesign/**: Game design documents and mechanics
  - **rule/**: Coding standards and architectural rules
  - **ubi/**: Ubiquitous language dictionary (now in `.md` format)
  - **usecase/**: Behavior specifications using Gherkin syntax

- **unity/**: Unity project containing generated code
  - Scripts follow exact hierarchy from `spec/code/` in `Assets/Scripts/`
  - Uses Unity 6 with URP (Universal Render Pipeline)

### Core Game Architecture
- **GameManager**: Central game progression and state management with Result display
- **Player**: Player input handling and coordination with skill system
- **EnemyBase**: Abstract base class for all enemy types implementing IHitTarget
- **SkillBase**: Abstract base class for all skills with cooldown and action interval management
- **StageCreator**: Stage generation system (specification pending)
- **Result**: Game result display system

### Skill System Architecture
- **SkillBase**: Provides cooldown management, execution checks, and action intervals
- **ShootBullet/ThreeLineShoot**: Concrete skill implementations
- **Bullet**: Projectile system with collision detection
- Skills have both action intervals (between actions) and cooldowns (skill reuse)

## Development Commands

### Unity Project
```bash
# Navigate to Unity project
cd unity/

# Unity solution file
# RunGame.sln (main solution)

# Build via Unity Editor or CLI builds
```

### Code Generation Rules
- All specifications in `/spec/code/` must be implemented as Unity C# scripts
- Place scripts in `/unity/Assets/Scripts/` maintaining exact hierarchy from spec/code/
- Class names and file names match the markdown specification names exactly
- Empty specification files indicate pending implementation

## Coding Standards

### C# Conventions
- Private variables prefixed with underscore: `_variableName`
- No public variables - use properties/accessors instead
- Function names in PascalCase
- Maintain loose coupling between classes
- File and class separation encouraged

### Unity-Specific Rules
- Use Unity 6 native implementations only - no external libraries except for interface inspector specification support
- MonoBehaviour inheritance for game objects
- SerializeField for private variables exposed to Inspector
- Abstract classes for extensible base systems (EnemyBase, SkillBase)

### Interface Design
- IHitTarget interface for damageable entities
- Abstract base classes for extensible systems
- Proper abstraction layers between systems

## Ubiquitous Language
- **インプット**: User input
- **スキル**: Character abilities with various effects
- **クールタイム**: Cooldown period after skill use
- **アイドル状態**: Idle state (no actions, no cooldowns)
- **ポーズ状態**: Game paused state

## Key Implementation Notes
- Player is cube-shaped and rotates during movement
- Skill system uses both action intervals and cooldown timers
- Enemy system uses abstract base class with IHitTarget implementation
- Result system integrated with GameManager for winner display
- All systems designed for loose coupling and testability
- Empty specification files indicate systems awaiting implementation