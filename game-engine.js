// FNAF VR Game - JavaScript Game Engine
// Enhanced version with Three.js for 3D rendering

class AnimatronicAI {
    constructor(name, initialPosition) {
        this.name = name;
        this.position = initialPosition;
        this.state = 'idle'; // idle, moving, hunting, aggressive
        this.threatLevel = 0;
        this.speed = Math.random() * 0.5 + 0.3;
        this.detectionRange = 15;
        this.aggressiveness = Math.random() * 0.5 + 0.3;
        this.lastUpdateTime = Date.now();
    }

    update(gameState, playerPosition) {
        const currentTime = Date.now();
        const deltaTime = (currentTime - this.lastUpdateTime) / 1000;
        this.lastUpdateTime = currentTime;

        const distanceToPlayer = Math.hypot(
            this.position[0] - playerPosition[0],
            this.position[1] - playerPosition[1],
            this.position[2] - playerPosition[2]
        );

        // State transitions
        if (gameState.isPowerOut) {
            this.state = 'aggressive';
            this.threatLevel = Math.min(1, this.threatLevel + deltaTime * 0.5);
        } else if (distanceToPlayer < this.detectionRange) {
            this.state = 'hunting';
            this.threatLevel = Math.min(1, this.threatLevel + deltaTime * 0.3);
        } else if (Math.random() < this.aggressiveness * deltaTime) {
            this.state = this.state === 'idle' ? 'moving' : 'idle';
        }

        // Movement
        if (this.state !== 'idle') {
            const moveDirection = this.state === 'hunting' 
                ? this.getDirectionToPlayer(playerPosition)
                : this.getRandomDirection();
            
            this.position[0] += moveDirection[0] * this.speed * deltaTime;
            this.position[1] += moveDirection[1] * this.speed * deltaTime;
            this.position[2] += moveDirection[2] * this.speed * deltaTime;
        }

        // Threat decay when not active
        if (this.state === 'idle') {
            this.threatLevel = Math.max(0, this.threatLevel - deltaTime * 0.2);
        }
    }

    getDirectionToPlayer(playerPosition) {
        const dx = playerPosition[0] - this.position[0];
        const dy = playerPosition[1] - this.position[1];
        const dz = playerPosition[2] - this.position[2];
        const dist = Math.hypot(dx, dy, dz) || 1;
        return [dx / dist, dy / dist, dz / dist];
    }

    getRandomDirection() {
        const angle = Math.random() * Math.PI * 2;
        const height = Math.random() - 0.5;
        return [Math.cos(angle), height, Math.sin(angle)];
    }

    getThreatIndicator() {
        if (this.threatLevel > 0.75) return '★★★';
        if (this.threatLevel > 0.5) return '★★☆';
        if (this.threatLevel > 0.25) return '★☆☆';
        return '☆☆☆';
    }
}

class SecurityCamera {
    constructor(name, position) {
        this.name = name;
        this.position = position;
        this.isActive = false;
        this.hasMotion = false;
        this.feedHistory = [];
    }

    detectMotion(animatronics) {
        const detectionRange = 20;
        this.hasMotion = animatronics.some(anim => {
            const distance = Math.hypot(
                anim.position[0] - this.position[0],
                anim.position[1] - this.position[1],
                anim.position[2] - this.position[2]
            );
            return distance < detectionRange;
        });
        return this.hasMotion;
    }
}

class GameAudio {
    constructor() {
        this.audioContext = new (window.AudioContext || window.webkitAudioContext)();
        this.masterVolume = this.audioContext.createGain();
        this.masterVolume.connect(this.audioContext.destination);
        this.masterVolume.gain.value = 0.3;
    }

    playBeep(frequency = 800, duration = 0.1) {
        const oscillator = this.audioContext.createOscillator();
        const gainNode = this.audioContext.createGain();
        
        oscillator.connect(gainNode);
        gainNode.connect(this.masterVolume);
        
        oscillator.frequency.value = frequency;
        gainNode.gain.setValueAtTime(0.3, this.audioContext.currentTime);
        gainNode.gain.exponentialRampToValueAtTime(0.01, this.audioContext.currentTime + duration);
        
        oscillator.start(this.audioContext.currentTime);
        oscillator.stop(this.audioContext.currentTime + duration);
    }

