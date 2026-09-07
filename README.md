# FNAF VR Game

A Five Nights at Freddy's VR experience built with Unity and XR support.

## Project Overview

This is a fan-made VR adaptation of the FNAF (Five Nights at Freddy's) game, featuring:
- **VR Immersion**: Full VR support with hand presence and controller interactions
- **Security Camera System**: Monitor multiple camera feeds in the facility
- **Animatronic AI**: Intelligent enemy behavior and movement patterns
- **Survival Mechanics**: Manage resources like power and door controls
- **Atmospheric Horror**: Sound design and jump-scares for an immersive experience

## Technology Stack

- **Engine**: Unity (2022 LTS or newer)
- **VR Framework**: OpenXR / SteamVR
- **Language**: C#
- **Target Platforms**: PC VR (Valve Index, HTC Vive, Oculus Rift)

## Project Structure

```
fnaf-vr-game/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/           # Core game logic
│   │   ├── VR/             # VR-specific code
│   │   ├── Animatronics/   # AI and behavior
│   │   ├── UI/             # In-game UI
│   │   └── Audio/          # Sound management
│   ├── Scenes/             # Game scenes
│   ├── Prefabs/            # Reusable game objects
│   ├── Models/             # 3D models
│   ├── Materials/          # Textures and materials
│   └── Audio/              # Sound effects and music
├── ProjectSettings/
└── Packages/
```

## Getting Started

### Prerequisites
- Unity 2022 LTS or newer
- OpenXR Plugin for Unity
- A VR headset (or use Unity's XR Device Simulator for testing)

### Setup Instructions

1. Clone the repository:
```bash
git clone https://github.com/aul264657-beep/fnaf-vr-game.git
cd fnaf-vr-game
```

2. Open the project in Unity

3. Install required packages:
   - OpenXR Plugin
   - XR Plugin Management
   - XR Hands

4. Build and deploy to your VR headset

## Features (Planned)

- [ ] VR Controller Input System
- [ ] Security Camera Switching Mechanic
- [ ] Animatronic AI System
- [ ] Power Management System
- [ ] Door/Vent Control System
- [ ] Audio Cues and Alerts
- [ ] Jump-Scare Events
- [ ] Night Progression System
- [ ] Score/Survival Tracking
- [ ] Settings and Difficulty Levels

## Contributing

Contributions are welcome! Please feel free to submit pull requests or open issues for bugs and feature requests.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Disclaimer

This is a fan-made project inspired by Five Nights at Freddy's. It is not affiliated with or endorsed by Scott Cawthon or Fazbear Entertainment.

## Resources

- [OpenXR Documentation](https://www.khronos.org/openxr/)
- [Unity XR Plugin Management](https://docs.unity3d.com/2022.2/Documentation/Manual/com.unity.xr.management.html)
- [FNAF Series Information](https://freddy-fazbears-pizza.fandom.com/)

---

**Current Status**: Pre-Alpha - Initial setup and planning phase
