# Chaotic Attractors

https://github.com/user-attachments/assets/86e9b083-0ea0-4cbb-8bf2-e9d14605d9fd

This is a visual exploration of chaotic dynamical systems that I built in Unity using C#. The project renders 9 different attractors as an animated swarm of points, tracing their trajectories with a beautiful trail, as they fly through three-dimensional space.
This project was inspired by this [video](https://www.youtube.com/watch?v=idpOunnpKTo) .

## Overview

Chaotic attractors are fascinating mathematical objects that emerge from nonlinear differential equations. Despite their sensitivity to initial conditions, they exhibit beautiful, bounded structures. I created this project to bring these abstract mathematical concepts to life with real-time 3D visualization, allowing you to observe how hundreds of points evolve simultaneously under different attractor dynamics.
The visualization includes 9 different attractors, each with distinct visual characteristics. I designed the transitions between attractors to feature a smooth spherical interpolation effect where all points converge back to the center before scattering again under new dynamics.

## Features

**Multiple Attractors**: Cycle through nine different chaotic systems, including Lorenz, Hyperchaotic Lorenz, Rossler, Chen, and several others. The actual area required for each attractor can vary wildly, so I tried to manually tune each attractor's speed and the camera distance to give the best visual results.

**Dynamic Camera Control**: Switch between an orbital camera that automatically frames each attractor and a fully manual first-person camera. I implemented manual controls with smooth mouselook and adjustable movement speed. You may need to reset the camera position after switching between attractors though.

**Visual Toggles**: You can hide or show the point swarm and their trails independently, allowing you to focus on the overall shape or trace the individual paths of points through space.

**Smooth Transitions**: I wanted to reuse the same points when switching attractors, so I made them smoothly converge to the center point using spherical linear interpolation, which makes a neat 'globe' effect.

<img width="1340" height="754" alt="" src="https://github.com/user-attachments/assets/546a5828-ad2d-46fb-baf9-99854511ff07" />

## Controls

- **RMB + Mouse Movement / WASD**: Enter manual camera mode; hold right-mouse to look around and WASD to move
- **+ / -**: Adjust camera movement speed
- **Q / E**: Cycle through attractors
- **R**: Reset current attractor
- **T**: Toggle trails on/off
- **Y**: Toggle points on/off

## Technical Details

I don't know enough about calculus to get a more accurate integration method working, so instead I decided to use Unity's `Fixed Timestep` at 0.01 seconds (rather than the default 0.02) to provide smoother point trajectories. The attractor computations use simple Euler integration where the derivatives (dx/dt, dy/dt, dz/dt) of the system get added to their current position, after getting scaled by `timestep`.

All attractors are computed on the CPU in C# and run in real-time with configurable resolution (up to 10,000 points). Performance is a lot lower than I expected, I suspect the bottleneck is CPU<->GPU communication. I recently dipped my toe into learning shaders, so I'd like to rewrite this project someday to run entirely on the GPU, if possible.

## Project Structure

- **Scripts/Attractors.cs**: Contains all attractor function definitions with their specific parameters
- **Scripts/Swarm.cs**: Manages the point swarm, transitions between attractors, and point lifecycle
- **Scripts/CameraController.cs**: Handles both orbital and manual camera modes with smooth interpolation

## Building and Running

Open the project in Unity (version 2021.3 or later recommended) and load the main scene from `Assets/Scenes/`.
Or just download the exe from github under `releases`.

<img width="909" height="512" alt="" src="https://github.com/user-attachments/assets/2bb60899-ecd7-4143-aedf-a26407849e71" />