    playWarning() {
        this.playBeep(1200, 0.1);
        setTimeout(() => this.playBeep(1000, 0.1), 150);
    }

    playAlarm() {
        this.playBeep(1600, 0.2);
        setTimeout(() => this.playBeep(1200, 0.2), 250);
    }
}

class FNAFGameEngine {
    constructor() {
        this.gameState = {
            isPowerOut: false,
            isGameActive: false,
            currentNight: 1,
            timeRemaining: 480,
            power: 100,
            maxPower: 100,
            score: 0,
            threat: 0
        };

        this.animatronics = [
            new AnimatronicAI('Freddy', [0, 0, 0]),
            new AnimatronicAI('Bonnie', [5, 0, 0]),
            new AnimatronicAI('Chica', [-5, 0, 0]),
            new AnimatronicAI('Foxy', [0, 0, -5])
        ];

        this.cameras = [
            new SecurityCamera('Office', [0, 1.5, 0]),
            new SecurityCamera('Main Hall', [10, 1.5, 0]),
            new SecurityCamera('West Wing', [-10, 1.5, 0]),
            new SecurityCamera('East Wing', [0, 1.5, 10])
        ];

        this.playerPosition = [0, 1.7, -5];
        this.audio = new GameAudio();
        this.powerDrainRate = 0.15;
        this.startTime = Date.now();
    }

    update() {
        if (!this.gameState.isGameActive) return;

        const deltaTime = (Date.now() - this.startTime) / 1000;
        
        // Update time
        this.gameState.timeRemaining = Math.max(0, 480 - deltaTime);
        
        // Power management
        if (!this.gameState.isPowerOut) {
            this.gameState.power -= this.powerDrainRate * (deltaTime - (this.gameState.power / 100));
            if (this.gameState.power <= 0) {
                this.gameState.isPowerOut = true;
                this.audio.playAlarm();
            }
        }

        // Update animatronics
        this.animatronics.forEach(anim => {
            anim.update(this.gameState, this.playerPosition);
        });

        // Update cameras
        this.cameras.forEach(cam => {
            cam.detectMotion(this.animatronics);
        });

        // Calculate overall threat
        this.calculateThreatLevel();

        // Check game over conditions
        if (this.gameState.timeRemaining <= 0) {
            this.winNight();
        }

        if (this.gameState.threat >= 1) {
            this.gameOver();
        }
    }

    calculateThreatLevel() {
        let totalThreat = this.animatronics.reduce((sum, anim) => sum + anim.threatLevel, 0);
        this.gameState.threat = Math.min(1, totalThreat / 4);
    }

    usePower(amount) {
        if (this.gameState.power >= amount) {
            this.gameState.power -= amount;
            this.audio.playBeep();
            return true;
        }
        return false;
    }

    activateCamera(cameraIndex) {
        if (this.usePower(2)) {
            return this.cameras[cameraIndex];
        }
        return null;
    }

    toggleDoors() {
        return this.usePower(5);
    }

    toggleLights() {
        if (this.usePower(3)) {
            this.animatronics.forEach(anim => {
                anim.threatLevel = Math.max(0, anim.threatLevel - 0.1);
            });
            return true;
        }
        return false;
    }

    startNight() {
        this.gameState.isGameActive = true;
        this.gameState.isPowerOut = false;
        this.gameState.power = 100;
        this.gameState.timeRemaining = 480;
        this.startTime = Date.now();
        
        this.animatronics.forEach(anim => {
            anim.threatLevel = 0;
            anim.state = 'idle';
        });
    }

    winNight() {
        this.gameState.isGameActive = false;
        this.gameState.score += 100;
        this.gameState.currentNight++;
    }

    gameOver() {
        this.gameState.isGameActive = false;
    }
}

// Export for use in HTML
window.FNAFGameEngine = FNAFGameEngine;
window.AnimatronicAI = AnimatronicAI;
window.SecurityCamera = SecurityCamera;
window.GameAudio = GameAudio;
