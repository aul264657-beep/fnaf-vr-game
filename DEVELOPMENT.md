# Development Guide - FNAF VR Game

## Overview
This document outlines the development roadmap, architecture, and guidelines for the FNAF VR project.

## Architecture

### Project Structure
```
Assets/
├── Scripts/
│   ├── Core/              # Game logic and managers
│   ├── VR/                # VR-specific implementations
│   ├── Animatronics/      # AI systems
│   ├── UI/                # User interface
│   ├── Audio/             # Sound management
│   ├── Utilities/         # Helper functions
│   └── Tests/             # Unit tests
├── Scenes/                # Game scenes
├── Prefabs/               # Reusable components
├── Models/                # 3D models and assets
├── Materials/             # Textures and shaders
└── Audio/                 # Sound effects and music
```

## Core Systems

### 1. Game Manager (`Core/GameManager.cs`)
- Manages game state and night progression
- Handles power system
- Triggers game events
- Controls game flow

**Key Methods:**
- `StartNight()` - Initialize a night
- `EndNight(survived)` - End current night
- `TriggerPowerOut()` - Cut power
- `RestorePower(amount)` - Restore power

### 2. VR Input Handler (`VR/VRInputHandler.cs`)
- Manages controller input
- Handles hand tracking
- Processes button presses and gestures

**Key Methods:**
- `UpdateLeftController()` - Process left hand input
- `UpdateRightController()` - Process right hand input
- `GetHandAnchor(hand)` - Get hand position

### 3. Animatronic AI (`Animatronics/AnimatronicAI.cs`)
- Base class for all animatronics
- Handles AI state management
- Implements behavior patterns
- Manages player detection

**States:**
- `Idle` - Passive state
- `Moving` - Wandering behavior
- `Hunting` - Actively pursuing player
- `Aggressive` - Attacking player

## Development Phases

### Phase 1: Foundation (Current)
- [x] Project setup
- [x] Core game manager
- [x] VR input system
- [x] Base AI framework
- [ ] Scene setup
- [ ] Player controller

### Phase 2: Core Gameplay
- [ ] Security camera system
- [ ] Door/vent mechanics
- [ ] Power management UI
- [ ] Animatronic implementations (Freddy, Bonnie, Chica, Foxy)
- [ ] Audio cues system

### Phase 3: Polish & Features
- [ ] Jump-scare events
- [ ] Score system
- [ ] Difficulty levels
- [ ] Settings menu
- [ ] Performance optimization

### Phase 4: Extended Content
- [ ] Additional nights
- [ ] Custom difficulties
- [ ] Leaderboard
- [ ] Modding support

## Coding Standards

### Naming Conventions
- **Classes**: PascalCase (e.g., `GameManager`, `VRInputHandler`)
- **Methods**: PascalCase (e.g., `UpdateAI()`, `DetectPlayer()`)
- **Fields**: camelCase with underscore prefix for private (e.g., `_gameActive`, `currentPower`)
- **Constants**: UPPER_CASE (e.g., `MAX_POWER`, `DETECTION_RANGE`)

### Namespaces
Always use appropriate namespaces:
```csharp
namespace FNAFVR.Core { }
namespace FNAFVR.VR { }
namespace FNAFVR.Animatronics { }
namespace FNAFVR.UI { }
namespace FNAFVR.Audio { }
```

### Documentation
- Add XML comments to public methods
- Include class-level summaries
- Explain complex logic with inline comments

Example:
```csharp
/// <summary>
/// Detects player within range and updates detection state
/// </summary>
protected virtual void DetectPlayer()
{
    // Implementation
}
```

## Testing

### Running Tests
```bash
# In Unity Editor
Window > General > Test Runner
```

### Test Structure
```
Assets/Scripts/Tests/
├── Core/
├── VR/
└── Animatronics/
```

## Performance Considerations

1. **AI Updates**: Use InvokeRepeating for AI updates, not Update()
2. **Physics**: Use Rigidbody constraints to reduce physics calculations
3. **Audio**: Use object pooling for audio sources
4. **Rendering**: Use LOD groups for distant objects

## Git Workflow

### Branching Strategy
- `main` - Stable release version
- `develop` - Development branch
- `feature/` - Feature branches
- `bugfix/` - Bug fix branches

### Commit Messages
```
[CATEGORY] Brief description

Detailed explanation of changes if needed.

- Bullet point for specific changes
- Another important point
```

Categories:
- `[FEATURE]` - New feature
- `[BUGFIX]` - Bug fix
- `[REFACTOR]` - Code refactoring
- `[DOCS]` - Documentation
- `[CHORE]` - Maintenance tasks

## Resources & References

### Unity XR Development
- [OpenXR Documentation](https://www.khronos.org/openxr/)
- [Unity XR Plugin Management](https://docs.unity3d.com/Manual/com.unity.xr.management.html)
- [XR Interaction Toolkit](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/manual/index.html)

### Game Design
- [FNAF Game Analysis](https://en.wikipedia.org/wiki/Five_Nights_at_Freddy%27s)
- [Survival Horror Design](https://www.gamasutra.com/view/feature/130456/)

### VR Best Practices
- [VR Locomotion](https://www.interaction-design.org/literature/articles/vr-locomotion)
- [VR Comfort & Safety](https://www.vrsafetyinstitute.org/)

## Contributing

When contributing to this project:

1. Create a feature branch from `develop`
2. Follow coding standards
3. Add appropriate documentation
4. Test thoroughly
5. Create a pull request with detailed description
6. Wait for code review before merging

## Questions?

For questions or clarifications, please open an issue or discussion on GitHub.
